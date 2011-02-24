using System;
using System.Collections.Generic;
using System.IO;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.Test.VirtualFileSystem
{
    class VirtualDirectoryInfo : IDirectoryInfo
    {
        private List<VirtualFileInfo> files;
        private List<VirtualDirectoryInfo> directories;

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
                throw new ArgumentNullException("path");

            string path = Path.Combine(parentDirectory.FullName, name);

            //This checks wether the path is valid and throws an exception if not
            DirectoryInfo directory = new DirectoryInfo(path);

            this.FullName = path;
            this.Parent = parentDirectory;
            this.IsLocked = isLocked;
            this.Exists = exists;

            this.files = new List<VirtualFileInfo>();
            this.directories = new List<VirtualDirectoryInfo>();
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
            if (this.IsLocked)
                throw new UnauthorizedAccessException("The directory is locked.");

            return this.files.AsReadOnly();
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
            if (this.IsLocked)
                throw new UnauthorizedAccessException("The directory is locked.");

            return this.directories.AsReadOnly();
        }
    }
}