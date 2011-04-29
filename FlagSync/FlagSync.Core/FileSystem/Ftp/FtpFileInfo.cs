using System;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Ftp
{
    internal class FtpFileInfo : IFileInfo
    {
        public DateTime LastWriteTime
        {
            get { throw new System.NotImplementedException(); }
        }

        public long Length
        {
            get { throw new System.NotImplementedException(); }
        }

        public IDirectoryInfo Directory
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Exists
        {
            get { throw new System.NotImplementedException(); }
        }

        public string FullName
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}