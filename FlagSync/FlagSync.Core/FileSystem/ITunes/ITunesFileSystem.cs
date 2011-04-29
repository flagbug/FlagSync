using System;
using System.IO;
using FlagLib.FileSystem;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.ITunes
{
    /// <summary>
    /// Represents the file system of an iTunes playlist
    /// </summary>
    /// <remarks></remarks>
    internal class ITunesFileSystem : IFileSystem
    {
        private string playlist;

        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler<CopyProgressEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Tries to delete a file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>A value indicating whether the file deletion has succeed</returns>
        /// <remarks></remarks>
        public bool TryDeleteFile(IFileInfo file)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Tries to create a directory in the specified directory (low level operation).
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>Returns a value indicating whether the directory creation has succeed</returns>
        /// <remarks></remarks>
        public bool TryCreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Tries to delete a directory (low level operation).
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <returns>A value indicating whether the deletion has succeed.</returns>
        /// <remarks></remarks>
        public bool TryDeleteDirectory(IDirectoryInfo directory)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Tries to copy a file to specified directory (low level operation).
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>True, if the copy operation has succeed; otherwise, false</returns>
        /// <remarks></remarks>
        public bool TryCopyFile(IFileSystem sourceFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the file info at the specified path.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public IFileInfo GetFileInfo(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the directory info at the specified path.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public IDirectoryInfo GetDirectoryInfo(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the file at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True, if the file exists; otherwise, false</returns>
        /// <remarks></remarks>
        public bool FileExists(string path)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Checks if the directory at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True, if the directory exists; otherwise, false</returns>
        /// <remarks></remarks>
        public bool DirectoryExists(string path)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ITunesFileSystem"/> class.
        /// </summary>
        /// <param name="playlist">The playlist.</param>
        /// <remarks></remarks>
        public ITunesFileSystem(string playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");

            if (playlist == string.Empty)
                throw new ArgumentException("The playlist name cannot be empty", "playlist");

            this.playlist = playlist;
        }

        public Stream OpenFileStream(IFileInfo file)
        {
            throw new NotSupportedException();
        }
    }
}