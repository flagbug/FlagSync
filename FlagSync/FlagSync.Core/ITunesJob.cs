using System;
using FlagSync.Core.FileSystem.ITunes;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    internal class ITunesJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ITunesJob"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <remarks></remarks>
        public ITunesJob(ITunesDirectoryInfo sourceDirectory, LocalDirectoryInfo targetDirectory)
            : base(new ITunesFileSystem(), new LocalFileSystem(), sourceDirectory, targetDirectory) { }

        /// <summary>
        /// Starts the iTunes job.
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