using System.IO;

namespace FlagSync.Core
{
    class SyncJob : Job
    {
        public SyncJob(JobSettings settings, bool preview)
            : base(settings, preview)
        {

        }

        /// <summary>
        /// Copies new and modified files from directory A to directory B and then switches the direction
        /// </summary>
        public override void Start()
        {
            //Backup directoryA to directoryB and then otherwise
            this.BackupDirectories(new DirectoryInfo(this.Settings.DirectoryA), new DirectoryInfo(this.Settings.DirectoryB), this.Preview);
            this.BackupDirectories(new DirectoryInfo(this.Settings.DirectoryB), new DirectoryInfo(this.Settings.DirectoryA), this.Preview);

            this.OnFinished();
        }
    }
}
