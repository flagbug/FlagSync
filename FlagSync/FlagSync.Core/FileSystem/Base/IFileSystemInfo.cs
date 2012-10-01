namespace FlagSync.Core.FileSystem.Base
{
    /// <summary>
    /// Provides the interface for the <see cref="FlagSync.Core.FileSystem.Base.IDirectoryInfo"/>
    /// and the <see cref="FlagSync.Core.FileSystem.Base.IFileInfo"/> interface.
    /// </summary>
    public interface IFileSystemInfo
    {
        /// <summary>
        /// Gets the full name of the file or directory.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the name of the file or directory.
        /// </summary>
        string Name { get; }
    }
}