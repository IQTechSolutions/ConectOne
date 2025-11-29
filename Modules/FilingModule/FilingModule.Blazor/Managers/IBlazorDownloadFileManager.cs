using System.Text;
using FilingModule.Domain.RequestFeatures;

namespace FilingModule.Blazor.Managers
{
    /// <summary>
    /// Defines methods for managing file download buffers and initiating file downloads in Blazor applications.
    /// </summary>
    /// <remarks>The IBlazorDownloadFileManager interface provides asynchronous methods to add data buffers
    /// from various sources, check and clear buffer state, and trigger file downloads using the buffered data or direct
    /// input. It supports multiple data formats, progress reporting, cancellation, and timeout options to accommodate
    /// different download scenarios in Blazor. Implementations are typically used to enable client-side file downloads
    /// in web applications without requiring server round-trips.</remarks>
    public interface IBlazorDownloadFileManager
    {
        /// <summary>
        /// Adds a buffer to the underlying collection using a base64-encoded string.
        /// </summary>
        /// <param name="bytesBase64">A string containing the buffer data encoded in base64. Cannot be null or empty.</param>
        /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
        ValueTask AddBuffer(string bytesBase64);

        /// <summary>
        /// Asynchronously adds a buffer from a Base64-encoded string.
        /// </summary>
        /// <param name="bytesBase64">A string containing the buffer data encoded in Base64. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
        ValueTask AddBuffer(string bytesBase64, CancellationToken cancellationToken);

        /// <summary>
        /// Adds a buffer to the underlying system using a base64-encoded string representation, with a specified
        /// timeout for the operation.
        /// </summary>
        /// <param name="bytesBase64">A base64-encoded string that represents the buffer to add. Cannot be null or empty.</param>
        /// <param name="timeOut">The maximum duration to wait for the buffer to be added before the operation times out.</param>
        /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
        ValueTask AddBuffer(string bytesBase64, TimeSpan timeOut);

        /// <summary>
        /// Asynchronously adds the specified byte array as a buffer to the underlying data stream or collection.
        /// </summary>
        /// <param name="bytes">The byte array to add as a buffer. Cannot be null.</param>
        /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
        ValueTask AddBuffer(byte[] bytes);

