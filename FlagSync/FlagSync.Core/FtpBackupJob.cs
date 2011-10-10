using System;
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
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="host">The host.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        public FtpBackupJob(string name, LocalDirectoryInfo sourceDirectory, FtpDirectoryInfo targetDirectory, Uri host, string userName, string password) :
            base(name, new LocalFileSystem(), new FtpFileSystem(host, new NetworkCredential(userName, password)), sourceDirectory, targetDirectory)
        { }

        /// <summary>
        /// Starts the job.
        /// </summary>
        /// <param name="preview">if set to <c>true</c>, a preview will be performed.</param>
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(this.DirectoryA, this.DirectoryB, !preview);
            this.CheckDeletionsRecursively(this.DirectoryB, this.DirectoryA, !preview);

            this.OnFinished(EventArgs.Empty);
        }
    }
}