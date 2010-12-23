namespace FlagSync.Core
{
    internal class SyncJob : FileSystemJob
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncJob"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="preview">if set to true no files will be deleted, mofified or copied.</param>
        public SyncJob(JobSetting settings)
            : base(settings) { }

        /// <summary>
        /// Starts the job, opies new and modified files from directory A to directory B and then switches the direction
        /// </summary>
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(Settings.DirectoryA, Settings.DirectoryB, !preview);
            this.BackupDirectoryRecursively(Settings.DirectoryB, Settings.DirectoryA, !preview);

            this.OnFinished(System.EventArgs.Empty);
        }
    }
}