        /// <summary>
        /// Asynchronously adds the specified byte buffer to the underlying data stream or storage.
        /// </summary>
        /// <param name="bytes">The buffer containing the bytes to add. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
        /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
        ValueTask AddBuffer(byte[] bytes, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously adds the specified buffer to the underlying data store, waiting up to the specified timeout
        /// for the operation to complete.
        /// </summary>
        /// <param name="bytes">The byte array containing the data to add. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the buffer to be added before the operation times out.</param>
        /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
        ValueTask AddBuffer(byte[] bytes, TimeSpan timeOut);

        /// <summary>
        /// Asynchronously adds the specified sequence of bytes to the buffer.
        /// </summary>
        /// <param name="bytes">The sequence of bytes to add to the buffer. Cannot be null.</param>
        /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
        ValueTask AddBuffer(IEnumerable<byte> bytes);

        /// <summary>
        /// Asynchronously adds a sequence of bytes to the buffer.
        /// </summary>
        /// <param name="bytes">The sequence of bytes to add to the buffer. Cannot be null.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
        ValueTask AddBuffer(IEnumerable<byte> bytes, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously adds a buffer of bytes to the underlying target, waiting up to the specified timeout for the
        /// operation to complete.
        /// </summary>
        /// <param name="bytes">The sequence of bytes to add. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the operation to complete before timing out.</param>
        /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
        ValueTask AddBuffer(IEnumerable<byte> bytes, TimeSpan timeOut);

        /// <summary>
        /// Asynchronously adds the contents of the specified stream as a buffer.
        /// </summary>
        /// <param name="stream">The stream containing the data to add as a buffer. The stream must be readable and positioned at the start
        /// of the data to add.</param>
        /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
        ValueTask AddBuffer(Stream stream);

        /// <summary>
        /// Asynchronously adds the contents of the specified stream as a buffer.
        /// </summary>
        /// <param name="stream">The stream containing the data to add as a buffer. The stream must be readable and positioned at the start
        /// of the data to add.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Passing a canceled token will attempt to stop the operation.</param>
        /// <returns>A ValueTask that represents the asynchronous add operation.</returns>
        ValueTask AddBuffer(Stream stream, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously adds a buffer from the specified stream, with support for cancellation and a JavaScript
        /// timeout.
        /// </summary>
        /// <param name="stream">The stream containing the data to be buffered. The stream must be readable and positioned at the start of
        /// the data to add.</param>
        /// <param name="streamReadcancellationToken">A cancellation token that can be used to cancel the read operation before it completes.</param>
        /// <param name="timeOutJavaScript">The maximum duration to wait for the JavaScript operation to complete before timing out.</param>
        /// <returns>A ValueTask that represents the asynchronous add buffer operation.</returns>
        ValueTask AddBuffer(Stream stream, CancellationToken streamReadcancellationToken, TimeSpan timeOutJavaScript);

        /// <summary>
        /// Determines whether any data is currently available in the buffer.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the buffer
        /// contains any data; otherwise, <see langword="false"/>.</returns>
        ValueTask<bool> AnyBuffer();

        /// <summary>
        /// Gets the number of available buffers.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of available buffers.</returns>
        ValueTask<int> BuffersCount();

        /// <summary>
        /// Asynchronously clears all internal buffers, discarding any data that has not yet been processed or
        /// transmitted.
        /// </summary>
        /// <returns>A ValueTask that represents the asynchronous clear operation.</returns>
        ValueTask ClearBuffers();

        /// <summary>
        /// Asynchronously downloads the specified file and returns its contents as a collection of base64-encoded
        /// buffers.
        /// </summary>
        /// <param name="fileName">The name of the file to download. Cannot be null or empty.</param>
        /// <param name="contentType">The MIME type to associate with the downloaded file. Defaults to "application/octet-stream" if not
        /// specified.</param>
        /// <returns>A task that represents the asynchronous download operation. The result contains the downloaded file's data
        /// as base64-encoded buffers and related metadata.</returns>
        ValueTask<DownloadFileResult> DownloadBase64Buffers(string fileName, string contentType = "application/octet-stream");

        /// <summary>
        /// Asynchronously downloads the specified file and returns its contents as a collection of base64-encoded
        /// buffers.
        /// </summary>
        /// <param name="fileName">The name of the file to download. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the download operation.</param>
        /// <param name="contentType">The MIME type to use for the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous download operation. The task result contains a <see
        /// cref="DownloadFileResult"/> with the base64-encoded file buffers and related metadata.</returns>
        ValueTask<DownloadFileResult> DownloadBase64Buffers(string fileName, CancellationToken cancellationToken, string contentType = "application/octet-stream");

        /// <summary>
        /// Downloads the specified file and returns its contents as a collection of base64-encoded buffers.
        /// </summary>
        /// <param name="fileName">The name of the file to download. Cannot be null or empty.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous download operation. The task result contains a <see
        /// cref="DownloadFileResult"/> with the base64-encoded file buffers.</returns>
        ValueTask<DownloadFileResult> DownloadBase64Buffers(string fileName, TimeSpan timeOut, string contentType = "application/octet-stream");

        /// <summary>
        /// Initiates an asynchronous download of a file as binary buffers with the specified file name and content
        /// type.
        /// </summary>
        /// <param name="fileName">The name of the file to be downloaded. This value is used as the suggested file name for the client.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains a DownloadFileResult with the
        /// binary buffers and related metadata.</returns>
        ValueTask<DownloadFileResult> DownloadBinaryBuffers(string fileName, string contentType = "application/octet-stream");

        /// <summary>
        /// Asynchronously downloads the specified file as a binary buffer.
        /// </summary>
        /// <param name="fileName">The name of the file to download. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the download operation.</param>
        /// <param name="contentType">The MIME type to use for the response. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous download operation. The result contains information about the
        /// downloaded file, including its binary content.</returns>
        ValueTask<DownloadFileResult> DownloadBinaryBuffers(string fileName, CancellationToken cancellationToken, string contentType = "application/octet-stream");

        /// <summary>
        /// Downloads the specified file as a sequence of binary buffers, using the provided content type and timeout
        /// settings.
        /// </summary>
        /// <param name="fileName">The name of the file to download. Cannot be null or empty.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete.</param>
        /// <param name="contentType">The MIME type to use for the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// downloaded file, including the binary buffers.</returns>
        ValueTask<DownloadFileResult> DownloadBinaryBuffers(string fileName, TimeSpan timeOut, string contentType = "application/octet-stream");

        /// <summary>
        /// Initiates a file download with the specified file name, content, and MIME type.
        /// </summary>
        /// <param name="fileName">The name of the file to be presented to the user for download. Cannot be null or empty.</param>
        /// <param name="bytesBase64">The file content encoded as a Base64 string. Must be a valid Base64-encoded representation of the file data.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the outcome of
        /// the file download request.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, string contentType = "application/octet-stream");

        /// <summary>
        /// Initiates a file download with the specified file name, content, and MIME type.
        /// </summary>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytesBase64">A base64-encoded string representing the file's binary content. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the download operation.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the outcome of
        /// the download request.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, CancellationToken cancellationToken, string contentType = "application/octet-stream");

        /// <summary>
        /// Initiates a file download with the specified file name, content, and content type, completing within the
        /// given timeout period.
        /// </summary>
        /// <remarks>If the operation does not complete within the specified timeout, the download may be
        /// canceled or fail. The caller is responsible for ensuring that the base64 string is valid and that the file
        /// name is appropriate for the target environment.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytesBase64">A base64-encoded string representing the file's binary content. Cannot be null or empty.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete before timing out.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, TimeSpan timeOut, string contentType = "application/octet-stream");

        /// <summary>
        /// Initiates a file download with the specified file name, content, and MIME type.
        /// </summary>
        /// <param name="fileName">The name of the file to be presented to the user for download. Cannot be null or empty.</param>
        /// <param name="bytes">The byte array containing the file's content to be downloaded. Cannot be null.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the file
        /// download, including status and any relevant metadata.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, string contentType = "application/octet-stream");

        /// <summary>
        /// Initiates a file download with the specified file name, content, and content type.
        /// </summary>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The byte array containing the file's content to be downloaded. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, CancellationToken cancellationToken, string contentType = "application/octet-stream");

        /// <summary>
        /// Initiates a file download with the specified content, file name, and content type, completing when the
        /// download is ready or the operation times out.
        /// </summary>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The file content as a byte array. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete before timing out.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the outcome of
        /// the download request.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, TimeSpan timeOut, string contentType = "application/octet-stream");

        /// <summary>
        /// Initiates a file download response with the specified file name, content, and MIME type.
        /// </summary>
        /// <param name="fileName">The name of the file to be presented to the user for download. Cannot be null or empty.</param>
        /// <param name="bytes">The file content as a sequence of bytes. Cannot be null.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the outcome of
        /// the file download response.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, string contentType = "application/octet-stream");

        /// <summary>
        /// Initiates a file download with the specified file name, content, and MIME type.
        /// </summary>
        /// <param name="fileName">The name of the file to be presented to the user for download. Cannot be null or empty.</param>
        /// <param name="bytes">The file content as a sequence of bytes to be downloaded. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
        /// <param name="contentType">The MIME type of the file to be sent to the client. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous operation. The result contains information about the outcome of the
        /// file download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, CancellationToken cancellationToken, string contentType = "application/octet-stream");

        /// <summary>
        /// Initiates a file download with the specified content, file name, and content type, completing when the
        /// download is ready or the operation times out.
        /// </summary>
        /// <remarks>If the operation does not complete within the specified timeout, the download will be
        /// aborted. The caller is responsible for ensuring that the provided file name and content type are valid for
        /// the intended client environment.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The file content to be downloaded, provided as a sequence of bytes. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete before timing out.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download request.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, TimeSpan timeOut, string contentType = "application/octet-stream");

        /// <summary>
        /// Downloads the specified file and writes its contents to the provided stream.
        /// </summary>
        /// <param name="fileName">The name of the file to download. Cannot be null or empty.</param>
        /// <param name="stream">The stream to which the file contents will be written. Must be writable and cannot be null.</param>
        /// <param name="contentType">The MIME type to use for the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, string contentType = "application/octet-stream");

        /// <summary>
        /// Downloads a file to the specified stream using the given file name and content type.
        /// </summary>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="stream">The stream to which the file content will be written. Must be writable and not null.</param>
        /// <param name="cancellationTokenBytesRead">A cancellation token that can be used to cancel the file read operation.</param>
        /// <param name="cancellationTokenJavaScriptInterop">A cancellation token that can be used to cancel the JavaScript interop operation.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, CancellationToken cancellationTokenBytesRead, CancellationToken cancellationTokenJavaScriptInterop, string contentType = "application/octet-stream");
        
        /// <summary>
        /// Downloads a file with the specified name and content to the client, writing the file data from the provided
        /// stream.
        /// </summary>
        /// <param name="fileName">The name to assign to the downloaded file. This value is used as the file name presented to the user. Cannot
        /// be null or empty.</param>
        /// <param name="stream">The stream containing the file data to be downloaded. The stream must be readable and positioned at the
        /// start of the data to send.</param>
        /// <param name="cancellationTokenBytesRead">A cancellation token that can be used to cancel the download operation while reading bytes from the stream.</param>
        /// <param name="timeOutJavaScriptInterop">The maximum duration to wait for the JavaScript interop operation to complete before timing out.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the file download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, CancellationToken cancellationTokenBytesRead, TimeSpan timeOutJavaScriptInterop, string contentType = "application/octet-stream");

        /// <summary>
        /// Asynchronously creates a downloadable file from the specified plain text content using the given encoding
        /// and content type.
        /// </summary>
        /// <remarks>The caller is responsible for providing valid file names and ensuring that the
        /// encoding and content type are appropriate for the intended use. This method does not perform file system
        /// operations; it prepares the file content for download, typically in a web application context.</remarks>
        /// <param name="fileName">The name of the file to be created and downloaded. Cannot be null or empty.</param>
        /// <param name="plainText">The plain text content to include in the file. Cannot be null.</param>
        /// <param name="encoding">The character encoding to use when converting the plain text to bytes. Cannot be null.</param>
        /// <param name="contentType">The MIME type to set for the file download. Defaults to "text/plain" if not specified.</param>
        /// <param name="encoderShouldEmitIdentifier">true to emit a byte order mark (BOM) in the encoded file; otherwise, false. Defaults to false.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the
        /// downloadable file, including its content and metadata.</returns>
        ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, string contentType = "text/plain", bool encoderShouldEmitIdentifier = false);

        /// <summary>
        /// Asynchronously creates a downloadable file from the specified plain text content using the given encoding
        /// and content type.
        /// </summary>
        /// <param name="fileName">The name of the file to be created and downloaded. This value is used as the file's name in the download
        /// prompt. Cannot be null or empty.</param>
        /// <param name="plainText">The plain text content to include in the file. Cannot be null.</param>
        /// <param name="encoding">The character encoding to use when converting the plain text to bytes. Cannot be null.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <param name="contentType">The MIME type to assign to the file. Defaults to "text/plain" if not specified.</param>
        /// <param name="encoderShouldEmitIdentifier">true to emit a byte order mark (BOM) in the encoded file; otherwise, false. Defaults to false.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the created
        /// downloadable file.</returns>
        ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, CancellationToken cancellationToken, string contentType = "text/plain", bool encoderShouldEmitIdentifier = false);

        /// <summary>
        /// Asynchronously creates a downloadable file from the specified plain text content using the given encoding
        /// and content type.
        /// </summary>
        /// <remarks>If the operation does not complete within the specified timeout, it may be canceled
        /// or fail. The caller is responsible for ensuring that the file name is valid for the target
        /// environment.</remarks>
        /// <param name="fileName">The name of the file to be created and downloaded. Cannot be null or empty.</param>
        /// <param name="plainText">The plain text content to include in the file. Cannot be null.</param>
        /// <param name="encoding">The character encoding to use when writing the text to the file. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the file generation and download operation to complete.</param>
        /// <param name="contentType">The MIME type to set for the file content. Defaults to "text/plain" if not specified.</param>
        /// <param name="encoderShouldEmitIdentifier">true to include a byte order mark (BOM) in the encoded file; otherwise, false.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the generated
        /// downloadable file.</returns>
        ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, TimeSpan timeOut, string contentType = "text/plain", bool encoderShouldEmitIdentifier = false);

