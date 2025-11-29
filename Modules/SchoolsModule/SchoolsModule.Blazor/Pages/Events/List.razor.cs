using ConectOne.Blazor.Extensions;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NeuralTech.Base.Enums;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;

namespace SchoolsModule.Blazor.Pages.Events
{
    /// <summary>
    /// The List component is responsible for displaying and managing events.
    /// </summary>
    public partial class List
    {
        #region Injections

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
        /// Gets or sets the service used to send notifications about school events.
        /// </summary>
        [Inject] public ISchoolEventNotificationService SchoolEventNotificationService { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Resends the attendance request notification for an activity group.
        /// </summary>
        /// <param name="args">The arguments for resending the permissions notification.</param>
        public async Task ResendActivityGroupAttendanceRequest(ResendPermissionsNotificationArgs args)
        {
            args.ConsentType = ConsentTypes.Attendance;

            var pushNotification = new NotificationDto()
            {
                EntityId = args.EventId,
                Title = "Some permissions are required.",
                ShortDescription = "Some permissions requests are required. please check event details. ",
                Message = "Some permissions requests are required. please check event details. ",
                MessageType = MessageType.Parent,
                Created = DateTime.Now,
                NotificationUrl = $"/events/{args.EventId}"
            };

            // Retrieve the list of parents/guardians who need to approve or deny permissions
            var eventPermissionNotificationListResult = await SchoolEventNotificationService
                .EventPermissionNotificationList(args.ConsentType!.Value, args.EventId, args.ParticipatingActivityGroupId);

            eventPermissionNotificationListResult.ProcessResponseForDisplay(SnackBar, () => { SnackBar.AddSuccess("Attendance consent notification successfully sent"); });
        }

        /// <summary>
        /// Resends the transport request notification for an activity group.
        /// </summary>
        /// <param name="args">The arguments for resending the permissions notification.</param>
        public async Task ResendActivityGroupTransportRequest(ResendPermissionsNotificationArgs args)
        {
            args.ConsentType = ConsentTypes.Transport;
            var pushNotification = new NotificationDto()
            {
                EntityId = args.EventId,
                Title = "Some permissions are required.",
                ShortDescription = "Some permissions requests are required. please check event details. ",
                Message = "Some permissions requests are required. please check event details. ",
                MessageType = MessageType.Parent,
                Created = DateTime.Now,
                NotificationUrl = $"/events/{args.EventId}"
            };

            // Retrieve the list of parents/guardians who need to approve or deny permissions
            var eventPermissionNotificationListResult = await SchoolEventNotificationService
                .EventPermissionNotificationList(args.ConsentType!.Value, args.EventId, args.ParticipatingActivityGroupId);
            eventPermissionNotificationListResult.ProcessResponseForDisplay(SnackBar, () => { SnackBar.AddSuccess("Transport consent notification successfully sent"); });
        }

        #endregion
    }
}

