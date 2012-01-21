using System.Collections.Generic;

namespace FlagSync.Core.FileSystem.Base
{
    /// <summary>
    /// Provides the interface that all directory infos have to implement.
    /// </summary>
    public interface IDirectoryInfo : IFileSystemInfo
    {
        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        IDirectoryInfo Parent { get; }

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the directory exists; otherwise, <c>false</c>.
        /// </value>
        bool Exists { get; }

        /// <summary>
        /// Returns a list of all files in the directory.
        /// </summary>
        /// <returns>
        /// The files in the directory.
        /// </returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The exception that is thrown if the directory is locked
        ///   </exception>
        IEnumerable<IFileInfo> GetFiles();

        /// <summary>
        /// Returns a list of all directories in the directory.
        /// </summary>
        /// <returns>
        /// The directories in the directory.
        /// </returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The exception that is thrown if the directory is locked.
        ///   </exception>
        IEnumerable<IDirectoryInfo> GetDirectories();
    }
}