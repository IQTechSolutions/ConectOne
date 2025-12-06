using ConectOne.Blazor.Extensions;
using IdentityModule.Application.ViewModels;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KiburiOnline.Blazor.Shared.Modals
{
    /// <summary>
    /// The AddRoleToUserModal component allows an administrator or privileged user
    /// to assign a new role to an existing user. It retrieves all available roles, 
    /// lets the user pick one, and then updates the selected user's role assignment 
    /// upon submission.
    /// </summary>
    public partial class AddRoleToUserModal
    {
        private bool _loaded = false;
        private UserRoleViewModel _userRole = new();
        private IEnumerable<UserRoleViewModel> _roles = Enumerable.Empty<UserRoleViewModel>();
        private string _userDisplayName = string.Empty;
        Func<UserRoleViewModel, string> roleConverter = p => p?.RoleName ?? string.Empty;

        /// <summary>
        /// The ID of the user to whom a role will be assigned, passed in via parameter.
        /// </summary>
        [Parameter] public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// A reference to the current MudDialogInstance, enabling control over the modal 
        /// (e.g., closing or canceling).
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

        /// <summary>
        /// Gets or sets the injected <see cref="ISnackbar"/> service used to display notifications or messages to the
        /// user.
        /// </summary>
        [Inject] private ISnackbar SnackBar { get; set; } = default!;               

        /// <summary>
        /// Injected service for account-related actions, including retrieving user info 
        /// and assigning roles.
        /// </summary>
        [Inject] public IRoleService AccountProvider { get; set; } = default!;

        /// <summary>
        /// Gets or sets the user service used to manage user-related operations within the component.
        /// </summary>
        /// <remarks>This property is typically injected by the framework to provide access to user
        /// management functionality, such as authentication, retrieval, or updates. Do not set this property manually
        /// unless overriding the default service implementation.</remarks>
        [Inject] public IUserService UserService { get; set; } = default!;

        /// <summary>
        /// Submits the selected role assignment to the user by calling the AccountProvider. 
        /// If successful, closes the modal; otherwise, shows an error and cancels the dialog.
        /// </summary>
        public async Task SubmitAsync()
        {
            // Prepare the request with the user ID and the selected role’s name
            var response = await AccountProvider.AddUserToRoleAsync(UserId, _userRole.RoleName);

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
            await base.OnInitializedAsync();

            // Fetch user info for display
            var user = await UserService.GetUserInfoAsync(UserId);
            _userDisplayName = $"{user.Data.FirstName} {user.Data.LastName}";

            // Fetch and convert roles to local view models
            var roleResult = await AccountProvider.AllRoles();
            _roles = roleResult.Data.Select(c => new UserRoleViewModel(c, false)).ToList();

            // Indicate that loading has completed
            _loaded = true;
        }

        #endregion
    }
}
