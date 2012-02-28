using System;
using Rareform.Extensions;

namespace FlagSync.Core
{
    /// <summary>
    /// Provides data for the events of the <see cref="FlagSync.Core.Job"/> and the <see cref="FlagSync.Core.JobWorker"/> class.
    /// </summary>
    public class FileProceededEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>
        /// The length of the file.
        /// </value>
        public long FileLength { get; private set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FilePath { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProceededEventArgs"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="fileLength">The file length.</param>
        public FileProceededEventArgs(string filePath, long fileLength)
        {
            filePath.ThrowIfNull(() => filePath);

            this.FilePath = filePath;
            this.FileLength = fileLength;
        }
    }
}