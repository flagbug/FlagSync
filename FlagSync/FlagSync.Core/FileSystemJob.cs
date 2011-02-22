using System;
using System.Collections.Generic;
using System.IO;
using FlagLib.FileSystem;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core
{
    internal abstract class FileSystemJob : Job
    {
        private HashSet<string> proceededFilePaths = new HashSet<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemJob"/> class.
        /// </summary>
        /// <param name="settings">The job settings.</param>
        protected FileSystemJob(JobSetting settings, IFileSystem fileSystem)
            : base(settings, fileSystem) { }

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

        #region High level operations

        /// <summary>
        /// Backups a directory recursively to another directory (without deletions).
        /// </summary>
        /// <param name="sourceDirectoryPath">The source directory path.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <param name="execute">if set to true, the modifications, creations and deletions will be executed executed.</param>
        protected void BackupDirectoryRecursively(string sourceDirectoryPath, string targetDirectoryPath, bool execute)
        {
            if (!this.FileSystem.DirectoryExists(sourceDirectoryPath))
                throw new ArgumentException("sourceDirectoryPath", "The source directory doesn't exist.");

            if (!this.FileSystem.DirectoryExists(sourceDirectoryPath))
                throw new ArgumentException("targetDirectoryPath", "The target directory doesn't exist.");

            IFileSystemScanner rootScanner =
                this.FileSystem.CreateFileSystemScanner(sourceDirectoryPath);

            IDirectoryInfo currentTargetDirectory =
                this.FileSystem.CreateDirectoryInfo(targetDirectoryPath);

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
                    if (!this.FileSystem.DirectoryExists(newTargetDirectoryPath))
                    {
                        this.PerformDirectoryCreationOperation(e.Directory, currentTargetDirectory, execute);
                    }

                    currentTargetDirectory = this.FileSystem.CreateDirectoryInfo(newTargetDirectoryPath);
                };

            rootScanner.DirectoryProceeded += (sender, e) =>
            {
                this.CheckPause();
                if (this.IsStopped)
                {
                    rootScanner.Stop();
                    return;
                }

                //When a directory has been completely preceeded, jump to the parent directory of the target directory
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
                    if (!File.Exists(targetFilePath))
                    {
                        this.PerformFileCreationOperation(e.File, currentTargetDirectory, execute);

                        //Add the created file to the proceeded files, to avoid a double-counting
                        this.proceededFilePaths.Add(Path.Combine(currentTargetDirectory.FullName, e.File.Name));
                    }

                    //Check if the source file is newer than the target file
                    else if (this.IsFileModified(e.File, this.FileSystem.CreateFileInfo(targetFilePath)))
                    {
                        this.PerformFileModificationOperation(e.File, currentTargetDirectory, execute);
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
        protected void CheckDeletionsRecursively(string sourceDirectoryPath, string targetDirectoryPath, bool execute)
        {
            if (!this.FileSystem.DirectoryExists(sourceDirectoryPath))
                throw new ArgumentException("sourceDirectoryPath", "The source directory doesn't exist.");

            if (!this.FileSystem.DirectoryExists(sourceDirectoryPath))
                throw new ArgumentException("targetDirectoryPath", "The target directory doesn't exist.");

            IFileSystemScanner rootScanner =
                this.FileSystem.CreateFileSystemScanner(sourceDirectoryPath);

            IDirectoryInfo currentTargetDirectory = this.FileSystem.CreateDirectoryInfo(targetDirectoryPath);

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
                    this.PerformDirectoryDeletionOperation(e.Directory, execute);
                }

                currentTargetDirectory = this.FileSystem.CreateDirectoryInfo(newTargetDirectoryPath);
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
                    this.PerformFileDeletionOperation(e.File, execute);

                    //Add the deleted file to the proceeded files, to avoid a double-counting
                    //(this can happen when the deletion of the file fails)
                    this.proceededFilePaths.Add(e.File.Name);
                }

                this.OnProceededFile(new FileProceededEventArgs(sourceFilePath, sourceFileLength));
            };

            rootScanner.Start();
        }

        #endregion High level operations

        #region Mid level operations

        /// <summary>
        /// Performs a file deletion (mid level operation).
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        protected void PerformFileDeletionOperation(IFileInfo file, bool execute)
        {
            FileDeletionEventArgs eventArgs = new FileDeletionEventArgs(file.FullName);

            this.OnDeletingFile(eventArgs);

            //Only delete the file, if the operation should get executed
            bool hasPerformed = execute ?
                this.FileSystem.TryDeleteFile(file) : false;

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
        protected void PerformDirectoryDeletionOperation(IDirectoryInfo directory, bool execute)
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
            bool hasPerformed = execute ?
                this.FileSystem.TryDeleteDirectory(directory) : false;

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
        protected void PerformFileCreationOperation(IFileInfo sourceFile, IDirectoryInfo targetDirectory, bool execute)
        {
            FileCopyEventArgs eventArgs = new FileCopyEventArgs(sourceFile, sourceFile.Directory, targetDirectory);

            this.OnCreatingFile(eventArgs);

            //Only copy the file, if the operation should get executed
            bool hasPerformed = execute ?
                this.FileSystem.TryCopyFile(sourceFile, targetDirectory) : false;

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
        protected void PerformFileModificationOperation(IFileInfo sourceFile, IDirectoryInfo targetDirectory, bool execute)
        {
            FileCopyEventArgs eventArgs = new FileCopyEventArgs(sourceFile, sourceFile.Directory, targetDirectory);

            this.OnModifyingFile(eventArgs);

            EventHandler<CopyProgressEventArgs> handler = (sender, e) =>
            {
                e.Cancel = this.IsStopped; //Stop the copy operation if the job is stopped

                this.OnFileProgressChanged(new CopyProgressEventArgs(e.TotalFileSize, e.TotalBytesTransferred));
            };

            this.FileSystem.FileCopyProgressChanged += handler;

            //Only copy the file, if the operation should get executed
            bool hasPerformed = execute ?
                this.FileSystem.TryCopyFile(sourceFile, targetDirectory) : false;

            this.FileSystem.FileCopyProgressChanged -= handler;

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
        protected void PerformDirectoryCreationOperation(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory, bool execute)
        {
            DirectoryCreationEventArgs eventArgs = new DirectoryCreationEventArgs(sourceDirectory, targetDirectory);

            this.OnCreatingDirectory(eventArgs);

            //Only create the directory, if the operation should get executed
            bool hasPerformed = execute ?
                this.FileSystem.TryCreateDirectory(sourceDirectory, targetDirectory) : false;

            if (hasPerformed)
            {
                this.OnCreatedDirectory(eventArgs);
            }

            else if (execute)
            {
                this.OnDirectoryCreationError(new DirectoryCreationEventArgs(sourceDirectory, targetDirectory));
            }
        }

        #endregion Mid level operations
    }
}