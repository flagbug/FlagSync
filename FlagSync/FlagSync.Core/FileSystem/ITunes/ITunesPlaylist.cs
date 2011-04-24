using System;
using System.Collections.Generic;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.ITunes
{
    /// <summary>
    /// Represents an iTunes playlist
    /// </summary>
    /// <remarks></remarks>
    internal class ITunesPlaylist : IDirectoryInfo
    {
        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        /// <value>The parent directory.</value>
        /// <remarks></remarks>
        public IDirectoryInfo Parent
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        /// <value><c>true</c> if the directory exists; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool Exists
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Return the files in the directory.
        /// </summary>
        /// <returns>The files in the directory</returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The exception that is thrown if the directory is locked
        ///   </exception>
        /// <remarks></remarks>
        public IEnumerable<IFileInfo> GetFiles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return the directories in the directory.
        /// </summary>
        /// <returns>The directories in the directory</returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The exception that is thrown if the directory is locked
        ///   </exception>
        /// <remarks></remarks>
        public IEnumerable<IDirectoryInfo> GetDirectories()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <remarks></remarks>
        public string FullName
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}