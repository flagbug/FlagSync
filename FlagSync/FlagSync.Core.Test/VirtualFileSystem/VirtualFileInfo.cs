using System;
using System.IO;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.Test.VirtualFileSystem
{
    class VirtualFileInfo : IFileInfo
    {
        /// <summary>
        /// Gets the last write time.
        /// </summary>
        /// <value>The last write time.</value>
        public DateTime LastWriteTime { get; private set; }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>The length of the file.</value>
        public long Length { get; private set; }

        /// <summary>
        /// Gets the directory of the file.
        /// </summary>
        /// <value>The directory of the file.</value>
        public IDirectoryInfo Directory
        {
            get { return new VirtualDirectoryInfo(Path.GetDirectoryName(this.FullName)); }
        }

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
        /// Initializes a new instance of the <see cref="VirtualFileInfo"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="length">The length.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        public VirtualFileInfo(string path, long length, DateTime lastWriteTime)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (lastWriteTime == null)
                throw new ArgumentNullException("lastWriteTime");

            //This checks wether the path is valid and throws an exception if not
            FileInfo directory = new FileInfo(path);

            this.FullName = path;
            this.Length = length;
            this.LastWriteTime = lastWriteTime;
        }
    }
}