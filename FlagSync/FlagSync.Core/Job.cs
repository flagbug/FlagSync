using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FlagSync.Core.FileSystem;
using FlagSync.Core.FileSystem.Base;
using Rareform.Extensions;
using Rareform.IO;
using Rareform.Reflection;

namespace FlagSync.Core
{
    /// <summary>
    /// Provides the base class for all jobs.
    /// </summary>
    public abstract class Job
    {
        private readonly HashSet<string> proceededFilePaths;
        private readonly HashSet<string> excludedPaths;
        private readonly HashSet<string> deletedDirectoryPaths;
        private readonly AutoResetEvent pauseHandle;

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
        /// true if the job is currently paused; otherwise, false.
        /// </value>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Job"/> is stopped.
        /// </summary>
        /// <value>
        /// true if the job is currently stopped; otherwise, false.
        /// </value>
        public bool IsStopped { get; private set; }

        /// <summary>
        /// Gets the bytes that the job has written, since it has been started.
        /// </summary>
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
        /// <param name="name">The name of the job.</param>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="targetFileSystem">The target file system.</param>
        /// <param name="directoryA">The directory A.</param>
        /// <param name="directoryB">The directory B.</param>
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

            this.Name = name;

            this.proceededFilePaths = new HashSet<string>();
            this.excludedPaths = new HashSet<string>();
            this.deletedDirectoryPaths = new HashSet<string>();

            this.pauseHandle = new AutoResetEvent(false);
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
        /// Pauses the job.
        /// </summary>
        public virtual void Pause()
        {
            this.IsPaused = true;
        }

        /// <summary>
        /// Continues the job (only if the job is currently paused).
        /// </summary>
        public virtual void Continue()
        {
            this.IsPaused = false;
            this.pauseHandle.Set();
        }

        /// <summary>
        /// Stops the job. This means that it can't be continued afterwards, and only be restarted.
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
        public FileCountResult CountFiles()
        {
            return FileCounter.CountFiles(this.DirectoryA) + FileCounter.CountFiles(this.DirectoryB);
        }

        /// <summary>
        /// Checks if the job is paused, when true,
        /// a loop will be enabled which causes the thread to sleep till the job gets continued.
        /// </summary>
        protected void CheckPause()
        {
            if (this.IsPaused)
            {
                this.pauseHandle.WaitOne();
            }
        }

