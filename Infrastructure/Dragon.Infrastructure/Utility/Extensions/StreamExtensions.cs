using System.IO;

namespace Auth.Infrastructure.Utility.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream stream, bool isDispose = false)
        {
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            if (isDispose)
                stream.Dispose();
            return buffer;
        }
    }
}
