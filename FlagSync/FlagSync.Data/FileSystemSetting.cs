using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Data
{
    public abstract class FileSystemSetting
    {
        public string Source { get; set; }

        public abstract IFileSystem GetFileSystem();

        public abstract IDirectoryInfo GetRootDirectory();
    }
}