using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using FlagLib.IO;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Local
{
    public class LocalFileSystem : IFileSystem
    {
        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        public event EventHandler<DataTransferEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Tries to delete a file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>
        ///   <c>true</c>, if the deletion has succeed; otherwise, <c>false</c>.
        /// </returns>
        public bool TryDeleteFile(IFileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (!(file is LocalFileInfo))
                throw new ArgumentException("The file must be of type LocalFileInfo.", "file");

            bool succeed = false;

            try
            {
                File.SetAttributes(file.FullName, FileAttributes.Normal);
                File.Delete(file.FullName);

                succeed = true;
            }

            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(string.Format("IOException while deleting file: {0}", file.FullName));
            }

            catch (SecurityException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(string.Format("SecurityException while deleting file: {0}", file.FullName));
            }

            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(string.Format("UnauthorizedAccessException while deleting file: {0}", file.FullName));
            }

            return succeed;
        }

        /// <summary>
        /// Tries to create a directory in the specified directory.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>
        ///   <c>true</c>, if the creation has succeed; otherwise, <c>false</c>.
        /// </returns>
        public bool TryCreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
        {
            if (sourceDirectory == null)
                throw new ArgumentNullException("sourceDirectory");

            if (targetDirectory == null)
                throw new ArgumentNullException("targetDirectory");

            bool succeed = false;

            try
            {
                Directory.CreateDirectory(this.CombinePath(targetDirectory.FullName, sourceDirectory.Name));

                succeed = true;
            }

            catch (DirectoryNotFoundException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(
                    string.Format("DirectoryNotFoundException while creating directory: {0} in directory: {1}",
                        sourceDirectory.Name, targetDirectory.FullName));
            }

            catch (PathTooLongException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(
                    string.Format("PathTooLongException while creating directory: {0} in directory: {1}",
                        sourceDirectory.Name, targetDirectory.FullName));
            }

            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(
                    string.Format("IOException while creating directory: {0} in directory: {1}",
                    sourceDirectory.Name, targetDirectory.FullName));
            }

            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(
                    string.Format("UnauthorizedAccessException while creating directory: {0} in directory: {1}",
                        sourceDirectory.Name, targetDirectory.FullName));
            }

            return succeed;
        }

        /// <summary>
        /// Tries to delete a directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <returns>
        ///   <c>true</c>, if the deletion has succeed; otherwise, <c>false</c>.
        /// </returns>
        public bool TryDeleteDirectory(IDirectoryInfo directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (!(directory is LocalDirectoryInfo))
                throw new ArgumentException("The directory must be of type LocalDirectoryInfo.", "directory");

            bool succeed = false;

            try
            {
                Directory.Delete(directory.FullName, true);

                succeed = true;
            }

            catch (DirectoryNotFoundException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(string.Format("DirectoryNotFoundException while deleting directory: {0}", directory.FullName));
            }

            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(string.Format("IOException while deleting directory: {0}", directory.FullName));
            }

            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(string.Format("UnauthorizedAccessException while deleting directory: {0}", directory.FullName));
            }

            return succeed;
        }

        /// <summary>
        /// Tries to copy a file to specified directory.
        /// </summary>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>
        ///   <c>true</c>, if the copy operation has succeed; otherwise, <c>false</c>.
        /// </returns>
        public bool TryCopyFile(IFileSystem sourceFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            if (sourceFileSystem == null)
                throw new ArgumentNullException("sourceFileSystem");

            if (sourceFile == null)
                throw new ArgumentNullException("sourceFile");

            if (targetDirectory == null)
                throw new ArgumentNullException("targetDirectory");

            if (!(targetDirectory is LocalDirectoryInfo))
                throw new ArgumentException("The target directory must be of type LocalDirectoryInfo.", "targetDirectory");

            bool succeed = false;

            try
            {
                using (Stream sourceStream = sourceFileSystem.OpenFileStream(sourceFile))
                {
                    string targetFilePath = this.CombinePath(targetDirectory.FullName, sourceFile.Name);

                    try
                    {
                        bool canceled = false;

                        using (FileStream targetStream = File.Create(targetFilePath))
                        {
                            StreamCopyOperation copyOperation = new StreamCopyOperation(sourceStream, targetStream, 256 * 1024, true);

                            copyOperation.CopyProgressChanged += (sender, e) =>
                                {
                                    if (this.FileCopyProgressChanged != null)
                                    {
                                        this.FileCopyProgressChanged(this, e);

                                        canceled = e.Cancel;
                                    }
                                };

                            copyOperation.Execute();
                        }

                        succeed = !canceled;
                    }

                    catch (IOException ex)
                    {
                        Debug.WriteLine(ex.Message);
                        File.Delete(targetFilePath);

                        throw;
                    }
                }
            }

            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(
                    string.Format("UnauthorizedAccessException while copying file: {0} to directory: {1}",
                        sourceFile.FullName, targetDirectory.FullName));
            }

            catch (SecurityException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(
                    string.Format("SecurityException while copying file: {0} to directory: {1}",
                    sourceFile.FullName, targetDirectory.FullName));
            }

            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Current.LogError(
                    string.Format("IOException while copying file: {0} to directory: {1}",
                    sourceFile.FullName, targetDirectory.FullName));
            }

            return succeed;
        }

        /// <summary>
        /// Gets the file info at the specified path.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>
        /// An <see cref="IFileInfo"/> of the file from the specified path.
        /// </returns>
        public IFileInfo GetFileInfo(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return new LocalFileInfo(new FileInfo(path));
        }

        /// <summary>
        /// Gets the directory info at the specified path.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>
        /// An <see cref="IDirectoryInfo"/> of the directory from the specified path.
        /// </returns>
        public IDirectoryInfo GetDirectoryInfo(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return new LocalDirectoryInfo(new DirectoryInfo(path));
        }

        /// <summary>
        /// Determines if the file at the specified path exists.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>
        ///   <c>true</c>, if the file exists; otherwise, <c>false</c>.
        /// </returns>
        public bool FileExists(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return File.Exists(path);
        }

        /// <summary>
        /// Determines if the directory at the specified path exists.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>
        ///   <c>true</c>, if the directory exists; otherwise, <c>false</c>.
        /// </returns>
        public bool DirectoryExists(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            return Directory.Exists(path);
        }

        /// <summary>
        /// Opens the stream of the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// A stream from the specified file.
        /// </returns>
        public Stream OpenFileStream(IFileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            return File.Open(file.FullName, FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// Combines two paths for the specific file system.
        /// </summary>
        /// <param name="path1">The first path.</param>
        /// <param name="path2">The second path.</param>
        /// <returns>
        /// A path, which is the combination of the first and second path.
        /// </returns>
        public string CombinePath(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }
    }
}