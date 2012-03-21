using System;
using System.Net;
using FlagFtp;
using FlagSync.Core.FileSystem.Base;
using FlagSync.Core.FileSystem.Ftp;
using FtpDirectoryInfo = FlagSync.Core.FileSystem.Ftp.FtpDirectoryInfo;

namespace FlagSync.Data
{
    public class FtpFileSystemSetting : FileSystemSetting
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public override IFileSystem GetFileSystem()
        {
            return new FtpFileSystem(new Uri(this.Source), new NetworkCredential(this.Username, this.Password));
        }

        public override IDirectoryInfo GetRootDirectory()
        {
            return new FtpDirectoryInfo(this.Source, new FtpClient(new NetworkCredential(this.Username, this.Password)));
        }
    }
}