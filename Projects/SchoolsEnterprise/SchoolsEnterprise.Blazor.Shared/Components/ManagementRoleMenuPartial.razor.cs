using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace SchoolsEnterprise.Blazor.Shared.Components
{
    /// <summary>
    /// Represents a partial Blazor component responsible for managing and displaying roles in a menu based on the
    /// current user's permissions and roles.
    /// </summary>
    /// <remarks>This component interacts with the authentication state, authorization service, and HTTP
    /// provider to dynamically fetch and display roles. It is designed to be used within a Blazor application and
    /// relies on dependency injection for its services.</remarks>
    public partial class ManagementRoleMenuPartial
    {
        private IEnumerable<RoleDto> _rolesToDisplayInMenu = new List<RoleDto>();

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
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        /// <remarks>The provider must be set before making any HTTP requests. Dependency injection is
        /// used to supply the implementation.</remarks>
        [Inject] public IRoleService Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// a Blazor application.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework and should not be set
        /// manually in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component, retrieving and processing user roles and their managed roles.
        /// </summary>
        /// <remarks>This method retrieves the authenticated user's roles and their associated managed
        /// roles from the server. The roles are then appended to a collection for display in the menu. The method also
        /// ensures that the base class's initialization logic is executed.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            // Extract user and userId from claims (e.g., the NameIdentifier claim).
            var userId = authState.User.GetUserId();

            var rolesResult = await Provider.UserRolesAsync(userId);
            if (rolesResult.Succeeded && rolesResult.Data is not null)
            {
                foreach (var role in rolesResult.Data)
                {
                    var managedRolesResult = await Provider.ChildrenAsync(role.Id);
                    if (managedRolesResult.Succeeded && managedRolesResult.Data is not null)
                    {
                        foreach (var managedRole in managedRolesResult.Data)
                        {
                            _rolesToDisplayInMenu = _rolesToDisplayInMenu.Append(managedRole);
                        }
                    }
                }
            }

            await base.OnInitializedAsync();
        }
    }
}
