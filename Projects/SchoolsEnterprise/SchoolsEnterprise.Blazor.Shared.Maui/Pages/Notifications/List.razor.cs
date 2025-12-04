using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Extensions;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using NeuralTech.Base.Enums;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Notifications
{
    /// <summary>
    /// Displays a list of notifications for the current user or all users (if the user is a SuperUser).
    /// Allows message-type filtering, navigation to details (marking notifications as read),
    /// and deletion of notifications from the system.
    /// </summary>
    public partial class List
    {
        private string TitleText => MessageType is not null ? $"{((MessageType)MessageType).GetDescription()} Messages" : "Notifications";
        private readonly NotificationPageParameters _args = new();
        private List<NotificationDto> _notificationList = [];
        private bool _loaded;

        /// <summary>
        /// The current authentication state, providing user identity/claims 
        /// to control filtering (e.g., if not a SuperUser, only show user's notifications).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Service for retrieving and manipulating notifications (e.g., marking read, deleting).
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The user ID for whom notifications might be fetched. Typically set 
        /// if you want to display someone else's notifications (SuperUser case).
        /// </summary>
        [Parameter] public string UserId { get; set; } = null!;

        /// <summary>
        /// If provided, filters notifications by the specified <see cref="MessageType"/> 
        /// (e.g., Global, Learner).
        /// </summary>
        [Parameter] public int? MessageType { get; set; }
        
        /// <summary>
        /// Navigates to the details page for a specified notification and marks it as read.
        /// </summary>
        /// <remarks>If the notification is related to an event that has already been completed, an error
        /// message is displayed and navigation does not occur. Otherwise, the method navigates to the appropriate
        /// details page and marks the notification as read.</remarks>
        /// <param name="url">The URL to navigate to for displaying the notification details.</param>
        /// <param name="notificationId">The unique identifier of the notification to display and mark as read.</param>
        /// <returns>A task that represents the asynchronous navigation and notification update operation.</returns>
        public async Task NavigateToNotificationDetails(string url, string notificationId)
        {
            var itemToCheck = _notificationList.FirstOrDefault(c => c.NotificationId == notificationId);

            if (itemToCheck.NotificationUrl.Contains("events"))
            {
                var eventResult = await Provider.GetAsync<SchoolEventDto>($"schoolevents/{itemToCheck.EntityId}");
                if (eventResult.Succeeded)
                {
                    if (eventResult.Data.StartDate.Date < DateTime.Now.Date)
                    {
                        SnackBar.AddError("This event has already been completed");
                        return;
                    }
                }
                else
                {
                    SnackBar.AddError("There was an error retrieving the event");
                    return;
                }
            }
            NavigationManager.NavigateTo(url);
            await Provider.GetAsync<NotificationDto>($"notifications/{notificationId}/markasread");
        }

        /// <summary>
        /// Deletes a notification after asking for user confirmation. 
        /// Refreshes the local list of notifications upon success.
        /// </summary>
        /// <param name="notificationId">ID of the notification to be removed.</param>
        private async Task DeleteNotification(string notificationId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this notification from this application?" },
                { x => x.ButtonText, "Yes" }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var authState = await AuthenticationStateTask;

                // Perform the delete operation
                var notificationResult = await Provider.DeleteAsync($"notifications", notificationId);
                if (notificationResult.Succeeded)
                {

                    // If the user is a SuperUser, fetch all notifications. Otherwise, fetch only their own.
                    if (!authState.User.IsInRole(RoleConstants.SuperUser))
                    {
                        _args.ReceiverId = authState.User.GetUserId();
                    }
                    var notificationListResult = await Provider.GetPagedAsync<NotificationDto, NotificationPageParameters>("notifications/paged", _args);
                    notificationListResult.ProcessResponseForDisplay(SnackBar, () => { _notificationList = notificationListResult.Data; });
                }
            }
        }

        #region Overrides

        /// <summary>
        /// Asynchronously initializes the component and loads the initial notification data.
        /// </summary>
        /// <remarks>This method retrieves the current user's authentication state and uses it to set the
        /// receiver ID for notifications.  If a <see cref="MessageType"/> is specified, it is applied to the
        /// notification query parameters.  The method then fetches a paginated list of notifications and processes the
        /// response for display.  Finally, it marks the component as loaded and calls the base
        /// implementation.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            if (MessageType.HasValue)
                _args.MessageType = (MessageType)MessageType;

            _args.ReceiverId = authState.User.GetUserId();

            var notificationListResult = await Provider.GetPagedAsync<NotificationDto, NotificationPageParameters>("notifications/paged", _args);
            notificationListResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _notificationList = notificationListResult.Data;
            });

            _loaded = true;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
