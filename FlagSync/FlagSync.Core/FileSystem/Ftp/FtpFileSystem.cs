using System;
using System.IO;
using System.Net;
using FlagFtp;
using FlagLib.Extensions;
using FlagLib.IO;
using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core.FileSystem.Ftp
{
    public class FtpFileSystem : IFileSystem
    {
        private readonly FtpClient client;

        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        public event EventHandler<DataTransferEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileSystem"/> class.
        /// </summary>
        /// <param name="serverAddress">The server address.</param>
        /// <param name="credentials">The credentials.</param>
        public FtpFileSystem(Uri serverAddress, NetworkCredential credentials)
        {
            serverAddress.ThrowIfNull(() => serverAddress);
            credentials.ThrowIfNull(() => credentials);

            this.client = new FtpClient(credentials);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <exception cref="AccessException">The file could not be accessed.</exception>
        /// <exception cref="ArgumentException">The file is not of type <see cref="FlagFtp.FtpFileInfo"/>.</exception>
        public void DeleteFile(IFileInfo file)
        {
            file.ThrowIfNull(() => file);

            if (!(file is FtpFileInfo))
                throw new ArgumentException("The file must be of type FtpFileInfo.", "file");

            try
            {
                this.client.DeleteFile(new Uri(file.FullName));
            }

            catch (WebException ex)
            {
                throw new AccessException("The file could not be accessed", ex);
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

            Uri newDirectory = (new Uri(new Uri(targetDirectory.FullName + "//"), sourceDirectory.Name));

            try
            {
                this.client.CreateDirectory(newDirectory);
            }

            catch (WebException ex)
            {
                throw new AccessException("The directory could not be accessed", ex);
            }
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <exception cref="AccessException">The directory could not be accessed.</exception>
        /// /// <exception cref="ArgumentException">The directory is not of type <see cref="FlagFtp.FtpDirectoryInfo"/>.</exception>
        public void DeleteDirectory(IDirectoryInfo directory)
        {
            directory.ThrowIfNull(() => directory);

            if (!(directory is FtpDirectoryInfo))
                throw new ArgumentException("The directory must be of type FtpDirectoryInfo.", "directory");
            try
            {
                this.client.DeleteDirectory(new Uri(directory.FullName + "/"));
            }

            catch (WebException ex)
            {
                throw new AccessException("The directory could not be accessed", ex);
            }
        }

        /// <summary>
        /// Copies the specified file to the target directory.
        /// </summary>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <exception cref="AccessException">The source file or target directory could not be accessed.</exception>
        /// /// <exception cref="ArgumentException">The target directory is not of type <see cref="FlagFtp.FtpDirectoryInfo"/>.</exception>
        public void CopyFile(IFileSystem sourceFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            sourceFileSystem.ThrowIfNull(() => sourceFileSystem);
            sourceFile.ThrowIfNull(() => sourceFile);
            targetDirectory.ThrowIfNull(() => targetDirectory);

            if (!(targetDirectory is FtpDirectoryInfo))
                throw new ArgumentException("The target directory must be of type FtpDirectoryInfo.", "targetDirectory");

            Uri targetFilePath = new Uri(this.CombinePath(targetDirectory.FullName, sourceFile.Name));

            try
            {
                using (Stream sourceStream = sourceFileSystem.OpenFileStream(sourceFile))
                {
                    using (Stream targetStream = this.client.OpenWrite(targetFilePath))
                    {
                        if (sourceFile.Length > 0)
                        {
                            var copyOperation = new StreamCopyOperation(sourceStream, targetStream, 8 * 1024, true);

                            copyOperation.CopyProgressChanged +=
                                (sender, e) => this.FileCopyProgressChanged.RaiseSafe(this, e);

                            copyOperation.Execute();
                        }
                    }
                }
            }

            catch (WebException ex)
            {
                throw new AccessException("The file could not be accessed", ex);
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

            path = FtpFileSystem.NormalizePath(path);
            FlagFtp.FtpFileInfo file = this.client.GetFileInfo(new Uri(path));

            return new FtpFileInfo(file.FullName, file.LastWriteTime, file.Length, this.client);
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

            path = FtpFileSystem.NormalizePath(path);
            FlagFtp.FtpDirectoryInfo directory = this.client.GetDirectoryInfo(new Uri(path));

            return new FtpDirectoryInfo(directory.FullName.Replace("%20", " "), this.client);
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

            path = FtpFileSystem.NormalizePath(path);
            return this.client.FileExists(new Uri(path));
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

            path = FtpFileSystem.NormalizePath(path);
            return this.client.DirectoryExists(new Uri(path));
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

            return this.client.OpenRead(new Uri(file.FullName));
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

            return path1 + "/" + path2;
        }

        /// <summary>
        /// Normalizes the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A normalized representation of the path.
        /// </returns>
        private static string NormalizePath(string path)
        {
            path.ThrowIfNull(() => path);

            return path.Replace("//", "/").Replace("/", "//").Replace("\\", "//").Replace("%20", " ");
        }
    }
}