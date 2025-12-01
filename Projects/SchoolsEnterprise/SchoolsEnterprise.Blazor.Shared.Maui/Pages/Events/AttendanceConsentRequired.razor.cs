using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using IdentityModule.Domain.Extensions;
using MessagingModule.Domain.Enums;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Events
{
    /// <summary>
    /// Blazor component responsible for managing attendance consent operations for a specific school event.
    /// Includes actions to give, refuse, or retract consent for a learner's participation in an event.
    /// </summary>
    public partial class AttendanceConsentRequired
    {
        private bool _loaded;
        private ClaimsPrincipal _user = null!;
        private string _userId = null!;
        private List<SchoolEventPermissionsDto> _schoolEventPermissions = new();

        /// <summary>
        /// Cascading parameter to obtain the user's authentication state
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API calls
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage permissions for school events.
        /// </summary>
        [Inject] public ISchoolEventPermissionService SchoolEventPermissionService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI in the application.
        /// </summary>
        /// <remarks>The navigation manager provides methods for navigating to different URIs and for
        /// handling navigation events within a Blazor application. This property is typically injected by the
        /// framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Event information passed down from a parent component or page
        /// </summary>
        [Parameter, EditorRequired] public SchoolEventDto SchoolEvent { get; set; } = null!;

        /// <summary>
        /// The parent ID associated with the current user
        /// </summary>
        [Parameter] public string ParentId { get; set; }    
        
        /// <summary>
        /// Sends a consent approval for a specific team member in a school event.
        /// </summary>
        /// <param name="consentType">Type of consent being granted.</param>
        /// <param name="eventId">Event ID associated with the consent.</param>
        /// <param name="teamMemberId">Team member's ID.</param>
        /// <param name="parentId">ID of the parent giving consent.</param>
        /// <param name="learnerId">ID of the learner involved in the event.</param>
        public async Task GiveConsent(ConsentTypes consentType, string eventId, string teamMemberId, string parentId, string learnerId, string participatingActivityGroupId, ConsentDirection? consentDirection)
        {
            var consentResult = await SchoolEventPermissionService.GiveConsent(
                new TeamMemberPermissionsParams(eventId, parentId, learnerId, participatingActivityGroupId, teamMemberId, true, consentType, consentDirection));

            consentResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess(consentResult.Messages.FirstOrDefault()!);
            });

            NavigationManager.NavigateTo($"/events/{SchoolEvent.EventId}", true);
        }

        /// <summary>
        /// Submits a refusal for consent for a specific team member in a school event.
        /// </summary>
        /// <param name="consentType">Type of consent being refused.</param>
        /// <param name="eventId">Event ID associated with the consent.</param>
        /// <param name="teamMemberId">Team member's ID.</param>
        /// <param name="parentId">ID of the parent refusing consent.</param>
        /// <param name="learnerId">ID of the learner involved in the event.</param>
        public async Task RefuseConsent(ConsentTypes consentType, string eventId, string teamMemberId, string parentId, string learnerId, string participatingActivityGroupId)
        {
            var consentResult = await SchoolEventPermissionService.RetractConsent(
                new TeamMemberPermissionsParams(eventId, parentId, learnerId, participatingActivityGroupId, teamMemberId, false, consentType));

            consentResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess(consentResult.Messages.FirstOrDefault()!);
            });

            NavigationManager.NavigateTo($"/events/{SchoolEvent.EventId}", true);
        }

        /// <summary>
        /// Revokes previously given consent for a specific learner in an event.
        /// </summary>
        /// <param name="consentType">Type of consent to be retracted.</param>
        /// <param name="eventId">ID of the event.</param>
        /// <param name="learnerId">ID of the learner.</param>
        public async Task RetractConsent(ConsentTypes consentType, string eventId, string learnerId, string participatingActivityGroupId)
        {
            var consentResult = await SchoolEventPermissionService.RetractConsent(new TeamMemberPermissionsParams
                {
                    ConsentType = consentType,
                    EventId = eventId,
                    ParentId = ParentId,
                    LearnerId = learnerId,
                    ParticipatingActivityGroupId = participatingActivityGroupId
                });

            consentResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess("Permissions successfully retracted");
            });

            NavigationManager.NavigateTo($"/events/{SchoolEvent.EventId}", true);
        }

        /// <summary>
        /// Handles the initial loading of school event permission data after the component is first rendered.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the component's first render.</param>
        /// <returns>A task that completes once the data has been loaded.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                var authState = await AuthenticationStateTask;
                _user = authState.User;
                _userId = _user.GetUserId();

                // Build request args for fetching permissions
                var schoolEventPermissionsRequestArgs = new SchoolEventPermissionsRequestArgs
                {
                    EventId = SchoolEvent.EventId!,
                    ParentId = ParentId,
                    ParentEmail = _user.GetUserEmail()
                };

                var result = await SchoolEventPermissionService.SchoolEventPermissionsListAsync(schoolEventPermissionsRequestArgs);

                result.ProcessResponseForDisplay(SnackBar, () =>
                {
                    _schoolEventPermissions = result.Data;
                });

                _loaded = true;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Navigates to the teams page for the event.
        /// </summary>
        public void NavigateToTeams()
        {
            NavigationManager.NavigateTo($"/event/{SchoolEvent.EventId}/teams");
        }
    }
}
