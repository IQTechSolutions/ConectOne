using ConectOne.Blazor.Extensions;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Domain.Constants;
using SchoolsModule.Domain.Interfaces;

namespace SchoolsModule.Blazor.Components.Parents
{
    /// <summary>
    /// Represents the actions menu for a row in the parents table.
    /// This component provides context-specific options such as editing or removing a parent, 
    /// depending on the current user's permissions.
    /// </summary>
    public partial class ParentsTableActionsMenu
    {
        #region Private Fields

        private bool _canEdit;
        private bool _canDelete;
        private bool _canCreateChat;
        private bool _canSendMessage;
        private UserInfoDto? _userInfo;
        private string _currentUserId = string.Empty;

        #endregion

        #region Cascading Parameters

        /// <summary>
        /// Gets or sets the authentication state task used to retrieve the current user's identity and roles.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        #endregion

        #region Injected Services

        /// <summary>
        /// Provides access to HTTP operations for server communication.
        /// </summary>
        [Inject] public IParentService ParentService { get; set; } = null!;

        /// <summary>
        /// Provides access to the snackbar UI component for displaying notifications.
        /// </summary>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Provides access to the authorization service for checking permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Provides access to the navigation manager for programmatic route changes.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the callback to invoke when the user chooses to remove a parent.
        /// </summary>
        [Parameter] public EventCallback<string> RemoveParent { get; set; }

        /// <summary>
        /// Gets or sets the ID of the parent this actions menu is associated with.
        /// </summary>
        [Parameter, EditorRequired] public string ParentId { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the delete option should be shown in the menu.
        /// </summary>
        [Parameter] public bool DisplayDeleteButton { get; set; } = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the <see cref="RemoveParent"/> callback has been assigned.
        /// </summary>
        private bool IsRemoveParentSet => RemoveParent.HasDelegate;

        #endregion

        #region Methods

        /// <summary>
        /// Initiates the creation or joining of a chat group for the specified parent.
        /// On success, navigates to the newly created chat group.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateChatGroup()
        {
            var result = await ParentService.CreateParentChatGroupAsync(ParentId, _currentUserId);

            if (result.Succeeded)
            {
                NavigationManager.NavigateTo($"chats/{result.Data}");
            }
            else
            {
                Snackbar.AddErrors(result.Messages);
            }
        }

        /// <summary>
        /// Handles the "Remove Parent" action by invoking the <see cref="RemoveParent"/> callback.
        /// </summary>
        private async Task OnRemoveParent()
        {
            await RemoveParent.InvokeAsync(ParentId);
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Called when the component is initialized.
        /// Retrieves the current user's identity and checks permissions to determine which actions to display.
        /// </summary>
        /// <returns>A task representing the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _currentUserId = authState.User.GetUserId();

            // Determine edit permission
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ParentPermissions.Edit)).Succeeded;

            // Determine delete permission
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ParentPermissions.Delete)).Succeeded;

            _canCreateChat = (await AuthorizationService.AuthorizeAsync(authState.User, IdentityModule.Domain.Constants.Permissions.Users.CanCreateChat)).Succeeded;
            _canSendMessage = (await AuthorizationService.AuthorizeAsync(authState.User, IdentityModule.Domain.Constants.Permissions.Users.CanSendMessage)).Succeeded;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
