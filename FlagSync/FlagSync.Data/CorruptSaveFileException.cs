using System;

namespace FlagSync.Data
{
    public class CorruptSaveFileException : Exception
    {
        public CorruptSaveFileException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}