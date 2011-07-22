using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlagSync.Core.FileSystem.Abstract;
using FlagSync.Core.FileSystem.Local;
using iTunesLib;

namespace FlagSync.Core.FileSystem.ITunes
{
    /// <summary>
    /// Represents an iTunes directory
    /// </summary>
    /// <remarks></remarks>
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
        public IDirectoryInfo Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        /// <value><c>true</c> if the directory exists; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool Exists
        {
            get { return true; }
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
                var files = new iTunesAppClass()
                    .LibrarySource
                    .Playlists
                    .Cast<IITPlaylist>()
                    .Single(playlist => playlist.Name == this.name)
                    .Tracks
                    .Cast<IITFileOrCDTrack>();

                var tracksByArtist = files
                    .GroupBy(file => file.Artist);

                List<ITunesDirectoryInfo> artistDirectories = new List<ITunesDirectoryInfo>();

                foreach (var artistGroup in tracksByArtist)
                {
                    var albumGroups = artistGroup.GroupBy(track => track.Album);

                    List<ITunesDirectoryInfo> albumDirectories = new List<ITunesDirectoryInfo>();

                    foreach (var album in albumGroups)
                    {
                        albumDirectories.Add
                            (
                                new ITunesDirectoryInfo
                                    (
                                        this.NormalizeArtistOrAlbumName(album.Key),
                                        album.Select(track => new LocalFileInfo(new FileInfo(track.Location))).Cast<IFileInfo>(),
                                        null
                                    )
                            );
                    }

                    ITunesDirectoryInfo artistDirectory = new ITunesDirectoryInfo(this.NormalizeArtistOrAlbumName(artistGroup.Key), albumDirectories, this);

                    foreach (ITunesDirectoryInfo albumDirectory in artistDirectory.GetDirectories())
                    {
                        albumDirectory.Parent = artistDirectory;
                    }

                    artistDirectories.Add(artistDirectory);
                }

                return artistDirectories.Cast<IDirectoryInfo>();
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
                    return Path.GetFullPath(this.Name);
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
            if (playlistName == null)
                throw new ArgumentNullException("playlistName");

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
            if (albumName == null)
                throw new ArgumentNullException("albumName");

            if (files == null)
                throw new ArgumentNullException("files");

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
            if (artistName == null)
                throw new ArgumentNullException("artistName");

            if (directories == null)
                throw new ArgumentNullException("directories");

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
        }

        /// <summary>
        /// Normalizes the name of the artist or album.
        /// </summary>
        /// <param name="artistOrAlbumName">Name of the artist or album.</param>
        /// <returns></returns>
        private string NormalizeArtistOrAlbumName(string artistOrAlbumName)
        {
            foreach (char invalidCharacter in Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars()).Distinct())
            {
                artistOrAlbumName = artistOrAlbumName.Replace(invalidCharacter.ToString(), string.Empty);
            }

            return artistOrAlbumName;
        }
    }
}