using System.IO;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    internal class SyncJob : FileSystemJob
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncJob"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <remarks></remarks>
        public SyncJob(JobSetting settings)
            : base(settings, new LocalFileSystem(), new LocalFileSystem()) { }

        /// <summary>
        /// Starts the job, opies new and modified files from directory A to directory B and then switches the direction
        /// </summary>
        /// <param name="preview">if set to <c>true</c> [preview].</param>
        /// <remarks></remarks>
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryA)),
                new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryB)), !preview);

            this.BackupDirectoryRecursively(new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryB)),
                new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryA)), !preview);

            this.OnFinished(System.EventArgs.Empty);
        }
    }
}