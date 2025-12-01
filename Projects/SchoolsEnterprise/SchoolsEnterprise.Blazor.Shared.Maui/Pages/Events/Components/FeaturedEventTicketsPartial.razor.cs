using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Events.Components
{
    /// <summary>
    /// Represents a Blazor component that displays a list of featured school events with available tickets.`
    /// </summary>
    /// <remarks>This partial class is intended for use within a Blazor application to present featured events
    /// for ticket sales. It relies on dependency injection for required services and initializes its data
    /// asynchronously when the component is first rendered.</remarks>
    public partial class FeaturedEventTicketsPartial
    {
        private readonly SchoolEventPageParameters _args = new() { Featured = true };
        private List<SchoolEventDto> _events = new();

        /// <summary>
        /// Gets or sets the service used to query school event data.
        /// </summary>
        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Use this
        /// service to show transient messages or alerts in the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs and for responding to navigation events within a Blazor application. This property is
        /// typically set by the framework through dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component and loads the list of school events available for ticket sales.
        /// </summary>
        /// <remarks>This method is called by the Blazor framework during component initialization. It
        /// retrieves event data and prepares it for display. Override this method to perform additional asynchronous
        /// setup when the component is initialized.</remarks>
        /// <returns>A task that represents the asynchronous initialization operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var result = await SchoolEventQueryService.PagedSchoolEventsForAppForTicketSalesAsync(_args);
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                _events = result.Data;
            });
            await base.OnInitializedAsync();
        }
    }
}
