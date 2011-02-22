namespace FlagSync.Core.FileSystem.Abstract
{
    public interface IFileSystemInfo
    {
        //Gets the full name.
        string FullName { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }
    }
}