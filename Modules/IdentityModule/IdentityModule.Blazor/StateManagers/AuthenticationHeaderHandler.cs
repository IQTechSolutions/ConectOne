using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace IdentityModule.Blazor.StateManagers
{
    /// <summary>
    /// A message handler that adds a Bearer authentication header to outgoing HTTP requests using a token retrieved
    /// from local storage.
    /// </summary>
    /// <remarks>This handler is typically used in HTTP client pipelines to automatically include an
    /// authentication token in requests that do not already specify a Bearer token. If the request's Authorization
    /// header is not set to use the Bearer scheme, the handler attempts to retrieve a token from local storage and adds
    /// it to the request. This enables seamless authentication for APIs that require Bearer tokens without manual
    /// header management.</remarks>
    public class AuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService localStorage;

        /// <summary>
        /// Initializes a new instance of the AuthenticationHeaderHandler class using the specified local storage
        /// service.
        /// </summary>
        /// <param name="localStorage">The local storage service used to retrieve authentication tokens for HTTP requests. Cannot be null.</param>
        public AuthenticationHeaderHandler(ILocalStorageService localStorage) => this.localStorage = localStorage;

        /// <summary>
        /// Sends an HTTP request asynchronously, ensuring that a Bearer token is present in the Authorization header if
        /// available.
        /// </summary>
        /// <remarks>If the request does not already include a Bearer token in the Authorization header,
        /// this method attempts to retrieve a saved token from local storage and adds it to the request. This ensures
        /// that outgoing requests are authenticated when possible.</remarks>
        /// <param name="request">The HTTP request message to send. If the Authorization header does not contain a Bearer token, a token is
        /// retrieved from local storage and added if available.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous send operation. The task result contains the HTTP response message
        /// received from the server.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization?.Scheme != "Bearer")
            {
                var savedToken = await localStorage.GetItemAsync<string>("authToken");

                if (!string.IsNullOrWhiteSpace(savedToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}