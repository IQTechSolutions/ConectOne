using System.Text;
using ConectOne.Domain.Extensions;
using Filing.Blazor.Scripts;
using FilingModule.Domain.RequestFeatures;
using Microsoft.JSInterop;

namespace FilingModule.Blazor.Managers
{
    /// <summary>
    /// Provides methods for managing file downloads in Blazor applications, including buffering file data and
    /// initiating downloads using JavaScript interop.
    /// </summary>
    /// <remarks>BlazorDownloadFileManager enables efficient file downloads by allowing data to be buffered in
    /// memory and then downloaded to the client browser. It supports various data sources, such as byte arrays,
    /// streams, and base64-encoded strings, and provides options for progress reporting, cancellation, and timeout
    /// control. This class is intended for use in Blazor applications that require client-side file downloads without
    /// server round-trips. Thread safety is not guaranteed; each instance should be used within the context of a single
    /// Blazor component or request.</remarks>
    public class BlazorDownloadFileManager : IBlazorDownloadFileManager
    {
        /// <summary>
        /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET code.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components or services to enable
        /// interoperability with JavaScript. Assign a valid IJSRuntime implementation before making JavaScript interop
        /// calls.</remarks>
        protected IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Initializes a new instance of the BlazorDownloadFileManager class and sets up the required JavaScript
        /// interop for file downloads in Blazor applications.
        /// </summary>
        /// <remarks>This constructor ensures that the necessary JavaScript code for file downloads is
        /// loaded and available for use. The JavaScript interop is initialized asynchronously upon
        /// construction.</remarks>
        /// <param name="jSRuntime">The IJSRuntime instance used to invoke JavaScript functions for file download operations. Cannot be null.</param>
        public BlazorDownloadFileManager(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
            Task.Run(async () => await JSRuntime.InvokeVoidAsync("eval", BlazorDownloadFileScript.InitializeBlazorDownloadFile()));
        }

        /// <summary>
        /// Adds a buffer to the download queue using a base64-encoded string.
        /// </summary>
        /// <param name="bytesBase64">A base64-encoded string representing the buffer to add. Cannot be null or empty.</param>
        /// <returns>A ValueTask that represents the asynchronous operation.</returns>
        public ValueTask AddBuffer(string bytesBase64)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", bytesBase64);
        }

