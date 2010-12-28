using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security;
using FlagLib.FileSystem;

namespace FlagSync.Core
{
    internal abstract class FileSystemJob : Job
    {
        private HashSet<string> proceededFilePaths = new HashSet<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemJob"/> class.
        /// </summary>
        /// <param name="settings">The job settings.</param>
        protected FileSystemJob(JobSetting settings)
            : base(settings) { }

        /// <summary>
        /// Determines if file A is newer than file B
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
            if (!Directory.Exists(sourceDirectoryPath))
                throw new ArgumentException("sourceDirectoryPath", "The source directory doesn't exist.");

            if (!Directory.Exists(sourceDirectoryPath))
                throw new ArgumentException("targetDirectoryPath", "The target directory doesn't exist.");

            DirectoryScanner rootScanner = new DirectoryScanner(sourceDirectoryPath);

            DirectoryInfo currentTargetDirectory = new DirectoryInfo(targetDirectoryPath);

            rootScanner.DirectoryFound += (sender, e) =>
                {
                    if (this.IsStopped)
                    {
                        rootScanner.Stop();
                        return;
                    }

                    string newTargetDirectoryPath = Path.Combine(currentTargetDirectory.FullName, e.Directory.Name);

                    //Check if the directory doesn't exist in the target directory
                    if (!Directory.Exists(newTargetDirectoryPath))
                    {
                        this.PerformDirectoryCreationOperation(e.Directory, currentTargetDirectory, execute);
                    }

                    currentTargetDirectory = new DirectoryInfo(newTargetDirectoryPath);
                };

            rootScanner.DirectoryProceeded += (sender, e) =>
            {
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
                    if (this.IsStopped)
                    {
                        rootScanner.Stop();
                        return;
                    }

                    string targetFilePath = Path.Combine(currentTargetDirectory.FullName, e.File.Name);

                    //Check if the file doesn't exist in the target directory
                    if (!File.Exists(targetFilePath))
                    {
                        this.PerformFileCreationOperation(e.File, currentTargetDirectory, execute);
                        this.proceededFilePaths.Add(Path.Combine(currentTargetDirectory.FullName, e.File.Name));
                    }

                    //Check if the source file is newer than the target file
                    else if (this.IsFileModified(e.File, new FileInfo(targetFilePath)))
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
            if (!Directory.Exists(sourceDirectoryPath))
                throw new ArgumentException("sourceDirectoryPath", "The source directory doesn't exist.");

            if (!Directory.Exists(sourceDirectoryPath))
                throw new ArgumentException("targetDirectoryPath", "The target directory doesn't exist.");

            DirectoryScanner rootScanner = new DirectoryScanner(sourceDirectoryPath);

            DirectoryInfo currentTargetDirectory = new DirectoryInfo(targetDirectoryPath);

            rootScanner.DirectoryFound += (sender, e) =>
            {
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

                currentTargetDirectory = new DirectoryInfo(newTargetDirectoryPath);
            };

            rootScanner.DirectoryProceeded += (sender, e) =>
            {
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
        /// Performs a directory deletion (mid level operation).
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <param name="execute">if set to true, the operation gets executed.</param>
        protected void PerformDirectoryDeletionOperation(DirectoryInfo directory, bool execute)
        {
            DirectoryDeletionEventArgs eventArgs = new DirectoryDeletionEventArgs(directory.FullName);

            this.OnDeletingDirectory(eventArgs);

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
                this.OnDirectoryCreationError(new DirectoryCreationEventArgs(sourceDirectory, targetDirectory));
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
                Logger.Current.LogError(string.Format("IOException while deleting file: {0}", file.FullName));
            }

            catch (SecurityException)
            {
                Logger.Current.LogError(string.Format("SecurityException while deleting file: {0}", file.FullName));
            }

            catch (UnauthorizedAccessException)
            {
                Logger.Current.LogError(string.Format("UnauthorizedAccessException while deleting file: {0}", file.FullName));
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
                Logger.Current.LogError(
                    string.Format("DirectoryNotFoundException while creating directory: {0} in directory: {1}",
                        sourceDirectory.Name, targetDirectory.FullName));
            }

            catch (PathTooLongException)
            {
                Logger.Current.LogError(
                    string.Format("PathTooLongException while creating directory: {0} in directory: {1}",
                        sourceDirectory.Name, targetDirectory.FullName));
            }

            catch (IOException)
            {
                Logger.Current.LogError(
                    string.Format("IOException while creating directory: {0} in directory: {1}",
                    sourceDirectory.Name, targetDirectory.FullName));
            }

            catch (UnauthorizedAccessException)
            {
                Logger.Current.LogError(
                    string.Format("UnauthorizedAccessException while creating directory: {0} in directory: {1}",
                        sourceDirectory.Name, targetDirectory.FullName));
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
                Logger.Current.LogError(string.Format("DirectoryNotFoundException while deleting directory: {0}", directory.FullName));
            }

            catch (IOException)
            {
                Logger.Current.LogError(string.Format("IOException while deleting directory: {0}", directory.FullName));
            }

            catch (UnauthorizedAccessException)
            {
                Logger.Current.LogError(string.Format("UnauthorizedAccessException while deleting directory: {0}", directory.FullName));
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
                Logger.Current.LogError(
                    string.Format("Win32Exception while copying file: {0} to directory: {1}",
                        sourceFile.FullName, targetDirectory.FullName));
            }

            return succeed;
        }

        #endregion Low level operations
    }
}