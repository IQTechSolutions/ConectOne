using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Events
{
    /// <summary>
    /// The Teams component is responsible for displaying the teams participating in a specific school event.
    /// It fetches the event details and the associated activity groups from the server.
    /// </summary>
    public partial class Teams
    {
        private SchoolEventDto _event = new();
        public bool _loaded;
        private List<ActivityGroupDto> _activityGroups = new();
        private readonly ActivityGroupPageParameters _args = new() { PageSize = 100 };

        /// <summary>
        /// Injected HTTP provider for making API calls.
        /// </summary>
        [Inject] public ISchoolEventQueryService Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query activity group information.
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupProvider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically set by dependency injection. Use the provided ISnackbar
        /// instance to show transient messages or alerts within the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The ID of the event for which the teams are to be displayed.
        /// </summary>
        [Parameter] public string? EventId { get; set; }
        
        /// <summary>
        /// Gets the title of the page, which includes the name of the event.
        /// </summary>
        private string Title => $"{_event.Name} Teams";

        /// <summary>
        /// Navigates to the team members page for the specified activity group.
        /// </summary>
        /// <param name="activityGroupId">The ID of the activity group.</param>
        private void NavigateToTeamMembers(string activityGroupId)
        {
            NavigationManager.NavigateTo($"/eventteams/{_event.EventId}/{activityGroupId}", true);
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
                _args.EventId = EventId;

                // Fetch the event details
                var eventItemResponse = await Provider.SchoolEventAsync(EventId);
                if (eventItemResponse.Succeeded)
                {
                    _event = eventItemResponse.Data;

                    // Fetch the activity groups associated with the event
                    var pagingResponse = await ActivityGroupProvider.PagedActivityGroupsAsync(_args);
                    _activityGroups = pagingResponse.Data;
                    StateHasChanged();

                    _loaded = true;
                }
                else
                {
                    // Display error messages if the event details could not be fetched
                    SnackBar.AddErrors(eventItemResponse.Messages);
                }

                StateHasChanged();
            }
        }

        #endregion
    }
}
