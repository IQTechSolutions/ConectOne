using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace SchoolsEnterprise.Blazor.Shared.Components.Dashboards
{
    /// <summary>
    /// Represents the parent dashboard view component, responsible for displaying and managing user-specific data such
    /// as classes, teams, messages, and events.
    /// </summary>
    /// <remarks>This component interacts with authentication and authorization services to fetch
    /// user-specific information and permissions. It also relies on injected services for HTTP operations and
    /// navigation. Ensure that all required dependencies are properly configured before using this component.</remarks>
    public partial class ParentDashboardView
    {
        private bool _canCreateMessages;

        private int classCount;
        private int teamCount;
        private int messageCount;
        private int eventCount;

        private string _userId;

        /// <summary>
        /// The authentication state for the current user, cascaded from a higher-level component
        /// (e.g., MainLayout). Used here to fetch and store user information when the component initializes.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework and should not be
        /// manually set in most cases.</remarks>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Lifecycle method that runs after the component is initialized.
        /// Fetches user details (e.g., userId) from the authentication state.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Retrieve the authentication state, including user claims.
            var authState = await AuthenticationStateTask;

            // Extract user and userId from claims (e.g., the NameIdentifier claim).
            var user = authState.User;
            _userId = user.GetUserId();

            _canCreateMessages = (await AuthorizationService.AuthorizeAsync(authState.User,
                MessagingModule.Domain.Constants.Permissions.MessagePermissions.Create)).Succeeded;
        }
    }
}
