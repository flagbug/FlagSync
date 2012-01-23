using System;

namespace FlagSync.Core.FileSystem
{
    /// <summary>
    /// The exception that is thrown, when a file or directory could not be accessed.
    /// </summary>
    public class AccessException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AccessException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}