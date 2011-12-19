using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using FlagLib.Extensions;
using FlagLib.IO;
using FlagSync.Core.FileSystem.Base;

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
            file.ThrowIfNull(() => file);

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
            }

            catch (SecurityException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
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
            sourceDirectory.ThrowIfNull(() => sourceDirectory);
            targetDirectory.ThrowIfNull(() => targetDirectory);

            bool succeed = false;

            try
            {
                Directory.CreateDirectory(this.CombinePath(targetDirectory.FullName, sourceDirectory.Name));

                succeed = true;
            }

            catch (DirectoryNotFoundException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            catch (PathTooLongException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
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
            directory.ThrowIfNull(() => directory);

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
            }

            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
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
            sourceFileSystem.ThrowIfNull(() => sourceFileSystem);
            sourceFile.ThrowIfNull(() => sourceFile);
            targetDirectory.ThrowIfNull(() => targetDirectory);

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
                            var copyOperation = new StreamCopyOperation(sourceStream, targetStream, 256 * 1024, true);

                            copyOperation.CopyProgressChanged += (sender, e) =>
                                {
                                    this.FileCopyProgressChanged.RaiseSafe(this, e);

                                    canceled = e.Cancel;
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
            }

            catch (SecurityException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
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
            path.ThrowIfNull(() => path);

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
            path.ThrowIfNull(() => path);

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
            path.ThrowIfNull(() => path);

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
            path.ThrowIfNull(() => path);

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
            file.ThrowIfNull(() => file);

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
            path1.ThrowIfNull(() => path1);
            path2.ThrowIfNull(() => path2);

            return Path.Combine(path1, path2);
        }
    }
}