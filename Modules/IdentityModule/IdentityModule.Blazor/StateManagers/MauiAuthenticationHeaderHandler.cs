using System.Net.Http.Headers;
using Microsoft.Maui.Storage;

namespace IdentityModule.Blazor.StateManagers
{
    /// <summary>
    /// A custom HTTP message handler that adds a Bearer token to the Authorization header of outgoing HTTP requests.
    /// </summary>
    public class MauiAuthenticationHeaderHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request with an added Bearer token in the Authorization header if it is not already present.
        /// </summary>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The HTTP response message from the server.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Check if the Authorization header is not already set to Bearer
            if (request.Headers.Authorization?.Scheme != "Bearer")
            {
                // Retrieve the saved token from secure storage
                var savedToken = await SecureStorage.GetAsync("accounttoken");

                // If a token is found, add it to the Authorization header
                if (!string.IsNullOrWhiteSpace(savedToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                }
            }

            // Call the base handler to send the request
            return await base.SendAsync(request, cancellationToken);
        }
    }
}