using System;

namespace FlagSync.Core.FileSystem
{
    /// <summary>
    /// The exception that is thrown, when the file system is unabailable.
    /// </summary>
    public class FileSystemUnavailableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemUnavailableException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public FileSystemUnavailableException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}