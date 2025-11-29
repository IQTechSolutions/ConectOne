namespace PayFast.Extensions
{
    /// <summary>
    /// Provides extension methods for the HttpClient class to support additional HTTP operations.
    /// </summary>
    /// <remarks>These extension methods enable scenarios not directly supported by the standard HttpClient
    /// API, such as sending HTTP PATCH requests. All methods are static and intended to be used as extension methods on
    /// HttpClient instances.</remarks>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Sends an HTTP PATCH request to the specified URI as an asynchronous operation.
        /// </summary>
        /// <remarks>This extension method enables sending PATCH requests, which are not natively
        /// supported by the HttpClient class. The caller is responsible for disposing the returned
        /// HttpResponseMessage.</remarks>
        /// <param name="httpClient">The HTTP client instance used to send the request.</param>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="iContent">The HTTP request content to send with the PATCH request. Can be null if no content is required.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, string requestUri, HttpContent iContent)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = iContent
            };

            return await httpClient.SendAsync(request);
        }
    }
}
