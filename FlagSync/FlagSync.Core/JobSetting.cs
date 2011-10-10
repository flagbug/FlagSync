using System;
using System.IO;
using System.Net;
using FlagFtp;
using FlagSync.Core.FileSystem.ITunes;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    [Serializable]
    public class JobSetting
    {
        /// <summary>
        /// Gets or sets the directory A.
        /// </summary>
        /// <value>The directory A.</value>
        public string DirectoryA { get; set; }

        /// <summary>
        /// Gets or sets the directory B.
        /// </summary>
        /// <value>The directory B.</value>
        public string DirectoryB { get; set; }

        /// <summary>
        /// Gets or sets the sync mode.
        /// </summary>
        /// <value>The sync mode.</value>
        public SyncMode SyncMode { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is included.
        /// </summary>
        /// <value>true if this instance is included; otherwise, false.</value>
        public bool IsIncluded { get; set; }

        /// <summary>
        /// Gets or sets the FTP server address.
        /// </summary>
        /// <value>The FTP server address.</value>
        public string FtpAddress { get; set; }

        /// <summary>
        /// Gets or sets the name of the FTP server user.
        /// </summary>
        /// <value>The name of the FTP user.</value>
        public string FtpUserName { get; set; }

        /// <summary>
        /// Gets or sets the FTP server password.
        /// </summary>
        /// <value>The FTP server password.</value>
        public string FtpPassword { get; set; }

        /// <summary>
        /// Gets or sets the iTunes playlist.
        /// </summary>
        /// <value>The iTunes playlist.</value>
        public string ITunesPlaylist { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether scheduling is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if scheduling is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EnableScheduling { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSetting"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public JobSetting(string name)
            : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobSetting"/> class.
        /// </summary>
        public JobSetting()
        {
            this.IsIncluded = true;
        }

        /// <summary>
        /// Creates a job from this job setting.
        /// </summary>
        /// <returns>A job that is created from this job setting.</returns>
        public Job CreateJob()
        {
            switch (this.SyncMode)
            {
                case SyncMode.LocalBackup:
                    {
                        var source = new LocalDirectoryInfo(new DirectoryInfo(this.DirectoryA));
                        var target = new LocalDirectoryInfo(new DirectoryInfo(this.DirectoryB));

                        return new LocalBackupJob(this.Name, source, target);
                    }

                case SyncMode.LocalSynchronization:
                    {
                        var directoryA = new LocalDirectoryInfo(new DirectoryInfo(this.DirectoryA));
                        var directoryB = new LocalDirectoryInfo(new DirectoryInfo(this.DirectoryB));

                        return new LocalSyncJob(this.Name, directoryA, directoryB);
                    }

                case SyncMode.ITunes:
                    {
                        var source = new ITunesDirectoryInfo(this.ITunesPlaylist);
                        var target = new LocalDirectoryInfo(new DirectoryInfo(this.DirectoryB));

                        return new ITunesJob(this.Name, source, target);
                    }

                case SyncMode.FtpBackup:
                    {
                        var source = new LocalDirectoryInfo(new DirectoryInfo(this.DirectoryA));

                        var client = new FtpClient(new NetworkCredential(this.FtpUserName, this.FtpPassword));
                        var target = new FlagSync.Core.FileSystem.Ftp.FtpDirectoryInfo(this.FtpAddress, client);

                        return new FtpBackupJob(this.Name, source, target, new Uri(this.FtpAddress), this.FtpUserName, this.FtpPassword);
                    }

                case SyncMode.FtpSynchronization:
                    {
                        var directoryA = new LocalDirectoryInfo(new DirectoryInfo(this.DirectoryA));

                        var client = new FtpClient(new NetworkCredential(this.FtpUserName, this.FtpPassword));
                        var directoryB = new FlagSync.Core.FileSystem.Ftp.FtpDirectoryInfo(this.FtpAddress, client);

                        return new FtpBackupJob(this.Name, directoryA, directoryB, new Uri(this.FtpAddress), this.FtpUserName, this.FtpPassword);
                    }
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}