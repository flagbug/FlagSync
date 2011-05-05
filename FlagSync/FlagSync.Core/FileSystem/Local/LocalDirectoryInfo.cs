using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Local
{
    internal class LocalDirectoryInfo : IDirectoryInfo
    {
        private DirectoryInfo directoryInfo;

        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        /// <value>The parent directory.</value>
        public IDirectoryInfo Parent
        {
            get { return new LocalDirectoryInfo(this.directoryInfo.Parent); }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get { return this.directoryInfo.FullName; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return this.directoryInfo.Name; }
        }

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        /// <value><c>true</c> if the directory exists; otherwise, <c>false</c>.</value>
        public bool Exists
        {
            get { return this.directoryInfo.Exists; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDirectoryInfo"/> class.
        /// </summary>
        /// <param name="directoryInfo">The directory info to wrap.</param>
        public LocalDirectoryInfo(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException("directoryInfo");

            this.directoryInfo = directoryInfo;
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
            return this.directoryInfo.GetFiles().
                Select(file => (IFileInfo)new LocalFileInfo(file));
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
            return this.directoryInfo.GetDirectories().
                Select(directory => (IDirectoryInfo)new LocalDirectoryInfo(directory));
        }
    }
}