        /// <summary>
        /// Downloads a file from the provided base64-encoded data, saving it with the specified file name and content
        /// type. Reports progress and supports configurable buffer size for the download operation.
        /// </summary>
        /// <remarks>This method is typically used in web applications to trigger a file download on the
        /// client side using base64-encoded data. The buffer size can be adjusted to optimize performance for large
        /// files. The progress callback is invoked periodically as the download proceeds.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytesBase64">The base64-encoded string representing the file's binary data. Cannot be null or empty.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used during the download operation. Must be a positive integer. The
        /// default is 32,768 bytes.</param>
        /// <param name="contentType">The MIME type to associate with the downloaded file. If not specified, defaults to
        /// "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that receives progress updates as a value between 0.0 and 1.0,
        /// representing the percentage of the download completed. If null, progress is not reported.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the file download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);

        /// <summary>
        /// Initiates an asynchronous download of a file to the client using the specified file name and Base64-encoded
        /// content.
        /// </summary>
        /// <remarks>This method streams the file content to the client in chunks, which can help reduce
        /// memory usage for large files. The operation can be cancelled via the provided cancellation token. If a
        /// progress callback is supplied, it will be called periodically with the percentage of the file that has been
        /// sent.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. This value is used as the file name presented to the user.</param>
        /// <param name="bytesBase64">A Base64-encoded string representing the file's binary content. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to request cancellation of the download operation.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used when streaming the file content. Must be a positive integer. The
        /// default is 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file to be sent to the client. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional callback that is invoked with the current progress percentage (from 0.0 to 100.0) as the file is
        /// downloaded. If null, progress updates are not reported.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the outcome of
        /// the download request.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, CancellationToken cancellationToken, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);

