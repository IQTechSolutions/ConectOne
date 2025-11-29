using ConectOne.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;

namespace SchoolsModule.Blazor.Pages.ParticipatingActivityGroups
{
    /// <summary>
    /// Represents the attendance component for a school event, providing functionality to display event details and
    /// manage participation information.
    /// </summary>
    /// <remarks>This component is responsible for retrieving and displaying minimal information about a
    /// school event,  as well as determining the name of the participating activity group. It uses injected services
    /// for  HTTP operations and user notifications. The component's lifecycle includes asynchronous initialization  to
    /// fetch event data.</remarks>
    public partial class Attendance
    {
        private SchoolEventDto? _event;
        private string? _activityGroupName;

        private bool _loaded;


        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the injected service for displaying snack bar notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the participating activity group.
        /// </summary>
        [Parameter] public string ParticipatingActivityGroupId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        [Parameter] public string EventId { get; set; } = null!;

        /// <summary>
        /// Asynchronously initializes the component and retrieves minimal information about the school event.
        /// </summary>
        /// <remarks>This method fetches data for the school event specified by <see cref="EventId"/> and
        /// processes the response for display. It also determines the name of the participating activity group, if
        /// applicable, and sets the component's loaded state.</remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var result = await SchoolEventQueryService.SchoolEventAsync(EventId);
            result.ProcessResponseForDisplay(SnackBar, async () =>
            {
                _event = result.Data;
                _activityGroupName = _event.ParticipatingTeams.FirstOrDefault(c => c.ParticipatingActivityGroupId == ParticipatingActivityGroupId)?.Name;
            });
            _loaded = true;
            await base.OnInitializedAsync();
        }
    }
}
