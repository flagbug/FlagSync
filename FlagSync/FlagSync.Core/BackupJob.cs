using System;

namespace FlagSync.Core
{
    /// <summary>
    /// A backup-job performs a synchronization only from directory A to directory B, but can check on deleted files
    /// </summary>
    internal class BackupJob : FileSystemJob
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupJob"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="preview">if set to <c>true</c>, no files will be deleted, copied or modified).</param>
        public BackupJob(JobSetting settings)
            : base(settings) { }

        /// <summary>
        /// Starts the BackupJob, copies new and modified files from directory A to directory B and finally checks for deletions
        /// </summary>*/
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(Settings.DirectoryA, Settings.DirectoryB, !preview);
            this.CheckDeletionsRecursively(Settings.DirectoryB, Settings.DirectoryA, !preview);
            this.OnFinished(EventArgs.Empty);
        }
    }
}