        /// <summary>
        /// Downloads a file using the specified parameters and reports progress asynchronously.
        /// </summary>
        /// <remarks>The method performs the download operation asynchronously and supports cancellation
        /// via the specified timeout. If the progress callback is provided, it will be invoked periodically with the
        /// current progress. The caller is responsible for handling any exceptions that may occur during the download
        /// process.</remarks>
        /// <param name="fileName">The name of the file to be downloaded. Cannot be null or empty.</param>
        /// <param name="bytesBase64">The base64-encoded string representing the file's contents. Cannot be null or empty.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete before timing out.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used during the download. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file content. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that receives progress updates as a value between 0.0 and 1.0,
        /// representing the percentage of the download completed. If null, progress is not reported.</param>
        /// <returns>A task that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, TimeSpan timeOut, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);

        /// <summary>
        /// Initiates a file download using the specified file name, content bytes, and content type, with optional
        /// buffering and progress reporting.
        /// </summary>
        /// <remarks>The method streams the file content in buffered chunks to optimize memory usage and
        /// performance. If a progress callback is provided, it is called periodically as the download progresses. This
        /// method is typically used in web applications to send files to clients.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The file content as a byte array. Cannot be null.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used when streaming the file. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file content. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the current download progress as a value between 0.0
        /// and 1.0. If null, progress is not reported.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the file
        /// download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);

