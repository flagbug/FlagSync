using System;
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
        public FtpSyncJob(string name, LocalDirectoryInfo directoryA, FtpDirectoryInfo directoryB, Uri host, string userName, string password) :
            base(name, new LocalFileSystem(), new FtpFileSystem(host, new NetworkCredential(userName, password)), directoryA, directoryB)
        { }

        /// <summary>
        /// Starts the job.
        /// </summary>
        /// <param name="preview">if set to <c>true</c>, a preview will be performed.</param>
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(this.DirectoryA, this.DirectoryB, !preview);
            this.BackupDirectoryRecursively(this.DirectoryB, this.DirectoryA, !preview);

            this.OnFinished(EventArgs.Empty);
        }
    }
}