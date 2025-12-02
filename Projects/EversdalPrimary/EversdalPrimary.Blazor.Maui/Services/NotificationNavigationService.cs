using Microsoft.AspNetCore.Components;
using Microsoft.Maui.ApplicationModel;

namespace EversdalPrimary.Blazor.Maui.Services
{
    public class NotificationNavigationService
    {
        private NavigationManager? _navigationManager;
        private string? _pendingUrl;

        public void RegisterNavigationManager(NavigationManager navigationManager)
        {
            _navigationManager ??= navigationManager;

            if (!string.IsNullOrWhiteSpace(_pendingUrl))
            {
                NavigateInternal(_pendingUrl);
                _pendingUrl = null;
            }
        }

        public void NavigateFromNotification(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            var cleanedUrl = NormalizeUrl(url);

            if (_navigationManager is null)
            {
                _pendingUrl = cleanedUrl;
                return;
            }

            NavigateInternal(cleanedUrl);
        }

        private void NavigateInternal(string url)
        {
            MainThread.BeginInvokeOnMainThread(() => _navigationManager?.NavigateTo(url, true));
        }

        private static string NormalizeUrl(string url)
        {
            return url.StartsWith("/") ? url : $"/{url}";
        }
    }
}
