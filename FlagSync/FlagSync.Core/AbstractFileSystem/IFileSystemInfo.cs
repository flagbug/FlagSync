namespace FlagSync.Core.AbstractFileSystem
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