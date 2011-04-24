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
        private string playlistName;
        private bool isRoot;
        private IEnumerable<ITunesDirectoryInfo> directories;
        private IEnumerable<LocalFileInfo> files;

        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        /// <value>The parent directory.</value>
        /// <remarks></remarks>
        public IDirectoryInfo Parent
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        /// <value><c>true</c> if the directory exists; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool Exists
        {
            get { throw new NotImplementedException(); }
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
            /*
            if (this.isRoot)
            {
                iTunesAppClass iTunes = new iTunesAppClass();

                var list = iTunes.LibrarySource.Playlists
                        .Cast<IITPlaylist>()
                        .Single(playlist => playlist.Name == this.playlistName);

                var tracks = list.Tracks.Cast<IITTrack>();

                List<ITunesDirectoryInfo> dirs = new List<ITunesDirectoryInfo>();

                var groupedByArtist = tracks.GroupBy(track => track.Artist);

                foreach (var group in groupedByArtist)
                {
                    var groupByAlbum = group.GroupBy(g => g.Album);

                    List<IDirectoryInfo> albumDirectories = new List<IDirectoryInfo>();

                    foreach (var album in groupByAlbum)
                    {
                        var trackFiles = album.Select(track => new LocalFileInfo(new FileInfo(((IITFileOrCDTrack)track).Location)));

                        albumDirectories.Add(new ITunesDirectoryInfo(null, false, new List<ITunesDirectoryInfo>(), trackFiles));
                    }
                }
            }

            else
            {
                return this.files.Cast<IFileInfo>();
            }
             * */

            return new iTunesAppClass()
                .LibrarySource
                .Playlists
                .Cast<IITPlaylist>()
                .Single(playlist => playlist.Name == this.playlistName)
                .Tracks
                .Cast<IITFileOrCDTrack>()
                .Select(track => new LocalFileInfo(new FileInfo(track.Location)))
                .Cast<IFileInfo>();
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
            return new List<IDirectoryInfo>();
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <remarks></remarks>
        public string FullName
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public ITunesDirectoryInfo(string playlistName)
        {
            this.playlistName = playlistName;
            this.isRoot = isRoot;
            this.directories = directories;
            this.files = files;
        }
    }
}