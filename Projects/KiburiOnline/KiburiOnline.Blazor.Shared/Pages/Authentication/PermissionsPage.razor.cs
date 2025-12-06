using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsEnterprise.Base.Extensions;

namespace KiburiOnline.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// The PermissionsPage component allows administrators to view and update role permissions.
    /// It does the following:
    /// 1. Fetches all available claims/permissions from reflection or a static set (depending on the role).
    /// 2. Groups permissions by category (e.g., "All Permissions", "Module X").
    /// 3. Retrieves existing permissions for the specified role from the server, marking them as selected.
    /// 4. Persists any changes to the selected permissions.
    /// 
    /// Authorization checks determine whether the user can edit or search permissions.
    /// </summary>
    public partial class PermissionsPage
    {
        private PermissionViewModel _model = new PermissionViewModel();
        private ClaimsPrincipal _currentUser = default!;
        private Dictionary<string, List<RoleClaimViewModel>> GroupedRoleClaims { get; } = new();
        private HashSet<RoleClaimViewModel?> _selectedActivityGroups = new();
        private RoleClaimViewModel _roleClaims = new();
        private string _searchString = string.Empty;
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;
        private bool _multiSelection => string.IsNullOrEmpty(UserId);
        private bool _canEditRolePermissions;
        private bool _canSearchRolePermissions;
        private bool _loaded;

        /// <summary>
        /// Cascaded authentication state for retrieving the current user’s identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the HTTP provider used for making network requests.
        /// </summary>
        [Inject] private IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling authorization operations.
        /// </summary>
        [Inject] private IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected instance of <see cref="ISnackbar"/> used to display notifications or messages.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for managing navigation and URI manipulation.
        /// </summary>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        
        /// <summary>
        /// The role ID for which we are managing permissions.
        /// </summary>
        [Parameter] public string RoleId { get; set; } = string.Empty;

        /// <summary>
        /// A description displayed in the UI, typically referencing the role name or purpose.
        /// </summary>
        [Parameter] public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Optional user ID, allowing for user-specific permission context or multi-selection usage.
        /// </summary>
        [Parameter] public string? UserId { get; set; }

        /// <summary>
        /// Saves the selected permissions for the given role by calling the RoleManager service.
        /// </summary>
        private async Task SaveAsync()
        {
            //// Create a permission update request with only the selected claims
            //var result = await RoleManager.UpdateRoleClaimsAsync(new PermissionRequest
            //{
            //    RoleId = RoleId,
            //    RoleClaims = _selectedActivityGroups
            //        .Select(c => c.ToRoleClaimRequest(RoleId))
            //        .ToList()
            //});

            //// Feedback for success or failure
            //if (result.Succeeded)
            //{
            //    SnackBar.Add("Permissions successfully updated", Severity.Success);
            //    NavigationManager.NavigateTo($"/permissions/{RoleId}");
            //}
            //else
            //{
            //    SnackbarExtensions.AddErrors(SnackBar, result.Messages);
            //}
        }

        /// <summary>
        /// Performs basic text filtering on role claims, checking if the claim's Value or Description 
        /// contains the entered search text.
        /// </summary>
        private bool Search(RoleClaimViewModel roleClaims)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;

            // Check claim's Value for search string
            if (roleClaims.Value?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
                return true;

            // Check claim's Description for search string
            if (roleClaims.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
                return true;

            return false;
        }

        /// <summary>
        /// Returns a color to display in a badge based on how many permissions are selected 
        /// within a particular group.
        /// </summary>
        private Color GetGroupBadgeColor(int selected, int all)
        {
            if (selected == 0)
                return Color.Error;

            if (selected == all)
                return Color.Success;

            return Color.Info;
        }

        /// <summary>
        /// Fetches all possible permissions for the role, organizes them by group, 
        /// and marks permissions that are already assigned to the role.
        /// </summary>
        private async Task GetRolePermissionsAsync()
        {
            // By default, super admins (or certain roles) can see all permission modules
            var modules = new List<Type>();
            if (_currentUser.IsInRole(RoleConstants.SuperUser))
            {
                modules = ClaimsPrincipalExtensions.GetAllPermissionTypesAsync();
            }
            else if (_currentUser.IsInRole(RoleConstants.Administrator) || _currentUser.IsInRole(RoleConstants.CompanyAdmin))
            {
                modules = ClaimsPrincipalExtensions.GetAllPermissionTypesAsync();
            }

            // allPermissions is populated with all possible permissions from the reflection-based approach
            var allPermissions = new List<RoleClaimResponse>();
            allPermissions.GetAllPermissions();

            // Convert these to RoleClaimViewModel
            _model.RoleClaims = allPermissions.Select(c => new RoleClaimViewModel(c)).ToList();

            // Retrieve assigned permissions for this role from the server
            var result = await Provider.GetAsync<IEnumerable<RoleClaimResponse>>($"account/roles/role/permissions/{RoleId}");

            // If that server call succeeds
            if (result.Succeeded)
            {
                // Mark items in _model.RoleClaims as selected if they match the role’s current permissions
                foreach (var item in _model.RoleClaims)
                {
                    if (Enumerable.Any<RoleClaimResponse>(result.Data, c => c.Value == item.Value))
                    {
                        item.Selectede = true;
                        _selectedActivityGroups.Add(item);
                    }
                }

                // Initially group all claims into "All Permissions"
                GroupedRoleClaims.Add("All Permissions", _model.RoleClaims);

                // Then group them further by the claim.Group property
                foreach (var claim in _model.RoleClaims)
                {
                    if (GroupedRoleClaims.ContainsKey(claim.Group))
                    {
                        GroupedRoleClaims[claim.Group].Add(claim);
                    }
                    else
                    {
                        GroupedRoleClaims.Add(claim.Group, new List<RoleClaimViewModel> { claim });
                    }
                }

                // If we have a model, update the description based on the role’s ID or name
                if (_model != null)
                {
                    Description = $"Manage {_model.RoleId} {_model.RoleName}'s Permissions";
                }

                StateHasChanged();
            }
            else
            {
                // On failure, show error and navigate away
                SnackbarExtensions.AddErrors(SnackBar, result.Messages);
                NavigationManager.NavigateTo("/identity/roles");
            }
        }

        /// <summary>
        /// Lifecycle method called once after the component is first initialized. Retrieves the current user 
        /// for authorization checks, then calls GetRolePermissionsAsync to load all claims/permissions for the given role.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Retrieve the current user from the AuthenticationState
            var authState = await AuthenticationStateTask;
            _currentUser = authState.User;

            // Check if the user is authorized to edit and search role permissions
            _canEditRolePermissions = (await AuthorizationServiceExtensions.AuthorizeAsync(AuthorizationService, _currentUser, Permissions.RoleClaims.Edit)).Succeeded;
            _canSearchRolePermissions = (await AuthorizationServiceExtensions.AuthorizeAsync(AuthorizationService, _currentUser, Permissions.RoleClaims.Search)).Succeeded;

            // Populate the role’s permissions
            await GetRolePermissionsAsync();

            // Signal that the component is ready to render
            _loaded = true;
        }
    }
}
