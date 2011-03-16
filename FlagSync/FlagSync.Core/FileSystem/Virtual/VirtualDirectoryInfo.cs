using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Virtual
{
    class VirtualDirectoryInfo : IDirectoryInfo
    {
        private List<IFileSystemInfo> fileSystemInfos;

        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        /// <value>The parent directory.</value>
        public IDirectoryInfo Parent { get; private set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return Path.GetFileName(this.FullName); }
        }

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        /// <value><c>true</c> if the directory exists; otherwise, <c>false</c>.</value>
        public bool Exists { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is locked.
        /// </summary>
        /// <value><c>true</c> if this instance is locked; otherwise, <c>false</c>.</value>
        public bool IsLocked { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualDirectoryInfo"/> class.
        /// </summary>
        /// <param name="path">The path of the virtual directory.</param>
        public VirtualDirectoryInfo(string name, VirtualDirectoryInfo parentDirectory, bool isLocked, bool exists)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            string path;

            if (parentDirectory != null)
            {
                path = Path.Combine(parentDirectory.FullName, name);
            }

            else
            {
                path = Path.GetFullPath(name);
            }

            //This checks wether the path is valid and throws an exception if not
            DirectoryInfo directory = new DirectoryInfo(path);

            this.FullName = path;
            this.Parent = parentDirectory;
            this.IsLocked = isLocked;
            this.Exists = exists;

            this.fileSystemInfos = new List<IFileSystemInfo>();
        }

        /// <summary>
        /// Return the files in the directory.
        /// </summary>
        /// <returns>The files in the directory</returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The exception that is thrown if the directory is locked
        /// </exception>
        public IEnumerable<IFileInfo> GetFiles()
        {
            return this.GetFiles(false);
        }

        /// <summary>
        /// Return the files in the directory.
        /// </summary>
        /// <param name="ignoreLock">if set to <c>true</c> ignore the lock that the directory has.</param>
        /// <returns>The files in the directory</returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The exception that is thrown if the directory is locked
        /// </exception>
        public IEnumerable<IFileInfo> GetFiles(bool ignoreLock)
        {
            if (this.IsLocked && !ignoreLock)
                throw new UnauthorizedAccessException("The directory is locked.");

            return this.fileSystemInfos.OfType<IFileInfo>();
        }

        /// <summary>
        /// Return the directories in the directory.
        /// </summary>
        /// <returns>The directories in the directory</returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The exception that is thrown if the directory is locked
        /// </exception>
        public IEnumerable<IDirectoryInfo> GetDirectories()
        {
            return this.GetDirectories(false);
        }

        /// <summary>
        /// Return the directories in the directory.
        /// </summary>
        /// <param name="ignoreLock">if set to <c>true</c> ignore the lock that the directory has.</param>
        /// <returns>The directories in the directory</returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The exception that is thrown if the directory is locked
        /// </exception>
        public IEnumerable<IDirectoryInfo> GetDirectories(bool ignoreLock)
        {
            if (this.IsLocked && !ignoreLock)
                throw new UnauthorizedAccessException("The directory is locked.");

            return this.fileSystemInfos.OfType<IDirectoryInfo>();
        }

        /// <summary>
        /// Registers the directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public void RegisterDirectory(VirtualDirectoryInfo directory)
        {
            if (!this.fileSystemInfos.Any(dir => dir.FullName == directory.FullName))
            {
                this.fileSystemInfos.Add(directory);
            }
        }

        /// <summary>
        /// Registers the file.
        /// </summary>
        /// <param name="directory">The file.</param>
        public void RegisterFile(VirtualFileInfo file)
        {
            if (!this.fileSystemInfos.Any(f => f.FullName == file.FullName))
            {
                this.fileSystemInfos.Add(file);
            }
        }

        /// <summary>
        /// Unregisters the directory.
        /// </summary>
        /// <param name="directory">The directory to unregister.</param>
        public void UnRegisterDirectory(VirtualDirectoryInfo directory)
        {
            this.fileSystemInfos.Remove(directory);
        }

        /// <summary>
        /// Unregisters the file.
        /// </summary>
        /// <param name="directory">The file to unregister.</param>
        public void UnRegisterFile(VirtualFileInfo file)
        {
            this.fileSystemInfos.Remove(file);
        }
    }
}