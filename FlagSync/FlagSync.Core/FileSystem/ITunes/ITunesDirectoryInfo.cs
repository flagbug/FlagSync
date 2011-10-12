using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FlagLib.Extensions;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.ITunes
{
    /// <summary>
    /// Represents a directory in the iTunes filesystem.
    /// </summary>
    /// <remarks>
    /// A directory in the iTunes filesystem is usually
    /// the representation of an artist or an album of a song.
    /// </remarks>
    [DebuggerDisplay("{FullName}")]
    internal class ITunesDirectoryInfo : IDirectoryInfo
    {
        private string name;
        private bool isRoot;
        private IEnumerable<ITunesDirectoryInfo> directories;
        private IEnumerable<IFileInfo> files;

        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        /// <value>The parent directory.</value>
        /// <remarks></remarks>
        public IDirectoryInfo Parent { get; set; }

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        /// <value><c>true</c> if the directory exists; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool Exists { get; private set; }

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
            return this.files;
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
            if (this.isRoot)
            {
                return ITunesFileSystem.MapPlaylistToDirectoryStructure(this.name)
                    .Cast<IDirectoryInfo>();
            }

            return this.directories.Cast<IDirectoryInfo>();
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <remarks></remarks>
        public string FullName
        {
            get
            {
                if (this.isRoot)
                {
                    return this.name;
                }

                else
                {
                    return Path.Combine(this.Parent.FullName, this.Name);
                }
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ITunesDirectoryInfo"/> class.
        /// The directory will be the root directory an a playlist.
        /// </summary>
        /// <param name="playlistName">The name of the playlist.</param>
        public ITunesDirectoryInfo(string playlistName)
            : this((ITunesDirectoryInfo)null)
        {
            playlistName.ThrowIfNull(() => playlistName);

            this.name = playlistName;
            this.isRoot = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ITunesDirectoryInfo"/> class.
        /// The directory will be the last level of the directory structure and contains the files.
        /// </summary>
        /// <param name="name">The name of the album.</param>
        /// <param name="files">The files that are contained in this directory.</param>
        public ITunesDirectoryInfo(string albumName, IEnumerable<IFileInfo> files, ITunesDirectoryInfo parent)
            : this(parent)
        {
            albumName.ThrowIfNull(() => albumName);
            files.ThrowIfNull(() => files);

            this.name = albumName;
            this.files = files;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ITunesDirectoryInfo"/> class.
        /// The directory will be the middle level of the directory structure and contains the album directories.
        /// </summary>
        /// <param name="name">The name of the artist.</param>
        /// <param name="directories">The directories that are contained in this directory.</param>
        public ITunesDirectoryInfo(string artistName, IEnumerable<ITunesDirectoryInfo> directories, ITunesDirectoryInfo parent)
            : this(parent)
        {
            artistName.ThrowIfNull(() => artistName);
            directories.ThrowIfNull(() => directories);

            this.name = artistName;
            this.directories = directories;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ITunesDirectoryInfo"/> class from being created.
        /// </summary>
        private ITunesDirectoryInfo(ITunesDirectoryInfo parent)
        {
            this.Parent = parent;
            this.files = Enumerable.Empty<IFileInfo>();
            this.directories = Enumerable.Empty<ITunesDirectoryInfo>();
            this.Exists = true;
        }

        /// <summary>
        /// Creates an iTunes directory info which does not exist.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parent">The parent directory.</param>
        /// <returns></returns>
        public static ITunesDirectoryInfo CreateNonExistantDirectoryInfo(string name, ITunesDirectoryInfo parent)
        {
            name.ThrowIfNull(() => name);
            parent.ThrowIfNull(() => parent);

            ITunesDirectoryInfo directory = new ITunesDirectoryInfo(parent);
            directory.name = name;
            directory.Exists = false;

            return directory;
        }
    }
}