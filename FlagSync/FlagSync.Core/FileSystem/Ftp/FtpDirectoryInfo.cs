using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlagFtp;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Ftp
{
    internal class FtpDirectoryInfo : IDirectoryInfo
    {
        private FtpClient client;

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
        /// Gets the parent directory.
        /// </summary>
        /// <value>
        /// The parent directory.
        /// </value>
        public IDirectoryInfo Parent
        {
            get
            {
                FlagFtp.FtpDirectoryInfo directory =
                    this.client.GetDirectoryInfo(new Uri(Path.GetDirectoryName(this.FullName)));

                return new FtpDirectoryInfo(directory.FullName, this.client);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the directory exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists
        {
            get { return true; }
        }

        /// <summary>
        /// Return the files in the directory.
        /// </summary>
        /// <returns>
        /// The files in the directory
        /// </returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The exception that is thrown if the directory is locked
        ///   </exception>
        public IEnumerable<IFileInfo> GetFiles()
        {
            return this.client.GetFiles(new Uri(this.FullName))
                .Select(file => new FtpFileInfo(file.FullName, file.LastWriteTime, file.Length, this.client))
                .Cast<IFileInfo>();
        }

        /// <summary>
        /// Return the directories in the directory.
        /// </summary>
        /// <returns>
        /// The directories in the directory
        /// </returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The exception that is thrown if the directory is locked
        ///   </exception>
        public IEnumerable<IDirectoryInfo> GetDirectories()
        {
            return this.client.GetDirectories(new Uri(this.FullName))
                .Select(directory => new FtpDirectoryInfo(directory.FullName, this.client))
                .Cast<IDirectoryInfo>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpDirectoryInfo"/> class.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <param name="client">The client.</param>
        public FtpDirectoryInfo(string fullName, FtpClient client)
        {
            this.FullName = fullName;
            this.client = client;
        }
    }
}