        /// <summary>
        /// Initiates an asynchronous download of the specified file to the client using the provided byte array as
        /// content.
        /// </summary>
        /// <remarks>This method streams the file content to the client in buffered chunks. If the
        /// operation is cancelled via the provided cancellation token, the download will be aborted. The progress
        /// callback, if supplied, is called periodically as the file is transmitted.</remarks>
        /// <param name="fileName">The name of the file to be presented to the client for download. Cannot be null or empty.</param>
        /// <param name="bytes">The byte array containing the file data to be downloaded. Cannot be null.</param>
        /// <param name="cancellationToken">A token that can be used to request cancellation of the download operation.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used when streaming the file. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file content. If not specified, defaults to "application/octet-stream".</param>
        /// <param name="progress">An optional callback that is invoked with the current progress of the download as a value between 0.0 and
        /// 1.0. If null, progress updates are not reported.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the outcome of
        /// the download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, CancellationToken cancellationToken, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);

        /// <summary>
        /// Initiates an asynchronous download of a file to the client using the specified file name, content, and
        /// options.
        /// </summary>
        /// <remarks>If the download does not complete within the specified timeout, the operation is
        /// canceled. The method streams the file in chunks determined by bufferSize, which may affect memory usage and
        /// performance. The progress callback, if provided, is called on the thread performing the download and may be
        /// invoked frequently for large files.</remarks>
        /// <param name="fileName">The name that will be used for the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The byte array containing the file data to be downloaded. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete before timing out.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used when streaming the file. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file content. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional callback that is invoked with the current progress percentage (from 0.0 to 100.0) during the
        /// download. If null, progress updates are not reported.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, TimeSpan timeOut, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);

