using System;

namespace FlagSync.Core.AbstractFileSystem
{
    public class DirectoryFoundEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the found the directory.
        /// </summary>
        /// <value>The found directory.</value>
        public IDirectoryInfo Directory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryFoundEventArgs"/> class.
        /// </summary>
        /// <param name="directory">The found directory.</param>
        public DirectoryFoundEventArgs(IDirectoryInfo directory)
        {
            this.Directory = directory;
        }
    }
}