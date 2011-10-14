using System;

namespace FlagSync.Core.FileSystem.Base
{
    /// <summary>
    /// Provides the interface which all file infos must implement.
    /// </summary>
    public interface IFileInfo : IFileSystemInfo
    {
        /// <summary>
        /// Gets the last write time.
        /// </summary>
        DateTime LastWriteTime { get; }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Gets the directory.
        /// </summary>
        IDirectoryInfo Directory { get; }

        /// <summary>
        /// Gets a value indicating whether the file exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exists; otherwise, <c>false</c>.
        /// </value>
        bool Exists { get; }
    }
}