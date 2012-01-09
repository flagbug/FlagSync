using System;

namespace FlagSync.Data
{
    public class ITunesNotOpenedException : Exception
    {
        public ITunesNotOpenedException(string message)
            : base(message)
        { }
    }
}