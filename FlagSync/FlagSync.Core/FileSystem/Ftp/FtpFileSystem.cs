using System;
using System.IO;
using System.Net;
using FlagFtp;
using FlagLib.FileSystem;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Ftp
{
    internal class FtpFileSystem : IFileSystem
    {
        private FtpClient client;

        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        public event EventHandler<CopyProgressEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Tries to delete a file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>
        /// A value indicating whether the file deletion has succeed
        /// </returns>
        public bool TryDeleteFile(IFileInfo file)
        {
            this.client.DeleteFile(new Uri(file.FullName));

            return true;
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
            this.client.CreateDirectory(new Uri(new Uri(targetDirectory.FullName), sourceDirectory.Name));

            return true;
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
            this.client.DeleteDirectory(new Uri(directory.FullName));

            return true;
        }

        /// <summary>
        /// Tries to copy a file to specified directory (low level operation).
        /// </summary>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>
        /// True, if the copy operation has succeed; otherwise, false
        /// </returns>
        public bool TryCopyFile(IFileSystem sourceFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            Uri targetFilePath = new Uri(new Uri(targetDirectory.FullName), sourceFile.Name);

            using (Stream sourceStream = sourceFileSystem.OpenFileStream(sourceFile))
            {
                using (Stream targetStream = this.client.OpenWrite(targetFilePath))
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

            return true;
        }

        /// <summary>
        /// Gets the file info at the specified path.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns></returns>
        public IFileInfo GetFileInfo(string path)
        {
            FlagFtp.FtpFileInfo file = this.client.GetFileInfo(new Uri(path));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the directory info at the specified path.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns></returns>
        public IDirectoryInfo GetDirectoryInfo(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the file at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// True, if the file exists; otherwise, false
        /// </returns>
        public bool FileExists(string path)
        {
            return this.client.FileExists(new Uri(path));
        }

        /// <summary>
        /// Checks if the directory at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// True, if the directory exists; otherwise, false
        /// </returns>
        public bool DirectoryExists(string path)
        {
            return this.client.DirectoryExists(new Uri(path));
        }

        /// <summary>
        /// Opens the stream of the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public Stream OpenFileStream(IFileInfo file)
        {
            return this.client.OpenRead(new Uri(file.FullName));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileSystem"/> class.
        /// </summary>
        /// <param name="serverAddress">The server address.</param>
        /// <param name="credentials">The credentials.</param>
        public FtpFileSystem(Uri serverAddress, NetworkCredential credentials)
        {
            this.client = new FtpClient(credentials);
            this.client.Credentials = credentials;
        }
    }
}