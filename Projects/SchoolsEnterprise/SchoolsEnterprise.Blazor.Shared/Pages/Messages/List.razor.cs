using ConectOne.Blazor.Extensions;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Messages
{
    /// <summary>
    /// The List component displays a table of messages (MessageDto) for a given user or entity.
    /// It includes the ability to re-send push notifications for specific messages.
    /// 
    /// Dependencies:
    /// - IMessagingProvider to retrieve messages from an API.
    /// - INotificationsProvider for notification logic (unused in this snippet, but presumably for additional features).
    /// - IPushNotificationsProvider to re-send message notifications as push notifications.
    /// </summary>
    public partial class List
    {
        private IEnumerable<MessageDto> _messages = null!;
        private MudTable<MessageDto> _table = null!;
        private int _totalItems;
        private int _currentPage;
        private bool _dense;
        private bool _striped = true;
        private bool _bordered;
        private bool _loaded;

        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display messages to the user interface.
        /// </summary>
        /// <remarks>This property is typically injected by the framework to provide message display
        /// functionality, such as notifications or alerts. Assigning a custom implementation allows for tailored
        /// message handling.</remarks>
        [Inject] public IMessageService MessageService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user service used to manage user-related operations within the component.
        /// </summary>
        [Inject] public IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and query user roles within the application.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Ensure
        /// that the assigned service implements the required role management operations for your application's
        /// authorization logic.</remarks>
        [Inject] public IRoleService RoleService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send notifications to learners.
        /// </summary>
        [Inject] public ILearnerNotificationService LearnerNotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query parent entities within the application.
        /// </summary>
        [Inject] public IParentService ParentService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage teacher-related operations.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Assign an
        /// implementation of <see cref="ITeacherService"/> to enable teacher management functionality within the
        /// component.</remarks>
        [Inject] public ITeacherService TeacherService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and retrieve age group information.
        /// </summary>
        [Inject] public IAgeGroupService AgeGroupService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send notifications about school events.
        /// </summary>
        [Inject] public ISchoolEventNotificationService SchoolEventNotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send notifications related to activity groups.
        /// </summary>
        [Inject] public IActivityGroupNotificationService ActivityGroupNotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send push notifications to clients.
        /// </summary>
        [Inject] public IPushNotificationService PushNotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// The EntityId could correspond to a related domain entity that the messages might be linked to.
        /// </summary>
        [Parameter] public string EntityId { get; set; } = null!;

        /// <summary>
        /// The user ID associated with these messages (or identified at runtime from the current user).
        /// </summary>
        [Parameter] public string UserId { get; set; } = null!;

        /// <summary>
        /// An integer representing the type of message (e.g., Global, BlogPost, etc.). Optional.
        /// </summary>
        [Parameter] public int? MessageType { get; set; }        

        /// <summary>
        /// Lifecycle method: Retrieves the current user's ID from the authentication state, 
        /// marks the component as loaded, then calls the base OnInitializedAsync logic.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            // Retrieve the current user's ID from the claims principal
            UserId = authState.User.GetUserId();

            // Mark the component as loaded so the UI can be displayed
            _loaded = true;

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Determines which endpoint to call based on the notification's <see cref="MessageType"/> 
        /// and retrieves a corresponding list of <see cref="UserInfoDto"/> objects that should 
        /// receive this message/notification. If an endpoint call fails, returns an empty list.
        /// </summary>
        private async Task<List<RecipientDto>> ProcessNotificationUserList(NotificationDto _notificationMessage)
        {
            switch (_notificationMessage.MessageType)
            {
                // Case 1: GLOBAL or BLOGPOST
                // Retrieves all users from the "account/users" endpoint if the message type is 
                // global or a blog post, as these presumably go out to everyone.
                case NeuralTech.Base.Enums.MessageType.Global:
                    {
                        var userListResult = await UserService.GlobalNotificationsUserList();
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }
                case NeuralTech.Base.Enums.MessageType.BlogPost:
                    {
                        var userListResult = await UserService.GlobalNotificationsUserList();
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                // Case 2: LEARNER
                // Retrieves a list of user accounts associated with learners, 
                // filtered by optional gender and specific LearnerId (EntityId).
                case NeuralTech.Base.Enums.MessageType.Learner:
                    {
                        var userListResult = await LearnerNotificationService.LearnersNotificationList(new LearnerPageParameters
                            {
                                Gender = _notificationMessage.Gender,
                                LearnerId = _notificationMessage.EntityId
                            }
                        );
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                // Case 3: PARENT
                // Looks up users identified as parents, optionally filtered by a specific ParentId (EntityId).
                case NeuralTech.Base.Enums.MessageType.Parent:
                    {
                        var userListResult = await ParentService.ParentsNotificationList(new ParentPageParameters
                            {
                                ParentId = _notificationMessage.EntityId
                            }
                        );
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                // Case 4: TEACHER
                // Looks up users identified as teachers, optionally filtered by a specific TeacherId (EntityId).
                case NeuralTech.Base.Enums.MessageType.Teacher:
                    {
                        var userListResult = await TeacherService.TeachersNotificationList(new TeacherPageParameters
                            {
                                TeacherId = _notificationMessage.EntityId
                            }
                        );
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                // Case 5: AGEGROUP
                // First retrieves the age group details (MinAge, MaxAge), then queries 
                // "learners/notificationList" to get all learners within that age range 
                // (plus any gender filter).
                case NeuralTech.Base.Enums.MessageType.AgeGroup:
                    {
                        var ageGroupResult = await AgeGroupService.AgeGroupAsync(_notificationMessage.EntityId);
                        if (!ageGroupResult.Succeeded)
                            return new List<RecipientDto>();

                        var userListResult = await LearnerNotificationService.LearnersNotificationList(new LearnerPageParameters
                            {
                                Gender = _notificationMessage.Gender,
                                MinAge = ageGroupResult.Data.MinAge,
                                MaxAge = ageGroupResult.Data.MaxAge
                            }
                        );
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                // Case 6: GRADE
                // Filters learners by the specified GradeId (EntityId).
                case NeuralTech.Base.Enums.MessageType.Grade:
                    {
                        var userListResult = await LearnerNotificationService.LearnersNotificationList(new LearnerPageParameters
                            {
                                Gender = _notificationMessage.Gender,
                                GradeId = _notificationMessage.EntityId
                            }
                        );
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                // Case 7: SCHOOLCLASS
                // Filters learners by the specified SchoolClassId (EntityId).
                case NeuralTech.Base.Enums.MessageType.SchoolClass:
                    {
                        var userListResult = await LearnerNotificationService.LearnersNotificationList(new LearnerPageParameters
                            {
                                Gender = _notificationMessage.Gender,
                                SchoolClassId = _notificationMessage.EntityId
                            }
                        );
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                // Case 8: EVENT
                // Fetches a list of user accounts to notify about an event creation. 
                // Possibly filtered by the event's ID or specifics.
                case NeuralTech.Base.Enums.MessageType.Event:
                    {
                        var userListResult = await SchoolEventNotificationService.EventNotificationList(_notificationMessage.EntityId);
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                // Case 9: ACTIVITYGROUP
                // Retrieves users from an activity group, identified by the given EntityId.
                case NeuralTech.Base.Enums.MessageType.ActivityGroup:
                    {
                        var userListResult = await ActivityGroupNotificationService.ActivityGroupCategoryNotificationList(_notificationMessage.EntityId);
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                // Case 10: ACTIVITYCATEGORY
                // Retrieves users from an activity category.
                case NeuralTech.Base.Enums.MessageType.ActivityCategory:
                    {
                        var userListResult = await ActivityGroupNotificationService.ActivityGroupCategoryNotificationList(_notificationMessage.EntityId);
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                // Case 11: PARTICIPATINGACTIVITYGROUP
                // Retrieves users who are actively participating in an activity group, 
                // which might differ from a standard activity group list (e.g., only those currently joined).
                case NeuralTech.Base.Enums.MessageType.ParticipatingActivityGroup:
                    {
                        var userListResult = await ActivityGroupNotificationService.ParticipatingActivityGroupNotificationList(_notificationMessage.EntityId);
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                case NeuralTech.Base.Enums.MessageType.RoleMessage:
                {
                    var userListResult = await RoleService.RoleNotificationsUserList(_notificationMessage.EntityId);
                    return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                }

                // DEFAULT:
                // If none of the above match, fallback to retrieving all users from "account/users".
                default:
                {
                    var userListResult = await UserService.GlobalNotificationsUserList();
                    return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                }
            }
        }

        /// <summary>
        /// Sends a push notification associated with a specific message by 
        /// constructing a NotificationDto and calling the PushNotificationsProvider.
        /// </summary>
        /// <param name="message">The source message, from which the notification details are extracted.</param>
        private async Task SendPushNotification(MessageDto message)
        {
            // Construct the notification from the message details
            var notification = new NotificationDto()
            {
                MessageType = message.MessageType,
                EntityId = message.EntityId,

                ReceiverId = message.ReceiverId,
                ReceiverName = message.ReceiverName,
                ReceiverImageUrl = message.ReceiverImageUrl,

                Gender = message.Gender,

                Title = message.Subject,
                ShortDescription = message.ShortDescription,
                Message = message.Message,
                NotificationUrl = $"/messages/{message.MessageId}",

                Created = DateTime.Now
            };

            var userList = await ProcessNotificationUserList(notification);

            var result = await PushNotificationService.SendNotifications(userList, notification);
            if (result.Succeeded)
                SnackBar.AddSuccess("Notification was resent successfully");
            else
                SnackBar.AddErrors(result.Messages);
        }
    }
}
