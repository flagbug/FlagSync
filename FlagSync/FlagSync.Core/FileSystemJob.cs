using System;
using System.ComponentModel;
using System.IO;
using System.Security;
using FlagLib.FileSystem;

namespace FlagSync.Core
{
    internal abstract class FileSystemJob : Job
    {
        /// <summary>
        /// Checks if file A is newer than file B
        /// </summary>
        /// <param name="fileA">File A</param>
        /// <param name="fileB">File B</param>
        /// <returns>
        /// True, if file A is newer, otherwise false
        /// </returns>
        protected bool IsFileModified(FileInfo fileA, FileInfo fileB)
        {
            return fileA.LastWriteTime.CompareTo(fileB.LastWriteTime) > 0;
        }

        private string StepIntoDirectory(DirectoryInfo currentTargetDirectory, DirectoryInfo foundDirectory)
        {
            return Path.Combine(currentTargetDirectory.FullName, foundDirectory.Name);
        }

        private string StepOutOfDirectory(DirectoryInfo currentTargetDirectory)
        {
            return currentTargetDirectory.Parent.FullName;
        }

        protected FileSystemJob(JobSetting settings)
            : base(settings)
        {
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
            if (!Directory.Exists(sourceDirectoryPath))
                throw new ArgumentException("sourceDirectoryPath", "The source directory doesn't exist.");

            if (!Directory.Exists(sourceDirectoryPath))
                throw new ArgumentException("targetDirectoryPath", "The target directory doesn't exist.");

            DirectoryScanner rootScanner = new DirectoryScanner(sourceDirectoryPath);

            DirectoryInfo currentTargetDirectory = new DirectoryInfo(targetDirectoryPath);

            rootScanner.DirectoryFound += (sender, e) =>
                {
                    if (this.IsStopped) { return; }

                    string newTargetDirectoryPath = Path.Combine(currentTargetDirectory.FullName, e.Directory.Name);

                    if (!Directory.Exists(newTargetDirectoryPath))
                    {
                        this.PerformDirectoryCreationOperation(e.Directory, currentTargetDirectory, execute);
                    }

                    currentTargetDirectory = new DirectoryInfo(newTargetDirectoryPath);
                };

            rootScanner.DirectoryProceeded += (sender, e) =>
                {
                    currentTargetDirectory = currentTargetDirectory.Parent;
                };

            rootScanner.FileFound += (sender, e) =>
                {
                    if (this.IsStopped) { return; }

                    string targetFilePath = Path.Combine(currentTargetDirectory.FullName, e.File.Name);

                    //Check if the file doesn't exist in the target directory
                    if (!File.Exists(targetFilePath))
                    {
                        this.PerformFileCreationOperation(e.File, currentTargetDirectory, execute);
                    }
                    //Check if the source file is newer than the target file
                    else if (this.IsFileModified(e.File, new FileInfo(targetFilePath)))
                    {
                        this.PerformFileModificationOperation(e.File, currentTargetDirectory, execute);
                    }
                };

            rootScanner.Start();

            this.OnFinished(EventArgs.Empty);
        }

        #endregion High level operations

        #region Mid level operations

