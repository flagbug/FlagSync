using System;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.Test.VirtualFileSystem
{
    class VirtualFileSystemScanner : IFileSystemScanner
    {
        public event EventHandler<FileFoundEventArgs> FileFound;

        public event EventHandler<DirectoryFoundEventArgs> DirectoryFound;

        public event EventHandler DirectoryProceeded;

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}