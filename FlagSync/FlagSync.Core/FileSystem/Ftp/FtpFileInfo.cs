using System;
using System.Diagnostics;
using System.IO;
using FlagFtp;
using Rareform.Extensions;
using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core.FileSystem.Ftp
{
    /// <summary>
    /// Represents a file on a FTP-server
    /// </summary>
    [DebuggerDisplay("{FullName}")]
    public class FtpFileInfo : IFileInfo
    {
        private readonly FtpClient client;

        /// <summary>
        /// Gets the last write time.
        /// </summary>
        public DateTime LastWriteTime { get; private set; }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        public long Length { get; private set; }

        /// <summary>
        /// Gets the directory of the file.
        /// </summary>
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
        /// Gets the full name of the file.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
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
        /// <param name="client">The client.</param>
        public FtpFileInfo(string fullName, DateTime lastWriteTime, long length, FtpClient client)
        {
            fullName.ThrowIfNull(() => fullName);
            client.ThrowIfNull(() => client);

            this.FullName = fullName;
            this.LastWriteTime = lastWriteTime;
            this.Length = length;
            this.client = client;
        }
    }
}