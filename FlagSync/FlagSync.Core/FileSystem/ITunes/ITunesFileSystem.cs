using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlagLib.FileSystem;
using FlagSync.Core.FileSystem.Abstract;
using FlagSync.Core.FileSystem.Local;
using iTunesLib;

namespace FlagSync.Core.FileSystem.ITunes
{
    /// <summary>
    /// Represents the file system of an iTunes playlist
    /// </summary>
    /// <remarks></remarks>
    internal class ITunesFileSystem : IFileSystem
    {
        private string playlist;

        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler<CopyProgressEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Tries to delete a file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>A value indicating whether the file deletion has succeed</returns>
        /// <remarks></remarks>
        public bool TryDeleteFile(IFileInfo file)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Tries to create a directory in the specified directory (low level operation).
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>Returns a value indicating whether the directory creation has succeed</returns>
        /// <remarks></remarks>
        public bool TryCreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Tries to delete a directory (low level operation).
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <returns>A value indicating whether the deletion has succeed.</returns>
        /// <remarks></remarks>
        public bool TryDeleteDirectory(IDirectoryInfo directory)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Tries to copy a file to specified directory (low level operation).
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>True, if the copy operation has succeed; otherwise, false</returns>
        /// <remarks></remarks>
        public bool TryCopyFile(IFileSystem sourceFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the file info at the specified path.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public IFileInfo GetFileInfo(string path)
        {
            string[] split = path.Split(Path.DirectorySeparatorChar);

            string playlist = split[0];
            string artist = split[1];
            string album = split[2];
            string title = split[3];

            var root = ITunesFileSystem.MapPlaylistToDirectoryStructure(playlist);

            IFileInfo fileInfo = root
                .Single(dir => dir.Name == artist)
                .GetDirectories()
                .Single(albumDir => albumDir.Name == album)
                .GetFiles()
                .Single(file => file.Name == title);

            return fileInfo;
        }

        /// <summary>
        /// Gets the directory info at the specified path.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public IDirectoryInfo GetDirectoryInfo(string path)
        {
            string[] split = path.Split(Path.DirectorySeparatorChar);

            string playlist = split[0];
            string artist = split[1];

            string album = null;

            if (split.Length > 2)
            {
                album = split[2];
            }

            var root = ITunesFileSystem.MapPlaylistToDirectoryStructure(playlist);

            IDirectoryInfo directoryInfo = root.Single(dir => dir.Name == artist);

            if (album != null)
            {
                directoryInfo = directoryInfo.GetDirectories().Single(albumDir => albumDir.Name == album);
            }

            return directoryInfo;
        }

        /// <summary>
        /// Checks if the file at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True, if the file exists; otherwise, false</returns>
        /// <remarks></remarks>
        public bool FileExists(string path)
        {
            string[] split = path.Split(Path.DirectorySeparatorChar);

            if (split.Length < 3)
                return false;

            string playlist = split[0];
            string artist = split[1];
            string album = split[2];
            string title = split[3];

            var root = ITunesFileSystem.MapPlaylistToDirectoryStructure(playlist);

            bool exists = root
                .Single(dir => dir.Name == artist)
                .GetDirectories()
                .Single(albumDir => albumDir.Name == album)
                .GetFiles()
                .Any(file => file.Name == title);

            return exists;
        }

        /// <summary>
        /// Checks if the directory at the specified path exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>True, if the directory exists; otherwise, false</returns>
        /// <remarks></remarks>
        public bool DirectoryExists(string path)
        {
            string[] split = path.Split(Path.DirectorySeparatorChar);

            string playlist = split[0];
            string artist = split[1];

            string album = null;

            if (split.Length > 3)
            {
                album = split[2];
            }

            var root = ITunesFileSystem.MapPlaylistToDirectoryStructure(playlist);

            if (album == null)
            {
                return root.Any(artistDir => artistDir.Name == artist);
            }

            return root
                .Single(artistDir => artistDir.Name == artist)
                .GetDirectories()
                .Any(albumDir => albumDir.Name == album);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ITunesFileSystem"/> class.
        /// </summary>
        /// <param name="playlist">The playlist.</param>
        /// <remarks></remarks>
        public ITunesFileSystem(string playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");

            if (playlist == string.Empty)
                throw new ArgumentException("The playlist name cannot be empty", "playlist");

            this.playlist = playlist;
        }

        /// <summary>
        /// Opens the stream of the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public Stream OpenFileStream(IFileInfo file)
        {
            return File.Open(file.FullName, FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// Maps the specified iTunes playlist to a directory structure.
        /// </summary>
        /// <param name="playlist">The playlist.</param>
        /// <returns>A directory structure which represents the specified iTunes playlist</returns>
        public static IEnumerable<ITunesDirectoryInfo> MapPlaylistToDirectoryStructure(string playlist)
        {
            ITunesDirectoryInfo root = new ITunesDirectoryInfo(playlist);

            var files = new iTunesAppClass()
                .LibrarySource
                .Playlists
                .Cast<IITPlaylist>()
                .Single(pl => pl.Name == playlist)
                .Tracks
                .Cast<IITFileOrCDTrack>();

            var tracksByArtist = files
                .GroupBy(file => file.Artist)
                .OrderBy(group => group.Key);

            List<ITunesDirectoryInfo> artistDirectories = new List<ITunesDirectoryInfo>();

            foreach (var artistGroup in tracksByArtist)
            {
                var albumGroups = artistGroup
                    .GroupBy(track => track.Album);

                string artistName = ITunesFileSystem.NormalizeArtistOrAlbumName(artistGroup.Key);

                List<ITunesDirectoryInfo> albumDirectories = new List<ITunesDirectoryInfo>();

                foreach (var album in albumGroups)
                {
                    string albumName = ITunesFileSystem.NormalizeArtistOrAlbumName(album.Key);
                    albumDirectories.Add
                        (
                            new ITunesDirectoryInfo
                                (
                                    albumName,
                                    album.Where(track => track.Location != null)
                                        .Select(track => new LocalFileInfo(new FileInfo(track.Location)))
                                        .Cast<IFileInfo>(),
                                    null
                                )
                        );
                }

                ITunesDirectoryInfo artistDirectory =
                    new ITunesDirectoryInfo(artistName, albumDirectories, root);

                foreach (ITunesDirectoryInfo albumDirectory in artistDirectory.GetDirectories())
                {
                    albumDirectory.Parent = artistDirectory;
                }

                artistDirectories.Add(artistDirectory);
            }

            return artistDirectories;
        }

        /// <summary>
        /// Normalizes the name of the artist or album.
        /// </summary>
        /// <param name="artistOrAlbumName">Name of the artist or album.</param>
        /// <returns>A normalized form of the artist or album string</returns>
        private static string NormalizeArtistOrAlbumName(string artistOrAlbumName)
        {
            var invalidCharacters = Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars()).Distinct();

            foreach (char invalidCharacter in invalidCharacters)
            {
                artistOrAlbumName = artistOrAlbumName.Replace(invalidCharacter.ToString(), string.Empty);
            }

            return artistOrAlbumName;
        }
    }
}