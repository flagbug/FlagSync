namespace FlagSync.Core
{
    /// <summary>
    /// Contains the number of files counted and the total size of all files
    /// </summary>
    public class FileCounterResults
    {
        /// <summary>
        /// Gets the counted files.
        /// </summary>
        /// <value>The counted files.</value>
        public int CountedFiles { get; private set; }

        /// <summary>
        /// Gets the counted bytes.
        /// </summary>
        /// <value>The counted bytes.</value>
        public long CountedBytes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCounterResults"/> class.
        /// </summary>
        /// <param name="countedFiles">The counted files.</param>
        /// <param name="countedBytes">The counted bytes.</param>
        public FileCounterResults(int countedFiles, long countedBytes)
        {
            this.CountedBytes = countedBytes;
            this.CountedFiles = countedFiles;
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