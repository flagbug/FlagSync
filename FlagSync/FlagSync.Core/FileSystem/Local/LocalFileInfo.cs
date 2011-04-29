using System;
using System.IO;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Local
{
    internal class LocalFileInfo : IFileInfo
    {
        private FileInfo fileInfo;

        /// <summary>
        /// Gets the last write time.
        /// </summary>
        /// <value>The last write time.</value>
        public DateTime LastWriteTime
        {
            get { return this.fileInfo.LastWriteTime; }
        }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>The length of the file.</value>
        public long Length
        {
            get { return this.fileInfo.Length; }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get { return this.fileInfo.FullName; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return this.fileInfo.Name; }
        }

        /// <summary>
        /// Gets the directory of the file.
        /// </summary>
        /// <value>The directory of the file.</value>
        public IDirectoryInfo Directory
        {
            get { return new LocalDirectoryInfo(this.fileInfo.Directory); }
        }

        /// <summary>
        /// Gets a value indicating whether the file exists.
        /// </summary>
        /// <value>true if the file exists; otherwise, false.</value>
        public bool Exists
        {
            get { return this.fileInfo.Exists; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFileInfo"/> class.
        /// </summary>
        /// <param name="fileInfo">The file info to wrap.</param>
        public LocalFileInfo(FileInfo fileInfo)
        {
            if (fileInfo == null)
                throw new ArgumentNullException("fileInfo");

            this.fileInfo = fileInfo;
        }
    }
}