using System;
using System.IO;
using System.Security;
using FlagLib.FileSystem;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Local
{
    public class LocalFileSystem : IFileSystem
    {
        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler<CopyProgressEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Tries to delete a file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>A value indicating whether the file deletion has succeed</returns>
        /// <remarks></remarks>
        public bool TryDeleteFile(IFileInfo file)
        {
            bool succeed = false;

            try
            {
                File.Delete(file.FullName);

                succeed = true;
            }

            catch (IOException ex)
            {
                Logger.Current.LogError(string.Format("IOException while deleting file: {0}", file.FullName));
            }

            catch (SecurityException ex)
            {
                Logger.Current.LogError(string.Format("SecurityException while deleting file: {0}", file.FullName));
            }

            catch (UnauthorizedAccessException ex)
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
        /// <remarks></remarks>
        public bool TryCreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
        {
            bool succeed = false;

            try
            {
                Directory.CreateDirectory(Path.Combine(targetDirectory.FullName, sourceDirectory.Name));

                succeed = true;
            }

            catch (DirectoryNotFoundException ex)
            {
                Logger.Current.LogError(
                    string.Format("DirectoryNotFoundException while creating directory: {0} in directory: {1}",
                        sourceDirectory.Name, targetDirectory.FullName));
            }

            catch (PathTooLongException ex)
            {
                Logger.Current.LogError(
                    string.Format("PathTooLongException while creating directory: {0} in directory: {1}",
                        sourceDirectory.Name, targetDirectory.FullName));
            }

            catch (IOException ex)
            {
                Logger.Current.LogError(
                    string.Format("IOException while creating directory: {0} in directory: {1}",
                    sourceDirectory.Name, targetDirectory.FullName));
            }

            catch (UnauthorizedAccessException ex)
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
        /// <remarks></remarks>
        public bool TryDeleteDirectory(IDirectoryInfo directory)
        {
            bool succeed = false;

            try
            {
                Directory.Delete(directory.FullName, true);

                succeed = true;
            }

            catch (DirectoryNotFoundException ex)
            {
                Logger.Current.LogError(string.Format("DirectoryNotFoundException while deleting directory: {0}", directory.FullName));
            }

            catch (IOException ex)
            {
                Logger.Current.LogError(string.Format("IOException while deleting directory: {0}", directory.FullName));
            }

            catch (UnauthorizedAccessException ex)
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
        /// <returns>True, if the copy operation has succeed; otherwise, false</returns>
        /// <remarks></remarks>
        public bool TryCopyFile(IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            bool succeed = false;

            try
            {
                string targetFilePath = Path.Combine(targetDirectory.FullName, sourceFile.Name);

                using (FileStream sourceStream = sourceFile.Open())
                {
                    using (FileStream targetStream = File.Create(targetFilePath))
                    {
                        long bytesTotal = sourceStream.Length;
                        long bytesCurrent = 0;
                        var buffer = new byte[128 * 1024];
                        int bytes;

                        while ((bytes = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            targetStream.Write(buffer, 0, bytes);
                            bytesCurrent += bytes;
                            FileCopyProgressChanged(this,
                                new CopyProgressEventArgs(bytesTotal, bytesCurrent));
                        }
                    }
                }

                succeed = true;
            }

            catch (UnauthorizedAccessException ex)
            {
                Logger.Current.LogError(

                    string.Format("UnauthorizedAccessException while copying file: {0} to directory: {1}",

                        sourceFile.FullName, targetDirectory.FullName));

                succeed = false;
            }

            catch (SecurityException ex)
            {
                Logger.Current.LogError(
                    string.Format("SecurityException while copying file: {0} to directory: {1}",
                    sourceFile.FullName, targetDirectory.FullName));

                succeed = false;
            }

            catch (IOException ex)
            {
                Logger.Current.LogError(
                    string.Format("IOException while copying file: {0} to directory: {1}",
                    sourceFile.FullName, targetDirectory.FullName));

                succeed = false;
            }

            return succeed;
        }

        /// <summary>
        /// Creates a new file info at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public IFileInfo GetFileInfo(string path)
        {
            return new LocalFileInfo(new FileInfo(path));
        }

        /// <summary>
        /// Creates a new directory info at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public IDirectoryInfo GetDirectoryInfo(string path)
        {
            return new LocalDirectoryInfo(new DirectoryInfo(path));
        }

        /// <summary>
        /// Checks if the file at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True, if the file exists; otherwise, false</returns>
        /// <remarks></remarks>
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Checks if the directory at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True, if the directory exists; otherwise, false</returns>
        /// <remarks></remarks>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
    }
}