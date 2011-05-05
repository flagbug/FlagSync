using System;
using System.IO;
using FlagFtp;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Ftp
{
    internal class FtpFileInfo : IFileInfo
    {
        private FtpClient client;

        /// <summary>
        /// Gets the last write time.
        /// </summary>
        /// <value>
        /// The last write time.
        /// </value>
        public DateTime LastWriteTime { get; private set; }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>
        /// The length of the file.
        /// </value>
        public long Length { get; private set; }

        /// <summary>
        /// Gets the directory of the file.
        /// </summary>
        /// <value>
        /// The directory of the file.
        /// </value>
        public IDirectoryInfo Directory
        {
            get
            {
                FlagFtp.FtpDirectoryInfo directory =
                    this.client.GetDirectoryInfo(new Uri(Path.GetDirectoryName(this.FullName)));

                return new FtpDirectoryInfo(directory.FullName, this.client);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the file exists.
        /// </summary>
        /// <value>
        /// true if the file exists; otherwise, false.
        /// </value>
        public bool Exists
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return Path.GetFileName(this.FullName); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileInfo"/> class.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        /// <param name="length">The length.</param>
        public FtpFileInfo(string fullName, DateTime lastWriteTime, long length, FtpClient client)
        {
            this.FullName = fullName;
            this.LastWriteTime = lastWriteTime;
            this.Length = length;
            this.client = client;
        }
    }
}