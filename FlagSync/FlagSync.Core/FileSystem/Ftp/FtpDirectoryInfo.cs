using System;
using System.Collections.Generic;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Ftp
{
    class FtpDirectoryInfo : IDirectoryInfo
    {
        private FtpConnectionInfo connectionInfo;

        public string FullName { get; private set; }

        public string Name { get; private set; }

        public IDirectoryInfo Parent { get; private set; }

        public bool Exists { get; private set; }

        public IEnumerable<IFileInfo> GetFiles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDirectoryInfo> GetDirectories()
        {
            throw new NotImplementedException();
        }

        public FtpDirectoryInfo(FtpConnectionInfo connectionInfo)
        {
            if (connectionInfo == null)
                throw new ArgumentNullException("connectionInfo");

            this.connectionInfo = connectionInfo;
        }
    }
}