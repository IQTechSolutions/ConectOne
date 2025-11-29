using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// Represents the details of managed roles for a user, including role summaries and associated metrics.
    /// </summary>
    /// <remarks>This class provides functionality to retrieve and process managed roles for a user, including
    /// calculating metrics such as the total number of managed roles, users, registration tiers, and services. It is
    /// designed to be used in Blazor applications and relies on dependency injection for HTTP requests and navigation
    /// management.</remarks>
    public partial class ManagedRolesUserDetails
    {
        private readonly Dictionary<string, ManagedRoleServiceTierSummary> _managedRoleSummaries = new(StringComparer.OrdinalIgnoreCase);
        private string _userId;

        /// <summary>
        /// Gets or sets the task that provides the current authentication state.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState>? AuthenticationStateTask { get; set; }


        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage URI navigation and interaction in a
        /// Blazor application.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        [Inject] public IRoleService RoleService { get; set; } = default!;

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Parameter] public string RoleId { get; set; }

        /// <summary>
        /// Gets a read-only collection of summaries for the managed roles.
        /// </summary>
        private IReadOnlyCollection<ManagedRoleServiceTierSummary> ManagedRoleUserSummaries => _managedRoleSummaries.Values;

        /// <summary>
        /// Gets or sets the total number of managed roles.
        /// </summary>
        private int TotalManagedRoleCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of managed users.
        /// </summary>
        private int TotalManagedUserCount { get; set; }

        /// <summary>
        /// Gets or sets the total count of managed registration tiers.
        /// </summary>
        private int TotalManagedRegistrationTierCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of managed services.
        /// </summary>
        private int TotalManagedServiceCount { get; set; }

        /// <summary>
        /// Asynchronously initializes the component's state by retrieving and processing user roles and their associated
        /// data.
        /// </summary>
        /// <remarks>This method retrieves the current user's authentication state and processes their roles to
        /// populate managed role summaries.  It ensures that only authenticated users with valid roles are processed. For
        /// each role, additional role-related data  is fetched and aggregated, including metrics for managed roles. The
        /// method updates the total counts for managed users,  registration tiers, services, and roles based on the
        /// retrieved data.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationStateTask is null)
            {
                return;
            }

            var authState = await AuthenticationStateTask.ConfigureAwait(false);
            var user = authState.User;
            _userId = user.GetUserId();

            if (user.Identity?.IsAuthenticated is not true)
            {
                return;
            }

            var roleNames = user.FindAll(ClaimTypes.Role)
                .Select(claim => claim.Value)
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var roleName in roleNames)
            {
                var roleInfo = await RoleService.RoleAsync(roleName).ConfigureAwait(false);
                if (roleInfo.Succeeded is not true || roleInfo.Data is null || string.IsNullOrWhiteSpace(roleInfo.Data.Id))
                {
                    continue;
                }

                var managedRoles = await RoleService.ChildrenAsync(roleInfo.Data.Id).ConfigureAwait(false);
                if (managedRoles.Succeeded is not true || managedRoles.Data is null)
                {
                    continue;
                }

                foreach (var managedRole in managedRoles.Data)
                {
                    if (managedRole is null || string.IsNullOrWhiteSpace(managedRole.Id) || string.IsNullOrWhiteSpace(managedRole.Name))
                    {
                        continue;
                    }

                    if (!_managedRoleSummaries.TryGetValue(managedRole.Id, out var summary))
                    {
                        summary = new ManagedRoleServiceTierSummary(managedRole.Id, managedRole.Name!, managedRole.Description ?? string.Empty);
                        _managedRoleSummaries.Add(managedRole.Id, summary);
                    }
                }

                TotalManagedUserCount = ManagedRoleUserSummaries.Sum(summary => summary.UserCount);
                TotalManagedRoleCount = ManagedRoleUserSummaries.Count;
            }
        }

        /// <summary>
        /// Represents a summary of a managed role, including its identifier, name, description, and associated metrics.
        /// </summary>
        /// <remarks>This class provides a high-level overview of a managed role, including its metadata and
        /// related counts for users,  registration tiers, and services. It is designed to encapsulate role-related
        /// information for display or reporting purposes.</remarks>
        private sealed class ManagedRoleServiceTierSummary
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ManagedRoleSummary"/> class with the specified role ID, role
            /// name, and description.
            /// </summary>
            /// <param name="roleId">The unique identifier for the role. Cannot be null or empty.</param>
            /// <param name="roleName">The name of the role. Cannot be null or empty.</param>
            /// <param name="description">A brief description of the role. Can be null or empty.</param>
            public ManagedRoleServiceTierSummary(string roleId, string roleName, string description)
            {
                ServiceTierId = roleId;
                ServiceTierName = roleName;
            }

            /// <summary>
            /// Gets the unique identifier for the role.
            /// </summary>
            public string ServiceTierId { get; }

            /// <summary>
            /// Gets the name of the role associated with the current user or entity.
            /// </summary>
            public string ServiceTierName { get; }

            /// <summary>
            /// Gets or sets the description of the object.
            /// </summary>
            public int UserCount { get; set; }

            /// <summary>
            /// Gets or sets the number of users who are currently in arrears.
            /// </summary>
            public int UsersInArearsCount { get; set; }
        }
    }
}
