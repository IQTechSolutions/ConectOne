using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.DataTransferObjects;
using KiburiOnline.Blazor.Shared.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;
namespace KiburiOnline.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// The UserRoles component manages the assignment and removal of roles for a given user.
    /// It provides functionality to:
    /// 1. Fetch and display a list of roles linked to the user.
    /// 2. Add a new role to the user via a modal dialog.
    /// 3. Remove an existing role from the user with a confirmation prompt.
    /// 4. Navigate to a permissions page for more detailed role management.
    /// 
    /// The user’s privileges are enforced via the AuthorizationService (e.g., _canEditUsers, _canSearchRoles).
    /// </summary>
    public partial class UserRoles
    {
        private string _searchString = string.Empty;
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;
        private bool _loaded;
        private ClaimsPrincipal _currentUser = default!;
        private bool _canCreateRoles;
        private bool _canEditRoles;
        private bool _canRemovetRoles;
        private bool _canSearchRoles;
        private bool _canEditUserInfo;
        private UserRoleViewModel _role = new();
        private MudTable<UserRoleViewModel> _userRoleTable = null!;

        /// <summary>
        /// The Task that provides the authentication state of the current user.
        /// </summary>
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for handling authorization operations.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the dialog service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used to manage navigation and URI manipulation in
        /// Blazor applications.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The user ID for whom roles are being managed (passed via query or route parameter).
        /// </summary>
        [Parameter] public string? UserId { get; set; }

        /// <summary>
        /// The title displayed in the UI, often based on the user's name.
        /// </summary>
        [Parameter] public string Title { get; set; } = string.Empty;

        /// <summary>
        /// A short description about the page or user being managed.
        /// </summary>
        [Parameter] public string Description { get; set; } = string.Empty;

        /// <summary>
        /// A list of user roles (wrapped by UserRoleViewModel) that appears in the MudTable.
        /// </summary>
        public List<UserRoleViewModel> UserRolesList { get; set; } = new();

        /// <summary>
        /// Displays a modal dialog that allows adding a new role to the current user.
        /// After the dialog completes, the user roles table is reloaded to reflect any changes.
        /// </summary>
        private async Task InvokeAddRoleToUserModal()
        {
            // Prepare parameters for the modal dialog
            var parameters = new DialogParameters<AddRoleToUserModal>
            {
                { x => x.UserId, UserId }
            };

            // Configure modal options
            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            // Show the modal dialog
            var dialog = DialogService.Show<AddRoleToUserModal>("Add Role to User", parameters, options);

            // Refresh the list of user roles from the server
            var response = await Provider.GetAsync<IEnumerable<RoleDto>>($"account/roles/user/{UserId}");
            UIExtensions.ProcessResponseForDisplay((IBaseResult)response, SnackBar, () =>
            {
                UserRolesList = Enumerable.Select<RoleDto, UserRoleViewModel>(response.Data, c => new UserRoleViewModel(c, false)).ToList();
            });

            // Wait for the dialog to close
            var dialogResult = await dialog.Result;
            if (!dialogResult.Canceled)
            {
                // Reload the table to show newly added roles (if any)
                await ReloadTable();
            }
        }

        /// <summary>
        /// Displays a modal dialog for creating a new user role and processes the result.
        /// </summary>
        /// <remarks>This method opens a modal dialog where the user can input details for a new user
        /// role.  If the dialog is not canceled, the method attempts to create the role by sending the  provided data
        /// to the server. Upon successful creation, the associated data table is  reloaded. If the creation fails,
        /// error messages are displayed in a snackbar notification.</remarks>
        private async Task CreateUserRole()
        {
            // Configure modal options
            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            // Show the modal dialog
            var dialog = await DialogService.ShowAsync<CreateNewRoleModal>("Create New User Role", options);

            // Wait for the dialog to close
            var dialogResult = await dialog.Result;
            if (!dialogResult!.Canceled)
            {
                var creationResult = await Provider.PostAsync("account/roles/create", ((UserRoleViewModel)dialogResult.Data).ToDto());
                if(creationResult.Succeeded) 
                    await ReloadTable();
                else
                    SnackBar.AddErrors(creationResult.Messages);
            }
        }

        /// <summary>
        /// Re-fetches the user's roles from the server and updates the UserRolesList.
        /// </summary>
        private async Task ReloadTable()
        {
            var url = string.IsNullOrEmpty(UserId) ? "account/roles" : $"account/roles/user/{UserId}";

            var response = await Provider.GetAsync<IEnumerable<RoleDto>>(url);
            ((IBaseResult)response).ProcessResponseForDisplay(SnackBar, () =>
            {
                UserRolesList = Enumerable.Select<RoleDto, UserRoleViewModel>(response.Data, c => new UserRoleViewModel(c, false)).ToList();
            });

            StateHasChanged();
        }

        /// <summary>
        /// Opens a confirmation dialog before removing the user from the specified role.
        /// If confirmed, the user is removed from the role on the server, and the local table is updated.
        /// </summary>
        /// <param name="role">The name/ID of the role to remove from the user.</param>
        private async Task RemoveRole(string roleId)
        {
            // Dialog parameters for confirmation
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this user from this role?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            // Show the confirmation dialog
            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If user confirms
            if (!result!.Canceled)
            {
               // Remove role from user on the server
                var deletionResult = await Provider.DeleteAsync($"account/roles/delete", roleId);
                if (!deletionResult.Succeeded)
                    SnackBar.AddErrors(deletionResult.Messages);
                else
                    SnackBar.AddSuccess("Role Successfully Removed");

                // Reload the roles table to reflect changes
                await ReloadTable();
                StateHasChanged();
            }
        }

        private async Task RemoveUserFromRole(string roleName)
        {
            // Dialog parameters for confirmation
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this role from this user?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            // Show the confirmation dialog
            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If user confirms
            if (!result!.Canceled)
            {
                // Remove role from user on the server
                var deletionResult = await Provider.DeleteAsync($"account/roles/delete/{UserId}", roleName);
                if (!deletionResult.Succeeded)
                    SnackBar.AddErrors(deletionResult.Messages);
                else
                    SnackBar.AddSuccess("Role Successfully Removed");

                // Reload the roles table to reflect changes
                await ReloadTable();
                StateHasChanged();
            }
        }

        /// <summary>
        /// Filters the user roles table based on the search string (_searchString),
        /// checking if the role name or description contains the query.
        /// </summary>
        private bool Search(UserRoleViewModel userRole)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;

            if (userRole.RoleName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (userRole.RoleDescription?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Navigates to a permissions page for the specified role, passing both 
        /// the roleId and the userId as part of the route.
        /// </summary>
        private void NavigateToPermissionsPage(string roleId)
        {
            NavigationManager.NavigateTo($"/permissions/{roleId}");
        }

        /// <summary>
        /// Called on component initialization. Retrieves the current user (to check authorization),
        /// loads user info (like name), then fetches the assigned roles for that user.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Call base to complete Blazor’s standard initialization tasks
            await base.OnInitializedAsync();

            
        }

        /// <summary>
        /// Executes logic after the component has rendered, including initialization tasks for the first render.
        /// </summary>
        /// <remarks>This method is invoked after the component's rendering process is complete. On the
        /// first render, it retrieves the current user's authentication state, checks their permissions for
        /// role-related actions, and fetches user-specific or general role data based on the provided <see
        /// cref="UserId"/>.  If <see cref="UserId"/> is specified, the method retrieves detailed user information and
        /// their assigned roles. Otherwise, it fetches a list of all roles. Errors encountered during data retrieval
        /// are displayed using the <c>SnackBar</c> component.</remarks>
        /// <param name="firstRender">A boolean value indicating whether this is the first time the component has rendered. If <see
        /// langword="true"/>, initialization tasks such as retrieving user information and permissions are performed.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                // Retrieve the authentication state (and thus the current user)
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                _currentUser = authState.User;

                // Check if the current user can edit or search roles
                _canCreateRoles = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.Create)).Succeeded;
                _canEditRoles = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.Edit)).Succeeded;
                _canRemovetRoles = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.Delete)).Succeeded;
                _canSearchRoles = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.Search)).Succeeded;
                _canEditUserInfo = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Edit)).Succeeded;

                if (!string.IsNullOrEmpty(UserId))
                {
                    
                    var result = await Provider.GetAsync<UserInfoDto>($"account/users/{UserId}");
                    if (result.Succeeded)
                    {
                        var user = result.Data;
                        if (user != null)
                        {
                            // Set up the Title and Description based on the retrieved user’s name
                            Title = $"{user.FirstName} {user.LastName}";
                            Description = $"Manage {user.FirstName} {user.LastName}'s Roles";

                            
                            // Fetch roles assigned to this user
                            var response = await Provider.GetAsync<IEnumerable<RoleDto>>($"account/roles/user/{UserId}");
                            if (response.Succeeded)
                            {
                                UserRolesList = response.Data.Select(c => new UserRoleViewModel(c, false)).ToList();
                            }
                            else
                            {
                                SnackBar.AddErrors(response.Messages);
                            }
                        }
                        // Mark the component as fully loaded
                        _loaded = true;
                    }
                    else
                    {
                        // If user info retrieval fails, display error messages
                        SnackBar.AddErrors(result.Messages);
                    }
                }
                else
                {


                    var response = await Provider.GetAsync<IEnumerable<RoleDto>>($"account/roles");
                    if (response.Succeeded)
                    {
                        UserRolesList = response.Data.Select(c => new UserRoleViewModel(c, false)).ToList(); 
                        _loaded = true;
                    }
                    else
                    {
                        SnackBar.AddErrors(response.Messages);
                    }
                }
                StateHasChanged();
            }
        }
    }
}
