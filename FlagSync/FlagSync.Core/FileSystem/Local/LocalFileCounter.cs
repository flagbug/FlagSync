using FlagLib.FileSystem;
using FlagSync.Core.FileSystem.Abstract;

namespace FlagSync.Core.FileSystem.Local
{
    internal class LocalFileCounter : IFileCounter
    {
        public FileCounterResults CountJobFiles(JobSetting settings)
        {
            var result = new FileCounterResults();
            result += this.CountFiles(settings.DirectoryA);
            result += this.CountFiles(settings.DirectoryB);

            return result;
        }

        private FileCounterResults CountFiles(string path)
        {
            int files = 0;
            long bytes = 0;

            DirectoryScanner scanner = new DirectoryScanner(path);

            scanner.FileFound += (sender, e) =>
            {
                files++;
                bytes += e.File.Length;
            };

            scanner.Start();

            return new FileCounterResults(files, bytes);
        }
    }
}