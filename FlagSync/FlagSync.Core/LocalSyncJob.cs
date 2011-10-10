using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    internal class LocalSyncJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalSyncJob"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <remarks></remarks>
        public LocalSyncJob(string name, LocalDirectoryInfo directoryA, LocalDirectoryInfo directoryB)
            : base(name, new LocalFileSystem(), new LocalFileSystem(), directoryA, directoryB) { }

        /// <summary>
        /// Starts the job, opies new and modified files from directory A to directory B and then switches the direction
        /// </summary>
        /// <param name="preview">if set to <c>true</c> [preview].</param>
        /// <remarks></remarks>
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(this.DirectoryA, this.DirectoryB, !preview);
            this.BackupDirectoryRecursively(this.DirectoryB, this.DirectoryA, !preview);

            this.OnFinished(System.EventArgs.Empty);
        }
    }
}