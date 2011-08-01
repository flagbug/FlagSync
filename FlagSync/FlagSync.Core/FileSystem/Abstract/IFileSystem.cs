using System;
using System.IO;
using FlagLib.FileSystem;

namespace FlagSync.Core.FileSystem.Abstract
{
    /// <summary>
    /// The base interface for all file systems
    /// </summary>
    /// <remarks></remarks>
    internal interface IFileSystem
    {
        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        /// <remarks></remarks>
        event EventHandler<CopyProgressEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Tries to delete a file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>A value indicating whether the file deletion has succeed</returns>
        /// <remarks></remarks>
        bool TryDeleteFile(IFileInfo file);

        /// <summary>
        /// Tries to create a directory in the specified directory (low level operation).
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>Returns a value indicating whether the directory creation has succeed</returns>
        /// <remarks></remarks>
        bool TryCreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory);

        /// <summary>
        /// Tries to delete a directory (low level operation).
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <returns>A value indicating whether the deletion has succeed.</returns>
        /// <remarks></remarks>
        bool TryDeleteDirectory(IDirectoryInfo directory);

        /// <summary>
        /// Tries to copy a file to specified directory (low level operation).
        /// </summary>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>
        /// True, if the copy operation has succeed; otherwise, false
        /// </returns>
        bool TryCopyFile(IFileSystem sourceFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory);

        /// <summary>
        /// Gets the file info at the specified path.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        IFileInfo GetFileInfo(string path);

        /// <summary>
        /// Gets the directory info at the specified path.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        IDirectoryInfo GetDirectoryInfo(string path);

        /// <summary>
        /// Checks if the file at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True, if the file exists; otherwise, false</returns>
        /// <remarks></remarks>
        bool FileExists(string path);

        /// <summary>
        /// Checks if the directory at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True, if the directory exists; otherwise, false</returns>
        /// <remarks></remarks>
        bool DirectoryExists(string path);

        /// <summary>
        /// Opens the stream of the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        Stream OpenFileStream(IFileInfo file);
    }
}