using System;
using System.IO;
using FlagSync.Core.FileSystem.ITunes;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Core
{
    internal class ITunesJob : FileSystemJob
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ITunesJob"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <remarks></remarks>
        public ITunesJob(JobSetting settings)
            : base(settings, new ITunesFileSystem(settings.ITunesPlaylist), new LocalFileSystem()) { }

        /// <summary>
        /// Starts the iTunes job.
        /// </summary>
        /// <param name="preview">if set to <c>true</c> a preview will be performed.</param>
        /// <remarks></remarks>
        public override void Start(bool preview)
        {
            this.BackupDirectoryRecursively(new ITunesDirectoryInfo(this.Settings.ITunesPlaylist),
                new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryB)), !preview);

            this.CheckDeletionsRecursively(new LocalDirectoryInfo(new DirectoryInfo(this.Settings.DirectoryB)),
                new ITunesDirectoryInfo(this.Settings.ITunesPlaylist), !preview);

            this.OnFinished(EventArgs.Empty);
        }
    }
}