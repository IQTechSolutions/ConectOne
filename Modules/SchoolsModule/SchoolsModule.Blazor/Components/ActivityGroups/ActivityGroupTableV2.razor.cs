// Refactored ActivityGroupTableV2 Component with Documentation

using ConectOne.Blazor.Components;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.ResultWrappers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using SchoolsModule.Blazor.Components.Learners.Tables;
using SchoolsModule.Blazor.StateManagers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Components.ActivityGroups
{
    /// <summary>
    /// Component to manage the display and interaction with activity groups.
    /// Supports filtering, paging, searching, selection, and administrative actions.
    /// </summary>
    public partial class ActivityGroupTableV2
    {
        #region Fields

        private List<TableSelectionItem<ActivityGroupDto>> _activityGroups = new();
        private bool _multiselection;
        private ActivityGroupPageParameters _args = new() { PageSize = 25 };
        private bool _preventDefault;
        private int _pageItemCount;
        private int _pageCount;
        private bool _allSelected;
        private bool _canDoAttendanceCheckForTransportTo;
        private bool _canDoAttendanceCheckForTransportFrom;

        #endregion

        #region Injections

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP operations.
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to execute commands related to activity groups.
        /// </summary>
        [Inject] public IActivityGroupCommandService ActivityGroupCommandService { get; set; } = null!;

        [Inject] public ISchoolEventPermissionService SchoolEventPermissionService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="SchoolEventStateManager"/> instance used to manage the state of school events.
        /// </summary>
        [Inject] public SchoolEventStateManager EventStateManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be manually set
        /// in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets a value indicating whether the button to link activity group team members is displayed.
        /// </summary>
        [Parameter] public bool ShowLinkActivityGroupTeamMembersButton { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the link activity group buttons are displayed.
        /// </summary>
        [Parameter] public bool ShowLinkActivityGroupButtons { get; set; } = true;

        /// <summary>
        /// Gets or sets the callback invoked when the selection changes.
        /// </summary>
        /// <remarks>Use this property to handle selection change events for <see
        /// cref="ActivityGroupDto"/> items.</remarks>
        [Parameter] public EventCallback<CustomCheckValueChangedEventArgs<ActivityGroupDto>> OnSelectionChange { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when a request to resend attendance permissions is triggered.
        /// </summary>
        /// <remarks>This property is typically used to handle user interactions that require resending
        /// attendance-related notifications.  Ensure the callback is properly configured to handle the <see
        /// cref="ResendPermissionsNotificationArgs"/> parameter.</remarks>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendAttendanceRequest { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when a request to resend transport permissions is triggered.
        /// </summary>
        /// <remarks>This property is typically used to handle user actions that require resending
        /// transport permissions, such as retrying a failed notification.</remarks>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendTransportRequest { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked to resend the activity group attendance request notification.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendActivityGroupAttendanceRequest { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when a request to resend activity group transport permissions is
        /// triggered.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendActivityGroupTransportRequest { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when the learner selection changes.
        /// </summary>
        [Parameter] public EventCallback<ActivityGroupSelectionChangedEventArgs> LearnerSelectionSelectionChanged { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when the selection state of an activity group changes.
        /// </summary>
        /// <remarks>This event is triggered whenever the selection state of an activity group is updated.
        /// Use this callback to handle changes in the selection, such as updating the UI or processing the selected
        /// data.</remarks>
        [Parameter] public EventCallback<CustomCheckValueChangedEventArgs<ActivityGroupDto>> ActivityGroupSelectionChanged { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when a consent removal action is triggered.
        /// </summary>
        /// <remarks>Use this property to handle consent removal events, such as when a user revokes their
        /// consent. The callback receives an instance of <see cref="RemoveConsentArgs"/>, which provides details about
        /// the consent being removed.</remarks>
        [Parameter] public EventCallback<RemoveConsentArgs> RemoveConsent { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of category IDs.
        /// </summary>
        [Parameter] public string CategoryIds { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the learner.
        /// </summary>
        [Parameter] public string? LearnerId { get; set; }

        #endregion
        
        #region Methods

        /// <summary>
        /// Handles Enter key for form submission prevention.
        /// </summary>
        public void HandleKeyDown(KeyboardEventArgs e)
        {
            _preventDefault = (e.Key == "Enter" && !e.ShiftKey);
        }

        /// <summary>
        /// Resets the search filters and reloads data.
        /// </summary>
        public async Task Reload()
        {
            _args.PageNr = 1;
            _args.SearchText = "";
            await LoadData();
        }

        /// <summary>
        /// Loads the data based on event or learner context.
        /// </summary>
        private async Task LoadData()
        {
            if (EventStateManager.HasEvent())
            {
                _args.CategoryIds = string.Join(",", EventStateManager.SchoolEvent!.SelectedActivityCategories.Select(c => c.CategoryId));

                if (_multiselection)
                {
                    var result = await GetPagedActivityGroupsAsync();
                    if (result.Succeeded)
                    {
                        _pageCount = result.TotalPages;
                        _pageItemCount = result.TotalCount;
                        _activityGroups = TransformToSelectionItems(result.Data);

                        var parentResult = await ActivityGroupQueryService.AllActivityGroupsAsync(_args);
                        if (parentResult.Succeeded && EventStateManager.SchoolEvent.SelectedActivityGroups?.Any() == true)
                        {
                            _allSelected = EventStateManager.SchoolEvent.SelectedActivityGroups.Select(c => c.ActivityGroupId).SequenceEqual(parentResult.Data.Select(c => c.ActivityGroupId));
                        }
                    }
                }
                else
                {
                    var selected = EventStateManager.SchoolEvent.SelectedActivityGroups.OrderBy(c => c.AgeGroup?.MinAge).ThenBy(c => c.Name).ToList();
                    var result = PaginatedResult<ActivityGroupDto>.Success(selected, selected.Count, _args.PageNr, _args.PageSize);

                    _pageCount = result.TotalPages;
                    _pageItemCount = result.TotalCount;
                    _activityGroups = TransformToSelectionItems(result.Data);
                }
            }
            else
            {
                _args.LearnerId = LearnerId;
                var result = await GetPagedActivityGroupsAsync();
                if (result.Succeeded)
                {
                    _pageCount = result.TotalPages;
                    _pageItemCount = result.TotalCount;
                    _activityGroups = result.Data.Select(b => new TableSelectionItem<ActivityGroupDto>(false, b)).ToList();
                }
            }

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Fetches paginated activity groups from the API.
        /// </summary>
        private async Task<PaginatedResult<ActivityGroupDto>> GetPagedActivityGroupsAsync()
        {
            var bb = await ActivityGroupQueryService.PagedActivityGroupsAsync(_args);
            return bb;
        }

        /// <summary>
        /// Converts DTOs to table selection items based on current selections.
        /// </summary>
        private List<TableSelectionItem<ActivityGroupDto>> TransformToSelectionItems(IEnumerable<ActivityGroupDto> data)
        {
            return data.Select(b => new TableSelectionItem<ActivityGroupDto>(EventStateManager.SchoolEvent.SelectedActivityGroups.Any(c => c.ActivityGroupId == b.ActivityGroupId), b)).ToList();
        }

        /// <summary>
        /// Toggles selection for all activity groups.
        /// </summary>
        public async Task OnAllSelectedChanged(bool value)
        {
            _allSelected = value;
            var parentResult = await ActivityGroupQueryService.AllActivityGroupsAsync(_args);
            if (!parentResult.Succeeded) return;

            EventStateManager.SchoolEvent.SelectedActivityGroups = value ? parentResult.Data.ToList() : [];
            foreach (var item in _activityGroups)
            {
                item.IsChecked = value;
            }
            StateHasChanged();
        }

        /// <summary>
        /// Updates the search term and reloads the data.
        /// </summary>
        private async Task OnSearch(string searchText)
        {
            _args.SearchText = searchText;
            await LoadData();
        }

        /// <summary>
        /// Toggles multiselection mode.
        /// </summary>
        private async Task OnLinkActivityGroups()
        {
            _multiselection = !_multiselection;
            await LoadData();
        }

        /// <summary>
        /// Updates page number and reloads data.
        /// </summary>
        private async Task OnPageChanged(int pageNr)
        {
            _args.PageNr = pageNr;
            await LoadData();
        }

        /// <summary>
        /// Handles individual row selection change.
        /// </summary>
        private async Task OnValueSelectionChanged(CustomCheckValueChangedEventArgs<ActivityGroupDto> args)
        {
            if (args.IsChecked)
                EventStateManager.SchoolEvent.SelectedActivityGroups.Add(args.Item);
            else
                EventStateManager.SchoolEvent.SelectedActivityGroups.RemoveAll(c => c.ActivityGroupId == args.Item.ActivityGroupId);

            var item = _activityGroups.FirstOrDefault(c => c.RowItem.ActivityGroupId == args.Item.ActivityGroupId);
            if (item != null) item.IsChecked = args.IsChecked;

            await OnSelectionChange.InvokeAsync(args);
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Toggles visibility of team members within an activity group.
        /// </summary>
        public async Task ShowTeamMembers(string activityGroupId)
        {
            var activityGroup = _activityGroups.FirstOrDefault(c => c.RowItem.ActivityGroupId == activityGroupId);
            if (activityGroup != null)
                activityGroup.RowItem.ShowTeamMembers = !activityGroup.RowItem.ShowTeamMembers;

            StateHasChanged();
        }

        /// <summary>
        /// Handles the resend attendance request operation by invoking the associated event asynchronously.
        /// </summary>
        /// <param name="args">The arguments containing the details required to process the resend attendance request.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task OnResendAttendanceRequest(ResendPermissionsNotificationArgs args) => await ResendAttendanceRequest.InvokeAsync(args);

        /// <summary>
        /// Handles the resend transport request by invoking the associated event asynchronously.
        /// </summary>
        /// <param name="args">The arguments containing the details of the resend permissions notification. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task OnResendTransportRequest(ResendPermissionsNotificationArgs args) => await ResendTransportRequest.InvokeAsync(args);

        /// <summary>
        /// Resends the attendance request for the specified activity group.
        /// </summary>
        /// <remarks>This method triggers the resend operation for the attendance request associated with
        /// the specified activity group. Ensure that the <paramref name="activityGroupId"/> corresponds to a valid and
        /// existing activity group.</remarks>
        /// <param name="activityGroupId">The unique identifier of the activity group for which the attendance request should be resent. This value
        /// cannot be <see langword="null"/> or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task OnResendActivityGroupAttendanceRequest(string activityGroupId)
        {
            await ResendActivityGroupAttendanceRequest.InvokeAsync(new ResendPermissionsNotificationArgs { ParticipatingActivityGroupId = activityGroupId });
        }

        /// <summary>
        /// Handles the request to resend transport notifications for a specific activity group.
        /// </summary>
        /// <remarks>This method triggers the resend operation by invoking the appropriate event with the
        /// provided activity group ID. Ensure that <paramref name="activityGroupId"/> is not null or empty before
        /// calling this method.</remarks>
        /// <param name="activityGroupId">The unique identifier of the activity group for which transport notifications should be resent.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task OnResendActivityGroupTransportRequest(string activityGroupId)
        {
            await ResendActivityGroupTransportRequest.InvokeAsync(new ResendPermissionsNotificationArgs { ParticipatingActivityGroupId = activityGroupId });
        }

        /// <summary>
        /// Handles selection changes within team member table.
        /// </summary>
        private async Task OnLearnerSelectionSelectionChanged(CustomCheckValueChangedEventArgs<LearnerDto> args, ActivityGroupDto activityGroup)
        {
            var eventArgs = new ActivityGroupSelectionChangedEventArgs(activityGroup, args.IsChecked ? ActivityGroupSelectionAction.TeamMemberAdded : ActivityGroupSelectionAction.TeamMemberRemoved, args.Item);
            await LearnerSelectionSelectionChanged.InvokeAsync(eventArgs);
        }

        /// <summary>
        /// Navigates to the update activity group page.
        /// </summary>
        public void NavigateToUpdateCategory(string activityGroupId)
        {
            NavigationManager.NavigateTo($"/activities/activitygroups/update/{activityGroupId}");
        }

        /// <summary>
        /// Confirms and deletes the specified activity group.
        /// </summary>
        private async Task DeleteActivityGroup(string categoryId)
        {
            if (!await DialogService.ConfirmAction("Are you sure you want to remove this activity group from this category?")) return;

            var response = await ActivityGroupCommandService.DeleteAsync(categoryId);
            if (response.Succeeded)
                _activityGroups.RemoveAll(c => c.RowItem.ActivityGroupId == categoryId);
            else
                SnackBar.AddErrors(response.Messages);

            StateHasChanged();
        }

        /// <summary>
        /// Retrieves the collection of parent permissions associated with the specified activity group if an event is
        /// active.
        /// </summary>
        /// <remarks>If no event is currently active, the method returns an empty collection. The returned
        /// collection may also be empty if the activity group has no associated parent permissions.</remarks>
        /// <param name="participatingActivityGroupId">The identifier of the activity group for which to check parent permissions. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of parent
        /// permissions for the specified activity group. Returns an empty collection if no event is active or if
        /// permissions cannot be retrieved.</returns>
        public async Task<IEnumerable<ParentPermissionDto>> CheckPermissionCount(string participatingActivityGroupId)
        {
            if (EventStateManager.HasEvent())
            {
                var canDoTransPortToResult = await SchoolEventPermissionService.GetAllParentPermissions(participatingActivityGroupId);
                if (canDoTransPortToResult.Succeeded)
                    return canDoTransPortToResult.Data;
            }
            return new List<ParentPermissionDto>();
        }

        /// <summary>
        /// Lifecycle method to load data on first render.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadData();
                StateHasChanged();
            }
        }

        #endregion
    }
}