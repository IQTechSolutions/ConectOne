using System.Net;
using System.Net.Http.Headers;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Toolbelt.Blazor;

namespace ConectOne.Blazor.Services
{
    /// <summary>
    /// HttpInterceptorService configures a global interceptor for HTTP requests/responses, 
    /// allowing you to inspect and modify them (e.g., refresh tokens automatically, 
    /// handle errors, or redirect on certain status codes).
    /// 
    /// Primary Responsibilities:
    /// 1. Registers an event (<see cref="HandleResponse"/>) on every HTTP response.
    /// 2. Attempts to refresh JWT tokens if certain criteria are met.
    /// 3. Displays feedback (e.g., success/error messages) via MudBlazor's Snackbar.
    /// 4. Navigates to an error or root page if server issues or token refresh failures occur.
    /// </summary>
    public class HttpInterceptorService
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly NavigationManager _navigationManager;
        private readonly IAccountsProvider _accountProvider;
        private readonly ISnackbar SnackBar;

        /// <summary>
        /// Constructor injects all required dependencies: interceptor, navigation manager, 
        /// snackbar, and account provider.
        /// </summary>
        public HttpInterceptorService(HttpClientInterceptor interceptor, NavigationManager navigationManager, ISnackbar snackBar, IAccountsProvider accountProvider)
        {
            _interceptor = interceptor;
            _navigationManager = navigationManager;
            SnackBar = snackBar;
           _accountProvider = accountProvider;
        }

        /// <summary>
        /// Subscribes the <see cref="_interceptor"/> to handle responses after every HTTP request.
        /// Call this to start monitoring requests.
        /// </summary>
        public void RegisterEvent() => _interceptor.AfterSendAsync += HandleResponse;

        /// <summary>
        /// Unsubscribes the <see cref="_interceptor"/> from handling responses. 
        /// Call this when the service is no longer needed or disposed.
        /// </summary>
        public void DisposeEvent() => _interceptor.AfterSendAsync -= HandleResponse;

        /// <summary>
        /// This method is called by the interceptor after each HTTP request completes. 
        /// It checks for server availability, attempts token refresh if needed, 
        /// and handles errors or forced logouts.
        /// </summary>
        private async Task HandleResponse(object? sender, HttpClientInterceptorEventArgs e)
        {        
            try
            {
                if (e.Response.StatusCode == HttpStatusCode.Unauthorized || e.Response.StatusCode == HttpStatusCode.Forbidden)
                {
                    SnackBar.Add("You are Logged Out.", Severity.Error);
                    await _accountProvider.Logout();
                    _navigationManager.NavigateTo("/", true);
                    return;
                }

                // Get the absolute path of the request URI to decide whether to refresh token.
                var absPath = e.Request.RequestUri.AbsolutePath;

                // Skip refreshing token if the request is for 'token' or 'account' endpoints.
                if (!absPath.Contains("token") && !absPath.Contains("account"))
                {
                    try
                    {
                        // Attempt to refresh the token (if the provider deems it's needed).
                        var token = await _accountProvider.TryRefreshToken();

                        if (!string.IsNullOrEmpty(token))
                        {
                            // Inform the user that the token has been refreshed.
                            SnackBar.Add("Refreshed Token.", Severity.Success);

                            // Update the Authorization header with the new token.
                            e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                        }
                    }
                    catch (Exception ex)
                    {
                        // If token refresh fails or user is unauthenticated, log out and redirect to home.
                        Console.WriteLine(ex.Message);
                        SnackBar.Add("You are Logged Out.", Severity.Error);
                        await _accountProvider.Logout();
                        _navigationManager.NavigateTo("/", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                SnackBar.Add(ex.Message, Severity.Error);
            }
        }
    }
}
