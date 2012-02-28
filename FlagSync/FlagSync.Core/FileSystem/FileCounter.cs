using Rareform.Extensions;
using FlagSync.Core.FileSystem.Base;

namespace FlagSync.Core.FileSystem
{
    /// <summary>
    /// Provides a method for counting recursively the files of a directory.
    /// </summary>
    internal static class FileCounter
    {
        /// <summary>
        /// Counts recursively the files of the directory.
        /// </summary>
        /// <param name="rootDirectory">The root directory.</param>
        /// <returns>
        /// A <see cref="FileCountResult"/> which indicates the result of the count.
        /// </returns>
        public static FileCountResult CountFiles(IDirectoryInfo rootDirectory)
        {
            rootDirectory.ThrowIfNull(() => rootDirectory);

            int files = 0;
            long bytes = 0;

            var scanner = new FileSystemScanner(rootDirectory);

            scanner.FileFound += (sender, e) =>
            {
                files++;
                bytes += e.File.Length;
            };

            scanner.Start();

            return new FileCountResult(files, bytes);
        }
    }
}