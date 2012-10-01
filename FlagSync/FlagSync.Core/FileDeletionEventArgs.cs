using System;
using Rareform.Extensions;

namespace FlagSync.Core
{
    /// <summary>
    /// Provides data for the events of the <see cref="FlagSync.Core.Job"/> and the <see cref="FlagSync.Core.JobWorker"/> class.
    /// </summary>
    public class FileDeletionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public long FileSize { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDeletionEventArgs"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileSize">Size of the file.</param>
        public FileDeletionEventArgs(string filePath, long fileSize)
        {
            filePath.ThrowIfNull(() => filePath);

            this.FilePath = filePath;
            this.FileSize = fileSize;
        }
    }
}