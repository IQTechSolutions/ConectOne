using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using IdentityModule.Domain.Extensions;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Notifications
{
    /// <summary>
    /// The Details component is responsible for displaying the details of a specific notification.
    /// It allows the user to view and remove the notification.
    /// </summary>
    public partial class Details
    {
        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// The ID of the user.
        /// </summary>
        [Parameter] public string UserId { get; set; } = null!;

        /// <summary>
        /// The ID of the notification to be displayed.
        /// </summary>
        [Parameter] public string NotificationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display notifications to the user.
        /// </summary>
        [Inject] public INotificationService NotificationService { get; set; }

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>Typically used to show notifications, alerts, or feedback messages in the
        /// application's user interface. The implementation of ISnackbar determines how messages are presented and
        /// managed.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to programmatically navigate to different URIs or to access information about the current navigation
        /// state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; }

        /// <summary>
        /// The notification details.
        /// </summary>
        private NotificationDto? _notificationDto;

        /// <summary>
        /// Indicates whether the component has completed loading and can render content.
        /// </summary>
        private bool _loaded;

        /// <summary>
        /// Removes the notification after user confirmation.
        /// </summary>
        private async Task RemoveNotification()
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this notification from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var removalResult = await NotificationService.RemoveNotificationAsync(_notificationDto!.NotificationId);
                removalResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    NavigationManager.NavigateTo("notifications");
                });
            }
        }

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
                var authState = await AuthenticationStateTask;
                UserId = authState.User.GetUserId();

                var notifiacationResult = await NotificationService.NotificationAsync(NotificationId);
                notifiacationResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    _notificationDto = notifiacationResult.Data;
                });

                _loaded = true;

                StateHasChanged();
            }
        }
    }
}
