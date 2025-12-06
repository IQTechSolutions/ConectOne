using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Application.ViewModels;
using FilingModule.Blazor.Modals;
using FilingModule.Domain.Constants;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.Interfaces;
using GroupingModule.Domain.RequestFeatures;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Enums;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using NeuralTech.Base.Enums;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Blazor.Components.ActivityCategories;
using SchoolsModule.Blazor.Components.ActivityGroups;
using SchoolsModule.Blazor.Modals;
using SchoolsModule.Blazor.StateManagers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.Events
{
    /// <summary>
    /// This component provides functionality for viewing and updating the details of an existing 
    /// school event, including setting dates/times, attaching documents/links, managing team 
    /// members within an activity group, and sending notifications/permissions.
    /// </summary>
    public partial class Update
    {
        private bool _overlayVisible = true;
        private DateRange _dateRange = new() { Start = DateTime.Now, End = DateTime.Now.AddMinutes(5) };
        private TimeSpan? _startTime = new TimeSpan(00, 00, 00);
        private TimeSpan? _endTime = new TimeSpan(00, 05, 00);
        private string _imageSource = "_content/SchoolsModule.Blazor/images/NoImage.jpg";
        private readonly List<BreadcrumbItem> _items =
        [
            new("Home", href: "#", icon: Icons.Material.Filled.Home),
            new("Events", href: "/events", icon: Icons.Material.Filled.Event),
            new("Update", href: null, disabled: true, icon: Icons.Material.Filled.Edit)
        ];        

        /// <summary>
        /// Provides the current authentication state, used to obtain the user's identity (e.g., for userId).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query activity group data.
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        [Inject] public ICategoryService<ActivityGroup> ActivityCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to execute commands related to activity groups.
        /// </summary>
        [Inject] public IActivityGroupCommandService ActivityGroupCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query school event data.
        /// </summary>
        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        [Inject] public ISchoolEventNotificationService SchoolEventNotificationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to send push notifications to clients.
        /// </summary>
        [Inject] public IPushNotificationService PushNotificationService { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the service responsible for executing commands related to school events.
        /// </summary>
        [Inject] public ISchoolEventCommandService SchoolEventCommandService { get; set; } = null!;

        /// <summary>
        /// A state manager that holds and manipulates the current event data (SchoolEvent object).
        /// </summary>
        [Inject] public SchoolEventStateManager EventStateManager { get; set; } = null!;

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
        /// The ID of the event being updated, supplied via query string or route parameter.
        /// </summary>
        [Parameter] public string EventId { get; set; } = null!;

        /// <summary>
        /// An optional parameter representing an activity category ID. 
        /// Could be used for filtering or relating to the event.
        /// </summary>
        [Parameter] public string ActivityCategoryId { get; set; } = null!;
        
        /// <summary>
        /// Called when the cover image is changed, updating the local field and the event's cover image URL.
        /// </summary>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Resends a request for attendance consent for a particular team member. 
        /// Useful if the user needs to re-notify a parent or learner about event attendance.
        /// </summary>
        public async Task ResendAttendanceRequest(ResendPermissionsNotificationArgs args)
        {
            args.ConsentType = ConsentTypes.Attendance;
            var pushNotification = new NotificationDto()
            {
                EntityId = args.EventId,
                Title = args.Message!.Subject,
                ShortDescription = args.Message.ShortDescription,
                Message = args.Message.Message,
                MessageType = MessageType.Parent,
                Created = DateTime.Now,
                NotificationUrl = $"/events/{args.EventId}"
            };
            var eventPermissionNotificationListResult = await SchoolEventNotificationService.EventPermissionNotificationList(args.ConsentType!.Value, args.EventId, args.ParticipatingActivityGroupId, args.LearnerId);


            eventPermissionNotificationListResult.ProcessResponseForDisplay(SnackBar, () =>
            {
                SnackBar.AddSuccess(eventPermissionNotificationListResult.Messages.FirstOrDefault() ?? "Attendance consent notification successfully sent");
            });
        }

        /// <summary>
        /// Similar to <see cref="ResendAttendanceRequest"/>, but for transport consent requests.
        /// </summary>
        public async Task ResendTransportRequest(ResendPermissionsNotificationArgs args)
        {
            args.ConsentType = ConsentTypes.Transport;
            var pushNotification = new NotificationDto()
            {
                EntityId = args.EventId,
                Title = args.Message!.Subject,
                ShortDescription = args.Message.ShortDescription,
                Message = args.Message.Message,
                MessageType = MessageType.Parent,
                Created = DateTime.Now,
                NotificationUrl = $"/events/{args.EventId}"
            };
            var eventPermissionNotificationListResult = await SchoolEventNotificationService.EventPermissionNotificationList(args.ConsentType!.Value, args.EventId, args.ParticipatingActivityGroupId, args.LearnerId);

            eventPermissionNotificationListResult.ProcessResponseForDisplay(SnackBar,
                () => SnackBar.AddSuccess("Attendance consent notification successfully sent"));
        }

        /// <summary>
        /// Sends an attendance consent request for an entire activity group, 
        /// rather than individual learners.
        /// </summary>
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

            eventPermissionNotificationListResult.ProcessResponseForDisplay(SnackBar,
                () => SnackBar.AddSuccess("Attendance consent notification successfully sent"));
        }

        /// <summary>
        /// Sends a transport consent request for an entire activity group.
        /// </summary>
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
            eventPermissionNotificationListResult.ProcessResponseForDisplay(SnackBar,
                () => SnackBar.AddSuccess("Attendance consent notification successfully sent"));
        }

        /// <summary>
        /// Removes the consent previously provided by a user (e.g., a parent or learner).
        /// </summary>
        public async Task RemoveConsent(RemoveConsentArgs args)
        {
            var result = await ActivityGroupCommandService.RemoveConsent(args);
            result.ProcessResponseForDisplay(SnackBar,
                () => SnackBar.AddSuccess("Consent was successfully revoked"));
        }

        /// <summary>
        /// Updates the event's date range (start & end dates) based on the <see cref="_dateRange"/> UI component values.
        /// </summary>
        private void SetDateRange()
        {
            var startDate = _dateRange.Start!.Value;
            var oldStartDate = EventStateManager.SchoolEvent.StartDate;

            var endDate = _dateRange.End!.Value;
            var oldEndDate = EventStateManager.SchoolEvent.EndDate;

            // Keep the original hours/minutes/seconds, but update the day/month/year.
            EventStateManager.SchoolEvent.StartDate =
                new DateTime(startDate.Year, startDate.Month, startDate.Day, oldStartDate.Hour, oldStartDate.Minute, oldStartDate.Second);

            EventStateManager.SchoolEvent.EndDate =
                new DateTime(endDate.Year, endDate.Month, endDate.Day, oldEndDate.Hour, oldEndDate.Minute, oldEndDate.Second);
        }

        /// <summary>
        /// Updates the event's start time based on <see cref="_startTime"/>, keeping the same date.
        /// </summary>
        private void SetStartTime()
        {
            EventStateManager.SchoolEvent.StartDate = EventStateManager.SchoolEvent.StartDate.Date.Add(_startTime!.Value);
        }

        /// <summary>
        /// Updates the event's end time based on <see cref="_endTime"/>, keeping the same date.
        /// </summary>
        private void SetEndTime()
        {
            EventStateManager.SchoolEvent.EndDate = EventStateManager.SchoolEvent.EndDate.Date.Add(_endTime!.Value);
        }

        /// <summary>
        /// Updates the event details but does not publish (i.e., doesn't send notifications).
        /// </summary>
        public async Task UpdateAsync()
        {
            var result = await SchoolEventCommandService.UpdateAsync(EventStateManager.SchoolEvent.ToDto(), default);

            result.ProcessResponseForDisplay(SnackBar, async () =>
            {
                SnackBar.AddSuccess("School Event was successfully updated");
                EventStateManager.NotifyStateChanged();
                await InvokeAsync(StateHasChanged);
            });
        }

        /// <summary>
        /// Updates and publishes the event, triggering a permission/attendance notification
        /// if necessary (e.g., to parents).
        /// </summary>
        public async Task UpdateAndSendAsync()
        {
            // If the event is already published, no need to republish.
            if (EventStateManager.SchoolEvent.Published) return;

            EventStateManager.SchoolEvent.Published = true;
            var result = await SchoolEventCommandService.UpdateAsync(EventStateManager.SchoolEvent.ToDto(), default);

            // Upon success, use an inline method for handling notifications
            result.ProcessResponseForDisplay(SnackBar, SuccessAction);

            async void SuccessAction()
            {
                var notification = new NotificationDto
                {
                    EntityId = EventStateManager.SchoolEvent.EventId,
                    Title = $"Attendance consent required for {EventStateManager.SchoolEvent.Name}",
                    ShortDescription = $"Your child/children is taking part in the {EventStateManager.SchoolEvent.Name} on {EventStateManager.SchoolEvent.StartDate.ToShortDateString()}, attendance consent is required",
                    Message = $"Your child/children is taking part in the {EventStateManager.SchoolEvent.Name} on {EventStateManager.SchoolEvent.StartDate.ToShortDateString()}, please advise if your child/children will be attending this event.",
                    MessageType = MessageType.Parent,
                    Created = DateTime.Now,
                    NotificationUrl = $"/events/{EventStateManager.SchoolEvent.EventId}"
                };

                var eventPermissionNotificationListResult = await SchoolEventNotificationService.EventPermissionNotificationList(ConsentTypes.Attendance, notification.EntityId);

                if (eventPermissionNotificationListResult.Succeeded)
                {
                    SnackBar.AddSuccess("Event created notification successfully sent");
                }

                SnackBar.AddSuccess("School Event was successfully updated");
                StateHasChanged();
            }
        }

        /// <summary>
        /// Opens a dialog to create or attach a document link to the event. 
        /// If documents are added, they attach to <see cref="Permissions.Documents"/>.
        /// If a single URL is provided, it’s added to DocumentLinks.
        /// </summary>
        private async Task CreateLinkAsync()
        {
            var parameters = new DialogParameters<CreateDocumentLinkUrlModal>
            {
                { x => x.EntityId, EventStateManager.SchoolEvent.EventId }
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
                        EventStateManager.SchoolEvent.Documents.Add(item);
                    }
                }
                else if (!string.IsNullOrEmpty(model.Url))
                {
                    EventStateManager.SchoolEvent.DocumentLinks.Add(model.Url);
                }
            }

            StateHasChanged();
        }

        /// <summary>
        /// Opens the specified link in the current browser context or a new tab.
        /// </summary>
        private void OpenMessageLink(string item)
        {
            NavigationManager.NavigateTo(item);
        }

        /// <summary>
        /// Removes a document link from the event's DocumentLinks collection after confirming the action.
        /// </summary>
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
                var linkToRemove = EventStateManager.SchoolEvent.DocumentLinks.FirstOrDefault(c => c == link);
                EventStateManager.SchoolEvent.DocumentLinks.Remove(linkToRemove!);
            }
        }

        /// <summary>
        /// Sends a push notification regarding a <see cref="MessageDto"/>.
        /// </summary>
        private async Task SendPushNotification(MessageDto message)
        {
            var notification = new NotificationDto
            {
                MessageType = message.MessageType,
                EntityId = message.EntityId,

                ReceiverId = message.ReceiverId,
                ReceiverName = message.ReceiverName,
                ReceiverImageUrl = message.ReceiverImageUrl,

                Title = message.Subject,
                ShortDescription = message.ShortDescription,
                Message = message.Message,
                NotificationUrl = $"/messages/{message.MessageId}",
                Created = DateTime.Now
            };

            var userListResult = await SchoolEventNotificationService.EventNotificationList(message.EntityId);
            var result = await PushNotificationService.SendNotifications(userListResult.Data, notification);
            if (result.Succeeded)
                SnackBar.AddSuccess("Messages notification was resent successfully");
            else
                SnackBar.AddErrors(result.Messages);
        }

        /// <summary>
        /// Cancels out of the update process and redirects the user to the dashboard.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo("/dashboard");
        }

        /// <summary>
        /// Determines the color of the publish button, typically 
        /// a different color if already published vs. unpublished.
        /// </summary>
        public Color PublishButtonColor => EventStateManager.SchoolEvent!.Published ? Color.Tertiary : Color.Error;

        /// <summary>
        /// Reference to the activity groups table component.
        /// </summary>
        private ActivityGroupsTable _activityGroupsTable = null!;

        /// <summary>
        /// Reference to the activity groups table component.
        /// </summary>
        private ActivityCategoriesTable _activityCategoriesTable = null!;

        /// <summary>
        /// Indicates whether multiple selection is enabled for the category table.
        /// </summary>
        private bool _categoryTableMultiSelection = false;

        /// <summary>
        /// Parameters for the category table pagination and sorting.
        /// </summary>
        private readonly CategoryPageParameters _categoryTableArgs = new CategoryPageParameters();

        /// <summary>
        /// Parameters for the activity group table pagination and sorting.
        /// </summary>
        private readonly ActivityGroupPageParameters _activityGroupTableArgs = new ActivityGroupPageParameters();

        #region Ticket Types

        /// <summary>
        /// Displays a dialog for creating a new ticket type and updates the event's documents or document links based
        /// on the user's input.
        /// </summary>
        /// <remarks>This method opens a dialog where the user can specify documents or a URL to associate
        /// with the current event.  If the user provides documents, they are added to the event's document collection.
        /// If a URL is provided instead,  it is added to the event's document links. The method ensures that the event
        /// state is updated and the UI is refreshed.</remarks>
        /// <returns></returns>
        private async Task CreateTicketType()
        {
            var parameters = new DialogParameters<TicketTypeModal>
            {
                { x => x.EventId, EventStateManager.SchoolEvent!.EventId }
            };

            var dialog = await DialogService.ShowAsync<TicketTypeModal>("Create New Ticket Type", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var newTicketType = ((SchoolEventTicketTypeViewModel)result.Data!).ToDto();
                var creationResult = await SchoolEventCommandService.CreateTicketTypeAsync(newTicketType);
                creationResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    EventStateManager.SchoolEvent.TicketTypes.Add(newTicketType);
                    SnackBar.AddSuccess("Ticket type was successfully created");
                });
            }

            StateHasChanged();
        }

        /// <summary>
        /// Updates the specified ticket type for the current school event.
        /// </summary>
        /// <remarks>This method displays a modal dialog to allow the user to update the details of the
        /// specified ticket type. If the user confirms the changes, the ticket type is updated in the event's ticket
        /// type collection.</remarks>
        /// <param name="ticketType">The ticket type to update. This parameter must not be <see langword="null"/>.</param>
        /// <returns></returns>
        private async Task UpdateTicketType(SchoolEventTicketTypeDto ticketType)
        {
            var index = EventStateManager.SchoolEvent.TicketTypes.IndexOf(ticketType);

            var parameters = new DialogParameters<TicketTypeModal>
            {
                { x => x.EventId, EventStateManager.SchoolEvent!.EventId },
                { x => x.TicketType, new SchoolEventTicketTypeViewModel(ticketType) }
            };

            var dialog = await DialogService.ShowAsync<TicketTypeModal>("Update Ticket Type", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var updateResult = await SchoolEventCommandService.UpdateTicketTypeAsync(ticketType);
                updateResult.ProcessResponseForDisplay(SnackBar, () =>
                {
                    var model = ((SchoolEventTicketTypeViewModel)result.Data!).ToDto();
                    EventStateManager.SchoolEvent.TicketTypes[index] = model;
                    SnackBar.AddSuccess("Ticket type was successfully updated");
                });
            }
        }

        /// <summary>
        /// Removes a ticket type from the current application after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before removing the specified
        /// ticket type. The removal is only performed if the user confirms the action.</remarks>
        /// <param name="link">The identifier of the ticket type to be removed.</param>
        private async Task RemoveTicketType(string ticketTypeId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this ticket type from this event?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Remove Ticket Type", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var linkToRemove = EventStateManager.SchoolEvent!.TicketTypes.FirstOrDefault(c => c.Id == ticketTypeId);
                if (linkToRemove != null)
                {
                    var removalResult = await SchoolEventCommandService.DeleteTicketTypeAsync(linkToRemove.Id);
                    removalResult.ProcessResponseForDisplay(SnackBar, () =>
                    {
                        EventStateManager.SchoolEvent.TicketTypes.Remove(linkToRemove);
                        SnackBar.AddSuccess("Ticket type was successfully removed");
                    });
                }
            }
        }

        #endregion

        #region Category Table : Paging & Loading

        /// <summary>
        /// Handles the event when the category selection changes.
        /// </summary>
        /// <param name="categories">The selected categories.</param>
        private async Task OnCategorySelectionChanged(HashSet<CategoryDto> categories)
        {
            EventStateManager.SchoolEvent!.SelectedActivityCategories = categories;
            _activityGroupTableArgs.CategoryIds = string.Join(",", categories.Select(c => c.CategoryId));

            StateHasChanged();
        }

        /// <summary>
        /// Sets the parameters for the category table pagination and sorting.
        /// </summary>
        /// <param name="state">The state of the table.</param>
        private void SetCategoryPageParameters(TableState state)
        {
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                var sortDirection = state.SortDirection switch
                {
                    SortDirection.Ascending => "asc",
                    SortDirection.Descending => "desc",
                    _ => string.Empty
                };

                _categoryTableArgs.OrderBy = state.SortDirection != SortDirection.None ? $"{state.SortLabel} {sortDirection}" : string.Empty;
            }

            _categoryTableArgs.PageSize = state.PageSize;
            _categoryTableArgs.PageNr = state.Page + 1;
        }

        /// <summary>
        /// Reloads the category table data based on the table state.
        /// </summary>
        /// <param name="state">The state of the table.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The table data containing the categories.</returns>
        private async Task<TableData<CategoryDto>> CategoryTableReload(TableState state, CancellationToken token)
        {
            SetCategoryPageParameters(state);

            if (!EventStateManager.SchoolEvent!.SelectedActivityCategories.Any())
                _categoryTableMultiSelection = true;


            if (_categoryTableMultiSelection)
            {
                _categoryTableArgs.ParentId = EventStateManager.SchoolEvent?.EntityId;
                var pagingResponse = await ActivityCategoryService.PagedCategoriesAsync(_categoryTableArgs, token);

                if (!pagingResponse.Succeeded) SnackBar.AddErrors(pagingResponse.Messages);

                return new TableData<CategoryDto> { TotalItems = pagingResponse.Data.Count, Items = pagingResponse.Data };
            }

            var eventCategories = PaginatedResult<CategoryDto>.Success(EventStateManager.SchoolEvent!.SelectedActivityCategories.ToList(),
                EventStateManager.SchoolEvent!.SelectedActivityCategories.Count, _categoryTableArgs.PageNr, _categoryTableArgs.PageSize);
            return new TableData<CategoryDto> { TotalItems = eventCategories.Data.Count, Items = eventCategories.Data };
        }

        #endregion

        #region Activity Group Table : Paging & Loading

        /// <summary>
        /// Handles the event when the activity group team member selection changes.
        /// </summary>
        /// <param name="args">The event arguments containing the selected activity group and action.</param>
        private void OnActivityGroupTeamMemberSelectionChanged(ActivityGroupSelectionChangedEventArgs args)
        {
            if (args.Action == ActivityGroupSelectionAction.TeamMemberAdded)
            {
                EventStateManager.SchoolEvent!.SelectedActivityGroups.FirstOrDefault(c => c.ActivityGroupId == args.ActivityGroup?.ActivityGroupId).TeamMembers.Add(args.Learner);
            }
            else
            {
                EventStateManager.SchoolEvent!.SelectedActivityGroups.FirstOrDefault(c => c.ActivityGroupId == args.ActivityGroup?.ActivityGroupId)
                    .TeamMembers.Remove(EventStateManager.SchoolEvent!.SelectedActivityGroups
                        .FirstOrDefault(c => c.ActivityGroupId == args.ActivityGroup.ActivityGroupId).TeamMembers
                        .FirstOrDefault(c => c.LearnerId == args.Learner.LearnerId));
            }
            StateHasChanged();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Lifecycle method called when the component is initialized. Loads the event 
        /// via the <see cref="EventId"/> parameter, storing it in <see cref="SchoolEvent{TEntity}"/>.
        /// Initializes relevant time picks for the event (start/end).
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var setEventResult = await EventStateManager.SetSchoolEventAsync(EventId);
            _dateRange = new DateRange(EventStateManager.SchoolEvent!.StartDate, EventStateManager.SchoolEvent.EndDate);

            if (setEventResult.Succeeded)
            {
                _startTime = new TimeSpan(EventStateManager.SchoolEvent.StartDate.Hour, EventStateManager.SchoolEvent.StartDate.Minute, EventStateManager.SchoolEvent.StartDate.Second);
                _endTime = new TimeSpan(EventStateManager.SchoolEvent.EndDate.Hour, EventStateManager.SchoolEvent.EndDate.Minute, EventStateManager.SchoolEvent.EndDate.Second);
            }

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Lifecycle method called after each render. If this is the first render,
        /// the overlay is set to hidden once the component has loaded.
        /// </summary>
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                _overlayVisible = false;
            }
        }

        #endregion
    }
}
