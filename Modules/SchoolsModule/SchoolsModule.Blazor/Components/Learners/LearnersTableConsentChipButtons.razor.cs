using ConectOne.Blazor.Extensions;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NeuralTech.Base.Enums;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Components.Learners
{
    /// <summary>
    /// Displays and manages consent/permission controls for a learner in relation to a specific event. 
    /// This includes removing existing consents or resending attendance/transport requests (permissions) to parents.
    /// It retrieves relevant event permissions from an <see cref="ISchoolEventProvider"/>
    /// and raises event callbacks for the parent component to handle (e.g., removing consent, resending requests).
    /// </summary>
    public partial class LearnersTableConsentChipButtons
    {
        private bool _loaded;
        private SchoolEventPermissionsDto _schoolEventPermissions = null!;

        /// <summary>
        /// Injected provider to fetch and update school event permissions and other event data.
        /// </summary>
        [Inject] public ISchoolEventPermissionService SchoolEventPermissionService { get; set; } = null!;

        /// <summary>
        /// Injected dialog service for displaying dialogs.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Injected snack bar service for displaying notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Injected navigation manager for navigating between pages.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The current event context in which the learner’s consents are managed.
        /// </summary>
        [Parameter, EditorRequired] public EventViewModel Event { get; set; } = null!;

        /// <summary>
        /// The learner for whom we're displaying or modifying consents (attendance, transport, etc.).
        /// </summary>
        [Parameter, EditorRequired] public LearnerDto Learner { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier for the activity group.
        /// </summary>
        [Parameter] public string ActivityGroupId { get; set; }

        /// <summary>
        /// An event callback for removing a learner’s previously given consent.
        /// </summary>
        [Parameter] public EventCallback<RemoveConsentArgs> RemoveConsent { get; set; }

        /// <summary>
        /// An event callback for re-sending attendance consent requests (e.g., to the parent's device).
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendAttendanceRequest { get; set; }

        /// <summary>
        /// An event callback for re-sending transport consent requests.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendTransportRequest { get; set; }
        
        /// <summary>
        /// Called when removing a learner’s consent for a particular aspect (e.g., attendance, transport).
        /// Triggers an event callback that the parent can handle (e.g., to remove the entry in the backend).
        /// </summary>
        public async Task OnRemoveConsent(string learnerId, string eventId, ConsentTypes consentType)
        {
            var removalModel = new RemoveConsentArgs(learnerId, eventId, consentType);
            await RemoveConsent.InvokeAsync(removalModel);
        }

        /// <summary>
        /// Re-sends an attendance request to confirm a learner’s participation in an event.
        /// </summary>
        public async Task OnResendAttendanceRequest(string learnerId, string learnerName, string eventId, string eventName, DateTime eventDate)
        {
            var notificationsModal = new ResendPermissionsNotificationArgs
            {
                LearnerId = learnerId,
                LearnerName = learnerName,
                EventName = eventName,
                EventId = eventId,
                EventDate = eventDate,
                EventUrl = $"/events/{eventId}",
                Message = new MessageDto
                {
                    MessageId = Guid.NewGuid().ToString(),
                    MessageType = MessageType.Parent,
                    Subject = $"Please confirm attendance of {learnerName}",
                    Message = $"Please confirm whether {learnerName} will be attending {eventName} on {eventDate.ToLongDateString()}"
                }
            };

            await ResendAttendanceRequest.InvokeAsync(notificationsModal);
        }

        /// <summary>
        /// Re-sends a transport request to confirm if the learner will use provided transport for the event.
        /// </summary>
        public async Task OnResendTransportRequest(string learnerId, string learnerName, string eventId, string eventName, DateTime eventDate)
        {
            var notificationsModal = new ResendPermissionsNotificationArgs
            {
                LearnerId = learnerId,
                LearnerName = learnerName,
                EventName = eventName,
                EventId = eventId,
                EventDate = eventDate,
                EventUrl = $"/events/{eventId}",
                Message = new MessageDto
                {
                    MessageId = Guid.NewGuid().ToString(),
                    MessageType = MessageType.Parent,
                    Subject = $"Please confirm transport permission for {learnerName}",
                    Message = $"Please confirm whether {learnerName} will be using the provided transport for {eventName} on {eventDate.ToLongDateString()}"
                }
            };

            await ResendTransportRequest.InvokeAsync(notificationsModal);
        }

        /// <summary>
        /// Lifecycle method that initializes the component. 
        /// Fetches the specific learner’s permission data for the event 
        /// from the <see cref="SchoolEventProvider"/> and sets _loaded to true.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var schoolEventPermissionsRequestArgs = new SchoolEventPermissionsRequestArgs
            {
                EventId = Event.EventId,
                LearnerId = Learner.LearnerId,
                ActivityGroupId = ActivityGroupId
            };

            var schoolEventPermissionsResult = await SchoolEventPermissionService.SchoolEventPermissionsAsync(schoolEventPermissionsRequestArgs);
            schoolEventPermissionsResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _schoolEventPermissions = schoolEventPermissionsResult.Data;
            });

            _loaded = true;
            await base.OnInitializedAsync();
        }
    }
}
