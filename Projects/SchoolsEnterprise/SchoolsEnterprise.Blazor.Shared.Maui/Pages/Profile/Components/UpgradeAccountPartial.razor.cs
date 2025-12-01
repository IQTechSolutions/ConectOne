using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Profile.Components
{
    /// <summary>
    /// Represents a partial component for upgrading a user's account, including role-based service tier management.
    /// </summary>
    /// <remarks>This partial class is responsible for initializing and managing the roles and associated
    /// service tiers for a specific user. It retrieves role and service tier data from the backend using the injected
    /// HTTP provider.</remarks>
    public partial class UpgradeAccountPartial
    {
        private Dictionary<string, List<ServiceTierDto>> _serviceTiersByRole = new();
        private IEnumerable<RoleDto> _roles = [];

        /// <summary>
        /// Gets or sets the service used to manage service tier operations.
        /// </summary>
        [Inject] public IServiceTierService Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage user roles within the application.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Assign an
        /// implementation of <see cref="IRoleService"/> to enable role-related operations such as retrieving, creating,
        /// or updating user roles.</remarks>
        [Inject] public IRoleService RoleService { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to programmatically navigate to different URIs or to access information about the current navigation
        /// state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Parameter] public string UserId { get; set; }

        /// <summary>
        /// Asynchronously initializes the component and retrieves role and service tier data for the current user.
        /// </summary>
        /// <remarks>This method fetches the roles associated with the current user and, for each role,
        /// retrieves the corresponding service tiers. The retrieved data is stored in local fields for use within the
        /// component.  It also invokes the base class's <see cref="ComponentBase.OnInitializedAsync"/> method to ensure
        /// proper initialization.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var rolesResult = await RoleService.UserRolesAsync(UserId);
            if (rolesResult.Succeeded)
            {
                _roles = rolesResult.Data;
                foreach (var role in _roles)
                {
                    var serviceTierResult = await Provider.AllEntityServiceTiersAsync(role.Id);
                    if (serviceTierResult.Succeeded && serviceTierResult.Data.Any())
                    {
                        _serviceTiersByRole.Add(role.Id, serviceTierResult.Data.ToList());
                    }
                }
            }

            await base.OnInitializedAsync();
        }
    }
}
