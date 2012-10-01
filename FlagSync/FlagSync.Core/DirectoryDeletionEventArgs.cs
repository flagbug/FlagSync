using System;
using Rareform.Extensions;

namespace FlagSync.Core
{
    /// <summary>
    /// Provides data for the events of the <see cref="FlagSync.Core.Job"/> and the <see cref="FlagSync.Core.JobWorker"/> class.
    /// </summary>
    public class DirectoryDeletionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the directory path.
        /// </summary>
        /// <value>
        /// The directory path.
        /// </value>
        public string DirectoryPath { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryDeletionEventArgs"/> class.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        public DirectoryDeletionEventArgs(string directoryPath)
        {
            directoryPath.ThrowIfNull(() => directoryPath);

            this.DirectoryPath = directoryPath;
        }
    }
}