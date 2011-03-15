using System;
using FlagLib.FileSystem;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Ftp
{
    class FtpFileSystem : IFileSystem
    {
        public event EventHandler<CopyProgressEventArgs> FileCopyProgressChanged;

        public bool TryDeleteFile(IFileInfo file)
        {
            throw new NotImplementedException();
        }

        public bool TryCreateDirectory(IDirectoryInfo sourceDirectory, IDirectoryInfo targetDirectory)
        {
            throw new NotImplementedException();
        }

        public bool TryDeleteDirectory(IDirectoryInfo directory)
        {
            throw new NotImplementedException();
        }

        public bool TryCopyFile(IFileInfo sourceFile, IDirectoryInfo targetDirectory)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }
    }
}