using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Interfaces;
using SchoolsEnterprise.Blazor.Shared.Modals;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// Represents the settings and initialization logic for a user role, including service tier data retrieval and
    /// dependency injection.
    /// </summary>
    /// <remarks>This class is a Blazor component that manages the settings for a specific user role. It
    /// retrieves service tier data during initialization and provides dependency-injected services for HTTP
    /// communication and user notifications.</remarks>
    public partial class UserRoleSettings
    {
        private bool _loaded;
        private List<ServiceTierDto> _serviceTiers = new();
        private MudTable<ServiceTierDto> _table;
        private UserRoleViewModel _role;
        private List<RoleDto> _childRoles = [];

        private bool _canAddRolesToBeManaged;
        private bool _canCreateRoles;
        private bool _canEditRoles;
        private bool _canRemovetRoles;

        private PermissionsPage? permissionsPage;

        #region Injections & Parameters

        /// <summary>
        /// The authentication state for the current user, cascaded from a higher-level component
        /// (e.g., MainLayout). Used here to fetch and store user information when the component initializes.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snackbars for user notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts to the user. Ensure that the service is properly initialized before attempting to use
        /// it.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage URI navigation and interaction in a
        /// Blazor application.
        /// </summary>
        /// <remarks>This property is typically injected by the Blazor framework and should not be set
        /// manually in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the service responsible for managing service tier operations.
        /// </summary>
        [Inject] public IServiceTierService ServiceTierService { get; set; }

        [Inject] public IRoleService RoleService { get; set; }

        

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the role associated with the current context.
        /// </summary>
        [Parameter] public string? RoleName { get; set; }

        /// <summary>
        /// Gets or sets the tab index of the component, which determines the order of focus when navigating through
        /// elements using the keyboard.
        /// </summary>
        /// <remarks>Use this property to control the keyboard navigation order for accessibility
        /// purposes. Ensure that the value is set appropriately to maintain a logical navigation flow.</remarks>
        [Parameter] public int TabIndex { get; set; } = 0;

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new service tier with default options.
        /// </summary>
        /// <remarks>This method initializes a new service tier using predefined dialog options.  The
        /// operation is asynchronous and does not return a result.</remarks>
        /// <returns></returns>
        public async Task CreateServiceTier()
        {
            NavigationManager.NavigateTo($"/service-tiers/create/{_role.RoleId}/{RoleName}");
        }

        /// <summary>
        /// Updates the details of an existing service tier.
        /// </summary>
        /// <remarks>This method opens a dialog with options configured for medium width and full width
        /// display. Ensure that the <paramref name="tier"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="tier">The <see cref="ServiceTierDto"/> object containing the updated details of the service tier. Cannot be null.</param>
        public async Task EditServiceTier(ServiceTierDto tier)
        {
            NavigationManager.NavigateTo($"/service-tiers/update/{RoleName}/{tier.Id}");
        }

        /// <summary>
        /// Deletes the specified service tier after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// deletion. If the user confirms, the service tier is removed from the system, and the associated data table
        /// is reloaded.</remarks>
        /// <param name="tier">The service tier to be deleted. Must not be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteServiceTier(ServiceTierDto tier)
        {
            var index = _serviceTiers.IndexOf(tier);

            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this tier from this role?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var removalResult = await ServiceTierService.DeleteAsync(tier.Id);
                if (removalResult.Succeeded)
                {
                    _serviceTiers.Remove(_serviceTiers[index]);
                }
                else
                    Snackbar.AddErrors(removalResult.Messages);
            }
        }

        /// <summary>
        /// Creates a new service tier asynchronously and processes the result.
        /// </summary>
        /// <remarks>This method sends a request to create a new service tier using the provided data.  If
        /// the operation is successful, it navigates to the settings page for the specified role.</remarks>
        /// <returns></returns>
        public async Task UpdateAsync()
        {
            var creationResult = await RoleService.UpdateApplicationRole(_role.ToDto());
            creationResult.ProcessResponseForDisplay(Snackbar, async () =>
            {
                if(permissionsPage is not null)
                    await permissionsPage.SaveAsync();
                NavigationManager.NavigateTo($"/userroles/settings/{_role.RoleName}", true);
            });
        }

        /// <summary>
        /// Cancels the current operation and navigates to the service tiers page.
        /// </summary>
        /// <remarks>This method redirects the user to the "/service-tiers" route. Ensure that the  <see
        /// cref="NavigationManager"/> is properly configured and the target route exists  in the application.</remarks>
        public void Cancel()
        {
            NavigationManager.NavigateTo("/security/roles");
        }

        /// <summary>
        /// Displays a dialog to add a role to be managed by the current parent role and updates the list of child roles
        /// if the operation is confirmed.
        /// </summary>
        /// <remarks>This method opens a dialog for the user to specify a role to be managed by the
        /// current parent role.  If the operation is confirmed, the method retrieves the updated list of child roles
        /// for the parent role  and updates the internal state accordingly.</remarks>
        /// <returns></returns>
        public async Task AddRoleToManage()
        {
            var parameters = new DialogParameters<AddRoleToBeManagedToParentRole>
            {
                { x => x.ParentRoleId, _role.RoleId },
                { x => x.ParentRoleName, _role.RoleName }
            };

            var dialog = await DialogService.ShowAsync<AddRoleToBeManagedToParentRole>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var childRolesResult = await RoleService.ChildrenAsync(_role.RoleId);
                childRolesResult.ProcessResponseForDisplay(Snackbar, () => { _childRoles = childRolesResult.Data.ToList(); });
            }
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Asynchronously initializes the component and loads the service tier data.
        /// </summary>
        /// <remarks>This method retrieves a list of service tiers from the provider and processes the
        /// response  for display. If the data retrieval is successful, the service tiers are stored for use within  the
        /// component. The method also sets the component's loaded state to indicate that initialization  is
        /// complete.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _canAddRolesToBeManaged = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Roles.CanAddRolesToBeManaged)).Succeeded;
            _canCreateRoles = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Roles.Create)).Succeeded;
            _canEditRoles = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Roles.Edit)).Succeeded;
            _canRemovetRoles = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.Roles.Delete)).Succeeded;

            var roleResult = await RoleService.RoleAsync(RoleName);
            roleResult.ProcessResponseForDisplay(Snackbar, async () =>
            {
                _role = new UserRoleViewModel(roleResult.Data, false);

                var serviceTierResult = await ServiceTierService.AllEntityServiceTiersAsync(_role.RoleId);
                serviceTierResult.ProcessResponseForDisplay(Snackbar, () => { _serviceTiers = serviceTierResult.Data.ToList(); });

                var childRolesResult = await RoleService.ChildrenAsync(_role.RoleId);
                childRolesResult.ProcessResponseForDisplay(Snackbar, () => { _childRoles = childRolesResult.Data.ToList(); });
            });

            await base.OnInitializedAsync();
            _loaded = true;
        }

        #endregion
    }
}
