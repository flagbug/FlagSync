using System;
using System.IO;

namespace FlagSync.Core.FileSystem.Abstract
{
    public interface IFileInfo : IFileSystemInfo
    {
        /// <summary>
        /// Gets the last write time.
        /// </summary>
        /// <value>The last write time.</value>
        DateTime LastWriteTime { get; }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>The length of the file.</value>
        long Length { get; }

        /// <summary>
        /// Gets the directory of the file.
        /// </summary>
        /// <value>The directory of the file.</value>
        IDirectoryInfo Directory { get; }

        /// <summary>
        /// Gets a value indicating whether the file exists.
        /// </summary>
        /// <value>true if the file exists; otherwise, false.</value>
        bool Exists { get; }

        /// <summary>
        /// Opens the stream of the file.
        /// </summary>
        /// <returns>The stream of the file.</returns>
        FileStream Open();
    }
}