using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using MudBlazor.ThemeManager;
using System.Security.Claims;
using Blazored.LocalStorage;
using CommunityToolkit.Mvvm.Messaging;
using ConectOne.Blazor.Services;
using ConectOne.Blazor.Settings;
using ConectOne.Domain.Constants;
using ConectOne.Domain.Interfaces;
using IdentityModule.Blazor.StateManagers;
using IdentityModule.Domain.Extensions;
using MessagingModule.Blazor.NotificationDelegates;
using Microsoft.JSInterop;
using MudBlazor;
using NeuralTech.Services;
using Plugin.Firebase.CloudMessaging;
using ShoppingModule.Blazor.Components;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Components.Layout
{
    /// <summary>
    /// MainLayout is a top-level layout component that:
    ///   - Manages a theme (via MudBlazor.ThemeManager).
    ///   - Handles push notification logic with <see cref="NotificationStateManager"/>.
    ///   - Responds to navigation events (location changing) to possibly intercept "/back" and call JavaScript.
    ///   - Tracks unread blog entry counts from <see cref="IBaseHttpProvider"/>.
    ///   - Demonstrates how to register/unregister for global messages via CommunityToolkit’s WeakReferenceMessenger.
    /// </summary>
    public partial class MainLayout : IDisposable
    {
        private ThemeManagerTheme _themeManager = new ThemeManagerTheme() { Theme = ApplciationTheme.LightTheme };
        private string _userId = null!;
        private bool _themeManagerOpen;
        private bool _drawerOpen;
        private const string RouteBack = "/back";
        private string? _notificationCount;
        private string? _cartItemCount;
        private string? _blogPostNotificationCount;
        private CancellationTokenSource? _countRefreshCts;
        private static readonly TimeSpan CountRefreshInterval = TimeSpan.FromMinutes(1);

        #region Cascading & Injected Services

        /// <summary>
        /// Blazor automatically provides the current authentication state as a cascading parameter,
        /// so that child components in the hierarchy can also access the user’s credentials/claims.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the cascading parameter that provides access to the cart state.
        /// </summary>
        /// <remarks>This property is typically set automatically by the Blazor framework when the
        /// component is used within a cascading context that provides a <see cref="CartStateProvider"/>.</remarks>
        [CascadingParameter] public CartStateProvider CartStateProvider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to interact with the browser's local storage.
        /// </summary>
        /// <remarks>This property is typically set by dependency injection. Use this service to store,
        /// retrieve, and remove data from the browser's local storage in a Blazor application.</remarks>
        [Inject] public ILocalStorageService LocalStorageService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP client used to send requests to remote servers.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection. Assigning a different
        /// instance allows customization of HTTP request behavior, such as setting default headers or configuring
        /// message handlers.</remarks>
        [Inject] public HttpClient HttpClient { get; set; } = null!;

        /// <summary>
        /// Logger instance for logging errors and information.
        /// </summary>
        [Inject] public ILogger<MainLayout> Logger { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET code.
        /// </summary>
        /// <remarks>This property is typically set by the Blazor framework through dependency injection.
        /// It enables interoperability between .NET and JavaScript in Blazor applications.</remarks>
        [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated by dependency injection and provides access to
        /// configuration values such as connection strings, application settings, and environment variables. Modifying
        /// this property at runtime may affect how the application retrieves configuration data.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the provider used to obtain authentication state information for the current user.
        /// </summary>
        /// <remarks>This property is typically set by the Blazor framework through dependency injection.
        /// Use this provider to query or subscribe to authentication state changes within the component.</remarks>
        [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }

        /// <summary>
        /// The <see cref="IConfiguration"/> instance provides access to application settings,
        /// </summary>
        [Inject] public HttpInterceptorService Interceptor { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for providing device information.
        /// </summary>
        [Inject] public IDeviceInfoService DeviceInfoService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatic navigation and
        /// for retrieving information about the current URI. This property is typically set by the framework through
        /// dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        #endregion

        #region Theme Management

        /// <summary>
        /// Opens or closes the theme manager UI. 
        /// Often triggered by a button in the layout’s top menu or settings icon.
        /// </summary>
        /// <param name="value">True if opening; false if closing.</param>
        private void OpenThemeManager(bool value)
        {
            _themeManagerOpen = value;
        }

        /// <summary>
        /// Receives an updated <see cref="ThemeManagerTheme"/> object, presumably from the 
        /// theme manager UI, and applies it to re-render the layout with the new theme.
        /// </summary>
        /// <param name="value">The updated theme object to apply.</param>
        private void UpdateTheme(ThemeManagerTheme value)
        {
            _themeManager = value;
            StateHasChanged();
        }

        /// <summary>
        /// Toggles the layout’s side drawer. Typically used for navigation items or user profile shortcuts.
        /// </summary>
        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        #endregion

        #region Navigation & Preferences

        /// <summary>
        /// Attempts to read "NavigationID" from <see cref="Preferences"/> (a Maui-specific local store).
        /// If found, navigates the user to that route and removes the stored key.
        /// This is used, for example, if a push notification indicated a user should open a certain page 
        /// upon app (re-)start.
        /// </summary>
        private async Task NavigateToPageAsync(string url)
        {
            if (DeviceInfoService.IsMobilePlatform)
            {
                NavigationManager.NavigateTo(url);
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// A location changing handler that checks if the user tries to navigate to "/back".
        /// If so, we intercept normal navigation and instead call JavaScript’s history.back().
        /// </summary>
        /// <param name="context">Provides info about the new target location, plus a way to cancel navigation.</param>
        private async ValueTask LocationChangingHandler(LocationChangingContext context)
        {
            var isBackRoute = context.TargetLocation.EndsWith(RouteBack, StringComparison.OrdinalIgnoreCase);
            if (isBackRoute)
            {
                context.PreventNavigation();
                await JsRuntime.InvokeVoidAsync("history.back");
            }
        }

        #endregion

        #region SignOut Logic

        /// <summary>
        /// Logs the user out of the current session asynchronously.
        /// </summary>
        /// <remarks>This method logs the user out by invoking the account provider's logout functionality
        /// and then redirects the user to the application's root page.</remarks>
        /// <returns></returns>
        private async Task PerformLogoutAsync()
        {
            if (!DeviceInfoService.IsMobilePlatform)
            {
                await LocalStorageService.RemoveItemAsync(StorageConstants.Local.AuthToken);
                await LocalStorageService.RemoveItemAsync(StorageConstants.Local.RefreshToken);
                await LocalStorageService.RemoveItemAsync(StorageConstants.Local.UserImageURL);
                await ((AuthStateProvider)AuthStateProvider).NotifyUserLogout();
            }
            else
            {
                await ((MauiAuthenticationStateProvider)AuthStateProvider).Logout();
            }
                
            HttpClient.DefaultRequestHeaders.Authorization = null;
            NavigationManager.NavigateTo("/");
        }

        /// <summary>
        /// Signs out the currently authenticated user and performs necessary cleanup operations.
        /// </summary>
        /// <remarks>This method removes the device token associated with the authenticated user, if
        /// applicable,  and then performs the logout process. Any errors encountered during the sign-out process  are
        /// logged but do not propagate to the caller.</remarks>
        /// <returns></returns>
        private async Task SignOut()
        {
            await PerformLogoutAsync();
        }

        #endregion
        
        #region Notification Logic

        /// <summary>
        /// Updates the count of unread notifications for the current user asynchronously.
        /// </summary>
        /// <remarks>This method retrieves the unread notification count from the server and updates the
        /// internal state with the retrieved value. If an error occurs during the operation, the exception is logged
        /// and rethrown.</remarks>
        /// <returns></returns>
        private async Task UpdateNotificationCountsAsync()
        {
            //try
            //{
            //    var args = new NotificationPageParameters { ReceiverId = _userId };
            //    var result = await Provider.GetAsync<int>($"notifications/unread/count?{args.GetQueryString()}");
            //    if (result.Succeeded)
            //        _notificationCount = result.Data.ToString();
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogError(ex, "Error fetching notification count");
            //    throw;
            //}
        }

        /// <summary>
        /// Updates the unread blog post count for the current user asynchronously.
        /// </summary>
        /// <remarks>This method retrieves the unread blog post count from the server and updates the
        /// internal notification count. If the operation fails, an exception is logged and rethrown.</remarks>
        /// <returns></returns>
        private async Task UpdateBlogPostCountsAsync()
        {
            //try
            //{
            //    var result = await Provider.GetAsync<int>($"blog/unread/count/{_userId}");
            //    if (result.Succeeded)
            //        _blogPostNotificationCount = result.Data.ToString();
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogError(ex, "Error fetching blog post count");
            //    throw;
            //}
        }

        /// <summary>
        /// Asynchronously updates the cart item count display to reflect the current number of items in the shopping
        /// cart.
        /// </summary>
        /// <remarks>Call this method after modifying the shopping cart to ensure the displayed item count
        /// remains accurate. This method schedules a UI update to reflect any changes.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task UpdateCartItemCounter()
        {
            _cartItemCount = CartStateProvider.ShoppingCart?.ItemCount.ToString();
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// A Blazor lifecycle method (synchronous, hence "void") that runs after the component 
        /// is initialized. 
        /// 1. Subscribes to notifications changed events.
        /// 2. Hooks into navigation changes for "/back".
        /// 3. Retrieves user ID from auth state.
        /// 4. Registers push notification messenger callbacks.
        /// 5. Possibly navigates to a stored route.
        /// 6. Attempts to load notification/blog counts if the user is authenticated.
        /// </summary>
        protected override async void OnInitialized()
        {
            try
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.Android ||
                    DeviceInfo.Current.Platform == DevicePlatform.iOS)
                {
                    CrossFirebaseCloudMessaging.Current.NotificationTapped += async (sender, e) =>
                    {
                        if (e.Notification.Data.TryGetValue("NavigationID", out string pageName))
                        {
                            NavigationManager.NavigateTo(pageName);
                        }
                    };
                }

                Interceptor.RegisterEvent();
                NavigationManager.RegisterLocationChangingHandler(LocationChangingHandler);
                CartStateProvider.OnShoppingCartChanged += async () => await UpdateCartItemCounter();
                
                _cartItemCount = CartStateProvider.ShoppingCart?.ItemCount.ToString();
                
                _drawerOpen = false;

                await OnInitializedAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error during initialization");
            }
        }

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    var authState = await AuthenticationStateTask;
                    _userId = authState.User.GetUserId();

                    if (!string.IsNullOrEmpty(_userId))
                    {
                        await UpdateBlogPostCountsAsync();
                        await UpdateNotificationCountsAsync();
                    }

                    // Force UI refresh
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error during after render");
                    SnackBar.Add("An error occurred while fetching blogEntry count.", Severity.Error);
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        #endregion

        #region Disposable Members

        /// <summary>
        /// Because we set up event subscriptions (NotificationsChanged) and 
        /// registered push notification delegates, we implement <see cref="IDisposable"/> 
        /// so we can clean them up when this layout is no longer in use.
        /// </summary>
        public void Dispose()
        {
            CartStateProvider.OnShoppingCartChanged -= async () => await UpdateCartItemCounter();
            Interceptor.DisposeEvent();
        }

        #endregion
    }
}
