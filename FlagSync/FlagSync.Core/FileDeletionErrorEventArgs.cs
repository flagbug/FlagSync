using System;
using Rareform.Extensions;
using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core
{
    /// <summary>
    /// Provides data for the events of the <see cref="FlagSync.Core.Job"/> and the <see cref="FlagSync.Core.JobWorker"/> class.
    /// </summary>
    public class FileDeletionErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public IFileInfo File { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDeletionErrorEventArgs"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public FileDeletionErrorEventArgs(IFileInfo file)
        {
            file.ThrowIfNull(() => file);

            this.File = file;
        }
    }
}