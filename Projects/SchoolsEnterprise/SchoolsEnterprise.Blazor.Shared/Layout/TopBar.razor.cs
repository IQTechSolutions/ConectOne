using Blazored.LocalStorage;
using ConectOne.Blazor.StateManagers;
using IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using MudBlazor;
using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace SchoolsEnterprise.Blazor.Shared.Layout
{
    /// <summary>
    /// Represents the top navigation bar component for the application, providing user authentication controls,
    /// navigation, theme toggling, and user profile display functionality.
    /// </summary>
    /// <remarks>The TopBar component integrates with authentication, user management, and client preference
    /// services to display user information, handle sign-in and sign-out operations, and allow users to toggle
    /// application settings such as dark mode and text direction. It is typically used as part of the application's
    /// main layout and relies on dependency injection for required services. The component manages its own state and
    /// responds to navigation changes, ensuring that user data and preferences are kept up to date. Event callbacks are
    /// provided to allow parent components to react to user actions such as toggling dark mode or changing text
    /// direction.</remarks>
    public partial class TopBar
    {
        private bool _rightToLeft;
        private string? _imageUrl;
        private bool _drawerOpen = true;
        private string _userId = null!;
        private string _firstName = null!;
        private string _lastName = null!;
        private string _email = null!;
        private ClaimsPrincipal _user = null!;
        private string? currentUrl;
        private ElementReference _logoutForm;

        #region Cascading and Injected Parameters

        /// <summary>
        /// Contains the current authentication state of the user as a CascadingParameter.
        /// This is typically provided by the Blazor authentication system.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage user sign-in operations for the application user type.
        /// </summary>
        [Inject] public SignInManager<ApplicationUser> SignInManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the client preference manager responsible for managing user-specific preferences.
        /// </summary>
        [Inject] public IClientPreferenceManager ClientPreferenceManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used for accessing local storage in the browser.
        /// </summary>
        [Inject] public ILocalStorageService LocalStorage { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be manually set
        /// in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user service used to manage user-related operations within the component.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and provides access to user
        /// management functionality such as authentication, retrieval, and updates. Assigning this property manually is
        /// not recommended outside of dependency injection scenarios.</remarks>
        [Inject] public IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used for invoking JavaScript functions from .NET.
        /// </summary>
        [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;


        /// <summary>
        /// An event callback that is invoked when the user toggles dark mode in the application.
        /// The parent layout or component can handle this event to apply the dark mode preference.
        /// </summary>
        [Parameter] public EventCallback OnDarkModeToggle { get; set; }

        /// <summary>
        /// An event callback that is invoked when the user toggles the text direction (e.g., RTL or LTR).
        /// This allows the parent layout or component to react and update styles accordingly.
        /// </summary>
        [Parameter] public EventCallback<bool> OnRightToLeftToggle { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Toggles the text direction of the application between Left-to-Right (LTR) and Right-to-Left (RTL).
        /// This is particularly useful for supporting languages that require RTL.
        /// </summary>
        private async Task RightToLeftToggle()
        {
            // Toggle the layout direction using the ClientPreferenceManager.
            var isRtl = await ClientPreferenceManager.ToggleLayoutDirection();
            _rightToLeft = isRtl;

            // Notify any subscribers (e.g., parent components) that RTL setting has changed.
            await OnRightToLeftToggle.InvokeAsync(isRtl);
        }

        /// <summary>
        /// Provides the first letter of the user's first name in uppercase.
        /// If the first name is empty, this will be an empty string.
        /// </summary>
        private string FirstLetterOfName => string.IsNullOrEmpty(_firstName) ? "" : _firstName.Substring(0, 1).ToUpper();

        /// <summary>
        /// Toggles the navigation drawer (side menu).
        /// When closed, the drawer is not visible; when open, it displays navigation links or options.
        /// </summary>
        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        /// <summary>
        /// Invokes the OnDarkModeToggle callback, allowing the parent component or layout
        /// to switch between light and dark mode.
        /// </summary>
        public async Task ToggleDarkMode()
        {
            await OnDarkModeToggle.InvokeAsync();
        }

        /// <summary>
        /// Submit the logout form so the server endpoint can perform SignOut and set cookies.
        /// </summary>
        /// <returns>A Task representing the asynchronous logout operation.</returns>
        private async Task Logout()
        {
            // Submit the POST form so the server can run SignInManager.SignOutAsync
            // and set response headers on a normal HTTP response.
            await JsRuntime.InvokeVoidAsync("submitLogoutForm", _logoutForm);
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Loads and initializes user data such as user profile information (user ID, email, first/last name).
        /// Also fetches the user's image URL from local storage and verifies the user's existence.
        /// If the user data is invalid (e.g., user deleted), redirect to the server logout endpoint.
        /// </summary>
        private async Task LoadDataAsync()
        {
            // Retrieve the current authentication state and extract user claims.
            var authState = await AuthenticationStateTask;
            _user = authState.User;

            _userId = _user.GetUserId();
            _email = _user.GetUserEmail();
            _firstName = _user.GetUserFirstName();
            _lastName = _user.GetUserLastName();

            // Attempt to load user image URL from LocalStorage.
            var imageResponse = "";
            if (!string.IsNullOrEmpty(imageResponse))
            {
                _imageUrl = imageResponse;
            }

            // Fetch current user information from the account provider.
            var currentUserResult = await UserService.GetUserInfoAsync(_userId);
            if (!currentUserResult.Succeeded || !currentUserResult.Succeeded || currentUserResult.Data == null)
            {
                // If the user could not be retrieved, it implies the user is invalid.
                // Show an error and clear all user-related data.
                SnackBar.AddError("You are logged out because the user with your Token has been deleted.");

                _userId = string.Empty;
                _imageUrl = "_content/FilingModule.Blazor/images/profileImage128x128.png";
                _firstName = string.Empty;
                _lastName = string.Empty;
                _email = string.Empty;

                // Redirect to server logout endpoint so server-side SignOut runs on a new HTTP response.
                NavigationManager.NavigateTo("/logout", forceLoad: true);
            }
            else
            {
                // If the user is valid, update the user's image URL if available.
                _imageUrl = currentUserResult.Data.CoverImageUrl;
            }
        }

        /// <summary>
        /// On component initialization, attempts to retrieve the preferred layout direction (RTL or LTR)
        /// and registers events for the HTTP Interceptor. Also displays a welcome message if the first name is known.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
                NavigationManager.LocationChanged += OnLocationChanged;


                // Display a welcome message (this may show a generic message if _firstName is not yet loaded).
                SnackBar.Add($"Welcome {_firstName}", Severity.Success);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Handles the event that occurs when the current navigation location has changed.
        /// </summary>
        /// <param name="sender">The source of the event. This is typically the navigation manager that raised the event.</param>
        /// <param name="e">The event data containing information about the new location.</param>
        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
            StateHasChanged();
        }

        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        /// <remarks>Call this method when the instance is no longer needed to unsubscribe from navigation
        /// events and prevent memory leaks. After calling this method, the instance should not be used.</remarks>
        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }

        /// <summary>
        /// After the component is rendered for the first time, load the user data from storage and provider.
        /// This deferred loading ensures that required parameters and cascading parameters are set.
        /// </summary>
        /// <param name="firstRender">True if this is the first time the component has been rendered.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _rightToLeft = await ClientPreferenceManager.IsRTL();
                await LoadDataAsync();

                StateHasChanged();
            }
        }

        #endregion
    }
}
