using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.Interfaces.SchoolEvents;

namespace SchoolsModule.Blazor.Modals
{
    /// <summary>
    /// Represents a modal component for managing ticket types in a school event.
    /// </summary>
    /// <remarks>This component is used to create or update ticket types associated with a specific school
    /// event. It interacts with the provided services to perform data operations and displays feedback to the user via
    /// a snackbar notification system.</remarks>
    public partial class TicketTypeModal
    {
        /// <summary>
        /// A reference to the <see cref="MudDialogInstance"/> managing this modal.
        /// Provides methods for closing or canceling the dialog.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query school event data.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation of <see cref="ISchoolEventQueryService"/> is provided before using this property.</remarks>
        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service responsible for executing commands related to school events.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation  is provided before using any functionality that depends on this service.</remarks>
        [Inject] public ISchoolEventCommandService SchoolEventCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snackbars for user notifications.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts to the user. Ensure that the service is properly configured and injected before use.</remarks>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        [Parameter] public string? EventId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the ticket type.
        /// </summary>
        [Parameter] public SchoolEventTicketTypeViewModel TicketType { get; set; } = new SchoolEventTicketTypeViewModel();

        /// <summary>
        /// Saves the current ticket type by either creating a new ticket type or updating an existing one.
        /// </summary>
        /// <remarks>If <see cref="TicketTypeId"/> is null or empty, a new ticket type is created.
        /// Otherwise, the existing ticket type identified by <see cref="TicketTypeId"/> is updated. The operation
        /// result is displayed using the provided <see cref="Snackbar"/> instance.</remarks>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public void Save()
        {
            MudDialog.Close(TicketType);
        }

        /// <summary>
        /// Cancels the current dialog operation.
        /// </summary>
        /// <remarks>This method triggers the cancellation of the dialog by invoking the <see
        /// cref="MudDialog.Cancel"/> method. It is typically used to close the dialog without returning a
        /// result.</remarks>
        public async void Cancel()
        {
            MudDialog.Cancel();
        }

        /// <summary>
        /// Asynchronously initializes the component and sets the <see cref="Microsoft.Extensions.Logging.EventId"/> property.
        /// </summary>
        /// <remarks>This method is called by the Blazor framework during the component's initialization
        /// phase. It sets the <see cref="Microsoft.Extensions.Logging.EventId"/> to the value of <see cref="EventId"/> and then invokes
        /// the base implementation to complete the initialization process.</remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            TicketType.EventId = EventId;
            await base.OnInitializedAsync();
        }
    }
}
