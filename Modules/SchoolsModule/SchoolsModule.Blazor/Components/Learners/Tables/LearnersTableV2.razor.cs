using ConectOne.Blazor.Components;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Enums;
using ConectOne.Domain.ResultWrappers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Blazor.StateManagers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Components.Learners.Tables
{
    /// <summary>
    /// Represents a table component for displaying and managing learners.
    /// </summary>
    public partial class LearnersTableV2 : IDisposable
    {
        #region Fields

        private List<TableSelectionItem<LearnerDto>> _learners = new List<TableSelectionItem<LearnerDto>>();
        private bool _multiselection;
        private LearnerPageParameters _args = new LearnerPageParameters() { PageSize = 100 };
        private bool _preventDefault;
        private int _pageItemCount;
        private int _pageCount;
        private ActivityGroupViewModel _activityGroup;

        #endregion

        #region Injections & Parameters

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ILearnerQueryService LearnerQueryService { get; set; } = null!;

        [Inject] public ISchoolEventCategoryService SchoolEventCategoryService { get; set; } = null!;

        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        /// <summary>
        /// Injected state manager for managing school events.
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
        /// Gets or sets the age group information for the current context.
        /// </summary>
        /// <remarks>This property is typically used to bind or display age group data in the user
        /// interface. Ensure that the value is properly initialized before accessing its members.</remarks>
        [Parameter] public AgeGroupViewModel AgeGroup { get; set; }

        /// <summary>
        /// Gets or sets the gender associated with the entity.
        /// </summary>
        [Parameter] public Gender Gender { get; set; }
        
        /// <summary>
        /// List of selected learners.
        /// </summary>
        [Parameter] public List<LearnerDto>? SelectedLearners { get; set; } = new List<LearnerDto>();

        /// <summary>
        /// The ID of the activity group.
        /// </summary>
        [Parameter] public string? ActivityGroupId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the participating activity group.
        /// </summary>
        [Parameter] public string? ParticipatingActivityGroupId { get; set; }

        /// <summary>
        /// Indicates whether to show the link learners button.
        /// </summary>
        [Parameter] public bool ShowLinkLearnersButton { get; set; } = true;

        /// <summary>
        /// Event callback for when the selection of learners changes.
        /// </summary>
        [Parameter] public EventCallback<CustomCheckValueChangedEventArgs<LearnerDto>> OnSelectionChange { get; set; }

        /// <summary>
        /// Event callback for removing consent for a given learner.
        /// </summary>
        [Parameter] public EventCallback<RemoveConsentArgs> RemoveConsent { get; set; }

        /// <summary>
        /// Event callback for resending attendance permission requests.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendAttendanceRequest { get; set; }

        /// <summary>
        /// Event callback for resending transport permission requests.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendTransportRequest { get; set; }

        #endregion
        
        #region Methods

        /// <summary>
        /// Handles the key down event for form submission.
        /// </summary>
        /// <param name="e">The keyboard event arguments.</param>
        private void HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" && !e.ShiftKey)
            {
                _preventDefault = true;
                // Handle form submission logic here
            }
            else
            {
                _preventDefault = false;
            }
        }

        /// <summary>
        /// Reloads the data by resetting the page number and search text.
        /// </summary>
        private async Task Reload()
        {
            _args.PageNr = 1;
            _args.SearchText = "";
            await LoadData();
        }

        /// <summary>
        /// Loads all paged learner data from the server.
        /// </summary>
        private async Task LoadAllPagedLearnerData()
        {
            var request = await LearnerQueryService.PagedLearnersAsync(_args);
            if (!request.Succeeded)
            {
                SnackBar.AddErrors(request.Messages);
            }

            _pageCount = request.TotalPages;
            _pageItemCount = request.TotalCount;
            _learners = request.Data.Select(b => new TableSelectionItem<LearnerDto>(SelectedLearners.Any(c => c.LearnerId == b.LearnerId), b)).ToList();
        }

        /// <summary>
        /// Loads learner data for the specified activity group.
        /// </summary>
        private async Task LoadActivityGroupLearnerData()
        {
            if (_multiselection)
            {
                await LoadAllPagedLearnerData();
            }
            else
            {
                if (SelectedLearners == null || !SelectedLearners.Any())
                {
                    _pageCount = 0;
                    _pageItemCount = 0;
                    _learners = new List<TableSelectionItem<LearnerDto>>();
                }
                else
                {
                    if (!string.IsNullOrEmpty(_args.SearchText))
                    {
                        var dne = _activityGroup.SelectedTeamMembers.Where(c => c.FirstName.ToLower().Contains(_args.SearchText.ToLower()) || c.LastName.ToLower().Contains(_args.SearchText.ToLower())).ToList();
                        var dneTeamMembers = PaginatedResult<LearnerDto>.Success(dne.ToList(), dne.Count, _args.PageNr, _args.PageSize);
                        _pageCount = dneTeamMembers.TotalPages;
                        _pageItemCount = dneTeamMembers.TotalCount;
                        _learners = dneTeamMembers.Data.Select(b => new TableSelectionItem<LearnerDto>(SelectedLearners.Any(c => c.LearnerId == b.LearnerId), b)).ToList();
                    }
                    else
                    {
                        var selectedTeamMembers = PaginatedResult<LearnerDto>.Success(SelectedLearners.ToList(), _activityGroup.SelectedTeamMembers.Count, _args.PageNr, _args.PageSize);
                        _pageCount = selectedTeamMembers.TotalPages;
                        _pageItemCount = selectedTeamMembers.TotalCount;
                        _learners = selectedTeamMembers.Data.Select(b => new TableSelectionItem<LearnerDto>(SelectedLearners.Any(c => c.LearnerId == b.LearnerId), b)).ToList();
                    }
                }
            }
        }

        /// <summary>
        /// Loads the learner data based on the current state and parameters.
        /// </summary>
        private async Task LoadData()
        {
            if (EventStateManager.HasEvent())
            {
                if (_multiselection)
                {
                    await LoadAllPagedLearnerData();
                }
                else
                {
                    var participatingTeamMemberResult = await SchoolEventCategoryService.ParticipatingActivityGroupTeamMembers(EventStateManager.SchoolEvent!.EventId, ActivityGroupId);
                    if (SelectedLearners.Any())
                    {
                        var selectedTeamMembers = PaginatedResult<LearnerDto>.Success(SelectedLearners.ToList(), SelectedLearners.Count(), _args.PageNr, _args.PageSize);
                        _pageCount = selectedTeamMembers.TotalPages;
                        _pageItemCount = selectedTeamMembers.TotalCount;
                        _learners = selectedTeamMembers.Data.Select(b => new TableSelectionItem<LearnerDto>(participatingTeamMemberResult.Data.Any(c => c.LearnerId == b.LearnerId), b)).ToList();
                    }
                    else
                    {
                        await LoadActivityGroupLearnerData();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ActivityGroupId))
                {
                    await LoadActivityGroupLearnerData();
                }
                else
                {
                    await LoadAllPagedLearnerData();
                }
            }
            StateHasChanged();
        }

        /// <summary>
        /// Handles the search event and reloads the data based on the search text.
        /// </summary>
        /// <param name="searchText">The search text entered by the user.</param>
        private async Task OnSearch(string searchText)
        {
            _args.SearchText = searchText;
            await LoadData();
        }

        /// <summary>
        /// Toggles the multi-selection mode and reloads the data.
        /// </summary>
        private async Task OnLinkTeamMembers()
        {
            _multiselection = !_multiselection;
            await LoadData();
        }

        /// <summary>
        /// Handles the page change event and reloads the data for the specified page number.
        /// </summary>
        /// <param name="pageNr">The new page number.</param>
        private async Task OnPageChanged(int pageNr)
        {
            _args.PageNr = pageNr;
            await LoadData();
        }

        /// <summary>
        /// Handles the value selection change event for learners.
        /// </summary>
        /// <param name="args">The event arguments containing the selected learner and its checked state.</param>
        private async Task OnValueSelectionChanged(CustomCheckValueChangedEventArgs<LearnerDto> args)
        {
            if (args.IsChecked)
            {
                SelectedLearners.Add(args.Item);
                _learners.FirstOrDefault(c => c.RowItem.LearnerId == args.Item.LearnerId).IsChecked = true;
            }
            else
            {
                SelectedLearners.Remove(SelectedLearners.FirstOrDefault(c => c.LearnerId == args.Item.LearnerId));
                _learners.FirstOrDefault(c => c.RowItem.LearnerId == args.Item.LearnerId).IsChecked = false;
            }

            await OnSelectionChange.InvokeAsync(args);
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Reloads the data by updating the gender and age parameters and then asynchronously loading the data.
        /// </summary>
        /// <remarks>This method updates the internal state with the current gender and age group settings
        /// before initiating the data load. Ensure that the <see cref="_args"/> object is properly initialized before
        /// calling this method.</remarks>
        /// <returns>A task that represents the asynchronous operation of reloading the data.</returns>
        public async Task ReloadData()
        {
            _args.Gender = Gender;
            _args.MinAge = AgeGroup.MinAge;
            _args.MaxAge = AgeGroup.MaxAge;

            await LoadData();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Asynchronously initializes the component and subscribes to notification change events.
        /// </summary>
        /// <remarks>This method overrides the base implementation to set up an event handler for
        /// notification changes. The event handler triggers a reload of the component when notifications are
        /// updated.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            EventStateManager.OnNotificationsChanged += async () => await Reload();
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Lifecycle method invoked after the component has rendered.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                if (!string.IsNullOrEmpty(ActivityGroupId))
                {
                    var result = await ActivityGroupQueryService.ActivityGroupAsync(ActivityGroupId);
                    result.ProcessResponseForDisplay(SnackBar, async () =>
                    {
                        if (result.Data is not null)
                            _activityGroup = new ActivityGroupViewModel(result.Data);
                        else
                            _activityGroup = new ActivityGroupViewModel();

                        if (EventStateManager.HasEvent())
                        {
                            var participatingTeamMemberResult = await SchoolEventCategoryService.ParticipatingActivityGroupTeamMembers(EventStateManager.SchoolEvent!.EventId, ActivityGroupId);
                            if (participatingTeamMemberResult.Succeeded && participatingTeamMemberResult.Data.Any())
                            {
                                SelectedLearners = participatingTeamMemberResult.Data;
                            }
                            else
                            {
                                SelectedLearners = _activityGroup.SelectedTeamMembers;
                            }
                        }
                        else
                        {
                            SelectedLearners = _activityGroup.SelectedTeamMembers;
                        }

                        _args.ActivityGroupId = ActivityGroupId;
                        _args.Gender = _activityGroup.Gender;
                        _args.MaxAge = _activityGroup.SelectedAgeGroup is null ? 100 : _activityGroup.SelectedAgeGroup.MaxAge;
                        _args.MinAge = _activityGroup.SelectedAgeGroup is null ? 0 : _activityGroup.SelectedAgeGroup.MinAge;
                    });
                }
                await LoadData();
                StateHasChanged();
            }
        }

        #endregion

        /// <summary>
        /// Releases resources used by the current instance and unsubscribes from event notifications.
        /// </summary>
        /// <remarks>This method detaches the event handler from the <see
        /// cref="EventStateManager.OnNotificationsChanged"/> event  to prevent memory leaks and ensure proper cleanup
        /// of resources.</remarks>
        public void Dispose()
        {
            EventStateManager.OnNotificationsChanged -= async () => await Reload();
        }
    }
}