        /// <summary>
        /// Adds a buffer to the download queue using a Base64-encoded string.
        /// </summary>
        /// <remarks>This method is typically used to queue file data for download in a Blazor
        /// application. The buffer is pushed to the client-side download mechanism. Ensure that the Base64 string
        /// represents valid binary data.</remarks>
        /// <param name="bytesBase64">A string containing the buffer data encoded in Base64. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A ValueTask that represents the asynchronous operation.</returns>
        public ValueTask AddBuffer(string bytesBase64, CancellationToken cancellationToken)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", cancellationToken, bytesBase64);
        }

        /// <summary>
        /// Adds a buffer of data, encoded as a Base64 string, to the download queue for processing within the specified
        /// timeout period.
        /// </summary>
        /// <param name="bytesBase64">The data to add to the buffer, encoded as a Base64 string. Cannot be null or empty.</param>
        /// <param name="timeOut">The maximum duration to wait for the buffer to be added before the operation times out.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The task completes when the buffer has been added or
        /// the timeout has elapsed.</returns>
        public ValueTask AddBuffer(string bytesBase64, TimeSpan timeOut)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", timeOut, bytesBase64);
        }

        /// <summary>
        /// Adds a buffer of bytes to the download file stream for processing by the JavaScript runtime.
        /// </summary>
        /// <remarks>This method is intended for use with Blazor applications that interact with
        /// JavaScript to handle file downloads. The buffer is sent to the JavaScript side for further processing or
        /// assembly.</remarks>
        /// <param name="bytes">The array of bytes to add to the download buffer. Cannot be null.</param>
        /// <returns>A ValueTask that represents the asynchronous operation.</returns>
        public ValueTask AddBuffer(byte[] bytes)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", bytes.Select(s => s));
        }

        /// <summary>
        /// Asynchronously adds a buffer of bytes to the download stream for the current file operation.
        /// </summary>
        /// <remarks>Call this method to push additional data to the file being downloaded. This method
        /// should be used in conjunction with the appropriate file download initialization and completion
        /// methods.</remarks>
        /// <param name="bytes">The array of bytes to add to the download buffer. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A ValueTask that represents the asynchronous operation.</returns>
        public ValueTask AddBuffer(byte[] bytes, CancellationToken cancellationToken)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", cancellationToken, bytes.Select(s => s));
        }

        /// <summary>
        /// Asynchronously adds a buffer of bytes to the download queue, waiting up to the specified timeout for the
        /// operation to complete.
        /// </summary>
        /// <param name="bytes">The byte array containing the data to add to the buffer. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the buffer to be added before timing out.</param>
        /// <returns>A ValueTask that represents the asynchronous operation.</returns>
        public ValueTask AddBuffer(byte[] bytes, TimeSpan timeOut)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", timeOut, bytes.Select(s => s));
        }

        /// <summary>
        /// Asynchronously adds a buffer of bytes to the current download operation.
        /// </summary>
        /// <remarks>This method is typically used in conjunction with other download-related methods to
        /// stream file data to the client in multiple parts. The operation completes when the buffer has been
        /// successfully passed to the JavaScript runtime.</remarks>
        /// <param name="bytes">The sequence of bytes to add to the download buffer. Cannot be null.</param>
        /// <returns>A ValueTask that represents the asynchronous operation.</returns>
        public ValueTask AddBuffer(IEnumerable<byte> bytes)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", bytes);
        }

        /// <summary>
        /// Asynchronously adds a buffer of bytes to the download stream.
        /// </summary>
        /// <param name="bytes">The sequence of bytes to add to the download buffer. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Passing a canceled token will attempt to cancel the operation.</param>
        /// <returns>A ValueTask that represents the asynchronous operation.</returns>
        public ValueTask AddBuffer(IEnumerable<byte> bytes, CancellationToken cancellationToken)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", cancellationToken, bytes);
        }

        /// <summary>
        /// Asynchronously adds a buffer of bytes to the download queue with a specified timeout.
        /// </summary>
        /// <param name="bytes">The sequence of bytes to add to the download buffer. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the buffer to be added before the operation times out.</param>
        /// <returns>A ValueTask that represents the asynchronous operation.</returns>
        public ValueTask AddBuffer(IEnumerable<byte> bytes, TimeSpan timeOut)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", timeOut, bytes);
        }

        /// <summary>
        /// Asynchronously adds the contents of the specified stream as a buffer to the download queue.
        /// </summary>
        /// <remarks>The method reads the entire contents of the provided stream. The caller is
        /// responsible for disposing the stream after the operation completes.</remarks>
        /// <param name="stream">The stream containing the data to be added as a buffer. The stream must be readable and positioned at the
        /// start of the data to be sent.</param>
        /// <returns>A ValueTask that represents the asynchronous operation.</returns>
        public ValueTask AddBuffer(Stream stream)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", stream.ToByteArrayAsync());
        }

        /// <summary>
        /// Asynchronously adds the contents of the specified stream to the download buffer.
        /// </summary>
        /// <remarks>The method reads the entire content of the provided stream and adds it to the buffer
        /// for subsequent download operations. The stream is not disposed by this method; callers are responsible for
        /// managing the stream's lifetime.</remarks>
        /// <param name="stream">The stream containing the data to add to the buffer. The stream must be readable and positioned at the start
        /// of the data to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A ValueTask that represents the asynchronous operation.</returns>
        public ValueTask AddBuffer(Stream stream, CancellationToken cancellationToken)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", cancellationToken, stream.ToByteArrayAsync(cancellationToken));
        }

        /// <summary>
        /// Adds the contents of the specified stream as a buffer for download via JavaScript interop.
        /// </summary>
        /// <param name="stream">The stream containing the data to be added as a download buffer. The stream must be readable.</param>
        /// <param name="streamReadcancellationToken">A cancellation token that can be used to cancel the asynchronous read operation from the stream.</param>
        /// <param name="timeOutJavaScript">The maximum duration to wait for the JavaScript interop call to complete before timing out.</param>
        /// <returns>A ValueTask that represents the asynchronous operation of adding the buffer. The task completes when the
        /// buffer has been successfully pushed to JavaScript.</returns>
        public ValueTask AddBuffer(Stream stream, CancellationToken streamReadcancellationToken, TimeSpan timeOutJavaScript)
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", timeOutJavaScript, stream.ToByteArrayAsync(streamReadcancellationToken));
        }

        /// <summary>
        /// Determines whether any buffer is available for download in the current Blazor session.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if a buffer is
        /// available; otherwise, <see langword="false"/>.</returns>
        public ValueTask<bool> AnyBuffer()
        {
            return JSRuntime.InvokeAsync<bool>("_blazorDownloadFileAnyBuffer");
        }

        /// <summary>
        /// Gets the number of file buffers currently managed by the JavaScript runtime.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of file buffers
        /// available. The value is zero if no buffers are present.</returns>
        public ValueTask<int> BuffersCount()
        {
            return JSRuntime.InvokeAsync<int>("_blazorDownloadFileBuffersCount");
        }

        /// <summary>
        /// Clears any temporary file buffers used for download operations in the current browser session.
        /// </summary>
        /// <remarks>Call this method to release resources or reset the download state before starting new
        /// file downloads. This method is typically used in Blazor applications that manage file downloads via
        /// JavaScript interop.</remarks>
        /// <returns>A ValueTask that represents the asynchronous clear operation.</returns>
        public ValueTask ClearBuffers()
        {
            return JSRuntime.InvokeVoidAsync("_blazorDownloadFileClearBuffer");
        }

        /// <summary>
        /// Initiates a file download by requesting the file as a sequence of Base64-encoded buffers from the browser.
        /// </summary>
        /// <remarks>This method relies on a JavaScript interop call to perform the download in the
        /// browser. The file is transferred in partitioned Base64-encoded buffers, which may be useful for large files
        /// or environments with limited memory.</remarks>
        /// <param name="fileName">The name of the file to be downloaded. This value is used as the suggested file name in the browser's
        /// download dialog. Cannot be null or empty.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the download,
        /// including its success or failure.</returns>
        public ValueTask<DownloadFileResult> DownloadBase64Buffers(string fileName, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64StringPartitioned", fileName, contentType);
        }

        /// <summary>
        /// Initiates a client-side download of a file represented as a base64-encoded buffer using JavaScript interop.
        /// </summary>
        /// <remarks>This method relies on JavaScript interop to perform the file download in the browser.
        /// The file content must be available as a base64-encoded string on the client side. The method is typically
        /// used in Blazor applications to enable file downloads without requiring a server round-trip.</remarks>
        /// <param name="fileName">The name of the file to be downloaded. This value is used as the suggested file name in the browser's
        /// download dialog. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the download operation.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// download, including its success or failure.</returns>
        public ValueTask<DownloadFileResult> DownloadBase64Buffers(string fileName, CancellationToken cancellationToken, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64StringPartitioned", cancellationToken, fileName, contentType);
        }

        /// <summary>
        /// Initiates a download of a file represented as partitioned Base64-encoded buffers using the specified file
        /// name and content type.
        /// </summary>
        /// <remarks>This method is intended for use in Blazor applications to download large files by
        /// partitioning them into Base64-encoded buffers. The operation may fail if the file is too large for the
        /// browser to handle or if the timeout is exceeded.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous download operation. The task result contains a <see
        /// cref="DownloadFileResult"/> indicating the outcome of the download.</returns>
        public ValueTask<DownloadFileResult> DownloadBase64Buffers(string fileName, TimeSpan timeOut, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64StringPartitioned", timeOut, fileName, contentType);
        }

        /// <summary>
        /// Initiates the download of a file as a binary buffer using the specified file name and content type.
        /// </summary>
        /// <remarks>This method is intended for use in Blazor applications and relies on JavaScript
        /// interop to perform the file download. The download is initiated on the client side, and the method returns
        /// immediately with a task representing the operation.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// download, including its success or failure.</returns>
        public ValueTask<DownloadFileResult> DownloadBinaryBuffers(string fileName, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", fileName, contentType);
        }

        /// <summary>
        /// Initiates the download of a file as a sequence of binary buffers using the specified file name and content
        /// type.
        /// </summary>
        /// <remarks>This method is intended for use in Blazor applications where large files are
        /// downloaded in partitioned binary buffers to improve performance and reliability. The operation may be
        /// cancelled by passing a triggered cancellation token.</remarks>
        /// <param name="fileName">The name of the file to be downloaded. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the download operation.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// downloaded file.</returns>
        public ValueTask<DownloadFileResult> DownloadBinaryBuffers(string fileName, CancellationToken cancellationToken, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", cancellationToken, fileName, contentType);
        }

        /// <summary>
        /// Initiates the download of a file as a sequence of binary buffers using the specified file name, timeout, and
        /// content type.
        /// </summary>
        /// <param name="fileName">The name of the file to be downloaded. Cannot be null or empty.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// downloaded file.</returns>
        public ValueTask<DownloadFileResult> DownloadBinaryBuffers(string fileName, TimeSpan timeOut, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", timeOut, fileName, contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using the specified file name, content, and MIME type.
        /// </summary>
        /// <remarks>This method relies on JavaScript interop to trigger the file download in the user's
        /// browser. The download may be subject to browser security restrictions or user settings.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytesBase64">A base64-encoded string representing the file's binary content. Cannot be null or empty.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the outcome of
        /// the download request.</returns>
        public ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64String", fileName, bytesBase64, contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using the specified file name, content, and MIME type.
        /// </summary>
        /// <remarks>This method relies on JavaScript interop to trigger the file download in the user's
        /// browser. The download may be subject to browser security restrictions or user settings.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. This value is displayed to the user in the browser's download
        /// dialog.</param>
        /// <param name="bytesBase64">A base64-encoded string representing the file's binary content. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the download operation.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the outcome of
        /// the download request.</returns>
        public ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, CancellationToken cancellationToken, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64String", cancellationToken, fileName, bytesBase64, contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using a Base64-encoded string as the file content.
        /// </summary>
        /// <remarks>This method relies on JavaScript interop to trigger the file download in the user's
        /// browser. The download will prompt the user to save the file with the specified name and content type. If the
        /// operation exceeds the specified timeout, it may fail.</remarks>
        /// <param name="fileName">The name of the file to be downloaded, including the file extension. Cannot be null or empty.</param>
        /// <param name="bytesBase64">A Base64-encoded string representing the file's binary content. Cannot be null or empty.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete before timing out.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download request.</returns>
        public ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, TimeSpan timeOut, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64String", timeOut, fileName, bytesBase64, contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using the specified file name, content bytes, and content type.
        /// </summary>
        /// <remarks>This method uses JavaScript interop to trigger a file download in the user's browser.
        /// The operation completes when the browser has processed the download request, but does not guarantee that the
        /// user has saved the file.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The file contents as a byte array. Cannot be null.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the download
        /// request.</returns>
        public ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64String", fileName, Convert.ToBase64String(bytes), contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using the specified file name, content, and MIME type.
        /// </summary>
        /// <remarks>This method uses JavaScript interop to trigger the file download in the user's
        /// browser. The operation may fail if the browser blocks downloads or if JavaScript is disabled.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The file content as a byte array. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the download
        /// request.</returns>
        public ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, CancellationToken cancellationToken, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64String", cancellationToken, fileName, Convert.ToBase64String(bytes), contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using the specified file name, content, and content type.
        /// </summary>
        /// <remarks>This method relies on JavaScript interop to trigger the file download in the user's
        /// browser. The operation may fail if the browser blocks downloads or if JavaScript is disabled.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The byte array containing the file's content to be downloaded. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the download request.</returns>
        public ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, TimeSpan timeOut, string contentType = "application/octet-stream")
        {
            return JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64String", timeOut, fileName, Convert.ToBase64String(bytes), contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using the specified file name, content, and MIME type.
        /// </summary>
        /// <remarks>This method is intended for use in Blazor applications to trigger client-side file
        /// downloads. The download is performed in the user's browser and may be subject to browser-specific
        /// limitations or user permissions.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The file content as a sequence of bytes to be downloaded. Cannot be null.</param>
        /// <param name="contentType">The MIME type of the file. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="DownloadFileResult"/> indicating the outcome of the download request.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, string contentType = "application/octet-stream")
        {
            await ClearBuffers();
            await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", bytes);
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", fileName, contentType);
        }

        /// <summary>
        /// Initiates a file download in the user's browser using the specified file name, content, and content type.
        /// </summary>
        /// <remarks>This method uses JavaScript interop to trigger the file download in the browser. The
        /// operation may be subject to browser security restrictions or user interaction requirements.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. This value is displayed to the user in the browser's download
        /// dialog.</param>
        /// <param name="bytes">The file content to download, provided as a sequence of bytes. Must not be null.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the download operation.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="DownloadFileResult"/> indicating the outcome of the download request.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, CancellationToken cancellationToken, string contentType = "application/octet-stream")
        {
            await ClearBuffers();
            await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", cancellationToken, bytes);
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", cancellationToken, fileName, contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using the specified file name, content bytes, and content type,
        /// with a configurable timeout.
        /// </summary>
        /// <remarks>This method is intended for use in Blazor applications and relies on JavaScript
        /// interop to trigger the browser's file download functionality. The download may fail if the user cancels the
        /// operation or if the browser blocks the download. Ensure that the provided byte sequence is not excessively
        /// large, as browser and platform limitations may apply.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. This value is used as the file name presented to the user in the
        /// browser's download dialog. Cannot be null or empty.</param>
        /// <param name="bytes">The file content to download, provided as a sequence of bytes. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete before timing out.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="DownloadFileResult"/> indicating the outcome of the download request.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, TimeSpan timeOut, string contentType = "application/octet-stream")
        {
            await ClearBuffers();
            await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", timeOut, bytes);
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", timeOut, fileName, contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using the specified file name, content stream, and content type.
        /// </summary>
        /// <remarks>This method is intended for use in Blazor applications to trigger file downloads on
        /// the client side. The entire content of the provided stream is read and sent to the browser. Large files may
        /// impact performance due to in-memory processing.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="stream">The stream containing the file data to download. The stream must be readable and positioned at the start of
        /// the content to download.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="DownloadFileResult"/> indicating the outcome of the download request.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, string contentType = "application/octet-stream")
        {
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64String", fileName, Convert.ToBase64String(await stream.ToByteArrayAsync()), contentType);
        }

        /// <summary>
        /// Downloads a file to the user's browser using the specified stream and file name.
        /// </summary>
        /// <remarks>This method uses JavaScript interop to trigger a file download in the user's browser.
        /// The entire stream is read into memory before initiating the download. For large files, consider the memory
        /// implications.</remarks>
        /// <param name="fileName">The name of the file to be downloaded. This value is used as the suggested file name in the browser's
        /// download dialog. Cannot be null or empty.</param>
        /// <param name="stream">The stream containing the file data to be downloaded. The stream is read to completion. Cannot be null.</param>
        /// <param name="cancellationTokenBytesRead">A cancellation token that can be used to cancel reading the stream data.</param>
        /// <param name="cancellationTokenJavaScriptInterop">A cancellation token that can be used to cancel the JavaScript interop operation.</param>
        /// <param name="contentType">The MIME type of the file content. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous download operation. The task result contains a DownloadFileResult
        /// indicating the outcome of the download request.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, CancellationToken cancellationTokenBytesRead, CancellationToken cancellationTokenJavaScriptInterop, string contentType = "application/octet-stream")
        {
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64String", cancellationTokenJavaScriptInterop, fileName, Convert.ToBase64String(await stream.ToByteArrayAsync(cancellationTokenBytesRead)), contentType);
        }

        /// <summary>
        /// Downloads a file to the client by streaming its contents via JavaScript interop.
        /// </summary>
        /// <remarks>This method uses JavaScript interop to trigger a file download in the user's browser.
        /// The file data is read from the provided stream, encoded as a Base64 string, and sent to the client. Ensure
        /// that the stream's position is set appropriately before calling this method. The method may throw exceptions
        /// if the JavaScript interop call fails or if the stream cannot be read.</remarks>
        /// <param name="fileName">The name of the file to be presented to the user for download. This value is used as the suggested file name
        /// in the browser's download dialog.</param>
        /// <param name="stream">The stream containing the file data to be downloaded. The stream is read in its entirety and must be
        /// readable and seekable.</param>
        /// <param name="cancellationTokenBytesRead">A cancellation token that can be used to cancel the asynchronous read operation from the stream.</param>
        /// <param name="timeOutJavaScriptInterop">The maximum duration to wait for the JavaScript interop call to complete before timing out.</param>
        /// <param name="contentType">The MIME type of the file being downloaded. Defaults to "application/octet-stream" if not specified.</param>
        /// <returns>A task that represents the asynchronous download operation. The task result contains a <see
        /// cref="DownloadFileResult"/> indicating the outcome of the download request.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, CancellationToken cancellationTokenBytesRead, TimeSpan timeOutJavaScriptInterop, string contentType = "application/octet-stream")
        {
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64String", timeOutJavaScriptInterop, fileName, Convert.ToBase64String(await stream.ToByteArrayAsync(cancellationTokenBytesRead)), contentType);
        }

        /// <summary>
        /// Initiates a file download from the specified plain text content, encoding the text using the provided
        /// encoding and content type.
        /// </summary>
        /// <param name="fileName">The name of the file to be presented to the user for download. Cannot be null or empty.</param>
        /// <param name="plainText">The plain text content to include in the downloaded file. Cannot be null.</param>
        /// <param name="encoding">The character encoding to use when converting the plain text to bytes. Cannot be null.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. Defaults to "text/plain" if not specified.</param>
        /// <param name="encoderShouldEmitIdentifier">true to emit a byte order mark (BOM) in the encoded file; otherwise, false.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the file
        /// download, including the file name and content.</returns>
        public ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, string contentType = "text/plain", bool encoderShouldEmitIdentifier = false)
        {
            return DownloadFile(fileName, plainText.ToBase64Encode(encoding, encoderShouldEmitIdentifier), contentType);
        }

        /// <summary>
        /// Initiates a file download from the specified plain text content, encoding it as a file with the given name
        /// and content type.
        /// </summary>
        /// <remarks>The method encodes the provided plain text using the specified encoding before
        /// initiating the download. If the encoding is UTF-8 and encoderShouldEmitUTF8Identifier is true, a BOM will be
        /// included at the start of the file. The caller is responsible for ensuring that fileName and plainText are
        /// valid and non-null.</remarks>
        /// <param name="fileName">The name of the file to be downloaded, including the file extension. Cannot be null or empty.</param>
        /// <param name="plainText">The plain text content to include in the downloaded file. Cannot be null.</param>
        /// <param name="encoding">The character encoding to use when converting the plain text to bytes. Cannot be null.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the download operation.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. Defaults to "text/plain" if not specified.</param>
        /// <param name="encoderShouldEmitUTF8Identifier">true to emit a UTF-8 byte order mark (BOM) in the encoded file; otherwise, false.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// file download, including the file name and content type.</returns>
        public ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, CancellationToken cancellationToken, string contentType = "text/plain", bool encoderShouldEmitUTF8Identifier = false)
        {
            return DownloadFile(fileName, plainText.ToBase64Encode(encoding, encoderShouldEmitUTF8Identifier), cancellationToken, contentType);
        }

        /// <summary>
        /// Initiates a file download from the specified plain text content using the given encoding and content type.
        /// </summary>
        /// <remarks>If the specified encoding is UTF-8 and encoderShouldEmitUTF8Identifier is true, a BOM
        /// will be included at the start of the file. The method encodes the plain text content before initiating the
        /// download. Ensure that fileName and plainText are valid to avoid errors during the download
        /// process.</remarks>
        /// <param name="fileName">The name of the file to be presented to the user for download. Cannot be null or empty.</param>
        /// <param name="plainText">The plain text content to include in the downloaded file. Cannot be null.</param>
        /// <param name="encoding">The character encoding to use when converting the plain text to bytes. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete before timing out.</param>
        /// <param name="contentType">The MIME type to set for the downloaded file. Defaults to "text/plain" if not specified.</param>
        /// <param name="encoderShouldEmitUTF8Identifier">true to include a UTF-8 byte order mark (BOM) in the encoded file if using UTF-8 encoding; otherwise, false.</param>
        /// <returns>A ValueTask that represents the asynchronous operation. The result contains information about the file
        /// download, including status and file metadata.</returns>
        public ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, TimeSpan timeOut, string contentType = "text/plain", bool encoderShouldEmitUTF8Identifier = false)
        {
            return DownloadFile(fileName, plainText.ToBase64Encode(encoding, encoderShouldEmitUTF8Identifier), timeOut, contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using a base64-encoded string, with optional progress reporting and
        /// configurable buffer size.
        /// </summary>
        /// <remarks>This method partitions the base64-encoded file content into smaller buffers for
        /// efficient transfer to the browser. It is suitable for downloading large files in Blazor applications. The
        /// method does not validate the base64 string; callers should ensure the input is valid. The download is
        /// initiated on the client side using JavaScript interop.</remarks>
        /// <param name="fileName">The name of the file to be downloaded, including the file extension. This value is used as the suggested
        /// filename in the browser's download dialog.</param>
        /// <param name="bytesBase64">A base64-encoded string representing the file's binary content. Must not be null or empty.</param>
        /// <param name="bufferSize">The size, in bytes, of each chunk to be sent to the browser. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the current download progress as a percentage (from
        /// 0.0 to 100.0). If null, progress is not reported.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DownloadFileResult indicating
        /// the outcome of the download request.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var bytesReaded = 0;
            foreach (var partFile in Partition(bytesBase64, bufferSize))
            {
                bytesReaded += partFile.Count;
                await InvokeNullableProgressTaskAsync(bytesReaded, bytesBase64.Length, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", string.Join("", partFile));
            }
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64StringPartitioned", fileName, contentType);
        }

        /// <summary>
        /// Downloads a file to the client by streaming a Base64-encoded file in partitioned buffers.
        /// </summary>
        /// <remarks>This method streams large files efficiently by partitioning the Base64-encoded
        /// content into smaller buffers, reducing memory usage and improving responsiveness for large downloads. The
        /// method is intended for use in Blazor applications where files must be transferred from server to client via
        /// JavaScript interop.</remarks>
        /// <param name="fileName">The name of the file to be downloaded by the client. This value is used as the suggested file name in the
        /// browser's download dialog.</param>
        /// <param name="bytesBase64">A Base64-encoded string representing the file's binary content. Must not be null or empty.</param>
        /// <param name="cancellationToken">A token that can be used to request cancellation of the download operation.</param>
        /// <param name="bufferSize">The size, in bytes, of each buffer used to partition the Base64 string. Must be a positive integer. The
        /// default is 32,768.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the current progress percentage (from 0.0 to 100.0)
        /// as the file is streamed. If null, progress updates are not reported.</param>
        /// <returns>A task that represents the asynchronous download operation. The task result contains a DownloadFileResult
        /// indicating the outcome of the download.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, CancellationToken cancellationToken, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var bytesReaded = 0;
            foreach (var partFile in Partition(bytesBase64, bufferSize))
            {
                bytesReaded += partFile.Count;
                await InvokeNullableProgressTaskAsync(bytesReaded, bytesBase64.Length, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", cancellationToken, string.Join("", partFile));
            }
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64StringPartitioned", cancellationToken, fileName, contentType);
        }

        /// <summary>
        /// Downloads a file to the client by streaming a Base64-encoded byte sequence using partitioned buffers.
        /// </summary>
        /// <remarks>This method streams large files to the client in multiple parts to avoid browser or
        /// memory limitations. The download is performed using JavaScript interop and may be subject to browser
        /// security restrictions. If the operation exceeds the specified timeout, it may fail.</remarks>
        /// <param name="fileName">The name of the file to be downloaded by the client. This value is used as the suggested file name in the
        /// browser's download dialog.</param>
        /// <param name="bytesBase64">A Base64-encoded string representing the file's binary content. Must not be null or empty.</param>
        /// <param name="timeOut">The maximum duration to wait for each JavaScript interop operation before timing out.</param>
        /// <param name="bufferSize">The size, in bytes, of each buffer partition used to stream the file. Must be a positive integer. The
        /// default is 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the current progress percentage (from 0.0 to 100.0)
        /// as the file is streamed. If null, progress updates are not reported.</param>
        /// <returns>A task that represents the asynchronous download operation. The task result contains a DownloadFileResult
        /// indicating the outcome of the download.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, string bytesBase64, TimeSpan timeOut, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var bytesReaded = 0;
            foreach (var partFile in Partition(bytesBase64, bufferSize))
            {
                bytesReaded += partFile.Count;
                await InvokeNullableProgressTaskAsync(bytesReaded, bytesBase64.Length, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", timeOut, string.Join("", partFile));
            }
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileBase64StringPartitioned", timeOut, fileName, contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using the specified file name and byte array content.
        /// </summary>
        /// <remarks>This method is intended for use in Blazor applications to enable large file downloads
        /// by partitioning the file data and sending it in multiple chunks. The method uses JavaScript interop to
        /// perform the download in the browser. The caller should ensure that the byte array size and buffer size are
        /// appropriate for the user's browser and network conditions.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. This value is displayed to the user in the browser's download
        /// dialog.</param>
        /// <param name="bytes">The byte array containing the file data to be downloaded. Cannot be null.</param>
        /// <param name="bufferSize">The size, in bytes, of each chunk to send to the browser. Must be a positive integer. The default is 32,768
        /// bytes.</param>
        /// <param name="contentType">The MIME type of the file content. If not specified, defaults to "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the current download progress as a value between 0.0
        /// and 1.0. If null, progress updates are not reported.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DownloadFileResult indicating
        /// the outcome of the download request.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var bytesReaded = 0;
            foreach (var partFile in Partition(bytes, bufferSize))
            {
                bytesReaded += partFile.Count;
                await InvokeNullableProgressTaskAsync(bytesReaded, bytes.Length, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", partFile);
            }
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", fileName, contentType);
        }

        /// <summary>
        /// Initiates a file download in the browser using the specified file name and byte array content, with optional
        /// progress reporting and content type.
        /// </summary>
        /// <remarks>This method partitions the file data into buffers and streams them to the browser for
        /// download. It is suitable for large files, as it avoids loading the entire file into memory at once. The
        /// method can be canceled via the provided cancellation token. If progress reporting is enabled, the callback
        /// is invoked after each buffer is sent.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The byte array containing the file data to be downloaded. Cannot be null.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation is canceled if the token is triggered.</param>
        /// <param name="bufferSize">The size, in bytes, of each buffer chunk sent to the browser. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that receives the download progress as a value between 0.0 and 1.0. If
        /// null, progress is not reported.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DownloadFileResult indicating
        /// the outcome of the download request.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, CancellationToken cancellationToken, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var bytesReaded = 0;
            foreach (var partFile in Partition(bytes, bufferSize))
            {
                bytesReaded += partFile.Count;
                await InvokeNullableProgressTaskAsync(bytesReaded, bytes.Length, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", cancellationToken, partFile);
            }
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", cancellationToken, fileName, contentType);
        }

        /// <summary>
        /// Downloads a file to the client by streaming the specified byte array using a partitioned approach.
        /// </summary>
        /// <remarks>This method streams large files to the client in multiple parts to avoid browser or
        /// memory limitations. The operation uses JavaScript interop and may be subject to browser restrictions. If the
        /// download is interrupted or cancelled, the returned result will indicate the outcome.</remarks>
        /// <param name="fileName">The name of the file to be downloaded by the client. This value is used as the suggested file name in the
        /// browser's download dialog.</param>
        /// <param name="bytes">The file data to download, provided as a byte array. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for each JavaScript interop operation before timing out.</param>
        /// <param name="bufferSize">The size, in bytes, of each data chunk sent to the client. Must be a positive integer. The default is 32,768
        /// bytes.</param>
        /// <param name="contentType">The MIME type of the file being downloaded. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the current progress percentage (from 0.0 to 100.0)
        /// as the file is streamed. If null, progress updates are not reported.</param>
        /// <returns>A task that represents the asynchronous download operation. The result contains information about the
        /// outcome of the file download.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, byte[] bytes, TimeSpan timeOut, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var bytesReaded = 0;
            foreach (var partFile in Partition(bytes, bufferSize))
            {
                bytesReaded += partFile.Count;
                await InvokeNullableProgressTaskAsync(bytesReaded, bytes.Length, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", timeOut, partFile);
            }
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", timeOut, fileName, contentType);
        }

        /// <summary>
        /// Asynchronously downloads a file to the client by streaming its contents in buffered partitions.
        /// </summary>
        /// <remarks>This method streams large files in buffered chunks to optimize memory usage and
        /// support efficient downloads. The method is intended for use in Blazor applications where files are
        /// downloaded from server to client via JavaScript interop.</remarks>
        /// <param name="fileName">The name of the file to be downloaded by the client. This value is used as the suggested filename in the
        /// browser's download dialog.</param>
        /// <param name="bytes">The file content to download, provided as a sequence of bytes. The entire sequence will be streamed to the
        /// client.</param>
        /// <param name="bufferSize">The size, in bytes, of each buffer partition used when streaming the file. Must be a positive integer. The
        /// default is 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file being downloaded. Defaults to "application/octet-stream" if not specified.</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the current progress percentage (from 0.0 to 100.0)
        /// as the file is streamed. If null, progress updates are not reported.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DownloadFileResult indicating
        /// the outcome of the download operation.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var bytesReaded = 0;
            var bytesLength = bytes.Count();
            foreach (var partFile in Partition(bytes, bufferSize))
            {
                bytesReaded += partFile.Count;
                await InvokeNullableProgressTaskAsync(bytesReaded, bytesLength, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", partFile);
            }
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", fileName, contentType);
        }

        /// <summary>
        /// Downloads a file to the user's browser using the specified file name and content bytes.
        /// </summary>
        /// <remarks>This method is intended for use in Blazor applications to enable file downloads from
        /// in-memory data. The download is performed in buffered chunks to support large files and to provide progress
        /// updates if a callback is supplied. The method relies on JavaScript interop and requires a compatible browser
        /// environment.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. This value is displayed to the user in the browser's download
        /// dialog.</param>
        /// <param name="bytes">The file content as a sequence of bytes to be downloaded.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the download operation.</param>
        /// <param name="bufferSize">The size, in bytes, of each buffer chunk sent to the browser. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the current progress percentage (from 0.0 to 100.0)
        /// as the file is downloaded. If null, progress updates are not reported.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DownloadFileResult indicating
        /// the outcome of the download.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, CancellationToken cancellationToken, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var bytesReaded = 0;
            var bytesLength = bytes.Count();
            foreach (var partFile in Partition(bytes, bufferSize))
            {
                bytesReaded += partFile.Count;
                await InvokeNullableProgressTaskAsync(bytesReaded, bytesLength, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", cancellationToken, partFile);
            }
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", cancellationToken, fileName, contentType);
        }

        /// <summary>
        /// Asynchronously initiates a file download in the browser using the specified file name, content, and options.
        /// </summary>
        /// <remarks>This method streams the file to the browser in multiple chunks to support large files
        /// and avoid memory issues. The operation relies on JavaScript interop and must be called from a Blazor context
        /// that supports JS interop. If the user cancels the download or a timeout occurs, the result will indicate the
        /// failure.</remarks>
        /// <param name="fileName">The name to assign to the downloaded file. Cannot be null or empty.</param>
        /// <param name="bytes">The file content as a sequence of bytes to be downloaded. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for each JavaScript interop operation before timing out.</param>
        /// <param name="bufferSize">The size, in bytes, of each chunk sent to the browser. Must be a positive integer. The default is 32,768
        /// bytes.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional asynchronous callback invoked with the current progress percentage (from 0.0 to 100.0) after
        /// each chunk is sent. If null, progress updates are not reported.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DownloadFileResult indicating
        /// the outcome of the download request.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, IEnumerable<byte> bytes, TimeSpan timeOut, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var bytesReaded = 0;
            var bytesLength = bytes.Count();
            foreach (var partFile in Partition(bytes, bufferSize))
            {
                bytesReaded += partFile.Count;
                await InvokeNullableProgressTaskAsync(bytesReaded, bytesLength, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", timeOut, partFile);
            }
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", timeOut, fileName, contentType);
        }

        /// <summary>
        /// Asynchronously downloads a file to the client by streaming its contents in buffered segments.
        /// </summary>
        /// <remarks>The method streams the file in multiple buffered segments to optimize memory usage
        /// and support large files. The download is initiated on the client side using JavaScript interop. The provided
        /// stream must remain open and readable for the duration of the operation.</remarks>
        /// <param name="fileName">The name of the file to be presented to the user for download. This value is used as the suggested filename
        /// in the browser's download dialog.</param>
        /// <param name="stream">The stream containing the file data to be downloaded. The stream must be readable and support seeking.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used to read and transmit file data in each segment. Must be a positive
        /// integer. The default is 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file to be sent to the client. Defaults to "application/octet-stream" if not specified.</param>
        /// <param name="progress">An optional asynchronous callback that is invoked with the current progress percentage (from 0.0 to 100.0)
        /// as the file is streamed. If null, progress updates are not reported.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DownloadFileResult indicating
        /// the outcome of the download operation.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var totalOfBytes = (int)stream.Length;
            var totalOfBytesReaded = 0;
            var pendingBytesToRead = totalOfBytes;
            do
            {
                var currentBufferSize = bufferSize > totalOfBytes ? totalOfBytes : bufferSize > pendingBytesToRead ? pendingBytesToRead : bufferSize;
                var buffer = new byte[currentBufferSize];
                totalOfBytesReaded += await stream.ReadAsync(buffer, 0, currentBufferSize);
                pendingBytesToRead -= totalOfBytesReaded;
                await InvokeNullableProgressTaskAsync(totalOfBytesReaded, totalOfBytes, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", buffer.Select(b => b));
            } while (pendingBytesToRead > 0);
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", fileName, contentType);
        }

        /// <summary>
        /// Downloads a file to the client by reading from the specified stream and sending the data in buffered chunks
        /// via JavaScript interop.
        /// </summary>
        /// <remarks>The method reads the entire content of the provided stream and sends it to the client
        /// in buffered segments using JavaScript interop. The stream's position is assumed to be at the start of the
        /// file data. The method supports cancellation for both the stream reading and the JavaScript interop phases.
        /// If a progress callback is provided, it is invoked after each buffer is sent to the client. This method is
        /// intended for use in Blazor applications where file download functionality is implemented via JavaScript
        /// interop.</remarks>
        /// <param name="fileName">The name of the file to be presented to the user for download. This value is used as the suggested filename
        /// in the browser's download dialog.</param>
        /// <param name="stream">The stream containing the file data to download. The stream must be readable and support seeking.</param>
        /// <param name="cancellationTokenBytesRead">A cancellation token that can be used to cancel the reading of bytes from the stream.</param>
        /// <param name="cancellationTokenJavaScriptInterop">A cancellation token that can be used to cancel the JavaScript interop operations involved in the download
        /// process.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used to read from the stream and send data to the client. Must be a
        /// positive integer. The default is 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional callback that is invoked with the current progress percentage (from 0.0 to 100.0) as the file is
        /// downloaded. If null, progress updates are not reported.</param>
        /// <returns>A task that represents the asynchronous download operation. The task result contains a DownloadFileResult
        /// indicating the outcome of the download.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, CancellationToken cancellationTokenBytesRead, CancellationToken cancellationTokenJavaScriptInterop, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var totalOfBytes = (int)stream.Length;
            var totalOfBytesReaded = 0;
            var pendingBytesToRead = totalOfBytes;
            do
            {
                var currentBufferSize = bufferSize > totalOfBytes ? totalOfBytes : bufferSize > pendingBytesToRead ? pendingBytesToRead : bufferSize;
                var buffer = new byte[currentBufferSize];
                totalOfBytesReaded += await stream.ReadAsync(buffer, 0, currentBufferSize, cancellationTokenBytesRead);
                pendingBytesToRead -= totalOfBytesReaded;
                await InvokeNullableProgressTaskAsync(totalOfBytesReaded, totalOfBytes, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", cancellationTokenJavaScriptInterop, buffer.Select(b => b));
            } while (pendingBytesToRead > 0);
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", cancellationTokenJavaScriptInterop, fileName, contentType);
        }

        /// <summary>
        /// Downloads a file to the client by streaming its contents in buffered segments using JavaScript interop.
        /// </summary>
        /// <remarks>This method streams the file to the client in multiple buffered segments to support
        /// large files and efficient memory usage. The download is performed via JavaScript interop, and the operation
        /// may be subject to browser limitations. The method does not close or dispose the provided stream.</remarks>
        /// <param name="fileName">The name of the file to be downloaded by the client. This value is used as the suggested file name in the
        /// browser's download dialog.</param>
        /// <param name="stream">The stream containing the file data to be sent to the client. The stream must be readable and its length
        /// must be known.</param>
        /// <param name="cancellationTokenBytesRead">A cancellation token that can be used to cancel the read operation from the stream.</param>
        /// <param name="timeOutJavaScriptInterop">The maximum duration to wait for each JavaScript interop call before timing out.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used to read and send data segments. Must be a positive integer. The
        /// default is 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file being downloaded. The default is "application/octet-stream".</param>
        /// <param name="progress">An optional callback that is invoked with the current progress percentage (from 0.0 to 100.0) as the file is
        /// downloaded. If null, progress updates are not reported.</param>
        /// <returns>A task that represents the asynchronous download operation. The task result contains a DownloadFileResult
        /// indicating the outcome of the download.</returns>
        public async ValueTask<DownloadFileResult> DownloadFile(string fileName, Stream stream, CancellationToken cancellationTokenBytesRead, TimeSpan timeOutJavaScriptInterop, int bufferSize = 32768, string contentType = "application/octet-stream", Func<double, Task> progress = null)
        {
            await ClearBuffers();
            var totalOfBytes = (int)stream.Length;
            var totalOfBytesReaded = 0;
            var pendingBytesToRead = totalOfBytes;
            do
            {
                var currentBufferSize = bufferSize > totalOfBytes ? totalOfBytes : bufferSize > pendingBytesToRead ? pendingBytesToRead : bufferSize;
                var buffer = new byte[currentBufferSize];
                totalOfBytesReaded += await stream.ReadAsync(buffer, 0, currentBufferSize, cancellationTokenBytesRead);
                pendingBytesToRead -= totalOfBytesReaded;
                await InvokeNullableProgressTaskAsync(totalOfBytesReaded, totalOfBytes, progress);
                await JSRuntime.InvokeVoidAsync("_blazorDownloadFileBuffersPush", timeOutJavaScriptInterop, buffer.Select(b => b));
            } while (pendingBytesToRead > 0);
            return await JSRuntime.InvokeAsync<DownloadFileResult>("_blazorDowloadFileByteArrayPartitioned", timeOutJavaScriptInterop, fileName, contentType);
        }

        /// <summary>
        /// Initiates a file download from the specified plain text content, encoding it as a file with the given name
        /// and content type.
        /// </summary>
        /// <remarks>This method encodes the provided plain text using the specified encoding and streams
        /// it to the client as a downloadable file. Use the progress callback to monitor download progress, especially
        /// for large files.</remarks>
        /// <param name="fileName">The name of the file to be downloaded, including the file extension. Cannot be null or empty.</param>
        /// <param name="plainText">The plain text content to include in the downloaded file. Cannot be null.</param>
        /// <param name="encoding">The character encoding to use when converting the plain text to bytes. Cannot be null.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used during the download operation. Must be a positive integer. The
        /// default is 32,768 bytes.</param>
        /// <param name="contentType">The MIME type to set for the downloaded file. The default is "text/plain".</param>
        /// <param name="progress">An optional callback that is invoked with the download progress as a value between 0.0 and 1.0. If null,
        /// progress updates are not reported.</param>
        /// <param name="encoderShouldEmitUTF8Identifier">true to emit a UTF-8 byte order mark (BOM) at the start of the file if the encoding is UTF-8; otherwise,
        /// false. The default is false.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// completed file download.</returns>
        public ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, int bufferSize = 32768, string contentType = "text/plain", Func<double, Task> progress = null, bool encoderShouldEmitUTF8Identifier = false)
        {
            return DownloadFile(fileName, plainText.ToBase64Encode(encoding, encoderShouldEmitUTF8Identifier), bufferSize, contentType, progress);
        }

        /// <summary>
        /// Initiates a file download containing the specified plain text, using the given encoding and content type.
        /// </summary>
        /// <remarks>If the specified encoding is UTF-8 and encoderShouldEmitUTF8Identifier is true, a
        /// UTF-8 BOM will be included at the start of the file. The progress callback, if provided, is called
        /// periodically as the download progresses.</remarks>
        /// <param name="fileName">The name of the file to be downloaded. This value is used as the filename presented to the user.</param>
        /// <param name="plainText">The plain text content to include in the downloaded file.</param>
        /// <param name="encoding">The character encoding to use when converting the plain text to bytes. Cannot be null.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the download operation.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used during the download. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type of the file to be downloaded. The default is "text/plain".</param>
        /// <param name="progress">An optional callback that is invoked with the download progress, represented as a value between 0.0 and 1.0.</param>
        /// <param name="encoderShouldEmitUTF8Identifier">true to emit a UTF-8 byte order mark (BOM) in the output if the encoding is UTF-8; otherwise, false.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// downloaded file.</returns>
        public ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, CancellationToken cancellationToken, int bufferSize = 32768, string contentType = "text/plain", Func<double, Task> progress = null, bool encoderShouldEmitUTF8Identifier = false)
        {
            return DownloadFile(fileName, plainText.ToBase64Encode(encoding, encoderShouldEmitUTF8Identifier), cancellationToken, bufferSize, contentType, progress);
        }

        /// <summary>
        /// Initiates a file download containing the specified plain text, using the given encoding and content type.
        /// </summary>
        /// <param name="fileName">The name of the file to be presented to the user for download. Cannot be null or empty.</param>
        /// <param name="plainText">The plain text content to include in the downloaded file. Cannot be null.</param>
        /// <param name="encoding">The character encoding to use when converting the plain text to bytes. Cannot be null.</param>
        /// <param name="timeOut">The maximum duration to wait for the download operation to complete before timing out.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer used during the download. Must be a positive integer. The default is
        /// 32,768 bytes.</param>
        /// <param name="contentType">The MIME type to set for the downloaded file. The default is "text/plain".</param>
        /// <param name="progress">An optional callback that is invoked with the download progress, represented as a value between 0.0 and 1.0.
        /// If null, progress updates are not reported.</param>
        /// <param name="encoderShouldEmitUTF8Identifier">true to emit a UTF-8 byte order mark (BOM) in the encoded file; otherwise, false. The default is false.</param>
        /// <returns>A ValueTask that represents the asynchronous download operation. The result contains information about the
        /// outcome of the file download.</returns>
        public ValueTask<DownloadFileResult> DownloadFileFromText(string fileName, string plainText, Encoding encoding, TimeSpan timeOut, int bufferSize = 32768, string contentType = "text/plain", Func<double, Task> progress = null, bool encoderShouldEmitUTF8Identifier = false)
        {
            return DownloadFile(fileName, plainText.ToBase64Encode(encoding, encoderShouldEmitUTF8Identifier), timeOut, bufferSize, contentType, progress);
        }

        /// <summary>
        /// Divides the elements of a sequence into consecutive buffers of a specified size.
        /// </summary>
        /// <remarks>The method enumerates the source sequence multiple times. If the source is not a
        /// collection or is expensive to enumerate, consider materializing it before calling this method.</remarks>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">The sequence of elements to partition. Cannot be null.</param>
        /// <param name="bufferSize">The maximum number of elements in each partition. Must be greater than zero.</param>
        /// <returns>An enumerable collection of lists, where each list contains up to bufferSize elements from the source
        /// sequence. The last list may contain fewer elements if the total number of elements is not evenly divisible
        /// by bufferSize.</returns>
        internal IEnumerable<List<T>> Partition<T>(IEnumerable<T> source, int bufferSize)
        {
            for (int i = 0; i < Math.Ceiling(source.Count() / (double)bufferSize); i++)
            {
                yield return new List<T>(source.Skip(bufferSize * i).Take(bufferSize));
            }
        }

        /// <summary>
        /// Invokes the specified asynchronous progress callback with the calculated progress percentage, if the
        /// callback is provided.
        /// </summary>
        /// <param name="bytesRead">The number of bytes that have been read so far. Must be less than or equal to <paramref name="bytesTotal"/>.</param>
        /// <param name="bytesTotal">The total number of bytes to be read. Must be greater than zero.</param>
        /// <param name="nullableTask">An optional asynchronous callback to report progress. If not <see langword="null"/>, the callback is invoked
        /// with the progress as a value between 0.0 and 1.0.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task InvokeNullableProgressTaskAsync(int bytesRead, int bytesTotal, Func<double, Task> nullableTask = null)
        {
            if (nullableTask != null)
            {
                var totalProgress = (double)bytesRead / bytesTotal;
                await nullableTask.Invoke(totalProgress);
            }
        }
    }
}