        /// <summary>
        /// Initiates a file download using the specified file name, content bytes, and optional parameters for
        /// buffering, content type, and progress reporting.
        /// </summary>
        /// <remarks>The method streams the file content to the client and supports large files
        /// efficiently by using the specified buffer size. If progress reporting is required, provide a callback to
        /// receive periodic updates. The method does not start the download if any required parameter is
        /// invalid.</remarks>
        /// <param name="fileName">The name of the file to be presented to the user for download. Cannot be null or empty.</param>
        /// <param name="bytes">The sequence of bytes representing the file content to be downloaded. Cannot be null.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used when streaming the file. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file content. If not specified, defaults to "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the current download progress as a value between 0.0
        /// and 1.0. If null, progress is not reported.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the outcome of
        /// the file download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);

        /// <summary>
        /// Asynchronously initiates a file download using the specified file name, content bytes, and optional
        /// parameters.
        /// </summary>
        /// <remarks>The method streams the file content to the client in buffered chunks. If the
        /// operation is cancelled via the provided cancellation token, the download will be aborted. The progress
        /// callback, if supplied, is called periodically as the download progresses.</remarks>
        /// <param name="fileName">The name of the file to be downloaded. This value is used as the suggested file name for the client. Cannot
        /// be null or empty.</param>
        /// <param name="bytes">The file content to be downloaded, provided as a sequence of bytes. Cannot be null.</param>
        /// <param name="cancellationToken">A token that can be used to request cancellation of the download operation.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used when streaming the file. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file content. If not specified, defaults to "application/octet-stream".</param>
        /// <param name="progress">An optional callback that is invoked with the current progress of the download as a value between 0.0 and
        /// 1.0. If null, progress updates are not reported.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the outcome of
        /// the file download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, CancellationToken cancellationToken, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);
        
