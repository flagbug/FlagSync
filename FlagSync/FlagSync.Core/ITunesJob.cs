using System;
using FlagSync.Core.FileSystem.ITunes;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    public class ITunesJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ITunesJob"/> class.
        /// </summary>
        /// <param name="name">The name of the job.</param>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        public ITunesJob(string name, ITunesDirectoryInfo sourceDirectory, LocalDirectoryInfo targetDirectory)
            : base(name, new ITunesFileSystem(), new LocalFileSystem(), sourceDirectory, targetDirectory) { }

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