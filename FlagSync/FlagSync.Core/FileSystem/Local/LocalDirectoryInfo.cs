using System;
using System.IO;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Local
{
    public class LocalDirectoryInfo : IDirectoryInfo
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
    }
}