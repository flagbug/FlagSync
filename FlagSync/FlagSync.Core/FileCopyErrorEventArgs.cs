using System;
using Rareform.Extensions;
using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core
{
    /// <summary>
    /// Provides data for the events of the <see cref="FlagSync.Core.Job"/> and the <see cref="FlagSync.Core.JobWorker"/> class.
    /// </summary>
    public class FileCopyErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public IFileInfo File { get; private set; }

        /// <summary>
        /// Gets the target directory.
        /// </summary>
        /// <value>
        /// The target directory.
        /// </value>
        public IDirectoryInfo TargetDirectory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCopyErrorEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        public FileCopyErrorEventArgs(IFileInfo file, IDirectoryInfo targetDirectory)
        {
            file.ThrowIfNull(() => file);
            targetDirectory.ThrowIfNull(() => targetDirectory);

            this.File = file;
            this.TargetDirectory = targetDirectory;
        }
    }
}