using System.IO;

namespace FlagSync.Core
{
    /// <summary>
    /// Counts the number of files in a folder an also counts the size of the folder
    /// </summary>
    public class FileCounter
    {
        private JobSetting settings;
        private FileCounterResults result;

        /// <summary>
        /// Contains the number of files counted and the total size of all files
        /// </summary>
        public class FileCounterResults
        {
            private int countedFiles;
            private long countedBytes;

            /// <summary>
            /// Gets the counted files.
            /// </summary>
            /// <value>The counted files.</value>
            public int CountedFiles
            {
                get
                {
                    return this.countedFiles;
                }
            }

            /// <summary>
            /// Gets the counted bytes.
            /// </summary>
            /// <value>The counted bytes.</value>
            public long CountedBytes
            {
                get
                {
                    return this.countedBytes;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FileCounterResults"/> class.
            /// </summary>
            /// <param name="countedFiles">The counted files.</param>
            /// <param name="countedBytes">The counted bytes.</param>
            public FileCounterResults(int countedFiles, long countedBytes)
            {
                this.countedBytes = countedBytes;
                this.countedFiles = countedFiles;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FileCounterResults"/> class.
            /// </summary>
            public FileCounterResults()
                : this(0, 0)
            {
            }

            /// <summary>
            /// Implements the operator +.
            /// </summary>
            /// <param name="a">The first file counter result</param>
            /// <param name="b">The second file counter result</param>
            /// <returns>The result of the operator.</returns>
            public static FileCounterResults operator +(FileCounterResults a, FileCounterResults b)
            {
                return new FileCounterResults(a.CountedFiles + b.CountedFiles, a.CountedBytes + b.CountedBytes);
            }
        }

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
            this.result += this.CountFiles(new DirectoryInfo(this.settings.DirectoryA));
            this.result += this.CountFiles(new DirectoryInfo(this.settings.DirectoryB));
        }

        /// <summary>
        /// Analyses a single directory with its subfolders
        /// </summary>
        /// <param name="root">The directory to count</param>
        /// <returns>The result</returns>
        private FileCounterResults CountFiles(DirectoryInfo root)
        {
            int files = 0;
            long bytes = 0;

            try
            {
                FileInfo[] rootFiles = root.GetFiles();

                foreach (FileInfo file in rootFiles)
                {
                    files++;
                    bytes += file.Length;
                }

                foreach (DirectoryInfo directory in root.GetDirectories())
                {
                    FileCounterResults result = CountFiles(directory);

                    files += result.CountedFiles;
                    bytes += result.CountedBytes;
                }
            }

            catch (System.UnauthorizedAccessException)
            {
                Logger.Instance.LogError("UnauthorizedAccessException at directory: " + root.FullName + " while counting files");
            }

            return new FileCounterResults(files, bytes);
        }
    }
}