using System;
using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core
{
    /// <summary>
    /// A backup-job performs a synchronization only from directory A to directory B, but can delete files.
    /// </summary>
    public class BackupJob : Job
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupJob"/> class.
        /// </summary>
        /// <param name="name">The name of the job.</param>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="targetFileSystem">The target file system.</param>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        public BackupJob(string name, IFileSystem sourceFileSystem, IFileSystem targetFileSystem, IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
            : base(name, sourceFileSystem, targetFileSystem, sourceDirectory, targetDirectory) { }

        /// <summary>
        /// Starts the BackupJob, copies new and modified files from directory A to directory B and finally checks for deletions
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