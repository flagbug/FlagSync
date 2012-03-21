using FlagSync.Core.FileSystem.Base;
using FlagSync.Core.FileSystem.ITunes;

namespace FlagSync.Data
{
    public class ITunesFileSystemSetting : FileSystemSetting
    {
        public override IFileSystem GetFileSystem()
        {
            return new ITunesFileSystem();
        }

        public override IDirectoryInfo GetRootDirectory()
        {
            return new ITunesDirectoryInfo(this.Source);
        }
    }
}