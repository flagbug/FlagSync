using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core
{
    public class SyncJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncJob"/> class.
        /// </summary>
        /// <param name="name">The name of the job.</param>
        /// <param name="fileSystemA">The file system A.</param>
        /// <param name="fileSystemB">The file system B.</param>
        /// <param name="directoryA">The directory A.</param>
        /// <param name="directoryB">The directory B.</param>
        public SyncJob(string name, IFileSystem fileSystemA, IFileSystem fileSystemB, IDirectoryInfo directoryA, IDirectoryInfo directoryB)
            : base(name, fileSystemA, fileSystemB, directoryA, directoryB) { }

        /// <summary>
        /// Starts the job, opies new and modified files from directory A to directory B and then switches the direction.
        /// </summary>
        /// <param name="preview">if set to <c>true</c> a preview will be performed.</param>
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(this.DirectoryA, this.DirectoryB, !preview);
            this.BackupDirectoryRecursively(this.DirectoryB, this.DirectoryA, !preview);

            this.OnFinished(System.EventArgs.Empty);
        }
    }
}