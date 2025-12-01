using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.RequestFeatures;
using MessagingModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Chats
{
    /// <summary>
    /// Blazor component for displaying and managing user's chat groups.
    /// </summary>
    public partial class ChatGroups
    {
        #region Injected Dependencies

        /// <summary>
        /// Provides the authentication state of the current user.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// HTTP provider used to communicate with the backend API.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Snackbar service for showing user notifications.
        /// </summary>
        [Inject] public ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to programmatically navigate to different URIs or to access information about the current navigation
        /// state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        #endregion

        #region Private Fields

        /// <summary>
        /// Indicates if the component has finished loading the chat groups.
        /// </summary>
        private bool _loaded = false;

        /// <summary>
        /// List of chat groups retrieved for the current user.
        /// </summary>
        private List<ChatGroupDto> _chatGroups = new();

        /// <summary>
        /// Represents a collection of user contact information.
        /// </summary>
        /// <remarks>This field is used to store a list of user contact details as <see
        /// cref="UserInfoDto"/> objects. It is initialized as an empty list and can be populated with user data as
        /// needed.</remarks>
        private List<UserInfoDto> _contacts = new();

        #endregion

        #region Navigation

        /// <summary>
        /// Navigates to the chat messages view for the specified group.
        /// </summary>
        /// <param name="groupId">The ID of the chat group to open.</param>
        public async Task OpenMessageList(string groupId, string groupname)
        {
            // Await a task delay to maintain async signature (can be removed if no longer needed).
            await Task.Delay(0);

            NavigationManager.NavigateTo($"/chats/{groupId}/{groupname}");
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Called when the component is initialized. Loads the user's chat groups.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var userId = authState.User.GetUserId();

            var result = await Provider.GetAsync<List<ChatGroupDto>>($"chats/groups/{userId}");
            result.ProcessResponseForDisplay(Snackbar, () =>
            {
                _chatGroups = result.Data;
                _loaded = true;
            });

            if (authState.User.IsInRole(RoleConstants.SuperUser) || authState.User.IsInRole(RoleConstants.Administrator))
            {
                var args = new UserPageParameters();
                var contactResult = await Provider.GetAsync<List<UserInfoDto>>($"account/users/{args.GetQueryString()}");
                if (contactResult.Succeeded)
                {
                    _contacts = contactResult.Data;
                }
            }
            
            await base.OnInitializedAsync();
        }

        #endregion
    }
}
