using System;
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
        /// Deletes the specified file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <exception cref="AccessException">The file could not be accessed.</exception>
        public void DeleteFile(IFileInfo file)
        {
            file.ThrowIfNull(() => file);

            if (!(file is LocalFileInfo))
                throw new ArgumentException("The file must be of type LocalFileInfo.", "file");

            try
            {
                File.SetAttributes(file.FullName, FileAttributes.Normal);
                File.Delete(file.FullName);
            }

            catch (IOException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }

            catch (SecurityException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }

            catch (UnauthorizedAccessException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }
        }

        /// <summary>
        /// Creates the specified directory in the target directory.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <exception cref="AccessException">The directory could not be accessed.</exception>
        public void CreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
        {
            sourceDirectory.ThrowIfNull(() => sourceDirectory);
            targetDirectory.ThrowIfNull(() => targetDirectory);

            try
            {
                Directory.CreateDirectory(this.CombinePath(targetDirectory.FullName, sourceDirectory.Name));
            }

            catch (DirectoryNotFoundException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }

            catch (PathTooLongException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }

            catch (IOException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }

            catch (UnauthorizedAccessException ex)
            {
                throw new AccessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <exception cref="AccessException">The directory could not be accessed.</exception>
        public void DeleteDirectory(IDirectoryInfo directory)
        {
            directory.ThrowIfNull(() => directory);

            if (!(directory is LocalDirectoryInfo))
                throw new ArgumentException("The directory must be of type LocalDirectoryInfo.", "directory");

            try
            {
                Directory.Delete(directory.FullName, true);
            }

            catch (DirectoryNotFoundException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }

            catch (IOException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }

            catch (UnauthorizedAccessException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }
        }

        /// <summary>
        /// Copies the specified file to the target directory.
        /// </summary>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <exception cref="AccessException">The source file or target directory could not be accessed.</exception>
        public void CopyFile(IFileSystem sourceFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            sourceFileSystem.ThrowIfNull(() => sourceFileSystem);
            sourceFile.ThrowIfNull(() => sourceFile);
            targetDirectory.ThrowIfNull(() => targetDirectory);

            if (!(targetDirectory is LocalDirectoryInfo))
                throw new ArgumentException("The target directory must be of type LocalDirectoryInfo.", "targetDirectory");

            try
            {
                using (Stream sourceStream = sourceFileSystem.OpenFileStream(sourceFile))
                {
                    string targetFilePath = this.CombinePath(targetDirectory.FullName, sourceFile.Name);

                    try
                    {
                        using (FileStream targetStream = File.Create(targetFilePath))
                        {
                            if (sourceFile.Length > 0)
                            {
                                var copyOperation = new StreamCopyOperation(sourceStream, targetStream, 256 * 1024, true);

                                copyOperation.CopyProgressChanged +=
                                    (sender, e) => this.FileCopyProgressChanged.RaiseSafe(this, e);

                                copyOperation.Execute();
                            }
                        }
                    }

                    catch (IOException)
                    {
                        File.Delete(targetFilePath);

                        throw;
                    }
                }
            }

            catch (UnauthorizedAccessException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }

            catch (SecurityException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }

            catch (IOException ex)
            {
                throw new AccessException("The file could not be accessed.", ex);
            }
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