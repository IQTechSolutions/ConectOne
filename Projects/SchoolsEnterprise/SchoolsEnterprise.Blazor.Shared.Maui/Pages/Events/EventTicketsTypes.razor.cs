using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using ShoppingModule.Blazor.Components;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Events
{
    /// <summary>
    /// Represents the ticket types associated with a specific school event.
    /// </summary>
    /// <remarks>This class is used to manage and display ticket types for a given event, identified by its
    /// unique <see cref="EventId"/>. It relies on the injected <see cref="ISchoolEventQueryService"/> to retrieve
    /// event-related data.</remarks>
    public partial class EventTicketsTypes
    {
        private bool _loaded;
        private IEnumerable<SchoolEventTicketTypeDto> _ticketTypes = new List<SchoolEventTicketTypeDto>();

        /// <summary>
        /// Gets or sets the provider that supplies the current cart state for the component.
        /// </summary>
        /// <remarks>This property is typically set automatically by the Blazor framework when the
        /// component is used within a cascading context. It enables access to shared cart state across components in
        /// the application.</remarks>
        [CascadingParameter] public CartStateProvider CartStateProvider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        [Parameter] public string EventId { get; set; }

        /// <summary>
        /// Gets or sets the service used to query school event data.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection and should be set to a
        /// non-null instance before use.</remarks>
        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        [Inject] public ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Processes the purchase of a ticket.
        /// </summary>
        /// <remarks>This method handles the necessary operations to complete a ticket purchase.  Ensure
        /// that all required preconditions, such as user authentication and ticket availability,  are met before
        /// calling this method.</remarks>
        public async Task PurchaseTicket(SchoolEventTicketTypeDto ticketType)
        {
            var result = await CartStateProvider.AddToCartAsync(ticketType);
            result.ProcessResponseForDisplay(Snackbar, () =>
            {
                Snackbar.Add($"{ticketType.Name} was successfully added to the cart");
            });
        }

        /// <summary>
        /// Asynchronously initializes the component and performs any required setup logic.
        /// </summary>
        /// <remarks>This method is called by the Blazor framework during the component's initialization
        /// phase. It can be overridden to include additional initialization logic specific to the component. Ensure
        /// that any asynchronous operations within this method complete before the component is rendered.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var result = await SchoolEventQueryService.SchoolEventTicketTypes(EventId);
            result.ProcessResponseForDisplay(Snackbar, () =>
            {
                _ticketTypes = result.Data;
            });
            _loaded = true;

            await base.OnInitializedAsync();
        }

    }
}