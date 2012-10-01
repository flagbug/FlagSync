using System;
using Rareform.Extensions;
using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core
{
    /// <summary>
    /// Provides data for the events of the <see cref="FlagSync.Core.Job"/> and the <see cref="FlagSync.Core.JobWorker"/> class.
    /// </summary>
    public class DirectoryCreationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        public IDirectoryInfo Directory { get; private set; }

        /// <summary>
        /// Gets the target directory.
        /// </summary>
        /// <value>
        /// The target directory.
        /// </value>
        public IDirectoryInfo TargetDirectory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryCreationEventArgs"/> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        public DirectoryCreationEventArgs(IDirectoryInfo directory, IDirectoryInfo targetDirectory)
        {
            directory.ThrowIfNull(() => directory);
            targetDirectory.ThrowIfNull(() => targetDirectory);

            this.Directory = directory;
            this.TargetDirectory = targetDirectory;
        }
    }
}