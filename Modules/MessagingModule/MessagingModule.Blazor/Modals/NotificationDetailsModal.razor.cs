using ConectOne.Blazor.Extensions;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MessagingModule.Blazor.Modals
{
    /// <summary>
    /// The NotificationDetailsModal component is responsible for displaying the details of a specific notification.
    /// It allows the user to view the notification and perform actions such as notifying or canceling.
    /// </summary>
    public partial class NotificationDetailsModal
    {
        /// <summary>
        /// The MudDialog instance for managing the dialog state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts in the user interface. Ensure that the service is properly configured and injected before
        /// use.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] public INotificationService NotificationService { get; set; } = null!;

        /// <summary>
        /// The ID of the notification to be displayed.
        /// </summary>
        [Parameter] public string NotificationId { get; set; } = null!;

        /// <summary>
        /// The notification details.
        /// </summary>
        private NotificationDto _notification = new NotificationDto();

        /// <summary>
        /// Closes the dialog and returns the notification details.
        /// </summary>
        public async Task NotifyAsync()
        {
            MudDialog.Close(DialogResult.Ok(_notification));
        }

        /// <summary>
        /// Cancels the dialog without returning any data.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Loads the notification details from the server.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await NotificationService.NotificationAsync(NotificationId);
            result.ProcessResponseForDisplay(SnackBar, () => {
                _notification = result.Data;
            });
            await base.OnInitializedAsync();
        }
    }
}