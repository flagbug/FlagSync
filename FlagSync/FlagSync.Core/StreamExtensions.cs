using System.IO;

namespace FlagSync.Core
{
    internal static class StreamExtensions
    {
        internal static void CopyTo(this Stream sourceStream, Stream targetStream)
        {
            byte[] buffer = new byte[32768];
            while (true)
            {
                int read = sourceStream.Read(buffer, 0, buffer.Length);

                if (read <= 0)
                {
                    return;
                }

                targetStream.Write(buffer, 0, read);
            }
        }
    }
}