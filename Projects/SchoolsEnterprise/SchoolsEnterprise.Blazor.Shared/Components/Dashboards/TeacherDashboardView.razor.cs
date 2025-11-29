using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Components.Dashboards
{
    /// <summary>
    /// Represents the dashboard view for a teacher, providing access to user-specific data such as class, team,
    /// message, and event counts.
    /// </summary>
    /// <remarks>This component is designed to interact with the authentication and authorization systems to
    /// fetch user-specific data and permissions. It relies on dependency injection for services such as <see
    /// cref="IAuthorizationService"/>, <see cref="IBaseHttpProvider"/>, and <see cref="NavigationManager"/>. The
    /// component initializes by retrieving the authenticated user's details and fetching relevant data for
    /// display.</remarks>
    public partial class TeacherDashboardView
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
        /// Gets or sets the service used to manage and retrieve school class data.
        /// </summary>
        [Inject] public ISchoolClassService SchoolClassService { get; set; }

        /// <summary>
        /// Gets or sets the service used to query activity group data.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Assign an
        /// implementation of <see cref="IActivityGroupQueryService"/> to enable querying of activity group information
        /// within the component.</remarks>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; }

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

            _canCreateMessages = (await AuthorizationService.AuthorizeAsync(authState.User, MessagingModule.Domain.Constants.Permissions.MessagePermissions.Create)).Succeeded;

            var schoolClassPageParameters = new SchoolClassPageParameters() { TeacherId = authState.User.GetUserId() };
            var schoolClassCountResult = await SchoolClassService.PagedSchoolClassesAsync(schoolClassPageParameters);
            if (schoolClassCountResult.Succeeded)
            {
                classCount = schoolClassCountResult.Data.Count();
            }

            var teamPageParameters = new ActivityGroupPageParameters() { CoachEmail = authState.User.GetUserId() };
            var teamCountResult = await ActivityGroupQueryService.PagedActivityGroupsAsync(teamPageParameters);
            if (teamCountResult.Succeeded)
            {
                teamCount = teamCountResult.Data.Count();
            }
        }
    }
}
