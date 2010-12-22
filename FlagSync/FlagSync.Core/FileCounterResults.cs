namespace FlagSync.Core
{
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
}