        /// <summary>
        /// Initiates an asynchronous file download using the specified file name, content bytes, and options.
        /// </summary>
        /// <remarks>If the download does not complete within the specified timeout, the operation is
        /// canceled. The method does not start a new thread; it returns when the download is complete or canceled. The
        /// progress callback, if provided, may be called multiple times as the download progresses.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The file content to be downloaded, provided as a sequence of bytes. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete before timing out.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used during the download. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file content. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the current download progress as a value between 0.0
        /// and 1.0. If null, progress updates are not reported.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, TimeSpan timeOut, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);
        
        /// <summary>
        /// Downloads the specified file and writes its contents to the provided stream asynchronously.
        /// </summary>
        /// <remarks>The caller is responsible for ensuring that the provided stream remains open and
        /// writable for the duration of the download. The method does not close or dispose the stream after completion.
        /// If the file does not exist or cannot be accessed, the result will indicate the failure.</remarks>
        /// <param name="fileName">The name of the file to download. Cannot be null or empty.</param>
        /// <param name="stream">The stream to which the downloaded file will be written. Must be writable and not null.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used during the download operation. Must be a positive integer. The
        /// default is 32,768 bytes.</param>
        /// <param name="contentType">The MIME type to associate with the file being downloaded. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional callback that is invoked with the download progress as a value between 0.0 and 1.0. If null,
        /// progress updates are not reported.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);
        
        /// <summary>
        /// Asynchronously downloads a file and writes its contents to the specified stream, reporting progress and
        /// supporting cancellation.
        /// </summary>
        /// <remarks>The caller is responsible for ensuring that the provided stream is writable and
        /// properly disposed after the operation completes. Progress reporting is best-effort and may not be available
        /// in all environments. Both cancellation tokens can be used independently to cancel different aspects of the
        /// operation.</remarks>
        /// <param name="fileName">The name of the file to download. This value is used to identify the file source. Cannot be null or empty.</param>
        /// <param name="stream">The stream to which the downloaded file contents will be written. Must be writable and remain open for the
        /// duration of the operation.</param>
        /// <param name="cancellationTokenBytesRead">A cancellation token that can be used to cancel the file reading operation. If cancellation is requested,
        /// the download will be aborted.</param>
        /// <param name="cancellationTokenJavaScriptInterop">A cancellation token that can be used to cancel JavaScript interop operations involved in the download
        /// process.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used when reading and writing the file. Must be a positive integer. The
        /// default is 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file being downloaded. Used to set the content type for the download. The default is
        /// "application/octet-stream".</param>
        /// <param name="progress">An optional callback that is invoked with the download progress as a value between 0.0 and 1.0. If null,
        /// progress will not be reported.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download, including success status and any relevant details.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, CancellationToken cancellationTokenBytesRead, CancellationToken cancellationTokenJavaScriptInterop, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);
        
        /// <summary>
        /// Downloads a file with the specified name and content to the provided stream, supporting progress reporting,
        /// cancellation, and custom content type.
        /// </summary>
        /// <remarks>The method supports cancellation via the provided cancellation token. If the
        /// operation is cancelled or times out, the result will indicate the failure. The caller is responsible for
        /// managing the lifetime of the provided stream.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="stream">The stream to which the file content will be written. Must be writable and not null.</param>
        /// <param name="cancellationTokenBytesRead">A cancellation token that can be used to cancel the download operation.</param>
        /// <param name="timeOutJavaScriptInterop">The maximum duration to wait for JavaScript interop operations before timing out.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used when reading and writing the file. Must be a positive integer. The
        /// default is 32,768 bytes.</param>
        /// <param name="contentType">The MIME type to use for the file content. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional callback that is invoked with the download progress as a value between 0.0 and 1.0. If null,
        /// progress is not reported.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download.</returns>
        ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, CancellationToken cancellationTokenBytesRead, TimeSpan timeOutJavaScriptInterop, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null);
        
