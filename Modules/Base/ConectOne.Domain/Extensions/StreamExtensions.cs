namespace ConectOne.Domain.Extensions
{
    /// <summary>
    /// Provides extension methods for converting a stream to a byte array, including synchronous and asynchronous
    /// operations.
    /// </summary>
    /// <remarks>These extension methods enable convenient conversion of a stream's contents to a byte array.
    /// They require that the stream supports reading and seeking. The methods do not close or dispose the stream after
    /// reading. Use these methods when you need to access the entire contents of a stream as a byte array.</remarks>
    public static class StreamExtensions
    {
        /// <summary>
        /// Returns the contents of the specified stream as a byte array.
        /// </summary>
        /// <remarks>The method reads the entire contents of the stream from the beginning to the end. The
        /// stream's position is reset to zero before reading. The stream must have a length that fits within a 32-bit
        /// signed integer. This method does not close or dispose the stream.</remarks>
        /// <param name="stream">The stream to read from. The stream must support reading and seeking.</param>
        /// <returns>A byte array containing the data from the stream.</returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            var streamLength = (int)stream.Length;
            var data = new byte[streamLength];
            stream.Position = 0;
            stream.Read(data, 0, streamLength);
            return data;
        }
        
        /// <summary>
        /// Asynchronously reads the entire contents of the specified stream and returns them as a byte array.
        /// </summary>
        /// <remarks>The method reads from the beginning of the stream to its end. The stream's position
        /// is reset to zero before reading. The stream must support the Length and Position properties. This method is
        /// not suitable for very large streams, as it allocates a byte array equal to the stream's length.</remarks>
        /// <param name="stream">The stream to read from. The stream must support reading and seeking.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a byte array with the contents
        /// of the stream.</returns>
        public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
        {
            var streamLength = (int)stream.Length;
            var data = new byte[streamLength];
            stream.Position = 0;
            await stream.ReadAsync(data, 0, streamLength);
            return data;
        }
        
        /// <summary>
        /// Asynchronously reads the entire contents of the specified stream into a byte array.
        /// </summary>
        /// <remarks>The method reads from the beginning of the stream to its end. The stream's position
        /// is reset to zero before reading. The caller is responsible for disposing the stream after use.</remarks>
        /// <param name="stream">The stream to read from. The stream must support reading and seeking.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous read operation.</param>
        /// <returns>A task that represents the asynchronous read operation. The task result contains a byte array with the
        /// contents of the stream.</returns>
        public static async Task<byte[]> ToByteArrayAsync(this Stream stream, CancellationToken cancellationToken)
        {
            var streamLength = (int)stream.Length;
            var data = new byte[streamLength];
            stream.Position = 0;
            await stream.ReadAsync(data, 0, streamLength, cancellationToken);
            return data;
        }
    }
}
