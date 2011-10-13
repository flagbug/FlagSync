using System;
using System.Net;
using FlagSync.Core.FileSystem.Ftp;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    public class FtpSyncJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpSyncJob"/> class.
        /// </summary>
        /// <param name="name">The name of the job.</param>
        /// <param name="directoryA">The directory A.</param>
        /// <param name="directoryB">The directory B.</param>
        /// <param name="host">The host server address.</param>
        /// <param name="userName">The name of the user.</param>
        /// <param name="password">The password.</param>
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