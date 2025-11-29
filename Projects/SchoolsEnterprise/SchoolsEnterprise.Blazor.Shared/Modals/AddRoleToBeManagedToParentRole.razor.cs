using ConectOne.Blazor.Extensions;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Modals
{
    /// <summary>
    /// The AddRoleToUserModal component allows an administrator or privileged user
    /// to assign a new role to an existing user. It retrieves all available roles, 
    /// lets the user pick one, and then updates the selected user's role assignment 
    /// upon submission.
    /// </summary>
    public partial class AddRoleToBeManagedToParentRole
    {
        private bool _loaded = false;
        private RoleDto _userRole = new();
        private List<RoleDto> _roles = [];
        private string _userDisplayName = string.Empty;
        Func<RoleDto, string> roleConverter = p => p?.Name ?? string.Empty;
        
        /// <summary>
        /// A reference to the current MudDialogInstance, enabling control over the modal 
        /// (e.g., closing or canceling).
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

        /// <summary>
        /// The ID of the user to whom a role will be assigned, passed in via parameter.
        /// </summary>
        [Parameter] public string ParentRoleName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the parent role.
        /// </summary>
        [Parameter] public string ParentRoleId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages to the
        /// user.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = default!;               

        /// <summary>
        /// Injected service for account-related actions, including retrieving user info 
        /// and assigning roles.
        /// </summary>
        [Inject] public IRoleService RoleService { get; set; } = default!;

        /// <summary>
        /// Submits the selected role assignment to the user by calling the AccountProvider. 
        /// If successful, closes the modal; otherwise, shows an error and cancels the dialog.
        /// </summary>
        public async Task SubmitAsync()
        {
            // Prepare the request with the user ID and the selected role’s name
            var response = await RoleService.AddRoleToBeManagedToParentAsync(ParentRoleId, _userRole.Id);

            if (response.Succeeded)
            {
                // Close the dialog if the role was successfully assigned
                MudDialog.Close();
            }
            else
            {
                // Show an error message and cancel the dialog
                SnackBar.AddErrors(response.Messages);
                MudDialog.Cancel();
            }
        }

        /// <summary>
        /// Cancels the modal dialog, discarding any changes to user roles.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        #region Overrides

        /// <summary>
        /// Called when the component is initialized. It retrieves user info (to display the name)
        /// and fetches all available roles to populate the selection. 
        /// Once loaded, the UI is marked as ready to display.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Fetch and convert roles to local view models
            var roleResult = await RoleService.AllRoles();
            _roles = roleResult.Data.ToList();

            _roles.Remove(_roles.FirstOrDefault(c => c.Name == ParentRoleName));

            // Indicate that loading has completed
            _loaded = true;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
