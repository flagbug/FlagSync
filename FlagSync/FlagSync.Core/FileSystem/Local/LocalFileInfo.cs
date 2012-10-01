using System;
using System.Diagnostics;
using System.IO;
using Rareform.Extensions;
using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core.FileSystem.Local
{
    /// <summary>
    /// Represents a file on the local file system.
    /// </summary>
    /// <remarks>
    /// The <see cref="LocalFileInfo"/> class is basically a wrapper around
    /// the <see cref="System.IO.FileInfo"/> class and abstracts it to be
    /// used as an <see cref="IFileInfo"/>.
    /// </remarks>
    [DebuggerDisplay("{FullName}")]
    public class LocalFileInfo : IFileInfo
    {
        private readonly FileInfo fileInfo;

        /// <summary>
        /// Gets the last write time.
        /// </summary>
        public DateTime LastWriteTime
        {
            get { return this.fileInfo.LastWriteTime; }
        }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        public long Length
        {
            get { return this.fileInfo.Length; }
        }

        /// <summary>
        /// Gets the full name of the file.
        /// </summary>
        public string FullName
        {
            get { return this.fileInfo.FullName; }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name
        {
            get { return this.fileInfo.Name; }
        }

        /// <summary>
        /// Gets the directory.
        /// </summary>
        public IDirectoryInfo Directory
        {
            get { return new LocalDirectoryInfo(this.fileInfo.Directory); }
        }

        /// <summary>
        /// Gets a value indicating whether the file exists.
        /// </summary>
        /// <value>
        /// true if the file exists; otherwise, false.
        /// </value>
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
            fileInfo.ThrowIfNull(() => fileInfo);

            this.fileInfo = fileInfo;
        }
    }
}