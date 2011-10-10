using System;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    /// <summary>
    /// A backup-job performs a synchronization only from directory A to directory B, but can delete files
    /// </summary>
    internal class LocalBackupJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalBackupJob"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public LocalBackupJob(LocalDirectoryInfo sourceDirectory, LocalDirectoryInfo targetDirectory)
            : base(new LocalFileSystem(), new LocalFileSystem(), sourceDirectory, targetDirectory) { }

        /// <summary>
        /// Starts the BackupJob, copies new and modified files from directory A to directory B and finally checks for deletions
        /// </summary>
        /// <param name="preview">if set to <c>true</c> a preview will be performed.</param>
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(this.DirectoryA, this.DirectoryB, !preview);

            this.CheckDeletionsRecursively(this.DirectoryB, this.DirectoryA, !preview);

            this.OnFinished(EventArgs.Empty);
        }
    }
}