        /// <summary>
        /// Asynchronously creates a downloadable file from the specified plain text content using the given encoding
        /// and returns a result representing the file download operation.
        /// </summary>
        /// <remarks>If the specified encoding does not support emitting a byte order mark, the
        /// encoderShouldEmitIdentifier parameter is ignored. The method does not write any data if plainText is
        /// empty.</remarks>
        /// <param name="fileName">The name of the file to be created for download. Cannot be null or empty.</param>
        /// <param name="plainText">The plain text content to be written to the downloadable file. Cannot be null.</param>
        /// <param name="encoding">The character encoding to use when writing the text content to the file. Cannot be null.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used during file generation. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type to set for the downloadable file. The default is "text/plain".</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the progress of the file generation as a value
        /// between 0.0 and 1.0. If null, progress reporting is disabled.</param>
        /// <param name="encoderShouldEmitIdentifier">true to emit a byte order mark (BOM) at the start of the file if supported by the encoding; otherwise,
        /// false.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the generated
        /// downloadable file.</returns>
        ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, int bufferSize = 32768, string contentType = "text/plain", Func<double, Task> progress = null, bool encoderShouldEmitIdentifier = false);
        
        /// <summary>
        /// Asynchronously creates a downloadable file from the specified plain text content using the given encoding
        /// and returns a result representing the file download operation.
        /// </summary>
        /// <remarks>If the operation is canceled via the cancellation token, the file creation is
        /// aborted. The method supports large text content efficiently by using a configurable buffer size. The
        /// progress callback, if provided, can be used to update UI elements or track the operation's
        /// progress.</remarks>
        /// <param name="fileName">The name of the file to be created for download. This value is used as the file's name in the download
        /// prompt. Cannot be null or empty.</param>
        /// <param name="plainText">The plain text content to be written to the file. Cannot be null.</param>
        /// <param name="encoding">The character encoding to use when writing the text content to the file. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used when writing the file. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type to set for the file download. The default is "text/plain".</param>
        /// <param name="progress">An optional callback that is invoked with the progress of the file writing operation, as a value between 0.0
        /// and 1.0. If null, progress is not reported.</param>
        /// <param name="encoderShouldEmitIdentifier">true to emit a byte order mark (BOM) in the output file if supported by the encoding; otherwise, false. The
        /// default is false.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the created
        /// downloadable file.</returns>
        ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, CancellationToken cancellationToken, int bufferSize = 32768, string contentType = "text/plain", Func<double, Task> progress = null, bool encoderShouldEmitIdentifier = false);
        
        /// <summary>
        /// Asynchronously creates a downloadable file from the specified plain text content using the given encoding
        /// and returns the result of the file download operation.
        /// </summary>
        /// <remarks>If the operation does not complete within the specified timeout, it will be canceled.
        /// The method supports large text content by streaming data in chunks determined by bufferSize. The caller is
        /// responsible for ensuring that fileName is a valid file name for the target environment.</remarks>
        /// <param name="fileName">The name of the file to be created and downloaded. Cannot be null or empty.</param>
        /// <param name="plainText">The plain text content to be written to the file. Cannot be null.</param>
        /// <param name="encoding">The character encoding to use when writing the text content to the file. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the file download operation to complete before timing out.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used when writing the file. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type to set for the file download response. The default is "text/plain".</param>
        /// <param name="progress">An optional callback that is invoked with the progress of the file download operation, represented as a
        /// value between 0.0 and 1.0. If null, progress updates are not reported.</param>
        /// <param name="encoderShouldEmitIdentifier">true to emit a byte order mark (BOM) in the encoded file; otherwise, false. The default is false.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the file
        /// download, including status and any relevant metadata.</returns>
        ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, TimeSpan timeOut, int bufferSize = 32768, string contentType = "text/plain", Func<double, Task> progress = null, bool encoderShouldEmitIdentifier = false);
    }
}
