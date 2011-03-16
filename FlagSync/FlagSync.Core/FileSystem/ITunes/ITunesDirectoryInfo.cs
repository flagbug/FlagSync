using System.Collections.Generic;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.ITunes
{
    class ITunesDirectoryInfo : IDirectoryInfo
    {
        public IDirectoryInfo Parent
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Exists
        {
            get { throw new System.NotImplementedException(); }
        }

        public IEnumerable<IFileInfo> GetFiles()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IDirectoryInfo> GetDirectories()
        {
            throw new System.NotImplementedException();
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