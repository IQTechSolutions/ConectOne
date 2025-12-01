using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Events
{
    /// <summary>
    /// Represents a component for managing and displaying event tickets within the application.
    /// </summary>
    /// <remarks>This class is responsible for initializing and displaying a list of school events available
    /// for ticket sales. It interacts with the <see cref="ISchoolEventQueryService"/> to fetch event data and provides
    /// navigation functionality to view ticket types for a specific event.</remarks>
    public partial class EventTickets
    {
        private readonly SchoolEventPageParameters _args = new();
        private List<SchoolEventDto> _events = new();
        private bool _loaded;

        /// <summary>
        /// Gets or sets the service used to query school event data.
        /// </summary>
        /// <remarks>This property is typically injected via dependency injection. Ensure that a valid
        /// implementation of <see cref="ISchoolEventQueryService"/> is provided before using this property.</remarks>
        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>The navigation manager provides methods for navigating to different URIs and
        /// retrieving the current navigation state within a Blazor application. This property is typically set by the
        /// Blazor framework via dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component and loads the school events for ticket sales.
        /// </summary>
        /// <remarks>This method retrieves a paginated list of school events using the provided query
        /// service  and processes the response for display. It sets the loaded state to indicate that the 
        /// initialization is complete.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var result = await SchoolEventQueryService.PagedSchoolEventsForAppForTicketSalesAsync(_args);
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                _events = result.Data;
            });
            _loaded = true;
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Navigates to the ticket types page for the specified event.
        /// </summary>
        /// <remarks>This method constructs the navigation URL using the provided <paramref
        /// name="eventId"/>  and redirects the user to the corresponding ticket types page.</remarks>
        /// <param name="eventId">The unique identifier of the event whose ticket types page should be opened. Cannot be null or empty.</param>
        protected void OpenTicketTypes(string eventId)
        {
            NavigationManager.NavigateTo($"/events/tickets/{eventId}");
        }
    }
}
