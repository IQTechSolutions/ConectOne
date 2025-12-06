using System.Security.Claims;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using KiburiOnline.Blazor.Shared.Theme;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.ThemeManager;

namespace KiburiOnline.Blazor.Shared.Layouts
{
    /// <summary>
    /// Represents the layout component for the administrative section of the application.
    /// </summary>
    /// <remarks>This class provides functionality for managing the layout and navigation behavior within the
    /// admin interface. It includes support for user authentication, theme customization, and navigation event
    /// handling.</remarks>
    public partial class WebsiteLayout
    {
        private bool _drawerOpen = true;
        private ThemeManagerTheme _themeManager = new();
        private ClaimsPrincipal _user;
        private ICollection<VacationDto> _vacations2 = [];
        private bool displayBookNowButton = true;
        private string vacationId;

        /// <summary>
        /// Gets the CSS style string for the menu, adjusting width based on user authentication status.
        /// </summary>
        private string _menuStyle
        {
            get
            {
                var widthText = "";
                if (_user != null && _user.Identity.IsAuthenticated)
                {
                    widthText = "width: -webkit-fill-available;  text-align:center;";
                }
                else
                {
                    widthText = "width: 100%;";
                }

                return $"position:fixed; {widthText} z-index:1100; background:#363835;";
            }

        }

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor components to access the authentication
        /// state provided  by the cascading parameter. Ensure that the task is awaited to retrieve the authentication
        /// state  before performing operations that depend on user authentication.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        private HttpContext HttpContext { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation
        /// in Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime abstraction used to invoke JavaScript functions from .NET code.
        /// </summary>
        [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service for displaying snackbars in the application.
        /// </summary>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for managing vacation-related operations.
        /// </summary>
        /// <remarks>This property is automatically injected by the dependency injection framework. Ensure
        /// that a valid  implementation of <see cref="IVacationService"/> is registered in the service
        /// container.</remarks>
        [Inject] public IVacationService VacationService { get; set; } = null!;

        /// <summary>
        /// Performs initialization logic for the component, including setting up navigation handling and configuring
        /// theme-related properties.
        /// </summary>
        /// <remarks>This method is called automatically during the component's initialization phase. It
        /// registers a location-changing handler with the <see cref="NavigationManager"/> and applies default theme
        /// settings such as the font family, border radius, and drawer clip mode.</remarks>
        protected override void OnInitialized()
        {
            NavigationManager.RegisterLocationChangingHandler(LocationChangingHandler);
            NavigationManager.LocationChanged += OnLocationChanged;

            _themeManager.Theme = new ProGolfMainTheme();
            _themeManager.DrawerClipMode = DrawerClipMode.Always;
            _themeManager.FontFamily = "Montserrat";
            _themeManager.DefaultBorderRadius = 3;
        }


        /// <summary>
        /// Handles the event triggered when the location is about to change.
        /// </summary>
        /// <remarks>This method is typically used to perform actions or validations before the navigation
        /// occurs.  Use the <see cref="LocationChangingContext.CanCancel"/> property of the <paramref name="context"/>
        /// parameter  to determine if the navigation can be canceled.</remarks>
        /// <param name="context">The context containing information about the location change, including the target URI and the ability to
        /// cancel the navigation.</param>
        /// <returns></returns>
        private async ValueTask LocationChangingHandler(LocationChangingContext context)
        {
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Logs the user out of the application and navigates to the home page.
        /// </summary>
        /// <remarks>This method performs the logout operation asynchronously and redirects the user to
        /// the root page. If an error occurs during the logout process, the exception is rethrown.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task OnLogOut()
        {
            if (HttpMethods.IsGet(HttpContext.Request.Method))
            {
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                NavigationManager.NavigateTo("/", true);
            }
        }

        /// <summary>
        /// Determines whether the current page is the home page.
        /// </summary>
        /// <returns><see langword="true"/> if the current page is the home page; otherwise, <see langword="false"/>.</returns>
        private bool IsHomePage()
        {
            var relative = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            relative = relative.Split('?')[0];
            relative = relative.Split('#')[0];
            return string.IsNullOrEmpty(relative);
        }

        /// <summary>
        /// Scrolls to the specified element on the current page or navigates to the element on a different page.
        /// </summary>
        /// <remarks>If the current page is the home page, this method scrolls to the specified element
        /// using JavaScript. Otherwise, it navigates to the element on a different page by updating the URL
        /// fragment.</remarks>
        /// <param name="elementId">The ID of the element to scroll to or navigate to. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task ScrollOrNavigateAsync(string elementId)
        {
            //if (IsHomePage())
            //{
            //    await JsRuntime.InvokeVoidAsync("ScrollTo", elementId);
            //}
            //else
            //{
                NavigationManager.NavigateTo($"/#{elementId}");
            //}
        }

        /// <summary>
        /// Initiates the booking process for the specified vacation.
        /// </summary>
        /// <remarks>This method retrieves the vacation details based on the provided vacation name and
        /// navigates  to the booking creation page if the retrieval is successful. If the retrieval fails, error 
        /// messages are displayed using the SnackBar.</remarks>
        /// <returns></returns>
        private async Task BookNow()
        {
            var vacationIdResult = await VacationService.VacationAsync(vacationId);
            if (!vacationIdResult.Succeeded)
            {
                Snackbar.AddErrors(vacationIdResult.Messages);
                return;
            }

            NavigationManager.NavigateTo($"/bookings/create/{vacationIdResult.Data.VacationId}", true);
        }

        /// <summary>
        /// Asynchronously initializes the component and retrieves the authenticated user.
        /// </summary>
        /// <remarks>This method overrides the base implementation to retrieve the current user's
        /// authentication state. The authenticated user is extracted from the <see cref="AuthenticationStateTask"/> and
        /// stored for use within the component.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _user = (await AuthenticationStateTask).User;
        }

        /// <summary>
        /// Handles logic to be executed after the component has rendered, with special behavior on the first render.
        /// </summary>
        /// <remarks>On the first render, this method updates component state based on the current URI and
        /// scrolls to a fragment if present. If no fragment is specified, the view scrolls to the top. Subsequent
        /// renders do not perform this initialization.</remarks>
        /// <param name="firstRender">Indicates whether this is the first time the component has rendered. If <see langword="true"/>,
        /// initialization logic specific to the first render is performed.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var uriParts = NavigationManager.Uri.Split("/");
                if (uriParts.All(c => c != "packages"))
                {
                    displayBookNowButton = false;
                }
                else
                {
                    vacationId = uriParts[uriParts.Length - 2];
                }

                var fragment = new Uri(NavigationManager.Uri).Fragment;
                if (!string.IsNullOrEmpty(fragment))
                {
                    await JsRuntime.InvokeVoidAsync("ScrollTo", fragment.TrimStart('#'));
                }
                else
                {
                    await ScrollToTopAsync();
                }
            }
        }

