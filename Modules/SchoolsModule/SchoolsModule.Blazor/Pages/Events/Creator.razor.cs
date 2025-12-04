using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Application.ViewModels;
using FilingModule.Blazor.Modals;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.Interfaces;
using GroupingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Blazor.Components.ActivityGroups;
using SchoolsModule.Blazor.Components.Learners.Tables;
using SchoolsModule.Blazor.Modals;
using SchoolsModule.Blazor.StateManagers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.Events
{
    /// <summary>
    /// The Creator component is responsible for creating new school events.
    /// It provides a form for inputting event details and handles the submission process.
    /// </summary>
    public partial class Creator : IDisposable
    {
        #region Injections & Parameters

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ICategoryService<ActivityGroup> ActivityCategoryService { get; set; } = null!;

        /// <summary>
        /// Injected state manager for handling school events.
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
        /// The ID of the activity category to which the event belongs.
        /// </summary>
        [Parameter] public string ActivityCategoryId { get; set; } = null!;

        #endregion

        #region Fields
        
        /// <summary>
        /// Indicates whether multiple selection is enabled for the category table.
        /// </summary>
        private readonly bool _categoryTableMultiSelection = true;

        /// <summary>
        /// Parameters for the category table pagination and sorting.
        /// </summary>
        private readonly CategoryPageParameters _categoryTableArgs = new CategoryPageParameters();

        /// <summary>
        /// Parameters for the activity group table pagination and sorting.
        /// </summary>
        private readonly ActivityGroupPageParameters _activityGroupTableArgs = new ActivityGroupPageParameters();

        /// <summary>
        /// Reference to the activity groups table component.
        /// </summary>
        private ActivityGroupTableV2 _activityGroupsTable = null!;

        /// <summary>
        /// The name of the category.
        /// </summary>
        private string? _categoryName;

        /// <summary>
        /// List of breadcrumb items for navigation.
        /// </summary>
        private readonly List<BreadcrumbItem> _items =
        [
            new ("Home", href: "#", icon: Icons.Material.Filled.Home),
            new ("Events", href: "/events", icon: Icons.Material.Filled.Event),
            new ("Create", href: null, disabled: true, icon: Icons.Material.Filled.Create)
        ];

        /// <summary>
        /// Indicates whether the component is currently working (e.g., processing a request).
        /// </summary>
        private bool _working = false;

        /// <summary>
        /// The date range for the event.
        /// </summary>
        private DateRange _dateRange = new() { Start = DateTime.Now, End = DateTime.Now.AddMinutes(5) };

        /// <summary>
        /// The start time for the event.
        /// </summary>
        private TimeSpan? _startTime = new(00, 00, 00);

        /// <summary>
        /// The end time for the event.
        /// </summary>
        private TimeSpan? _endTime = new(00, 00, 00);

        /// <summary>
        /// The count of subcategories within the category.
        /// </summary>
        private int _categoryCount;

        /// <summary>
        /// Source URL for the cover image.
        /// </summary>
        private string _imageSource = "_content/SchoolsModule.Blazor/images/NoImage.jpg";

        #endregion

        #region Methods

        /// <summary>
        /// Updates the cover image source when the image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        private void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Sets the date range for the event based on the selected start and end dates.
        /// </summary>
        private void SetDateRange()
        {
            var startDate = _dateRange.Start!.Value;
            var oldStartDate = EventStateManager.SchoolEvent!.StartDate;

            var endDate = _dateRange.End!.Value;
            var oldEndDate = EventStateManager.SchoolEvent.EndDate;

            EventStateManager.SchoolEvent.StartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, oldStartDate.Hour, oldStartDate.Minute, oldStartDate.Second);
            EventStateManager.SchoolEvent.EndDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, oldEndDate.Hour, oldEndDate.Minute, oldEndDate.Second);
        }

        /// <summary>
        /// Sets the start time for the event.
        /// </summary>
        private void SetStartTime()
        {
            EventStateManager.SchoolEvent!.StartDate = EventStateManager.SchoolEvent.StartDate.Date.Add(_startTime!.Value);
        }

        /// <summary>
        /// Sets the end time for the event.
        /// </summary>
        private void SetEndTime()
        {
            EventStateManager.SchoolEvent!.EndDate = EventStateManager.SchoolEvent.EndDate.Date.Add(_endTime!.Value);
        }

        /// <summary>
        /// Creates a new school event by sending a request to the API.
        /// Displays a success message upon successful creation and navigates to the update page for the event.
        /// </summary>
        public async Task CreateAsync()
        {
            if(_dateRange.Start.Value.Date <= DateTime.Now.Date)
                SnackBar.AddError("Please select a date in the future you cannot create events in the past");


            var result = await EventStateManager.CreateSchoolEvent(ActivityCategoryId);
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                NavigationManager.NavigateTo($"/activities/activitygroups/events/update/{EventStateManager.SchoolEvent!.EventId}");
            });
        }

        /// <summary>
        /// Cancels the creation process and navigates back to the dashboard.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo("/dashboard");
        }

        #endregion

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
                EventStateManager.SchoolEvent.TicketTypes.Add(newTicketType);
                SnackBar.AddSuccess("Ticket type was successfully created");
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
                var model = ((SchoolEventTicketTypeViewModel)result.Data!).ToDto();
                EventStateManager.SchoolEvent.TicketTypes[index] = model;
                SnackBar.AddSuccess("Ticket type was successfully updated");
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
                    EventStateManager.SchoolEvent.TicketTypes.Remove(linkToRemove);
            }
        }

        #endregion

        #region Documents Link Management

        /// <summary>
        /// Creates a new document link for the event.
        /// </summary>
        private async Task CreateLinkAsync()
        {
            var parameters = new DialogParameters<CreateDocumentLinkUrlModal>
            {
                { x => x.EntityId, EventStateManager.SchoolEvent!.EventId }
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
        /// Removes a document link from the event.
        /// </summary>
        /// <param name="link">The link to be removed.</param>
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
                var linkToRemove = EventStateManager.SchoolEvent!.DocumentLinks.FirstOrDefault(c => c == link);

                if (linkToRemove != null)
                    EventStateManager.SchoolEvent.DocumentLinks.Remove(linkToRemove);
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
            
            if(_activityGroupsTable is not null)
                await _activityGroupsTable.Reload();
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

            if (_categoryTableMultiSelection)
            {
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
        /// Handles the event when the activity group selection changes.
        /// </summary>
        /// <param name="args">The event arguments containing the selected activity group and action.</param>
        private void OnActivityGroupSelectionChanged(CustomCheckValueChangedEventArgs<ActivityGroupDto> args)
        {
            //if (args.IsChecked)
            //{
            //    //EventStateManager.SchoolEvent!.SelectedActivityGroups.Add(args.Item);
            //    var a = 0;
            //}
            //else
            //{
            //    EventStateManager.SchoolEvent!.SelectedActivityGroups.Remove(args.Item);
            //}
        }

        /// <summary>
        /// Handles the event when the activity group team member selection changes.
        /// </summary>
        /// <param name="args">The event arguments containing the selected activity group and action.</param>
        private void OnActivityGroupTeamMemberSelectionChanged(ActivityGroupSelectionChangedEventArgs args)
        {
            if (args.Action == ActivityGroupSelectionAction.TeamMemberAdded)
            {
                EventStateManager.SchoolEvent!.SelectedActivityGroups.FirstOrDefault(c => c?.ActivityGroupId == args.ActivityGroup!.ActivityGroupId)?.TeamMembers.Add(args.Learner);
            }
            else
            {
                EventStateManager.SchoolEvent!.SelectedActivityGroups.FirstOrDefault(c => c?.ActivityGroupId == args.ActivityGroup!.ActivityGroupId)?.TeamMembers.Remove(args.Learner);
            }
            StateHasChanged();
        }

        #endregion

        #region Overrides & Interfaces Members

        /// <summary>
        /// Initializes the component by setting the event state and category.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            EventStateManager.OnNotificationsChanged += StateHasChanged;
            _categoryTableArgs.ParentId = ActivityCategoryId;

            var setEventResult = await EventStateManager.SetSchoolEventAsync();
            if (!setEventResult.Succeeded) SnackBar.AddErrors(setEventResult.Messages);

            EventStateManager.SetSchoolEventCategory(ActivityCategoryId);

            var categoryResult = await ActivityCategoryService.CategoryAsync(ActivityCategoryId);
            if (categoryResult.Succeeded)
            {
                _categoryCount = categoryResult.Data.SubCategoryCount;
                _categoryName = categoryResult.Data.Name;
            }

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Disposes the component and removes event handlers.
        /// </summary>
        public void Dispose()
        {
            EventStateManager.OnNotificationsChanged -= StateHasChanged;
        }

        #endregion
    }
}
