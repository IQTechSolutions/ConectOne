using System.Net;

namespace FilingModule.Application.Services
{
    /// <summary>
    /// Provides HTTP content based on a stream and reports progress during data transfer.
    /// </summary>
    /// <remarks>This class is useful for scenarios where monitoring the progress of data transfer is
    /// required, such as uploading large files. The source stream must support seeking and provide a valid <see
    /// cref="Stream.Length"/> property. Progress updates are reported periodically via the provided <see
    /// cref="Action{T}"/> delegate.</remarks>
    public class ProgressableStreamContent : HttpContent
    {
        private const int DefaultBuffer = 80 * 1024;
        private readonly Stream _source;
        private readonly int _bufferSize;
        private readonly Action<long> _progress;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressableStreamContent"/> class, which provides  HTTP
        /// content based on a stream and reports progress during data transfer.
        /// </summary>
        /// <remarks>The <paramref name="source"/> stream must support seeking and provide a valid <see
        /// cref="Stream.Length"/> property. The <paramref name="progress"/> delegate is called periodically during the
        /// transfer to report the number of bytes written.</remarks>
        /// <param name="source">The source <see cref="Stream"/> from which the content is read. Must be readable and non-null.</param>
        /// <param name="bufferSize">The size of the buffer, in bytes, used for reading the stream. Must be greater than zero.</param>
        /// <param name="progress">An <see cref="Action{T}"/> delegate that is invoked with the number of bytes transferred. Can be null if
        /// progress reporting is not required.</param>
        public ProgressableStreamContent(Stream source, int bufferSize, Action<long> progress)
        {
            _source = source;
            _bufferSize = bufferSize <= 0 ? DefaultBuffer : bufferSize;
            _progress = progress;
            Headers.ContentLength = _source.Length;
        }

        /// <summary>
        /// Asynchronously serializes data from the source stream to the target stream.
        /// </summary>
        /// <remarks>This method reads data from the source stream in chunks and writes it to the target
        /// stream. It invokes the progress callback after each chunk is written to report the total number of bytes
        /// uploaded.</remarks>
        /// <param name="target">The stream to which the data will be written.</param>
        /// <param name="context">The transport context associated with the serialization operation, or <see langword="null"/> if no context
        /// is provided.</param>
        /// <returns>A task that represents the asynchronous serialization operation.</returns>
        protected override async Task SerializeToStreamAsync(Stream target, TransportContext? context)
        {
            var buffer = new byte[_bufferSize];
            long uploaded = 0;
            int read;
            while ((read = await _source.ReadAsync(buffer)) > 0)
            {
                await target.WriteAsync(buffer.AsMemory(0, read));
                uploaded += read;
                _progress.Invoke(uploaded);
            }
        }

        /// <summary>
        /// Attempts to compute the length of the source data.
        /// </summary>
        /// <remarks>This method overrides the base implementation to provide the length of the source
        /// data. The method always returns <see langword="true"/> as the length is directly accessible.</remarks>
        /// <param name="length">When the method returns, contains the computed length of the source data.</param>
        /// <returns><see langword="true"/> if the length was successfully computed; otherwise, <see langword="false"/>.</returns>
        protected override bool TryComputeLength(out long length)
        {
            length = _source.Length;
            return true;
        }

        /// <summary>
        /// Releases the resources used by the current instance of the class.
        /// </summary>
        /// <remarks>This method disposes of managed resources if <paramref name="disposing"/> is <see
        /// langword="true"/>. It should be called when the object is no longer needed to ensure proper cleanup of
        /// resources.</remarks>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release
        /// only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing) _source.Dispose();
        }
    }
}
