namespace FlagSync.Core.AbstractFileSystem
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