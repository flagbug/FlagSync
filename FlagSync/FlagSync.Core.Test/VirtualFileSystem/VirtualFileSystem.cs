using System;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.Test.VirtualFileSystem
{
    class VirtualFileSystem : IFileSystem
    {
        private VirtualDirectoryInfo rootDirectory = new VirtualDirectoryInfo(@"root:\");

        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        public event EventHandler<FlagLib.FileSystem.CopyProgressEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Tries to delete a file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>
        /// A value indicating whether the file deletion has succeed
        /// </returns>
        public bool TryDeleteFile(IFileSystemInfo file)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries to create a directory in the specified directory (low level operation).
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>
        /// Returns a value indicating whether the directory creation has succeed
        /// </returns>
        public bool TryCreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries to delete a directory (low level operation).
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <returns>
        /// A value indicating whether the deletion has succeed.
        /// </returns>
        public bool TryDeleteDirectory(IDirectoryInfo directory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries to copy a file to specified directory (low level operation).
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns></returns>
        public bool TryCopyFile(IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new file info at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public IFileInfo CreateFileInfo(string path)
        {
            return new VirtualFileInfo(path, 0, DateTime.Now);
        }

        /// <summary>
        /// Creates a new directory info at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public IDirectoryInfo CreateDirectoryInfo(string path)
        {
            return new VirtualDirectoryInfo(path);
        }

        /// <summary>
        /// Creates a file scanner which searches in the specified directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IFileSystemScanner CreateFileSystemScanner(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the file counter which counts the files of a directory in the current file system.
        /// </summary>
        /// <returns></returns>
        public IFileCounter CreateFileCounter()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the file at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public bool FileExists(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the directory at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }
    }
}