        /// <summary>
        /// Performs a file creation (mid level operation).
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        protected void PerformFileCreationOperation(FileInfo sourceFile, DirectoryInfo targetDirectory, bool execute)
        {
            FileCopyEventArgs eventArgs = new FileCopyEventArgs(sourceFile, sourceFile.Directory, targetDirectory);

            this.OnCreatingFile(eventArgs);

            //Only copy the file, if the operation should get executed
            bool hasPerformed = execute ?
                this.TryCopyFile(sourceFile, targetDirectory) : false;

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
        protected void PerformFileModificationOperation(FileInfo sourceFile, DirectoryInfo targetDirectory, bool execute)
        {
            FileCopyEventArgs eventArgs = new FileCopyEventArgs(sourceFile, sourceFile.Directory, targetDirectory);

            this.OnModifyingFile(eventArgs);

            //Only copy the file, if the operation should get executed
            bool hasPerformed = execute ?
                this.TryCopyFile(sourceFile, targetDirectory) : false;

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
        /// Performs a file deletion (mid level operation).
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        protected void PerformFileDeletionOperation(FileInfo file, bool execute)
        {
            FileDeletionEventArgs eventArgs = new FileDeletionEventArgs(file.FullName);

            this.OnDeletingFile(eventArgs);

            //Only delete the file, if the operation should get executed
            bool hasPerformed = execute ?
                this.TryDeleteFile(file) : false;

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
        /// Performs a directory creation (mid level operation).
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        protected void PerformDirectoryCreationOperation(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, bool execute)
        {
            DirectoryCreationEventArgs eventArgs = new DirectoryCreationEventArgs(sourceDirectory, targetDirectory);

            this.OnCreatingDirectory(eventArgs);

            //Only create the directory, if the operation should get executed
            bool hasPerformed = execute ?
                this.TryCreateDirectory(sourceDirectory, targetDirectory) : false;

            if (hasPerformed)
            {
                this.OnCreatedDirectory(eventArgs);
            }

            else if (execute)
            {
                //TODO: Add error handling (OnDirectoryCreationError)
            }
        }

        /// <summary>
        /// Performs a directory deletion (mid level operation).
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        protected void PerformDirectoryDeletionOperation(DirectoryInfo directory, bool execute)
        {
            DirectoryDeletionEventArgs eventArgs = new DirectoryDeletionEventArgs(directory.FullName);

            this.OnDeletedDirectory(eventArgs);

            //Only delete the directory, if the operation should get executed
            bool hasPerformed = execute ?
                this.TryDeleteDirectory(directory) : false;

            if (hasPerformed)
            {
                this.OnDeletedDirectory(eventArgs);
            }

            else if (execute)
            {
                this.OnDirectoryDeletionError(new DirectoryDeletionEventArgs(directory.FullName));
            }
        }

        #endregion Mid level operations

        #region Low level operations

        /// <summary>
        /// Tries to delete a file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>A value indicating whether the file deletion has succeed</returns>
        private bool TryDeleteFile(FileInfo file)
        {
            bool succeed = false;

            try
            {
                file.Delete();

                succeed = true;
            }

            catch (IOException)
            {
                //TODO: Add logging
            }

            catch (SecurityException)
            {
                //TODO: Add logging
            }

            catch (UnauthorizedAccessException)
            {
                //TODO: Add logging
            }

            return succeed;
        }

        /// <summary>
        /// Tries to create a directory in the specified directory (low level operation).
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>Returns a value indicating whether the directory creation has succeed</returns>
        private bool TryCreateDirectory(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
        {
            bool succeed = false;

            try
            {
                Directory.CreateDirectory(Path.Combine(targetDirectory.FullName, sourceDirectory.Name));

                succeed = true;
            }

            catch (DirectoryNotFoundException)
            {
                //TODO: Add logging (maybe the user deleted the directory)
            }

            catch (PathTooLongException)
            {
                //TODO: Add logging
            }

            catch (IOException)
            {
                //TODO: Add logging
            }

            catch (UnauthorizedAccessException)
            {
                //TODO: Add logging
            }

            return succeed;
        }

        /// <summary>
        /// Tries to delete a directory (low level operation).
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <returns>A value indicating whether the deletion has succeed.</returns>
        private bool TryDeleteDirectory(DirectoryInfo directory)
        {
            bool succeed = false;

            DirectoryScanner directoryScanner = new DirectoryScanner(directory.FullName);

            directoryScanner.FileFound += (sender, e) =>
                {
                    this.OnProceededFile(new FileProceededEventArgs(e.File.FullName, e.File.Length));
                };

            directoryScanner.Start();

            try
            {
                directory.Delete(true);

                succeed = true;
            }

            catch (DirectoryNotFoundException)
            {
                //TODO: Add logging (maybe the user deleted the directory)
            }

            catch (IOException)
            {
                //TODO: Add logging
            }

            catch (UnauthorizedAccessException)
            {
                //TODO: Add logging
            }

            return succeed;
        }

        /// <summary>
        /// Tries to copy a file to specified directory (low level operation).
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <exception cref="System.ComponentModel.Win32Exception">Gets thrown when the file copy fails.</exception>
        private bool TryCopyFile(FileInfo sourceFile, DirectoryInfo targetDirectory)
        {
            bool succeed = false;

            FileCopyOperation fileCopyOperation = new FileCopyOperation();

            fileCopyOperation.CopyProgressUpdated += (sender, e) =>
            {
                e.Cancel = this.IsStopped; //Stop the copy operation if the job is stopped

                this.OnFileProgressChanged(new CopyProgressEventArgs(e.TotalFileSize, e.TotalBytesTransferred));
            };

            try
            {
                string targetFilePath = Path.Combine(targetDirectory.FullName, sourceFile.Name);

                fileCopyOperation.CopyFile(sourceFile.FullName, targetFilePath);

                succeed = true;
            }

            catch (Win32Exception)
            {
                //TODO: Add error handling and logging
            }

            return succeed;
        }

        #endregion Low level operations
    }
}