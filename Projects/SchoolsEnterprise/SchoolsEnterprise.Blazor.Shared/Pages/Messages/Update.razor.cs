using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FilingModule.Application.ViewModels;
using FilingModule.Blazor.Modals;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using MessagingModule.Application.ViewModels;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.Interfaces.Parents;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Messages
{
    /// <summary>
    /// The Update component handles editing and sending notifications/messages. 
    /// It interacts with a messaging provider to fetch or update the current message, 
    /// manages document link creation, and triggers push notifications using the NotificationStateManager.
    /// </summary>
    public partial class Update
    {
        private List<BreadcrumbItem> _items = null!;
        private MessageViewModel _notificationMessage = new MessageViewModel();

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        [Inject] public IPushNotificationService PushNotificationService { get; set; } = null!;

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
        [Inject] public IParentQueryService ParentQueryService { get; set; } = null!;

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
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be manually set
        /// in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The ID of the message to update.
        /// </summary>
        [Parameter] public string MessageId { get; set; } = null!;
        
        /// <summary>
        /// Opens a dialog to create or attach a document link or file to the message. 
        /// The user can pick documents or specify a URL. 
        /// On success, updates _notificationMessage.Documents or DocumentLinks accordingly.
        /// </summary>
        public async Task CreateNotificationLinkAsync()
        {
            // Dialog parameters specify the entity for which to create a link.
            var parameters = new DialogParameters<CreateDocumentLinkUrlModal>
            {
                { x => x.EntityId, _notificationMessage.EntityId }
            };

            // Show the dialog to confirm or cancel adding a document link.
            var dialog = await DialogService.ShowAsync<CreateDocumentLinkUrlModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If not canceled, update the _notificationMessage with the new documents or URL.
            if (!result!.Canceled)
            {
                var model = (CreateDocumentLinkUrlViewModel)result.Data!;
                if (model.Documents.Any())
                {
                    // Add each selected document to the message's document collection.
                    foreach (var item in model.Documents)
                    {
                        _notificationMessage.Documents.Add(item);
                    }
                }
                else if (!string.IsNullOrEmpty(model.Url))
                {
                    // Add the entered URL to the message's links collection if provided.
                    _notificationMessage.DocumentLinks.Add(model.Url);
                }
            }

            // Force a UI refresh after changes.
            StateHasChanged();
        }

        /// <summary>
        /// Sends an update request to the backend for the current message 
        /// without sending the push notification. On success, navigates back to the message list.
        /// </summary>
        /// <returns>A <see cref="MessageDto"/> with updated message data.</returns>
        public async Task<MessageDto> UpdateAsync()
        {
            // Convert the current ViewModel to a DTO to send to the provider.
            var messageDto = _notificationMessage.ToDto();

            // Execute the update via the IMessagingProvider service.
            var notificationResult = await MessageService.UpdateMessageAsync(messageDto);

            // Process the response, showing success or error messages in a Snackbar 
            // and navigating to the message list on success.
            notificationResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess("Message saved successfully");
                NavigateToMessageList();
            });

            return messageDto;
        }

        /// <summary>
        /// Sends an update request to the backend for the current message
        /// </summary>
        /// <returns>A list of Recipients <see cref="RecipientDto"/></returns>
        private async Task<List<RecipientDto>> ProcessNotificationUserList()
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
                        var userListResult = await ParentQueryService.ParentsNotificationList(new ParentPageParameters
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
        /// Sends an update request to the backend for the current message 
        /// and then triggers sending the push notification if successful.
        /// </summary>
        /// <returns>A <see cref="MessageDto"/> with updated message data.</returns>
        public async Task<MessageDto> UpdateAndSendAsync()
        {
            // Convert the current ViewModel to a DTO for the provider.
            var messageDto = _notificationMessage.ToDto();

            // Update the message on the server.
            var notificationResult = await MessageService.UpdateMessageAsync(messageDto);

            // If the update succeeded, call the method that sends the push notification.
            notificationResult.ProcessResponseForDisplay(SnackBar, UpdateAndSendAction);

            return messageDto;
        }

        /// <summary>
        /// Action called after a successful update that sends the actual push notification 
        /// and shows appropriate success messages. Also navigates back to the message list.
        /// </summary>
        private async void UpdateAndSendAction()
        {
            // Notify the user that the message was saved.
            SnackBar.AddSuccess("Message saved successfully");

            // Immediately navigate away; the next steps (sending notification) happen asynchronously.
            NavigateToMessageList();

            var userInfos = await ProcessNotificationUserList();
            var result = await PushNotificationService.SendNotifications(userInfos, _notificationMessage.ToNotificationDto());

            // Display success or failure outcome in a Snackbar.
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                NavigateToMessageList();
                SnackBar.AddSuccess("PushNotification was sent successfully");
            });
        }

        /// <summary>
        /// Cancels the update and navigates back to the message list (without saving changes).
        /// </summary>
        public void Cancel()
        {
            NavigateToMessageList();
        }

        /// <summary>
        /// Navigates back to the message list view. 
        /// The list is filtered by the message type and entity ID.
        /// </summary>
        private void NavigateToMessageList()
        {
            NavigationManager.NavigateTo($"/messages/bytype/{(int)_notificationMessage.MessageType}/{_notificationMessage.EntityId}");
        }

        /// <summary>
        /// Opens a link (e.g., a document or external resource) in the same tab.
        /// </summary>
        /// <param name="item">The URL or link to open.</param>
        private void OpenMessageLink(string item)
        {
            NavigationManager.NavigateTo(item);
        }

        /// <summary>
        /// Removes a document link from the current message after confirming with the user.
        /// (The code to delete the link on the server is commented out but can be re-enabled.)
        /// </summary>
        /// <param name="link">The link to remove.</param>
        private async Task RemoveDocumentLink(string link)
        {
            // Setup confirmation dialog parameters for user confirmation.
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this learner from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            // Show the modal to confirm the removal of the link.
            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If the user clicks "Yes", remove the link from the local message model.
            if (!result!.Canceled)
            {
                var linkToRemove = _notificationMessage.DocumentLinks.FirstOrDefault(c => c == link);

                // Uncomment and adjust if needed for server-side link deletion:
                // var removalResponse = await MessageProvider.DeleteMessageLinkAsync(_notificationMessage.MessageId, linkToRemove);
                // if(!removalResponse.Succeeded) SnackBar.AddErrors(removalResponse.Messages);

                _notificationMessage.DocumentLinks.Remove(linkToRemove);
                // Optionally show success message:
                // SnackBar.AddSuccess("Document Link removed successfully");
            }
        }

        /// <summary>
        /// Removes a document link from the current notification message after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before removing the specified
        /// document link.  If the user confirms the action, the link is removed from the local collection of
        /// documents.</remarks>
        /// <param name="link">The URL of the document link to be removed.</param>
        /// <returns></returns>
        private async Task RemoveDocument(string link)
        {
            // Setup confirmation dialog parameters for user confirmation.
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this learner from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            // Show the modal to confirm the removal of the link.
            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            // If the user clicks "Yes", remove the link from the local message model.
            if (!result!.Canceled)
            {
                var linkToRemove = _notificationMessage.Documents.FirstOrDefault(c => c.Url == link);

                // Uncomment and adjust if needed for server-side link deletion:
                // var removalResponse = await MessageProvider.DeleteMessageLinkAsync(_notificationMessage.MessageId, linkToRemove);
                // if(!removalResponse.Succeeded) SnackBar.AddErrors(removalResponse.Messages);

                _notificationMessage.Documents.Remove(linkToRemove);
                // Optionally show success message:
                // SnackBar.AddSuccess("Document Link removed successfully");
            }
        }

        /// <summary>
        /// Lifecycle method called when the component initializes. 
        /// Fetches the existing message details by EventId and sets up the ViewModel.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Fetch the message from the server using the EventId parameter.
            var messageResult = await MessageService.GetMessageAsync(MessageId);

            // Process the result to display any errors or populate _notificationMessage on success.
            messageResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                _notificationMessage = new MessageViewModel(messageResult.Data);
            });

            await base.OnInitializedAsync();
        }
    }
}