        /// <summary>
        /// Handles location change events by scrolling the view to the top when the new location does not include a
        /// fragment identifier.
        /// </summary>
        /// <remarks>If the new location includes a fragment identifier, the view will not be scrolled to
        /// the top. This behavior ensures that navigation to anchor links preserves the user's scroll
        /// position.</remarks>
        /// <param name="sender">The source of the event, typically the navigation service or component that triggered the location change.</param>
        /// <param name="e">An object containing data related to the location change, including the new location URL.</param>
        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(new Uri(e.Location).Fragment))
            {
                return;
            }

            _ = InvokeAsync(ScrollToTopAsync);
        }

        /// <summary>
        /// Asynchronously scrolls the browser window to the top of the page.
        /// </summary>
        /// <remarks>This method invokes a JavaScript function to perform the scroll. It does not block
        /// the calling thread.</remarks>
        /// <returns>A task that represents the asynchronous scroll operation.</returns>
        private async Task ScrollToTopAsync()
        {
            //await JsRuntime.InvokeVoidAsync("scrollToTop");
        }

        /// <summary>
        /// Releases resources used by the instance and unsubscribes from location change notifications.
        /// </summary>
        /// <remarks>Call this method when the instance is no longer needed to prevent memory leaks caused
        /// by event subscriptions. After calling <see cref="Dispose"/>, the instance should not be used.</remarks>
        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