        /// <summary>
        /// Raises the <see cref="CreatingFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatingFile(FileCopyEventArgs e)
        {
            this.CreatingFile.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="CreatedFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatedFile(FileCopyEventArgs e)
        {
            this.CreatedFile.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ModifyingFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnModifyingFile(FileCopyEventArgs e)
        {
            this.ModifyingFile.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ModifiedFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnModifiedFile(FileCopyEventArgs e)
        {
            this.ModifiedFile.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DeletingFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletingFile(FileDeletionEventArgs e)
        {
            this.DeletingFile.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DeletedFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletedFile(FileDeletionEventArgs e)
        {
            this.DeletedFile.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="CreatingDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatingDirectory(DirectoryCreationEventArgs e)
        {
            this.CreatingDirectory.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="CreatedDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnCreatedDirectory(DirectoryCreationEventArgs e)
        {
            this.CreatedDirectory.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DeletingDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletingDirectory(DirectoryDeletionEventArgs e)
        {
            this.DeletingDirectory.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DeletedDirectory"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeletedDirectory(DirectoryDeletionEventArgs e)
        {
            this.DeletedDirectory.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Finished"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnFinished(EventArgs e)
        {
            this.Finished.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="FileCopyError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileCopyErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileCopyError(FileCopyErrorEventArgs e)
        {
            this.FileCopyError.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DirectoryDeletionError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryDeletionEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDirectoryDeletionError(DirectoryDeletionEventArgs e)
        {
            this.DirectoryDeletionError.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="FileDeletionError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileDeletionErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileDeletionError(FileDeletionErrorEventArgs e)
        {
            this.FileDeletionError.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="FileCopyProgressChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Rareform.IO.DataTransferEventArgs"/> instance containing the event data.</param>
        protected virtual void OnFileProgressChanged(DataTransferEventArgs e)
        {
            this.FileCopyProgressChanged.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DirectoryCreationError"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.DirectoryCreationEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDirectoryCreationError(DirectoryCreationEventArgs e)
        {
            this.DirectoryCreationError.RaiseSafe(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ProceededFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileProceededEventArgs"/> instance containing the event data.</param>
        protected virtual void OnProceededFile(FileProceededEventArgs e)
        {
            if (!this.proceededFilePaths.Contains(e.FilePath))
            {
                this.ProceededFile.RaiseSafe(this, e);
            }
        }

        /// <summary>
        /// Backups a directory recursively to another directory (without deletions).
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="execute">if set to true, the modifications, creations and deletions will be executed executed.</param>
        protected void BackupDirectoryRecursively(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory, bool execute)
        {
            if (!sourceDirectory.Exists)
                throw new ArgumentException("The source directory doesn't exist.", Reflector.GetMemberName(() => sourceDirectory));

            if (!targetDirectory.Exists)
                throw new ArgumentException("The target directory doesn't exist.", Reflector.GetMemberName(() => targetDirectory));

            var rootScanner = new FileSystemScanner(sourceDirectory);

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

                    bool newTargetDirectoryExists = this.TargetFileSystem.DirectoryExists(newTargetDirectoryPath);
                    bool newTargetDirectoryIsExcluded = this.excludedPaths.Any(path => NormalizePath(newTargetDirectoryPath).StartsWith(path));

                    //Check if the new target directory exists and if not, create it
                    if (!newTargetDirectoryExists && !newTargetDirectoryIsExcluded)
                    {
                        this.PerformDirectoryCreationOperation(this.TargetFileSystem, e.Directory,
                                                               currentTargetDirectory, execute);
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
                    if (!this.excludedPaths.Any(path => NormalizePath(targetFilePath).StartsWith(path)))
                    {
                        //Check if the target file exists in the target directory and if not, create it
                        if (!this.TargetFileSystem.FileExists(targetFilePath))
                        {
                            this.PerformFileCreationOperation(this.SourceFileSystem, this.TargetFileSystem, e.File, currentTargetDirectory, execute);

                            //Add the created file to the proceeded files, to avoid a double-counting
                            this.proceededFilePaths.Add(this.TargetFileSystem.CombinePath(currentTargetDirectory.FullName, e.File.Name));
                        }

                        //Check if the source file is newer than the target file
                        else if (IsFileModified(e.File, this.TargetFileSystem.GetFileInfo(targetFilePath)))
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
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="execute">if set to true, the modifications, creations and deletions will be executed executed.</param>
        protected void CheckDeletionsRecursively(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory, bool execute)
        {
            if (!sourceDirectory.Exists)
                throw new ArgumentException("The source directory doesn't exist.", Reflector.GetMemberName(() => sourceDirectory));

            if (!targetDirectory.Exists)
                throw new ArgumentException("The target directory doesn't exist.", Reflector.GetMemberName(() => targetDirectory));

            var rootScanner = new FileSystemScanner(sourceDirectory);

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

                bool newTargetDirectoryExists = this.SourceFileSystem.DirectoryExists(newTargetDirectoryPath);
                bool newTargetDirectoryIsExcluded = this.deletedDirectoryPaths.Any(path => NormalizePath(newTargetDirectoryPath).StartsWith(path));

                // Delete the directory if it doesn't exist in the source directory
                if (!newTargetDirectoryExists && !newTargetDirectoryIsExcluded)
                {
                    // If we perform a preview, add the directory that gets deleted to the deleted paths,
                    // so that the subdirectories don't get included.
                    if (!execute)
                    {
                        this.deletedDirectoryPaths.Add(NormalizePath(newTargetDirectoryPath));
                    }

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

                bool targetFileExists = this.SourceFileSystem.FileExists(targetFilePath);
                bool targetFileIsExcluded = this.deletedDirectoryPaths.Any(path => NormalizePath(targetFilePath).StartsWith(path));

                //Check if the file doesn't exist in the target directory
                if (!targetFileExists && !targetFileIsExcluded)
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
        /// Performs a file deletion.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="file">The file to delete.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        private void PerformFileDeletionOperation(IFileSystem fileSystem, IFileInfo file, bool execute)
        {
            var eventArgs = new FileDeletionEventArgs(file.FullName, file.Length);

            this.OnDeletingFile(eventArgs);

            if (execute)
            {
                try
                {
                    fileSystem.DeleteFile(file);

                    this.OnDeletedFile(eventArgs);
                }

                catch (AccessException)
                {
                    this.OnFileDeletionError(new FileDeletionErrorEventArgs(file));
                }
            }
        }

        /// <summary>
        /// Performs a directory deletion.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="directory">The directory to delete.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        private void PerformDirectoryDeletionOperation(IFileSystem fileSystem, IDirectoryInfo directory, bool execute)
        {
            var eventArgs = new DirectoryDeletionEventArgs(directory.FullName);

            this.OnDeletingDirectory(eventArgs);

            var directoryScanner = new FileSystemScanner(directory);

            directoryScanner.FileFound += (sender, e) =>
                this.OnProceededFile(new FileProceededEventArgs(e.File.FullName, e.File.Length));

            directoryScanner.Start();

            if (execute)
            {
                try
                {
                    fileSystem.DeleteDirectory(directory);

                    this.OnDeletedDirectory(eventArgs);
                }

                catch (AccessException)
                {
                    this.OnDirectoryDeletionError(new DirectoryDeletionEventArgs(directory.FullName));
                }
            }
        }

        /// <summary>
        /// Performs a file creation.
        /// </summary>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="targetFileSystem">The target file system.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        private void PerformFileCreationOperation(IFileSystem sourceFileSystem, IFileSystem targetFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory, bool execute)
        {
            var eventArgs = new FileCopyEventArgs(sourceFile, sourceFile.Directory, targetDirectory);

            this.OnCreatingFile(eventArgs);

            if (execute)
            {
                EventHandler<DataTransferEventArgs> handler = (sender, e) =>
                {
                    e.Cancel = this.IsStopped; //Stop the copy operation if the job is stopped

                    this.OnFileProgressChanged(e);
                };

                targetFileSystem.FileCopyProgressChanged += handler;

                try
                {
                    targetFileSystem.CopyFile(sourceFileSystem, sourceFile, targetDirectory);

                    this.OnCreatedFile(eventArgs);
                }

                catch (AccessException)
                {
                    this.OnFileCopyError(new FileCopyErrorEventArgs(sourceFile, targetDirectory));
                }

                targetFileSystem.FileCopyProgressChanged -= handler;
            }
        }

        /// <summary>
        /// Performs a file modification.
        /// </summary>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="targetFileSystem">The target file system.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        private void PerformFileModificationOperation(IFileSystem sourceFileSystem, IFileSystem targetFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory, bool execute)
        {
            var eventArgs = new FileCopyEventArgs(sourceFile, sourceFile.Directory, targetDirectory);

            this.OnModifyingFile(eventArgs);

            if (execute)
            {
                EventHandler<DataTransferEventArgs> handler = (sender, e) =>
                {
                    e.Cancel = this.IsStopped; //Stop the copy operation if the job is stopped

                    this.OnFileProgressChanged(e);
                };

                targetFileSystem.FileCopyProgressChanged += handler;

                try
                {
                    targetFileSystem.CopyFile(sourceFileSystem, sourceFile, targetDirectory);

                    this.OnModifiedFile(eventArgs);
                }

                catch (AccessException)
                {
                    this.OnFileCopyError(new FileCopyErrorEventArgs(sourceFile, targetDirectory));
                }

                targetFileSystem.FileCopyProgressChanged -= handler;
            }
        }

        /// <summary>
        /// Performs a directory creation.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        private void PerformDirectoryCreationOperation(IFileSystem fileSystem, IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory, bool execute)
        {
            var eventArgs = new DirectoryCreationEventArgs(sourceDirectory, targetDirectory);

            this.OnCreatingDirectory(eventArgs);

            if (execute)
            {
                try
                {
                    fileSystem.CreateDirectory(sourceDirectory, targetDirectory);

                    this.OnCreatedDirectory(eventArgs);
                }

                catch (AccessException)
                {
                    this.excludedPaths.Add(NormalizePath(targetDirectory.FullName));
                    this.OnDirectoryCreationError(new DirectoryCreationEventArgs(sourceDirectory, targetDirectory));
                }
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
        private static bool IsFileModified(IFileInfo fileA, IFileInfo fileB)
        {
            return fileA.LastWriteTime > fileB.LastWriteTime;
        }

        /// <summary>
        /// Normalizes the specified path.
        /// </summary>
        /// <param name="path">The path to normalize.</param>
        /// <returns>A normalized version of the path.</returns>
        private static string NormalizePath(string path)
        {
            return path.Replace("\\", "/").Replace("//", "/");
        }
    }
}