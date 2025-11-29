using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SchoolsModule.Domain.Constants;

namespace SchoolsModule.Blazor.Components.Teachers
{
    /// <summary>
    /// Component for displaying actions menu in the teachers table.
    /// </summary>
    public partial class TeachersTableActionsMenu
    {
        private bool _canEdit;
        private bool _canDelete;
        private bool _canAddMessage;
        private bool _canCreateChat;
        private UserInfoDto? _userInfo;

        /// <summary>
        /// Gets or sets the authentication state task, which provides the current authentication state.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Event callback for removing a teacher.
        /// </summary>
        [Parameter] public EventCallback<string> RemoveTeacher { get; set; }

        /// <summary>
        /// Gets or sets the ID of the teacher.
        /// </summary>
        [Parameter, EditorRequired] public string TeacherId { get; set; } = null!;
        
        /// <summary>
        /// Indicates whether the RemoveTeacher event callback is set.
        /// </summary>
        private bool IsRemoveTeacherSet => RemoveTeacher.HasDelegate;

        /// <summary>
        /// Invokes the RemoveTeacher event callback.
        /// </summary>
        private async Task OnRemoveLearner()
        {
            await RemoveTeacher.InvokeAsync(TeacherId);
        }

        /// <summary>
        /// Initializes the component asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ParentPermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ParentPermissions.Delete)).Succeeded;
            _canAddMessage = (await AuthorizationService.AuthorizeAsync(authState.User, IdentityModule.Domain.Constants.Permissions.Users.CanSendMessage)).Succeeded;
            _canCreateChat = (await AuthorizationService.AuthorizeAsync(authState.User, IdentityModule.Domain.Constants.Permissions.Users.CanCreateChat)).Succeeded;

            var userInfoResult = await UserService.GetUserInfoAsync(TeacherId);
            if (userInfoResult is { Succeeded: true, Data: not null })
            {
                _userInfo = userInfoResult.Data;
            }

            await base.OnInitializedAsync();
        }
    }
}
