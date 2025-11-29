using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace SchoolsEnterprise.Blazor.Shared.Components.Dashboards
{
    public partial class LearnerDashboardView
    {
        private bool _canCreateMessages;

        private int classCount;
        private int teamCount;
        private int messageCount;
        private int eventCount;

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
            
        }
    }
}