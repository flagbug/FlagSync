using System;
using System.IO;
using System.Net;
using FlagSync.Core.FileSystem.Ftp;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    internal class FtpSyncJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpSyncJob"/> class.
        /// </summary>
        /// <param name="setting">The setting.</param>
        public FtpSyncJob(JobSetting setting) :
            base(new LocalFileSystem(), new FtpFileSystem(new Uri(setting.FtpAddress),
                 new NetworkCredential(setting.FtpUserName, setting.FtpPassword)))
        { }

        /// <summary>
        /// Starts the job.
        /// </summary>
        /// <param name="preview">if set to <c>true</c>, a preview will be performed.</param>
        public override void Start(bool preview)
        {
            LocalDirectoryInfo localDirectory = new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryB));
            FtpDirectoryInfo ftpDirectory = new FtpDirectoryInfo(this.Settings.FtpAddress,
                new FlagFtp.FtpClient(new NetworkCredential(this.Settings.FtpUserName, this.Settings.FtpPassword)));

            this.BackupDirectoryRecursively(localDirectory, ftpDirectory, !preview);
            this.BackupDirectoryRecursively(ftpDirectory, localDirectory, !preview);

            this.OnFinished(EventArgs.Empty);
        }
    }
}