using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Extensions;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using NeuralTech.Base.Enums;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Messages
{
    /// <summary>
    /// Displays a list of messages for a given user or message type. 
    /// Retrieves and displays messages from the <see cref="IMessagingProvider"/>,
    /// and allows navigation to message details or deletion of messages.
    /// </summary>
    public partial class List
    {
        private readonly MessagePageParameters _args = new();
        private List<MessageDto> _messageList = [];
        private bool _loaded;

        /// <summary>
        /// Provides the current authentication state (for determining the user ID 
        /// or checking roles if not a super user).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Service used for fetching, updating, and deleting messages.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications in the user interface.
        /// </summary>
        /// <remarks>This property is typically set by dependency injection. Use the provided ISnackbar
        /// instance to show transient messages or alerts to users.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The ID of the user whose messages we are displaying (optional if 
        /// the current user is the only one targeted).
        /// </summary>
        [Parameter] public string UserId { get; set; } = null!;

        /// <summary>
        /// A nullable integer representing the <see cref="MessageType"/> of messages 
        /// to filter. If null, no type-based filtering is applied.
        /// </summary>
        [Parameter] public int? MessageType { get; set; }

        // Returns a string for the title of the page or section, based on whether a 
        // specific MessageType was provided.
        private string TitleText => MessageType is not null
            ? $"{(MessageType)MessageType} Messages"
            : "Messages";

        /// <summary>
        /// Navigates to the message details page, optionally including a notification ID if relevant.
        /// </summary>
        /// <param name="messageId">The unique ID of the message to view.</param>
        /// <param name="notificationId">An optional notification ID if this message relates to a notification record.</param>
        public void NavigateToMessageDetails(string messageId, string? notificationId = null)
        {
            // If no notification ID is supplied, navigate to "messages/{messageId}";
            // otherwise, navigate to "notifications/{notificationId}".
            NavigationManager.NavigateTo(string.IsNullOrEmpty(notificationId)
                ? $"messages/{messageId}"
                : $"notifications/{notificationId}");
        }

        /// <summary>
        /// Removes a message from the system after user confirmation, 
        /// and then refreshes the message list.
        /// </summary>
        /// <param name="productId">The ID of the message to delete.</param>
        private async Task RemoveNotification(string? productId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this message from this application?" },
                { x => x.ButtonText, "Yes" }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var removalResult = await Provider.DeleteAsync("messages/deleteMessage", productId);
                if (removalResult.Succeeded)
                {
                    // Reload the messages with the existing _args filter parameters.
                    var messageListResult = await Provider.GetPagedAsync<MessageDto, MessagePageParameters>("messages/bynotification", _args);
                    messageListResult.ProcessResponseForDisplay(SnackBar, () =>
                    {
                        _messageList = messageListResult.Data;
                    });
                }
            }
        }

        #region Overrides

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                // If a MessageType parameter is provided, apply it to our arguments.
                if (MessageType is not null)
                    _args.MessageType = (MessageType)MessageType;

                var authState = await AuthenticationStateTask;

                // If the user isn't a super user, only fetch messages for their own user ID.
                if (!authState.User.IsInRole(RoleConstants.SuperUser))
                {
                    _args.ReceiverId = authState.User.GetUserId();
                }

                //_args.EntityId = authState.User.GetUserId();
                //_args.MessageType = NeuralTech.Base.Enums.MessageType.Parent;

                // Retrieve messages matching our filter parameters.
                var messageListResult = await Provider.GetPagedAsync<MessageDto, MessagePageParameters>("messages/bynotification", _args);
                messageListResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    _messageList = messageListResult.Data;
                });

                _loaded = true;
                StateHasChanged();
            }
        }

        #endregion
    }
}