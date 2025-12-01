using Microsoft.AspNetCore.Components;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Events
{
    /// <summary>
    /// Represents a component for displaying and managing team members of a specific activity group within an event.
    /// </summary>
    public partial class TeamMembers
    {
        private SchoolEventDto _event;
        private ActivityGroupDto _team;
        private List<LearnerDto> _learners = new();
        private bool _loaded;
        private LearnerPageParameters _args = new LearnerPageParameters() { PageSize = 100 };

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework and should
        /// be set before making any HTTP requests. Ensure that a valid implementation is provided to avoid runtime
        /// errors.</remarks>
        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier for the activity group.
        /// </summary>
        [Parameter] public string ActivityGroupId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        [Parameter] public string EventId { get; set; } = null!;
        
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
                // Set the ActivityGroupId parameter for fetching learners
                _args.ActivityGroupId = ActivityGroupId;

                // Fetch the event details based on the provided EventId
                var eventResult = await SchoolEventQueryService.SchoolEventAsync(EventId);
                if (eventResult.Succeeded)
                {
                    _event = eventResult.Data;

                    // Find the specific activity group within the event
                    _team = _event.ParticipatingTeams.FirstOrDefault(c => c.ActivityGroupId == ActivityGroupId);

                    // Get the list of learners in the team
                    _learners = _team.TeamMembers.ToList();

                    // Mark the component as loaded
                    _loaded = true;
                }
                StateHasChanged();
            }
        }
    }
}
