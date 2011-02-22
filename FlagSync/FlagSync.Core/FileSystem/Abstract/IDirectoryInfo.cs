namespace FlagSync.Core.FileSystem.Abstract
{
    public interface IDirectoryInfo : IFileSystemInfo
    {
        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        /// <value>The parent directory.</value>
        IDirectoryInfo Parent { get; }
    }
}