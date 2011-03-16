using System;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.ITunes
{
    class ITunesFileInfo : IFileInfo
    {
        public DateTime LastWriteTime
        {
            get { throw new NotImplementedException(); }
        }

        public long Length
        {
            get { throw new NotImplementedException(); }
        }

        public IDirectoryInfo Directory
        {
            get { throw new NotImplementedException(); }
        }

        public bool Exists
        {
            get { throw new NotImplementedException(); }
        }

        public string FullName
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }
    }
}