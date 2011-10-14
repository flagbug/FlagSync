using System;
using System.IO;
using FlagLib.IO;

namespace FlagSync.Core.FileSystem.Base
{
    /// <summary>
    /// Provides the interface which all file system must implement.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        event EventHandler<DataTransferEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Tries to delete a file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>
        ///   <c>true</c>, if the deletion has succeed; otherwise, <c>false</c>.
        /// </returns>
        bool TryDeleteFile(IFileInfo file);

        /// <summary>
        /// Tries to create a directory in the specified directory.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>
        ///   <c>true</c>, if the creation has succeed; otherwise, <c>false</c>.
        /// </returns>
        bool TryCreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory);

        /// <summary>
        /// Tries to delete a directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <returns>
        ///   <c>true</c>, if the deletion has succeed; otherwise, <c>false</c>.
        /// </returns>
        bool TryDeleteDirectory(IDirectoryInfo directory);

        /// <summary>
        /// Tries to copy a file to specified directory.
        /// </summary>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>
        ///   <c>true</c>, if the copy operation has succeed; otherwise, <c>false</c>.
        /// </returns>
        bool TryCopyFile(IFileSystem sourceFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory);

        /// <summary>
        /// Gets the file info at the specified path.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>
        /// An <see cref="IFileInfo"/> of the file from the specified path.
        /// </returns>
        IFileInfo GetFileInfo(string path);

        /// <summary>
        /// Gets the directory info at the specified path.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>
        /// An <see cref="IDirectoryInfo"/> of the directory from the specified path.
        /// </returns>
        IDirectoryInfo GetDirectoryInfo(string path);

        /// <summary>
        /// Determines if the file at the specified path exists.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>
        ///   <c>true</c>, if the file exists; otherwise, <c>false</c>.
        /// </returns>
        bool FileExists(string path);

        /// <summary>
        /// Determines if the directory at the specified path exists.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>
        ///   <c>true</c>, if the directory exists; otherwise, <c>false</c>.
        /// </returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Opens the stream of the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// A stream from the specified file.
        /// </returns>
        Stream OpenFileStream(IFileInfo file);

        /// <summary>
        /// Combines two paths for the specific file system.
        /// </summary>
        /// <param name="path1">The first path.</param>
        /// <param name="path2">The second path.</param>
        /// <returns>
        /// A path, which is the combination of the first and second path.
        /// </returns>
        string CombinePath(string path1, string path2);
    }
}