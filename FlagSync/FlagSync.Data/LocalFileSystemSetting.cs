using System.IO;
using FlagSync.Core.FileSystem.Base;
using FlagSync.Core.FileSystem.Local;

namespace FlagSync.Data
{
    public class LocalFileSystemSetting : FileSystemSetting
    {
        public override IFileSystem GetFileSystem()
        {
            return new LocalFileSystem();
        }

        public override IDirectoryInfo GetRootDirectory()
        {
            return new LocalDirectoryInfo(new DirectoryInfo(this.Source));
        }
    }
}