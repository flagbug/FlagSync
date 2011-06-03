using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlagLib.FileSystem;
using FlagSync.Core.FileSystem;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core
{
    internal abstract class FileSystemJob : Job
    {
        private HashSet<string> proceededFilePaths;
        private HashSet<string> excludedPaths;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemJob"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="targetFileSystem">The target file system.</param>
        /// <remarks></remarks>
        protected FileSystemJob(JobSetting settings, IFileSystem sourceFileSystem, IFileSystem targetFileSystem)
            : base(settings, sourceFileSystem, targetFileSystem)
        {
            this.proceededFilePaths = new HashSet<string>();
            this.excludedPaths = new HashSet<string>();
        }

        /// <summary>
        /// Determines if file A is newer than file B
        /// </summary>
        /// <param name="fileA">File A</param>
        /// <param name="fileB">File B</param>
        /// <returns>
        /// True, if file A is newer, otherwise false
        /// </returns>
        protected bool IsFileModified(IFileInfo fileA, IFileInfo fileB)
        {
            return fileA.LastWriteTime.CompareTo(fileB.LastWriteTime) > 0;
        }

        /// <summary>
        /// Raises the <see cref="E:ProceededFile"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FlagSync.Core.FileProceededEventArgs"/> instance containing the event data.</param>
        protected override void OnProceededFile(FileProceededEventArgs e)
        {
            if (!this.proceededFilePaths.Contains(e.FilePath))
            {
                base.OnProceededFile(e);
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
                throw new ArgumentException("The source directory doesn't exist.", "sourceDirectoryPath");

            if (!targetDirectory.Exists)
                throw new ArgumentException("The target directory doesn't exist.", "targetDirectoryPath");

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
                    string newTargetDirectoryPath = Path.Combine(currentTargetDirectory.FullName, e.Directory.Name);

                    //Check if the new target directory exists and if not, create it
                    if (!this.TargetFileSystem.DirectoryExists(newTargetDirectoryPath))
                    {
                        if (!this.excludedPaths.Any(path => newTargetDirectoryPath.StartsWith(path)))
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

                    //Assemble the path of the target file
                    string targetFilePath = Path.Combine(currentTargetDirectory.FullName, e.File.Name);

                    //Check if the target file exists in the target directory and if not, create it
                    if (!this.TargetFileSystem.FileExists(targetFilePath))
                    {
                        this.PerformFileCreationOperation(this.SourceFileSystem, this.TargetFileSystem, e.File, currentTargetDirectory, execute);

                        //Add the created file to the proceeded files, to avoid a double-counting
                        this.proceededFilePaths.Add(Path.Combine(currentTargetDirectory.FullName, e.File.Name));
                    }

                    //Check if the source file is newer than the target file
                    else if (this.IsFileModified(e.File, this.TargetFileSystem.GetFileInfo(targetFilePath)))
                    {
                        this.PerformFileModificationOperation(this.SourceFileSystem, this.TargetFileSystem, e.File, currentTargetDirectory, execute);

                        //Add the created file to the proceeded files, to avoid a double-counting
                        this.proceededFilePaths.Add(Path.Combine(currentTargetDirectory.FullName, e.File.Name));
                    }

                    this.OnProceededFile(new FileProceededEventArgs(e.File.FullName, e.File.Length));
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
                throw new ArgumentException("The source directory doesn't exist.", "sourceDirectoryPath");

            if (!targetDirectory.Exists)
                throw new ArgumentException("The target directory doesn't exist.", "targetDirectoryPath");

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

                string newTargetDirectoryPath = Path.Combine(currentTargetDirectory.FullName, e.Directory.Name);

                //Check if the directory doesn't exist in the target directory
                if (!Directory.Exists(newTargetDirectoryPath))
                {
                    this.PerformDirectoryDeletionOperation(this.TargetFileSystem, e.Directory, execute);
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

                string targetFilePath = Path.Combine(currentTargetDirectory.FullName, e.File.Name);

                //Save the file path and length for the case that the file gets deleted,
                //so that the FileProceeded event can be raises properly
                string sourceFilePath = e.File.FullName;
                long sourceFileLength = e.File.Length;

                //Check if the file doesn't exist in the target directory
                if (!File.Exists(targetFilePath))
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
        protected void PerformFileDeletionOperation(IFileSystem fileSystem, IFileInfo file, bool execute)
        {
            FileDeletionEventArgs eventArgs = new FileDeletionEventArgs(file.FullName);

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
        protected void PerformDirectoryDeletionOperation(IFileSystem fileSystem, IDirectoryInfo directory, bool execute)
        {
            DirectoryDeletionEventArgs eventArgs = new DirectoryDeletionEventArgs(directory.FullName);

            this.OnDeletingDirectory(eventArgs);

            DirectoryScanner directoryScanner = new DirectoryScanner(directory.FullName);

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
        protected void PerformFileCreationOperation(IFileSystem sourceFileSystem, IFileSystem targetFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory, bool execute)
        {
            FileCopyEventArgs eventArgs = new FileCopyEventArgs(sourceFile, sourceFile.Directory, targetDirectory);

            this.OnCreatingFile(eventArgs);

            EventHandler<CopyProgressEventArgs> handler = (sender, e) =>
            {
                e.Cancel = this.IsStopped; //Stop the copy operation if the job is stopped

                this.OnFileProgressChanged(new CopyProgressEventArgs(e.TotalFileSize, e.TotalBytesTransferred));
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
        protected void PerformFileModificationOperation(IFileSystem sourceFileSystem, IFileSystem targetFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory, bool execute)
        {
            FileCopyEventArgs eventArgs = new FileCopyEventArgs(sourceFile, sourceFile.Directory, targetDirectory);

            this.OnModifyingFile(eventArgs);

            EventHandler<CopyProgressEventArgs> handler = (sender, e) =>
            {
                e.Cancel = this.IsStopped; //Stop the copy operation if the job is stopped

                this.OnFileProgressChanged(new CopyProgressEventArgs(e.TotalFileSize, e.TotalBytesTransferred));
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
        protected void PerformDirectoryCreationOperation(IFileSystem fileSystem, IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory, bool execute)
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
                this.excludedPaths.Add(targetDirectory.FullName);
                this.OnDirectoryCreationError(new DirectoryCreationEventArgs(sourceDirectory, targetDirectory));
            }
        }
    }
}