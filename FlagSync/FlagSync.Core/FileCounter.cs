using FlagLib.FileSystem;

namespace FlagSync.Core
{
    /// <summary>
    /// Counts the number and size of the files in a folder
    /// </summary>
    internal class FileCounter
    {
        private JobSetting settings;
        private FileCounterResults result;

        /// <summary>
        /// Counts the number of files and the total size for the specifies job
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The result</returns>
        public FileCounterResults CountJobFiles(JobSetting settings)
        {
            this.settings = settings;
            this.CountJobFiles();
            return this.result;
        }

        /// <summary>
        /// Analyses directory A and directory B and adds the results
        /// </summary>
        private void CountJobFiles()
        {
            this.result = new FileCounterResults();
            this.result += this.CountFiles(this.settings.DirectoryA);
            this.result += this.CountFiles(this.settings.DirectoryB);
        }

        /// <summary>
        /// Analyses a single directory with its subfolders
        /// </summary>
        /// <param name="root">The directory to count</param>
        /// <returns>The result</returns>
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