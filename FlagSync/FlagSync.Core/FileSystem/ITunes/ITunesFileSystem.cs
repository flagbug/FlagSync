using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlagLib.Extensions;
using FlagLib.IO;
using FlagLib.Reflection;
using FlagSync.Core.FileSystem.Abstract;
using FlagSync.Core.FileSystem.Local;
using iTunesLib;

namespace FlagSync.Core.FileSystem.ITunes
{
    /// <summary>
    /// Represents the file system of an iTunes playlist
    /// </summary>
    public class ITunesFileSystem : IFileSystem
    {
        private IEnumerable<ITunesDirectoryInfo> playlistStructureChache;

        /// <summary>
        /// Occurs when the file copy progress has changed.
        /// </summary>
        public event EventHandler<DataTransferEventArgs> FileCopyProgressChanged;

        /// <summary>
        /// Tries to delete a file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        /// <returns>
        ///   <c>true</c>, if the deletion has succeed; otherwise, <c>false</c>.
        /// </returns>
        public bool TryDeleteFile(IFileInfo file)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Tries to create a directory in the specified directory.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>
        ///   <c>true</c>, if the creation has succeed; otherwise, <c>false</c>.
        /// </returns>
        public bool TryCreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Tries to delete a directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <returns>
        ///   <c>true</c>, if the deletion has succeed; otherwise, <c>false</c>.
        /// </returns>
        public bool TryDeleteDirectory(IDirectoryInfo directory)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Tries to copy a file to specified directory.
        /// </summary>
        /// <param name="sourceFileSystem">The source file system.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <returns>
        ///   <c>true</c>, if the copy operation has succeed; otherwise, <c>false</c>.
        /// </returns>
        public bool TryCopyFile(IFileSystem sourceFileSystem, IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the file info at the specified path.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>
        /// An <see cref="IFileInfo"/> of the file from the specified path.
        /// </returns>
        public IFileInfo GetFileInfo(string path)
        {
            path.ThrowIfNull(() => path);

            string[] split = path.Split(Path.DirectorySeparatorChar);

            if (split.Length < 4)
                throw new ArgumentException("The path is not valid.", "path");

            string playlist = split[0];
            string artist = split[1];
            string album = split[2];
            string title = split[3];

            var root = this.GetPlaylistStructure(playlist);

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
        /// <returns>
        /// An <see cref="IDirectoryInfo"/> of the directory from the specified path.
        /// </returns>
        public IDirectoryInfo GetDirectoryInfo(string path)
        {
            path.ThrowIfNull(() => path);

            if (!this.DirectoryExists(path))
            {
                return ITunesDirectoryInfo
                    .CreateNonExistantDirectoryInfo(Path.GetFileName(path), (ITunesDirectoryInfo)this.GetDirectoryInfo(Path.GetDirectoryName(path)));
            }

            string[] split = path.Split(Path.DirectorySeparatorChar);

            string playlist = split[0];
            string artist = null;
            string album = null;

            if (split.Length > 1)
            {
                artist = split[1];

                if (split.Length > 2)
                {
                    album = split[2];
                }

                var root = this.GetPlaylistStructure(playlist);

                IDirectoryInfo directoryInfo = root.SingleOrDefault(dir => dir.Name == artist);

                if (directoryInfo == null)
                {
                    directoryInfo = ITunesDirectoryInfo
                        .CreateNonExistantDirectoryInfo(album ?? artist, (ITunesDirectoryInfo)this.GetDirectoryInfo(Path.GetDirectoryName(path)));
                }

                else if (album != null)
                {
                    directoryInfo = directoryInfo
                        .GetDirectories()
                        .Single(albumDir => albumDir.Name == album);
                }

                return directoryInfo;
            }

            else
            {
                return new ITunesDirectoryInfo(playlist);
            }
        }

        /// <summary>
        /// Determines if the file at the specified path exists.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>
        ///   <c>true</c>, if the file exists; otherwise, <c>false</c>.
        /// </returns>
        public bool FileExists(string path)
        {
            path.ThrowIfNull(() => path);

            string[] split = path.Split(Path.DirectorySeparatorChar);

            if (split.Length < 4)
                throw new ArgumentException("The path is not valid.", Reflector.GetMemberName(() => path));

            string playlist = split[0];
            string artist = split[1];
            string album = split[2];
            string title = split[3];

            var root = this.GetPlaylistStructure(playlist);

            var artistDirectory = root.SingleOrDefault(dir => dir.Name == artist);

            if (artistDirectory != null)
            {
                var albumDirectory = artistDirectory
                    .GetDirectories()
                    .SingleOrDefault(albumDir => albumDir.Name == album);

                if (albumDirectory != null)
                {
                    return albumDirectory
                        .GetFiles()
                        .Any(file => file.Name == title);
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if the directory at the specified path exists.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>
        ///   <c>true</c>, if the directory exists; otherwise, <c>false</c>.
        /// </returns>
        public bool DirectoryExists(string path)
        {
            path.ThrowIfNull(() => path);

            string[] split = path.Split(Path.DirectorySeparatorChar);

            //HACK: The case that the path is only the playlist name should also be checked
            if (split.Length == 1)
            {
                return true;
            }

            string playlist = split[0];
            string artist = split[1];

            string album = null;

            if (split.Length > 2)
            {
                album = split[2];
            }

            var root = this.GetPlaylistStructure(playlist);

            if (album == null)
            {
                return root.Any(artistDir => artistDir.Name == artist);
            }

            var artistDirectory = root.SingleOrDefault(artistDir => artistDir.Name == artist);

            if (artistDirectory != null)
            {
                return artistDirectory
                    .GetDirectories()
                    .Any(albumDir => albumDir.Name == album);
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Opens the stream of the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// A stream from the specified file.
        /// </returns>
        public Stream OpenFileStream(IFileInfo file)
        {
            file.ThrowIfNull(() => file);

            return File.Open(file.FullName, FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// Combines two paths for the specific file system.
        /// </summary>
        /// <param name="path1">The first path.</param>
        /// <param name="path2">The second path.</param>
        /// <returns>
        /// A path, which is the combination of the first and second path.
        /// </returns>
        public string CombinePath(string path1, string path2)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Maps the specified iTunes playlist to a directory structure.
        /// </summary>
        /// <param name="playlistName">The playlist.</param>
        /// <returns>A directory structure which represents the specified iTunes playlist</returns>
        public static IEnumerable<ITunesDirectoryInfo> MapPlaylistToDirectoryStructure(string playlistName)
        {
            if (String.IsNullOrEmpty(playlistName))
                throw new ArgumentException("The playlist name name cannot be null or empty", Reflector.GetMemberName(() => playlistName));

            ITunesDirectoryInfo root = new ITunesDirectoryInfo(playlistName);

            var files = new iTunesAppClass()
                .LibrarySource
                .Playlists
                .Cast<IITPlaylist>()
                .Single(pl => pl.Name == playlistName)
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
            artistOrAlbumName.ThrowIfNull(() => artistOrAlbumName);

            var invalidCharacters = Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars()).Distinct();

            foreach (char invalidCharacter in invalidCharacters)
            {
                artistOrAlbumName = artistOrAlbumName.Replace(invalidCharacter.ToString(), string.Empty);
            }

            return artistOrAlbumName;
        }

        /// <summary>
        /// Gets the structure of the specified playlist.
        /// </summary>
        /// <param name="playlistName">The playlist.</param>
        /// <returns></returns>
        private IEnumerable<ITunesDirectoryInfo> GetPlaylistStructure(string playlistName)
        {
            if (String.IsNullOrEmpty(playlistName))
                throw new ArgumentException("The playlist name cannot be empty", Reflector.GetMemberName(() => playlistName));

            if (this.playlistStructureChache == null)
            {
                this.playlistStructureChache = ITunesFileSystem.MapPlaylistToDirectoryStructure(playlistName);
            }

            return playlistStructureChache;
        }
    }
}