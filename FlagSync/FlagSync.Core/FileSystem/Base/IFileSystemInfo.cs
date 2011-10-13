namespace FlagSync.Core.FileSystem.Base
{
    public interface IFileSystemInfo
    {
        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <remarks></remarks>
        string FullName { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
        string Name { get; }
    }
}