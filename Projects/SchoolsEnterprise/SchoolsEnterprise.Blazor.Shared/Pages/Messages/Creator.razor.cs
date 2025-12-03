using Azure.Core;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FilingModule.Application.ViewModels;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.DataTransferObjects;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using MessagingModule.Application.ViewModels;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Infrastructure.Implementation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using NeuralTech.Base.Enums;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Messages
{
    /// <summary>
    /// The <c>Creator</c> component allows users to create and send messages or notifications 
    /// to various entities (e.g., a learner, parent, teacher, event, activity group, etc.).
    /// This class integrates with multiple injected services to fetch context data 
    /// and dispatch notifications or push messages.
    /// </summary>
    public partial class Creator
    {
        const string MlAuto = "ml-auto";
        private List<BreadcrumbItem> _items = null!;
        private MessageViewModel _notificationMessage = new() { MessageId = Guid.NewGuid().ToString() };
        private string _userId = null!;

        #region CascadingParameters & Injected Services

        /// <summary>
        /// Cascades the current user's authentication state.
        /// This is used to retrieve claims-based information such as UserID or roles.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display messages to the user interface.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework. Assigning
        /// a custom implementation allows for tailored message presentation or localization.</remarks>
        [Inject] public IMessageService MessageService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user service used to manage user-related operations within the component.
        /// </summary>
        [Inject] public IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send push notifications.
        /// </summary>
        [Inject] public IPushNotificationService PushNotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and query user roles within the application.
        /// </summary>
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
        [Inject] public ITeacherService TeacherService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and retrieve age group information.
        /// </summary>
        [Inject] public IAgeGroupService AgeGroupService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send notifications for school events.
        /// </summary>
        [Inject] public ISchoolEventNotificationService SchoolEventNotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send notifications related to activity groups.
        /// </summary>
        [Inject] public IActivityGroupNotificationService ActivityGroupNotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts in the user interface. Ensure that the service is properly configured and injected before
        /// use.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application's configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated by dependency injection and provides access to
        /// key-value pairs and configuration sections used throughout the application.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation and URI management in
        /// the application.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework and should
        /// not be manually set in most cases.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The string version of a <see cref="NeuralTech.Base.Enums.MessageType"/>, 
        /// indicating the type or domain context of the message being created 
        /// (e.g., "Learner", "Parent", "Teacher", "Global", etc.).
        /// </summary>
        [Parameter] public string MessageType { get; set; } = null!;

        /// <summary>
        /// Represents the ID of the entity for whom the message is intended, or the context entity.
        /// This could be a user ID, a group ID, an event ID, etc.
        /// </summary>
        [Parameter] public string? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the notification recipient address or identifier.
        /// </summary>
        [Parameter] public string? SendNotification { get; set; } 

        /// <summary>
        /// Gets or sets the public message to be displayed.
        /// </summary>
        [Parameter] public string? PublicMessage { get; set; }

        #endregion

        #region Create & Link Handling

        /// <summary>
        /// Opens a dialog for creating or selecting document links (e.g., adding attachments from a file store).
        /// If a user adds or selects documents, they are appended to the current message's 
        /// Documents or DocumentLinks collections.
        /// </summary>
        public async Task CreateNotificationLinkAsync()
        {
            var parameters = new DialogParameters<CreateDocumentLinkUrlModal>
            {
                { x => x.EntityId, EntityId }
            };

            var dialog = await DialogService.ShowAsync<CreateDocumentLinkUrlModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var model = (CreateDocumentLinkUrlViewModel)result.Data!;
                if (model.Documents.Any())
                {
                    foreach (var item in model.Documents)
                    {
                        _notificationMessage.Documents.Add(item);
                    }
                }
                else if (!string.IsNullOrEmpty(model.Url))
                {
                    _notificationMessage.DocumentLinks.Add(model.Url);
                }
            }

            StateHasChanged();
        }

        /// <summary>
        /// Determines which endpoint to call based on the notification's <see cref="MessageType"/> 
        /// and retrieves a corresponding list of <see cref="UserInfoDto"/> objects that should 
        /// receive this message/notification. If an endpoint call fails, returns an empty list.
        /// </summary>
        private async Task<List<RecipientDto>> ProcessNotificationUserList()
        {
            switch (_notificationMessage.MessageType)
            {
                case NeuralTech.Base.Enums.MessageType.Global:
                    {
                        var userListResult = await UserService.GlobalNotificationsUserList();
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                case NeuralTech.Base.Enums.MessageType.RoleMessage:
                {
                    var userListResult = await RoleService.RoleNotificationsUserList(_notificationMessage.EntityId);
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                }

                case NeuralTech.Base.Enums.MessageType.BlogPost:
                    {
                        var userListResult = await UserService.GlobalNotificationsUserList();
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

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

                case NeuralTech.Base.Enums.MessageType.Parent:
                    {
                        var userListResult = await ParentService.ParentsNotificationList(new ParentPageParameters
                            {
                                ParentId = _notificationMessage.EntityId
                            }
                        );
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                case NeuralTech.Base.Enums.MessageType.Teacher:
                    {
                        var userListResult = await TeacherService.TeachersNotificationList(new TeacherPageParameters
                            {
                                TeacherId = _notificationMessage.EntityId
                            }
                        );
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

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

                case NeuralTech.Base.Enums.MessageType.Event:
                    {
                        var userListResult = await SchoolEventNotificationService.EventNotificationList(_notificationMessage.EntityId);
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                case NeuralTech.Base.Enums.MessageType.ActivityGroup:
                    {
                        var userListResult = await ActivityGroupNotificationService.ActivityGroupCategoryNotificationList(_notificationMessage.EntityId);
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                case NeuralTech.Base.Enums.MessageType.ActivityCategory:
                {
                    var userListResult = await ActivityGroupNotificationService.ActivityGroupCategoryNotificationList(_notificationMessage.EntityId);
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                }

                case NeuralTech.Base.Enums.MessageType.ParticipatingActivityGroup:
                    {
                        var userListResult = await ActivityGroupNotificationService.ParticipatingActivityGroupNotificationList(_notificationMessage.EntityId);
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }

                default:
                    {
                        var userListResult = await UserService.GlobalNotificationsUserList();
                        return !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    }
            }
        }

        /// <summary>
        /// Creates the message (by calling <see cref="CreateAsync"/>) and then sends 
        /// a push notification for it, if applicable. 
        /// Typically used to finalize message creation and dispatch to the relevant user(s).
        /// </summary>
        public async Task CreateNotificationAndPushAsync()
        {
            try
            {
                // First, create the message on the server
                await CreateAsync();

                // If it's a global message, set the entity as the newly created message's ID
                if (_notificationMessage.MessageType == NeuralTech.Base.Enums.MessageType.Global)
                    _notificationMessage.EntityId = _notificationMessage.MessageId;

                if(_notificationMessage.SendNotification == false)
                {
                    SnackBar.Add("PushNotification was not sent as the SendNotification flag is set to false.", Severity.Warning);
                    NavigationManager.NavigateTo($"/messages/bytype/{(int)_notificationMessage.MessageType}/{EntityId}");
                    return;
                }
                
                var userInfos = await ProcessNotificationUserList();
                var notificationUrl = $"/messages/bytype/{(int)_notificationMessage.MessageType}/{_notificationMessage.MessageId}";
                var result = await PushNotificationService.EnqueueNotificationsAsync(userInfos, _notificationMessage.ToNotificationDto(notificationUrl));
                result.ProcessResponseForDisplay(SnackBar, async () =>
                {
                    var notificationBody = string.IsNullOrWhiteSpace(_notificationMessage.Message)
                        ? (_notificationMessage.ShortDescription ?? _notificationMessage.Subject ?? string.Empty)
                        : _notificationMessage.Message;

                    SnackBar.AddSuccess("PushNotification was sent successfully");
                    NavigationManager.NavigateTo($"/messages/bytype/{(int)_notificationMessage.MessageType}/{EntityId}");
                });

                // NOTE: Additional to-do logic for activityGroup or team-based notifications could be here
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        /// <summary>
        /// Creates a new notification message asynchronously and adds it to the message service.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MessageDto representing the
        /// newly created message.</returns>
        public async Task<MessageDto> CreateAsync()
        {
            var messageDto = _notificationMessage.ToDto();
            var notificationResult = await MessageService.AddMessageAsync(messageDto);

            notificationResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess(string.IsNullOrEmpty(notificationResult.Messages.FirstOrDefault()) ? "Message saved successfully" : notificationResult.Messages.FirstOrDefault());

                // If it's a global message, return to the relevant message listing
                if (_notificationMessage.MessageType == NeuralTech.Base.Enums.MessageType.Global)
                    NavigationManager.NavigateTo($"/messages/bytype/{(int)_notificationMessage.MessageType}/{EntityId}");
            });

            return messageDto;
        }

        /// <summary>
        /// Cancels the creation of a new message by navigating back to the relevant listing,
        /// which depends on <c>MessageType</c> and <c>EntityId</c>.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo($"/messages/bytype/{(int)_notificationMessage.MessageType}/{EntityId}");
        }

        #endregion

        #region Document Link Removal

        /// <summary>
        /// Removes a specific link from the <c>_notificationMessage</c>. 
        /// Presents a confirmation modal to ensure the user is sure before removing.
        /// </summary>
        /// <param name="link">
        /// The string representing the link to remove from the <c>DocumentLinks</c> collection.
        /// </param>
        private async Task RemoveDocumentLink(string link)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this learner from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var linkToRemove = _notificationMessage.DocumentLinks.FirstOrDefault(c => c == link);
                _notificationMessage.DocumentLinks.Remove(linkToRemove);
            }
        }

        /// <summary>
        /// Removes a specific link from the <c>_notificationMessage</c>. 
        /// Presents a confirmation modal to ensure the user is sure before removing.
        /// </summary>
        /// <param name="link">
        /// The string representing the link to remove from the <c>DocumentLinks</c> collection.
        /// </param>
        private async Task RemoveDocumentLink(DocumentDto link)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this learner from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var linkToRemove = _notificationMessage.Documents.FirstOrDefault(c => c.Url == link.Url);
                _notificationMessage.Documents.Remove(linkToRemove);
            }
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Blazor lifecycle event invoked on initialization. Determines the 
        /// currently logged-in user's ID, sets the message type (from parameters),
        /// and configures the entity context for the new message based on 
        /// <see cref="EntityId"/> and <see cref="MessageType"/>.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Retrieve the user's ID
                var authState = await AuthenticationStateTask;
                _userId = authState.User.GetUserId();

                // Convert the parameter string to an enum so we know which context we are dealing with
                _notificationMessage.MessageType = Enum.Parse<MessageType>(MessageType);

                if(!string.IsNullOrEmpty(PublicMessage))
                    _notificationMessage.Public = Convert.ToBoolean(PublicMessage);

                if (!string.IsNullOrEmpty(SendNotification))
                    _notificationMessage.SendNotification = Convert.ToBoolean(SendNotification);

                // Assign the entity ID in our local message
                _notificationMessage.EntityId = EntityId;

                await base.OnInitializedAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion
    }
}
