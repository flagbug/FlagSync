using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FlagLib.Extensions;
using FlagLib.IO;
using FlagLib.Reflection;
using FlagSync.Core.FileSystem;
using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core
{
    public abstract class Job
    {
        private HashSet<string> proceededFilePaths;
        private HashSet<string> excludedPaths;

        /// <summary>
        /// Gets the source file system.
        /// </summary>
        public IFileSystem SourceFileSystem { get; private set; }

        /// <summary>
        /// Gets the target file system.
        /// </summary>
        public IFileSystem TargetFileSystem { get; private set; }

        /// <summary>
        /// Gets the directory A.
        /// </summary>
        public IDirectoryInfo DirectoryA { get; private set; }

        /// <summary>
        /// Gets the directory B.
        /// </summary>
        public IDirectoryInfo DirectoryB { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Job"/> is paused.
        /// </summary>
        /// <value>
        /// true if paused; otherwise, false.
        /// </value>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Job"/> is stopped.
        /// </summary>
        /// <value>
        /// true if stopped; otherwise, false.
        /// </value>
        public bool IsStopped { get; private set; }

        /// <summary>
        /// Gets the written bytes.
        /// </summary>
        /// <value>
        /// The written bytes.
        /// </value>
        public long WrittenBytes { get; private set; }

        /// <summary>
        /// Gets the name of the job.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Occurs when a file has been proceeded.
        /// </summary>
        public event EventHandler<FileProceededEventArgs> ProceededFile;

        /// <summary>
        /// Occurs when the job has finished.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Occurs before a file gets deleted.
        /// </summary>
        public event EventHandler<FileDeletionEventArgs> DeletingFile;

        /// <summary>
        /// Occurs when a file has been deleted.
        /// </summary>
        public event EventHandler<FileDeletionEventArgs> DeletedFile;

        /// <summary>
        /// Occurs before a new file gets created.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> CreatingFile;

        /// <summary>
        /// Occurs when a new file has been created.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> CreatedFile;

        /// <summary>
        /// Occurs before a file gets modified.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> ModifyingFile;

        /// <summary>
        /// Occurs when a file has been modified.
        /// </summary>
        public event EventHandler<FileCopyEventArgs> ModifiedFile;

        /// <summary>
        /// Occurs before a new directory gets created.
        /// </summary>
        public event EventHandler<DirectoryCreationEventArgs> CreatingDirectory;

        /// <summary>
        /// Occurs when a new directory has been created.
        /// </summary>
        public event EventHandler<DirectoryCreationEventArgs> CreatedDirectory;

        /// <summary>
        /// Occurs before a directory has been deleted.
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DeletingDirectory;

        /// <summary>
        /// Occurs when a directory has been deleted.
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DeletedDirectory;

        /// <summary>
        /// Occurs when a file copy error has occured.
        /// </summary>
        public event EventHandler<FileCopyErrorEventArgs> FileCopyError;

        /// <summary>
        /// Occurs when a file deletion error has occured.
        /// </summary>
        public event EventHandler<FileDeletionErrorEventArgs> FileDeletionError;

        /// <summary>
        /// Occurs when a directory creation error has occured.
        /// </summary>
        public event EventHandler<DirectoryCreationEventArgs> DirectoryCreationError;

        /// <summary>
        /// Occurs when a directory deletion error has occured.
        /// </summary>
        public event EventHandler<DirectoryDeletionEventArgs> DirectoryDeletionError;

        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        public event EventHandler<DataTransferEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Job"/> class.
        /// </summary>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="targetFileSystem">The target file system.</param>
        protected Job(string name, IFileSystem sourceFileSystem, IFileSystem targetFileSystem, IDirectoryInfo directoryA, IDirectoryInfo directoryB)
        {
            sourceFileSystem.ThrowIfNull(() => sourceFileSystem);
            targetFileSystem.ThrowIfNull(() => targetFileSystem);
            directoryA.ThrowIfNull(() => directoryA);
            directoryB.ThrowIfNull(() => directoryB);

            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("The name cannot be null or empty.", Reflector.GetMemberName(() => name));

            this.SourceFileSystem = sourceFileSystem;
            this.TargetFileSystem = targetFileSystem;

            this.DirectoryA = directoryA;
            this.DirectoryB = directoryB;

            this.proceededFilePaths = new HashSet<string>();
            this.excludedPaths = new HashSet<string>();
        }

        /// <summary>
        /// Starts the job.
        /// </summary>
        /// <param name="preview">if set to <c>true</c>, a preview will be performed.</param>
        /// <remarks>
        /// This is the main entry point of every job.
        /// The start method starts all actions that are necessary to perform the job.
        /// </remarks>
        public abstract void Start(bool preview);

        /// <summary>
        /// Pauses the job
        /// </summary>
        public virtual void Pause()
        {
            this.IsPaused = true;
        }

        /// <summary>
        /// Continues the job (only after pause)
        /// </summary>
        public virtual void Continue()
        {
            this.IsPaused = false;
        }

        /// <summary>
        /// Stops the job (can't be continued)
        /// </summary>
        public virtual void Stop()
        {
            this.IsPaused = false;
            this.IsStopped = true;
        }

        /// <summary>
        /// Counts the files of both directories of this job.
        /// </summary>
        /// <returns>The result of the counting of both directories of this job.</returns>
        public FileCounterResult CountFiles()
        {
            return FileCounter.CountFiles(this.DirectoryA) + FileCounter.CountFiles(this.DirectoryB);
        }

        /// <summary>
        /// Checks if the job is paused, when true,
        /// a loop will be enabled which causes the thread to sleep till the job gets continued
        /// </summary>
        protected void CheckPause()
        {
            while (this.IsPaused)
            {
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CreatingFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatingFile(FileCopyEventArgs e)
        {
            if (this.CreatingFile != null)
            {
                this.CreatingFile(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CreatedFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatedFile(FileCopyEventArgs e)
        {
            if (this.CreatedFile != null)
            {
                this.CreatedFile(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ModifyingFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnModifyingFile(FileCopyEventArgs e)
        {
            if (this.ModifyingFile != null)
            {
                this.ModifyingFile(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ModifiedFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnModifiedFile(FileCopyEventArgs e)
        {
            if (this.ModifiedFile != null)
            {
                this.ModifiedFile(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DeletingFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletingFile(FileDeletionEventArgs e)
        {
            if (this.DeletingFile != null)
            {
                this.DeletingFile(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DeletedFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletedFile(FileDeletionEventArgs e)
        {
            if (this.DeletedFile != null)
            {
                this.DeletedFile(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CreatingDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatingDirectory(DirectoryCreationEventArgs e)
        {
            if (this.CreatingDirectory != null)
            {
                this.CreatingDirectory(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:CreatedDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatedDirectory(DirectoryCreationEventArgs e)
        {
            if (this.CreatedDirectory != null)
            {
                this.CreatedDirectory(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DeletingDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletingDirectory(DirectoryDeletionEventArgs e)
        {
            if (this.DeletingDirectory != null)
            {
                this.DeletingDirectory(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DeletedDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletedDirectory(DirectoryDeletionEventArgs e)
        {
            if (this.DeletedDirectory != null)
            {
                this.DeletedDirectory(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Finished"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnFinished(EventArgs e)
        {
            if (this.Finished != null)
            {
                this.Finished(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:FileCopyError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileCopyError(FileCopyErrorEventArgs e)
        {
            if (this.FileCopyError != null)
            {
                this.FileCopyError(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DirectoryDeletionError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDirectoryDeletionError(DirectoryDeletionEventArgs e)
        {
            if (this.DirectoryDeletionError != null)
            {
                this.DirectoryDeletionError(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:FileDeletionError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileDeletionError(FileDeletionErrorEventArgs e)
        {
            if (this.FileDeletionError != null)
            {
                this.FileDeletionError(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:FileProgressChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagLib.IO.DataTransferEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileProgressChanged(DataTransferEventArgs e)
        {
            if (this.FileCopyProgressChanged != null)
            {
                this.FileCopyProgressChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DirectoryCreationError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDirectoryCreationError(DirectoryCreationEventArgs e)
        {
            if (this.DirectoryCreationError != null)
            {
                this.DirectoryCreationError(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ProceededFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileProceededEventArgs"/> instance containing the event data.</param>
        protected virtual void OnProceededFile(FileProceededEventArgs e)
        {
            if (!this.proceededFilePaths.Contains(e.FilePath))
            {
                if (this.ProceededFile != null)
                {
                    this.ProceededFile(this, e);
                }
            }
        }

        /// <summary>
        /// Backups a directory recursively to another directory (without deletions).
        /// </summary>
        /// <param name="sourceDirectoryPath">The source directory path.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <param name="execute">if set to true, the modifications, creations and deletions will be executed executed.</param>
        protected void BackupDirectoryRecursively(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory, bool execute)
        {
            if (!sourceDirectory.Exists)
                throw new ArgumentException("The source directory doesn't exist.", Reflector.GetMemberName(() => sourceDirectory));

            if (!targetDirectory.Exists)
                throw new ArgumentException("The target directory doesn't exist.", Reflector.GetMemberName(() => targetDirectory));

            FileSystemScanner rootScanner = new FileSystemScanner(sourceDirectory);

            IDirectoryInfo currentTargetDirectory = targetDirectory;

            rootScanner.DirectoryFound += (sender, e) =>
                {
                    this.CheckPause();
                    if (this.IsStopped)
                    {
                        rootScanner.Stop();
                        return;
                    }

                    //Assemble the path of the new target directory
                    string newTargetDirectoryPath = this.TargetFileSystem.CombinePath(currentTargetDirectory.FullName, e.Directory.Name);

                    //Check if the new target directory exists and if not, create it
                    if (!this.TargetFileSystem.DirectoryExists(newTargetDirectoryPath))
                    {
                        //The directory must not be a subdirectory of any excluded path
                        if (!this.excludedPaths.Any(path => this.NormalizePath(newTargetDirectoryPath).StartsWith(path)))
                        {
                            this.PerformDirectoryCreationOperation(this.TargetFileSystem, e.Directory, currentTargetDirectory, execute);
                        }
                    }

                    currentTargetDirectory = this.TargetFileSystem.GetDirectoryInfo(newTargetDirectoryPath);
                };

            rootScanner.DirectoryProceeded += (sender, e) =>
            {
                this.CheckPause();
                if (this.IsStopped)
                {
                    rootScanner.Stop();
                    return;
                }

                //When a directory has been completely proceeded, jump to the parent directory of the target directory
                if (currentTargetDirectory.Parent != null)
                {
                    currentTargetDirectory = currentTargetDirectory.Parent;
                }
            };

            rootScanner.FileFound += (sender, e) =>
                {
                    this.CheckPause();
                    if (this.IsStopped)
                    {
                        rootScanner.Stop();
                        return;
                    }

                    //Assemble the path of the target file
                    string targetFilePath = this.TargetFileSystem.CombinePath(currentTargetDirectory.FullName, e.File.Name);

                    //The file must not be a contained in any subfolder of the excluded folders
                    if (!this.excludedPaths.Any(path => this.NormalizePath(targetFilePath).StartsWith(path)))
                    {
                        //Check if the target file exists in the target directory and if not, create it
                        if (!this.TargetFileSystem.FileExists(targetFilePath))
                        {
                            this.PerformFileCreationOperation(this.SourceFileSystem, this.TargetFileSystem, e.File, currentTargetDirectory, execute);

                            //Add the created file to the proceeded files, to avoid a double-counting
                            this.proceededFilePaths.Add(this.TargetFileSystem.CombinePath(currentTargetDirectory.FullName, e.File.Name));
                        }

                        //Check if the source file is newer than the target file
                        else if (this.IsFileModified(e.File, this.TargetFileSystem.GetFileInfo(targetFilePath)))
                        {
                            this.PerformFileModificationOperation(this.SourceFileSystem, this.TargetFileSystem, e.File, currentTargetDirectory, execute);

                            //Add the created file to the proceeded files, to avoid a double-counting
                            this.proceededFilePaths.Add(this.TargetFileSystem.CombinePath(currentTargetDirectory.FullName, e.File.Name));
                        }

                        this.OnProceededFile(new FileProceededEventArgs(e.File.FullName, e.File.Length));
                    }
                };

            rootScanner.Start();
        }

        /// <summary>
        /// Checks the source directory recursively for directories and files that are not in the target directory and deletes them.
        /// </summary>
        /// <param name="sourceDirectoryPath">The source directory path.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <param name="execute">if set to true, the modifications, creations and deletions will be executed executed.</param>
        protected void CheckDeletionsRecursively(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory, bool execute)
        {
            if (!sourceDirectory.Exists)
                throw new ArgumentException("The source directory doesn't exist.", Reflector.GetMemberName(() => sourceDirectory));

            if (!targetDirectory.Exists)
                throw new ArgumentException("The target directory doesn't exist.", Reflector.GetMemberName(() => targetDirectory));

            FileSystemScanner rootScanner = new FileSystemScanner(sourceDirectory);

            IDirectoryInfo currentTargetDirectory = targetDirectory;

            rootScanner.DirectoryFound += (sender, e) =>
            {
                this.CheckPause();
                if (this.IsStopped)
                {
                    rootScanner.Stop();
                    return;
                }

                string newTargetDirectoryPath = this.TargetFileSystem.CombinePath(currentTargetDirectory.FullName, e.Directory.Name);

                //Check if the directory doesn't exist in the target directory
                if (!this.SourceFileSystem.DirectoryExists(newTargetDirectoryPath))
                {
                    this.PerformDirectoryDeletionOperation(this.TargetFileSystem, e.Directory, execute);
                }

                currentTargetDirectory = this.SourceFileSystem.GetDirectoryInfo(newTargetDirectoryPath);
            };

            rootScanner.DirectoryProceeded += (sender, e) =>
            {
                this.CheckPause();
                if (this.IsStopped)
                {
                    rootScanner.Stop();
                    return;
                }

                //When a directory has been completely proceeded, jump to the parent directory of the target directory
                currentTargetDirectory = currentTargetDirectory.Parent;
            };

            rootScanner.FileFound += (sender, e) =>
            {
                this.CheckPause();
                if (this.IsStopped)
                {
                    rootScanner.Stop();
                    return;
                }

                string targetFilePath = this.TargetFileSystem.CombinePath(currentTargetDirectory.FullName, e.File.Name);

                //Save the file path and length for the case that the file gets deleted,
                //so that the FileProceeded event can be raises properly
                string sourceFilePath = e.File.FullName;
                long sourceFileLength = e.File.Length;

                //Check if the file doesn't exist in the target directory
                if (!this.SourceFileSystem.FileExists(targetFilePath))
                {
                    this.PerformFileDeletionOperation(this.TargetFileSystem, e.File, execute);

                    //Add the deleted file to the proceeded files, to avoid a double-counting
                    //(this can happen when the deletion of the file fails)
                    this.proceededFilePaths.Add(e.File.Name);
                }

                this.OnProceededFile(new FileProceededEventArgs(sourceFilePath, sourceFileLength));
            };

            rootScanner.Start();
        }

        /// <summary>
        /// Performs a file deletion (mid level operation).
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        private void PerformFileDeletionOperation(IFileSystem fileSystem, IFileInfo file, bool execute)
        {
            FileDeletionEventArgs eventArgs = new FileDeletionEventArgs(file.FullName, file.Length);

            this.OnDeletingFile(eventArgs);

            //Only delete the file, if the operation should get executed
            bool hasPerformed = execute ? fileSystem.TryDeleteFile(file) : false;

            if (hasPerformed)
            {
                this.OnDeletedFile(eventArgs);
            }

            else if (execute)
            {
                this.OnFileDeletionError(new FileDeletionErrorEventArgs(file));
            }
        }

        /// <summary>
        /// Performs a directory deletion (mid level operation).
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        private void PerformDirectoryDeletionOperation(IFileSystem fileSystem, IDirectoryInfo directory, bool execute)
        {
            DirectoryDeletionEventArgs eventArgs = new DirectoryDeletionEventArgs(directory.FullName);

            this.OnDeletingDirectory(eventArgs);

            FileSystemScanner directoryScanner = new FileSystemScanner(directory);

            directoryScanner.FileFound += (sender, e) =>
            {
                this.OnProceededFile(new FileProceededEventArgs(e.File.FullName, e.File.Length));
            };

            directoryScanner.Start();

            //Only delete the directory, if the operation should get executed
            bool hasPerformed = execute ? fileSystem.TryDeleteDirectory(directory) : false;

            if (hasPerformed)
            {
                this.OnDeletedDirectory(eventArgs);
            }

            else if (execute)
            {
                this.OnDirectoryDeletionError(new DirectoryDeletionEventArgs(directory.FullName));
            }
        }

        /// <summary>
        /// Performs a file creation (mid level operation).
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        private void PerformFileCreationOperation(IFileSystem sourceFileSystem, IFileSystem targetFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory, bool execute)
        {
            FileCopyEventArgs eventArgs = new FileCopyEventArgs(sourceFile, sourceFile.Directory, targetDirectory);

            this.OnCreatingFile(eventArgs);

            EventHandler<DataTransferEventArgs> handler = (sender, e) =>
            {
                e.Cancel = this.IsStopped; //Stop the copy operation if the job is stopped

                this.OnFileProgressChanged(e);
            };

            targetFileSystem.FileCopyProgressChanged += handler;

            //Only copy the file, if the operation should get executed
            bool hasPerformed = execute ? targetFileSystem.TryCopyFile(sourceFileSystem, sourceFile, targetDirectory) : false;

            targetFileSystem.FileCopyProgressChanged -= handler;

            if (hasPerformed)
            {
                this.OnCreatedFile(eventArgs);
            }

            else if (execute)
            {
                this.OnFileCopyError(new FileCopyErrorEventArgs(sourceFile, targetDirectory));
            }
        }

        /// <summary>
        /// Performs a file modification (mid level operation).
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        private void PerformFileModificationOperation(IFileSystem sourceFileSystem, IFileSystem targetFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory, bool execute)
        {
            FileCopyEventArgs eventArgs = new FileCopyEventArgs(sourceFile, sourceFile.Directory, targetDirectory);

            this.OnModifyingFile(eventArgs);

            EventHandler<DataTransferEventArgs> handler = (sender, e) =>
            {
                e.Cancel = this.IsStopped; //Stop the copy operation if the job is stopped

                this.OnFileProgressChanged(e);
            };

            targetFileSystem.FileCopyProgressChanged += handler;

            //Only copy the file, if the operation should get executed
            bool hasPerformed = execute ? targetFileSystem.TryCopyFile(sourceFileSystem, sourceFile, targetDirectory) : false;

            targetFileSystem.FileCopyProgressChanged -= handler;

            if (hasPerformed)
            {
                this.OnModifiedFile(eventArgs);
            }

            else if (execute)
            {
                this.OnFileCopyError(new FileCopyErrorEventArgs(sourceFile, targetDirectory));
            }
        }

        /// <summary>
        /// Performs a directory creation (mid level operation).
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        private void PerformDirectoryCreationOperation(IFileSystem fileSystem, IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory, bool execute)
        {
            DirectoryCreationEventArgs eventArgs = new DirectoryCreationEventArgs(sourceDirectory, targetDirectory);

            this.OnCreatingDirectory(eventArgs);

            //Only create the directory, if the operation should get executed
            bool hasPerformed = execute ? fileSystem.TryCreateDirectory(sourceDirectory, targetDirectory) : false;

            if (hasPerformed)
            {
                this.OnCreatedDirectory(eventArgs);
            }

            else if (execute)
            {
                this.excludedPaths.Add(this.NormalizePath(targetDirectory.FullName));
                this.OnDirectoryCreationError(new DirectoryCreationEventArgs(sourceDirectory, targetDirectory));
            }
        }

        /// <summary>
        /// Determines if file A is newer than file B
        /// </summary>
        /// <param name="fileA">File A</param>
        /// <param name="fileB">File B</param>
        /// <returns>
        /// True, if file A is newer, otherwise false
        /// </returns>
        private bool IsFileModified(IFileInfo fileA, IFileInfo fileB)
        {
            return fileA.LastWriteTime.CompareTo(fileB.LastWriteTime) > 0;
        }

        private string NormalizePath(string path)
        {
            return path.Replace("\\", "/").Replace("//", "/");
        }
    }
}