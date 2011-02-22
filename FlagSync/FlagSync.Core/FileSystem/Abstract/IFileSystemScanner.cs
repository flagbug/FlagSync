using System;

namespace FlagSync.Core.FileSystem.Abstract
{
    public interface IFileSystemScanner
    {
        /// <summary>
        /// Occurs when a file has been found.
        /// </summary>
        event EventHandler<FileFoundEventArgs> FileFound;

        /// <summary>
        /// Occurs when a directory has been found.
        /// </summary>
        event EventHandler<DirectoryFoundEventArgs> DirectoryFound;

        /// <summary>
        /// Occurs when a directory has been proceeded.
        /// </summary>
        event EventHandler DirectoryProceeded;

        /// <summary>
        /// Stops the scanner.
        /// </summary>
        void Stop();

        /// <summary>
        /// Starts the scanner.
        /// </summary>
        void Start();
    }
}