using System;
using System.ComponentModel;
using System.IO;
using System.Security;
using FlagLib.FileSystem;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Local
{
    internal class LocalFileSystem : IFileSystem
    {
        public event EventHandler<CopyProgressEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Tries to delete a file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>
        /// A value indicating whether the file deletion has succeed
        /// </returns>
        public bool TryDeleteFile(IFileSystemInfo file)
        {
            bool succeed = false;

            try
            {
                File.Delete(file.FullName);

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
        /// <returns>
        /// Returns a value indicating whether the directory creation has succeed
        /// </returns>
        public bool TryCreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
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
        /// <returns>
        /// A value indicating whether the deletion has succeed.
        /// </returns>
        public bool TryDeleteDirectory(IDirectoryInfo directory)
        {
            bool succeed = false;

            try
            {
                Directory.Delete(directory.FullName, true);

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
        /// <returns></returns>
        public bool TryCopyFile(IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            bool succeed = false;

            FileCopyOperation fileCopyOperation = new FileCopyOperation();

            fileCopyOperation.CopyProgressUpdated += (sender, e) =>
            {
                if (this.FileCopyProgressChanged != null)
                {
                    this.FileCopyProgressChanged(this, e);
                }
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

        /// <summary>
        /// Creates a new file info at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public IFileInfo GetFileInfo(string path)
        {
            return new LocalFileInfo(new FileInfo(path));
        }

        /// <summary>
        /// Creates a new directory info at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public IDirectoryInfo GetDirectoryInfo(string path)
        {
            return new LocalDirectoryInfo(new DirectoryInfo(path));
        }

        /// <summary>
        /// Checks if the file at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Checks if the directory at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
    }
}