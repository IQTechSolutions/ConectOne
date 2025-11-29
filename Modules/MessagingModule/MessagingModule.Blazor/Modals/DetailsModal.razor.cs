using ConectOne.Blazor.Extensions;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MessagingModule.Blazor.Modals
{
    /// <summary>
    /// The DetailsModal component is responsible for displaying the details of a specific message.
    /// It allows the user to view the message and perform actions such as notifying or canceling.
    /// </summary>
    public partial class DetailsModal
    {
        /// <summary>
        /// The MudDialog instance for managing the dialog state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection and must be set before
        /// use.</remarks>
        [Inject] public IMessageService Service { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// The ID of the message to be displayed.
        /// </summary>
        [Parameter] public string MessageId { get; set; } = null!;

        /// <summary>
        /// The ID of the related notification, if any.
        /// </summary>
        [Parameter] public string? NotificationId { get; set; }

        /// <summary>
        /// The message details.
        /// </summary>
        private MessageDto _message = new MessageDto();

        /// <summary>
        /// Closes the dialog and returns the message details.
        /// </summary>
        public async Task NotifyAsync()
        {
            MudDialog.Close(DialogResult.Ok(_message));
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
        /// Loads the message details from the server.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetMessageAsync(MessageId);
            result.ProcessResponseForDisplay(SnackBar, () => { _message = result.Data; });
            await base.OnInitializedAsync();
        }
    }
}
