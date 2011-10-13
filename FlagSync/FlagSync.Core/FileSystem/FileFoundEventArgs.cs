using System;
using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core.FileSystem
{
    /// <summary>
    /// Provides data for the events of the <see cref="FlagSync.Core.FileSystemScanner"/> class.
    /// </summary>
    public class FileFoundEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the found file.
        /// </summary>
        /// <value>The file.</value>
        public IFileInfo File { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileEventArgs"/> class.
        /// </summary>
        /// <param name="file">The found file.</param>
        public FileFoundEventArgs(IFileInfo file)
        {
            this.File = file;
        }
    }
}