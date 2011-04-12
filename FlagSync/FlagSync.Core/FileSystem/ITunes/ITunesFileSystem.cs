using System;
using FlagLib.FileSystem;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.ITunes
{
    internal class ITunesFileSystem : IFileSystem
    {
        private string playlist;

        public event EventHandler<CopyProgressEventArgs> FileCopyProgressChanged;

        public bool TryDeleteFile(IFileInfo file)
        {
            throw new NotSupportedException();
        }

        public bool TryCreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
        {
            throw new NotSupportedException();
        }

        public bool TryDeleteDirectory(IDirectoryInfo directory)
        {
            throw new NotSupportedException();
        }

        public bool TryCopyFile(IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            throw new NotSupportedException();
        }

        public IFileInfo GetFileInfo(string path)
        {
            throw new NotImplementedException();
        }

        public IDirectoryInfo GetDirectoryInfo(string path)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string path)
        {
            throw new NotSupportedException();
        }

        public bool DirectoryExists(string path)
        {
            throw new NotSupportedException();
        }

        public ITunesFileSystem(string playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");

            if (playlist == string.Empty)
                throw new ArgumentException("The playlist name cannot be empty", "playlist");

            this.playlist = playlist;
        }
    }
}