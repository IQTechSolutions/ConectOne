using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Messages
{
    /// <summary>
    /// Represents the details page for viewing and managing a specific message.
    /// </summary>
    public partial class Details
    {
        #region Injected Services

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications in the user interface.
        /// </summary>
        /// <remarks>This property is typically provided via dependency injection. Use this service to
        /// show brief messages or alerts to users, such as confirmations or error notifications.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// It enables components to perform navigation and respond to URI changes.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display notifications to the user.
        /// </summary>
        [Inject] public INotificationService NotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display messages to the user.
        /// </summary>
        [Inject] public IMessageService MessageService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used to invoke JavaScript functions from .NET code.
        /// </summary>
        /// <remarks>This property is typically set by the Blazor framework through dependency injection.
        /// Use this instance to perform JavaScript interop operations within the component.</remarks>
        [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the user ID associated with the message.
        /// </summary>
        [Parameter] public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the message ID to be displayed.
        /// </summary>
        [Parameter] public string MessageId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional notification ID associated with the message.
        /// </summary>
        [Parameter] public string? NotificationId { get; set; }

        #endregion

        #region Private Fields

        /// <summary>
        /// Holds the details of the notification message.
        /// </summary>
        private MessageDto _notificationDto = null!;

        /// <summary>
        /// Indicates whether the component has finished loading.
        /// </summary>
        private bool _loaded;

        #endregion

        #region Private Methods

        /// <summary>
        /// Removes the notification after user confirmation.
        /// </summary>
        private async Task RemoveNotification()
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this notification from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, MudBlazor.Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var removalResult = await NotificationService.RemoveNotificationAsync(_notificationDto.NotificationId);
                removalResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    NavigationManager.NavigateTo("notifications");
                });
            }
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Method invoked after the component has rendered.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var messageResult = await MessageService.GetMessageAsync(MessageId);
                if(messageResult.Succeeded) 
                    _notificationDto = messageResult.Data;
                _loaded = true;
                StateHasChanged();

                if (_notificationDto == null)
                {
                    var parameters = new DialogParameters<ConformtaionModal>
                    {
                        { x => x.ContentText, "This message was removed from the database by Admin, would you like to remove this notification?" },
                        { x => x.ButtonText, "Yes" }
                    };

                    var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
                    var result = await dialog.Result;

                    if (!result!.Canceled)
                    {
                        var removalResult = await NotificationService.RemoveNotificationAsync(NotificationId);
                        if (!removalResult.Succeeded) SnackBar.AddErrors(removalResult.Messages);
                        SnackBar.Add("Notification was removed successfully", Severity.Success);
                    }
                    await JsRuntime.InvokeVoidAsync("history.back");
                }
            }
        }

        #endregion
    }
}
