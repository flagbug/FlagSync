using System;
using System.IO;
using System.Net;
using FlagSync.Core.FileSystem.Ftp;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    internal class FtpBackupJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpBackupJob"/> class.
        /// </summary>
        /// <param name="setting">The setting.</param>
        public FtpBackupJob(JobSetting setting) :
            base(new LocalFileSystem(),
             new FtpFileSystem(new Uri(setting.FtpAddress),
                 new NetworkCredential(setting.FtpUserName, setting.FtpPassword)))
        { }

        /// <summary>
        /// Starts the job.
        /// </summary>
        /// <param name="preview">if set to <c>true</c>, a preview will be performed.</param>
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(
                new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryB)),
                new FtpDirectoryInfo(this.Settings.FtpAddress,
                    new FlagFtp.FtpClient(new NetworkCredential(this.Settings.FtpUserName, this.Settings.FtpPassword))), !preview);

            this.CheckDeletionsRecursively(
                new FtpDirectoryInfo(this.Settings.FtpAddress,
                    new FlagFtp.FtpClient(new NetworkCredential(this.Settings.FtpUserName, this.Settings.FtpPassword))),
                    new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryB)), !preview);

            this.OnFinished(EventArgs.Empty);
        }
    }
}