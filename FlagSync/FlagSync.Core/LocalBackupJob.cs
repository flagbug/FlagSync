using System;
using System.IO;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    /// <summary>
    /// A backup-job performs a synchronization only from directory A to directory B, but can delete files
    /// </summary>
    /// <remarks></remarks>
    internal class LocalBackupJob : FileSystemJob
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalBackupJob"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <remarks></remarks>
        public LocalBackupJob(JobSetting settings)
            : base(settings, new LocalFileSystem(), new LocalFileSystem()) { }

        /// <summary>
        /// Starts the BackupJob, copies new and modified files from directory A to directory B and finally checks for deletions
        /// </summary>
        /// <param name="preview">if set to <c>true</c> a preview will be performed.</param>
        /// <remarks></remarks>
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryA)),
                new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryB)), !preview);

            this.CheckDeletionsRecursively(new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryB)),
                new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryA)), !preview);

            this.OnFinished(EventArgs.Empty);
        }
    }
}