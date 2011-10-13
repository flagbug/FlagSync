using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    public class LocalSyncJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalSyncJob"/> class.
        /// </summary>
        /// <param name="name">The name of the job.</param>
        /// <param name="directoryA">The directory A.</param>
        /// <param name="directoryB">The directory B.</param>
        public LocalSyncJob(string name, LocalDirectoryInfo directoryA, LocalDirectoryInfo directoryB)
            : base(name, new LocalFileSystem(), new LocalFileSystem(), directoryA, directoryB) { }

        /// <summary>
        /// Starts the job, opies new and modified files from directory A to directory B and then switches the direction
        /// </summary>
        /// <param name="preview">if set to <c>true</c> [preview].</param>
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(this.DirectoryA, this.DirectoryB, !preview);
            this.BackupDirectoryRecursively(this.DirectoryB, this.DirectoryA, !preview);

            this.OnFinished(System.EventArgs.Empty);
        }
    }
}