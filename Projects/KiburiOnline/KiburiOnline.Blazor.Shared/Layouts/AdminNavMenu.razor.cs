using System.Security.Claims;
using AccomodationModule.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Layouts
{
    /// <summary>
    /// Represents the navigation menu for administrative functionality in the application.
    /// </summary>
    /// <remarks>This component provides navigation links to various administrative pages, such as user
    /// management and role management. It integrates with Blazor's authentication system to determine the current
    /// user's identity and roles.</remarks>
    public partial class AdminNavMenu
    {
        private ClaimsPrincipal _user;
        private bool _canViewVacationHosts;
        private bool _canViewLodgings;
        private bool _canCreateLodgings;
        private bool _canViewDestinations;
        private bool _canViewSettings;
        private bool _canViewGolfCourses;
        private bool _canViewContacts;
        private bool _canViewBookings;
        private bool _canViewUsers;
        private bool _canViewRoles;
        private bool _canViewLodgingTypes;
        private bool _canViewTemplates;
        private bool _canViewGallery;

        /// <summary>
        /// Gets or sets the task that represents the current authentication state.
        /// </summary>
        /// <remarks>This property is typically used in Blazor applications to access the user's
        /// authentication state. The <see cref="AuthenticationState"/> object includes details such as the user's
        /// identity and claims.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation.
        /// </summary>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] private IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used for invoking JavaScript functions from .NET.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework and should
        /// not be manually set. Use this instance to call JavaScript functions or interact with the browser
        /// environment.</remarks>
        [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling authorization operations.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = default!;

        /// <summary>
        /// Handles a click event by navigating to the specified URL.
        /// </summary>
        /// <remarks>This method performs a navigation operation using the <see
        /// cref="NavigationManager"/>. The navigation is forced to reload the page by setting the <paramref
        /// name="forceLoad"/> parameter to <see langword="true"/>.</remarks>
        /// <param name="url">The URL to navigate to. Must be a valid, non-null string.</param>
        private void HandleClick(string url)
        {
            NavigationManager.NavigateTo(url, true);
        }

        /// <summary>
        /// Navigates the user to the roles management page within the security section.
        /// </summary>
        /// <remarks>This method redirects the user to the "/security/roles" URL using the <see
        /// cref="NavigationManager"/>. It is intended to be used for accessing the roles management
        /// functionality.</remarks>
        private async Task UserRoles()
        {
            NavigationManager.NavigateTo("/security/roles");
        }

        /// <summary>
        /// Navigates to the Users page within the application.
        /// </summary>
        /// <remarks>This method redirects the user to the "/registrations/users" route. It is typically
        /// used to navigate to the user registration management section.</remarks>
        private void Users()
        {
            NavigationManager.NavigateTo("/registrations/users");
        }

        /// <summary>
        /// Asynchronously initializes the component and sets user authorization state for various permissions.
        /// </summary>
        /// <remarks>This method retrieves the current authentication state and evaluates the user's
        /// permissions for viewing and creating resources. It should be called during component initialization to
        /// ensure that authorization checks are performed before rendering content. The method overrides the base
        /// implementation and may affect which UI elements are displayed based on the user's access rights.</remarks>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _user = authState.User;

            _canViewVacationHosts = (await AuthorizationService.AuthorizeAsync(_user, Permissions.VacationHost.View)).Succeeded;
            _canViewLodgings = (await AuthorizationService.AuthorizeAsync(_user, Permissions.Lodging.View)).Succeeded;
            _canCreateLodgings = (await AuthorizationService.AuthorizeAsync(_user, Permissions.Lodging.Create)).Succeeded;
            _canViewDestinations = (await AuthorizationService.AuthorizeAsync(_user, Permissions.Destination.View)).Succeeded;
            _canViewSettings = (await AuthorizationService.AuthorizeAsync(_user, Permissions.Settings.View)).Succeeded;
            _canViewGolfCourses = (await AuthorizationService.AuthorizeAsync(_user, Permissions.GolfCourse.View)).Succeeded;
            _canViewContacts = (await AuthorizationService.AuthorizeAsync(_user, Permissions.Contact.View)).Succeeded;
            _canViewBookings = (await AuthorizationService.AuthorizeAsync(_user, Permissions.Booking.View)).Succeeded;
            _canViewUsers = (await AuthorizationService.AuthorizeAsync(_user, IdentityModule.Domain.Constants.Permissions.Users.View)).Succeeded;
            _canViewRoles = (await AuthorizationService.AuthorizeAsync(_user, IdentityModule.Domain.Constants.Permissions.Roles.View)).Succeeded;
            _canViewLodgingTypes = (await AuthorizationService.AuthorizeAsync(_user, Permissions.LodgingTypes.View)).Succeeded;
            _canViewTemplates = (await AuthorizationService.AuthorizeAsync(_user, Permissions.Templates.View)).Succeeded;
            _canViewGallery = (await AuthorizationService.AuthorizeAsync(_user, FilingModule.Domain.Constants.Permissions.Gallery.View)).Succeeded;

            await base.OnInitializedAsync();
        }
    }
}
