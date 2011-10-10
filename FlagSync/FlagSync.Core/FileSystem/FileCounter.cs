using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem
{
    internal class FileCounter
    {
        public static FileCounterResult CountFiles(IDirectoryInfo rootDirectory)
        {
            int files = 0;
            long bytes = 0;

            FileSystemScanner scanner = new FileSystemScanner(rootDirectory);

            scanner.FileFound += (sender, e) =>
            {
                files++;
                bytes += e.File.Length;
            };

            scanner.Start();

            return new FileCounterResult(files, bytes);
        }
    }
}