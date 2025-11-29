using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FilingModule.Blazor.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Blazor.Components.Learners;
using SchoolsModule.Blazor.Components.Learners.Tables;
using SchoolsModule.Blazor.StateManagers;
using SchoolsModule.Domain.Constants;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Components.ActivityGroups
{
    /// <summary>
    /// The ActivityGroupsTable component is responsible for displaying a table of activity groups.
    /// It provides various functionalities such as searching, selecting, and managing activity groups.
    /// </summary>
    public partial class ActivityGroupsTable
    {
        #region Fields

        private bool _dense;
        private bool _striped;
        private bool _bordered;
        private bool _loaded;

        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;
        private bool _canLinkGroups;
        private bool _canAddTeamMembers;
        private bool _canSearch;
        private bool _canExport;
        private bool _canCreateMessage;
        private bool _canSentNotification;

        private readonly ActivityGroupPageParameters _args = new();

        private List<ActivityGroupDto> _activityGroups = new();
        private MudTable<ActivityGroupDto?> _table = null!;


        #endregion

        /// <summary>
        /// Updates internal query parameters whenever component parameters are set.
        /// </summary>
        protected override void OnParametersSet()
        {
            if (!string.IsNullOrWhiteSpace(CategoryId))
                _args.CategoryIds = CategoryId;

            if (!string.IsNullOrWhiteSpace(LearnerId))
                _args.LearnerId = LearnerId;
        }

        #region Injected Services

        /// <summary>
        /// The cascading authentication state, used to determine 
        /// the current user's permissions (e.g., create/edit/delete learners).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API calls.
        /// </summary>
        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to execute commands related to activity groups.
        /// </summary>
        [Inject] public IActivityGroupCommandService ActivityGroupCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to export activity group data.
        /// </summary>
        [Inject] public IActivityGroupExportService ActivityGroupExportService { get; set; } = null!;

        /// <summary>
        /// Injected state manager for managing school events.
        /// </summary>
        [Inject] public SchoolEventStateManager EventStateManager { get; set; } = null!;

        /// <summary>
        /// Injected file manager for handling file downloads.
        /// </summary>
        [Inject] public IBlazorDownloadFileManager BlazorDownloadFileManager { get; set; } = null!;

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


        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets a value indicating whether the "Link Team Members" button is displayed.
        /// </summary>
        [Parameter] public bool ShowLinkTeamMembersButton { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether multiple items can be selected.
        /// </summary>
        [Parameter] public bool MultiSelection { get; set; }

        /// <summary>
        /// The ID of the category to filter activity groups by.
        /// </summary>
        [Parameter] public string? CategoryId { get; set; }

        /// <summary>
        /// The ID of the learner to filter activity groups by.
        /// </summary>
        [Parameter] public string? LearnerId { get; set; }

        /// <summary>
        /// Function to provide server-side data for the table.
        /// </summary>
        [Parameter] public Func<TableState, CancellationToken, Task<TableData<ActivityGroupDto>>>? ServerData { get; set; }

        /// <summary>
        /// Event callback for toggling multi-selection mode from external contexts.
        /// </summary>
        [Parameter] public EventCallback<bool> MultiSelectionChanged { get; set; }

        /// <summary>
        /// Collection of selected activity groups in the current page.
        /// </summary>
        [Parameter] public HashSet<ActivityGroupDto?> SelectedActivityGroups { get; set; } = new();

        /// <summary>
        /// Event callback triggered when an activity group is removed.
        /// </summary>
        [Parameter] public EventCallback<string> ActivityGroupRemoved { get; set; }

        /// <summary>
        /// Render fragment for additional toolbar buttons.
        /// </summary>
        [Parameter] public RenderFragment ToolbarButtons { get; set; } = null!;

        /// <summary>
        /// Indicates whether to show the link group button.
        /// </summary>
        [Parameter] public bool ShowLinkGroupButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the "Link Learners" button should be displayed.
        /// </summary>
        [Parameter] public bool ShowLinkLearnersButton { get; set; } = false;

        /// <summary>
        /// Event callback for saving the table.
        /// </summary>
        [Parameter] public EventCallback SaveTable { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when the selection of learners changes.
        /// </summary>
        /// <remarks>This callback is triggered whenever the selection of learners is updated, providing
        /// details about the change through the <see cref="LearnerSelectionChangedEventArgs"/> parameter. Use this to
        /// handle selection changes, such as updating the UI or processing the selected learners.</remarks>
        [Parameter] public EventCallback<LearnerSelectionChangedEventArgs> SelectedLearnersChanged { get; set; }

        /// <summary>
        /// Gets or sets the callback that is invoked when the selected activity groups change.
        /// </summary>
        /// <remarks>Use this property to handle changes in the selected activity groups, such as updating
        /// the UI or processing the new selection.</remarks>
        [Parameter] public EventCallback<ActivityGroupSelectionChangedEventArgs> SelectedActivityGroupsChanged { get; set; }

        /// <summary>
        /// Event callback for resending attendance requests.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendAttendanceRequest { get; set; }

        /// <summary>
        /// Event callback for resending transport requests.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendTransportRequest { get; set; }

        /// <summary>
        /// Event callback for resending activity group attendance requests.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendActivityGroupAttendanceRequest { get; set; }

        /// <summary>
        /// Event callback for resending activity group transport requests.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendActivityGroupTransportRequest { get; set; }

        /// <summary>
        /// Event callback for removing consent.
        /// </summary>
        [Parameter] public EventCallback<RemoveConsentArgs> RemoveConsent { get; set; }

        /// <summary>
        /// Gets or sets the callback invoked when the learner selection changes.
        /// </summary>
        /// <remarks>Use this property to handle changes in learner selection, such as updating the UI or
        /// processing the selected learner data.</remarks>
        [Parameter] public EventCallback<CustomCheckValueChangedEventArgs<LearnerDto>> OnLearnerSelectionSelectionChanged { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets or sets a value indicating whether a row is automatically selected when clicked.
        /// </summary>
        private bool _selectOnRowClick;

        /// <summary>
        /// Handles the action of opening or closing the row menu.
        /// </summary>
        /// <remarks>When the row menu is opened, the ability to select rows on click is
        /// disabled.</remarks>
        /// <param name="open">A boolean value indicating whether the row menu should be opened.  <see langword="true"/> to open the row
        /// menu; <see langword="false"/> to close it.</param>
        public void OnOpenRowMenu(bool open)
        {
            if(open)
                _selectOnRowClick = false;
        }

        /// <summary>
        /// Gets the text to display for showing team members.
        /// </summary>
        /// <param name="showTeamMembers">Indicates whether to show team members.</param>
        /// <returns>The text to display.</returns>
        private static string ShowTeamMembersText(bool showTeamMembers) => showTeamMembers ? "Hide Team Members" : "Show Team Members";

        /// <summary>
        /// Handles the search functionality.
        /// </summary>
        /// <param name="text">The search text.</param>
        private async Task OnSearch(string text)
        {
            _args.SearchText = text;
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Navigates to the specified category.
        /// </summary>
        /// <param name="activityGroupId">The ID of the activity group.</param>
        public void NavigateToUpdateCategory(string activityGroupId)
        {
            NavigationManager.NavigateTo($"/activities/activitygroups/update/{activityGroupId}");
        }

        /// <summary>
        /// Shows the team members for the specified activity group.
        /// </summary>
        /// <param name="activityGroupId">The ID of the activity group.</param>
        public async Task ShowTeamMembers(string activityGroupId)
        {
            var value = _table.FilteredItems.FirstOrDefault(c => c.ActivityGroupId == activityGroupId)!.ShowTeamMembers;
            _table.FilteredItems.FirstOrDefault(c => c.ActivityGroupId == activityGroupId)!.ShowTeamMembers = !value;

            if (value && EventStateManager.HasEvent())
            {
                foreach (var item in _table.FilteredItems)
                {
                    EventStateManager.SchoolEvent.SelectedActivityGroups = new List<ActivityGroupDto?>();
                    EventStateManager.SchoolEvent.SelectedActivityGroups.Add(item);
                }
            }

            _selectOnRowClick = true;
            StateHasChanged();
        }

        /// <summary>
        /// Handles the event when the selected activity groups change.
        /// </summary>
        /// <param name="activityGroups">The selected activity groups.</param>
        private async Task OnSelectedActivityGroupsChangedAsync(HashSet<ActivityGroupDto> activityGroups)
        {
            await SelectedActivityGroupsChanged.InvokeAsync(new ActivityGroupSelectionChangedEventArgs(activityGroups.FirstOrDefault(), ActivityGroupSelectionAction.TeamMembersUpdated, null));
        }

        /// <summary>
        /// Handles the event triggered when the learner selection changes.
        /// </summary>
        /// <remarks>This method invokes the <see cref="OnLearnerSelectionSelectionChanged"/> event
        /// asynchronously.  Ensure that any event handlers attached to this event are prepared to handle asynchronous
        /// execution.</remarks>
        /// <param name="activityGroups">A set of activity groups associated with the learner selection. This parameter is currently unused but
        /// reserved for future functionality.</param>
        /// <returns></returns>
        private async Task LearnerSelectionSelectionChanged(HashSet<ActivityGroupDto> activityGroups)
        {
            await OnLearnerSelectionSelectionChanged.InvokeAsync();
        }

        /// <summary>
        /// Handles the event for resending attendance requests.
        /// </summary>
        /// <param name="args">The arguments for resending the attendance request.</param>
        public async Task OnResendAttendanceRequest(ResendPermissionsNotificationArgs args)
        {
            await ResendAttendanceRequest.InvokeAsync(args);
        }

        /// <summary>
        /// Handles the event for resending transport requests.
        /// </summary>
        /// <param name="args">The arguments for resending the transport request.</param>
        public async Task OnResendTransportRequest(ResendPermissionsNotificationArgs args)
        {
            await ResendTransportRequest.InvokeAsync(args);
        }

        /// <summary>
        /// Handles the event for resending activity group attendance requests.
        /// </summary>
        /// <param name="activityGroupId">The ID of the activity group.</param>
        public async Task OnResendActivityGroupAttendanceRequest(string activityGroupId)
        {
            var args = new ResendPermissionsNotificationArgs { ParticipatingActivityGroupId = activityGroupId };
            await ResendActivityGroupAttendanceRequest.InvokeAsync(args);
        }

        /// <summary>
        /// Handles the event for resending activity group transport requests.
        /// </summary>
        /// <param name="activityGroupId">The ID of the activity group.</param>
        public async Task OnResendActivityGroupTransportRequest(string activityGroupId)
        {
            var args = new ResendPermissionsNotificationArgs { ParticipatingActivityGroupId = activityGroupId };
            await ResendActivityGroupTransportRequest.InvokeAsync(args);
        }

        /// <summary>
        /// Links the selected activity groups.
        /// </summary>
        private async Task OnLinkActivityGroups()
        {
            MultiSelection = !MultiSelection;
            await MultiSelectionChanged.InvokeAsync(MultiSelection);
        }

        /// <summary>
        /// Sets the page arguments for the table state.
        /// </summary>
        /// <param name="state">The state of the table.</param>
        private void SetPageArguments(TableState state)
        {
            _args.PageNr = state.Page + 1;
            _args.PageSize = state.PageSize;
            _args.OrderBy = state.SortDirection == SortDirection.None ? null : $"{state.SortLabel} {state.SortDirection}";
        }

        /// <summary>
        /// Exports the actvitiy group team members to an Excel file.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        private async Task ExportActivityGroup(string acrivityGroupId)
        {
            var result = await ActivityGroupQueryService.ActivityGroupAsync(acrivityGroupId);
            result.ProcessResponseForDisplay(SnackBar, async () =>
            {
                var response = await ActivityGroupExportService.ExportActivityGroup(acrivityGroupId);
                if (!response.Succeeded) SnackBar.AddErrors(response.Messages);
                else
                {
                    if (response.Data is not null)
                    {
                        await BlazorDownloadFileManager.DownloadFile($"{result.Data.CategoryName}_{result.Data.Name}_ActivityGroup_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                            Convert.FromBase64String(response.Data),
                            "application/octet-stream");
                        SnackBar.Add(@"School Events Exported", Severity.Success);
                    }
                    else
                    {
                        SnackBar.Add(@"Activity Group Contains no team members", Severity.Success);
                    }
                }
            });
        }

        /// <summary>
        /// Reloads the server data for the table.
        /// </summary>
        public async Task ReloadServerData()
        {
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Fetches the activity groups from the server based on the table state.
        /// </summary>
        /// <param name="state">The state of the table.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The table data containing the activity groups.</returns>
        public async Task<TableData<ActivityGroupDto>> GetActivityGroupsAsync(TableState state, CancellationToken token)
        {
            if (ServerData is not null)
                return await ServerData.Invoke(state, token);

            SetPageArguments(state);
            var response = await ActivityGroupQueryService.PagedActivityGroupsAsync(_args);
            
            return new TableData<ActivityGroupDto>
            {
                TotalItems = response.TotalCount,
                Items = response.Data
            };
        }

        /// <summary>
        /// Deletes an activity group, showing a confirmation dialog first.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete the activity group from.</param>
        private async Task DeleteActivityGroup(string categoryId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, $"Are you sure you want to remove this activity group from this category?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var removalResponse = await ActivityGroupCommandService.DeleteAsync(categoryId);
                removalResponse.ProcessResponseForDisplay(SnackBar, async () =>
                {
                    await ActivityGroupRemoved.InvokeAsync(categoryId);
                    await _table.ReloadServerData();
                });
            }
        }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Asynchronously initializes the component and determines the user's authorization for specific activity
        /// permissions.
        /// </summary>
        /// <remarks>This method evaluates the user's authorization state and sets internal flags
        /// indicating whether the user has permissions to create, edit, or delete activities. It also invokes the base
        /// class's initialization logic.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _canSearch = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityPermissions.Search)).Succeeded;
            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityPermissions.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityPermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityPermissions.Delete)).Succeeded;
            _canExport = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityPermissions.Export)).Succeeded;
            _canLinkGroups = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityPermissions.LinkGroups)).Succeeded;
            _canAddTeamMembers = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityPermissions.LinkTeamMembers)).Succeeded;
            _canCreateMessage = (await AuthorizationService.AuthorizeAsync(authState.User, IdentityModule.Domain.Constants.Permissions.Users.CanSendMessage)).Succeeded;
            _canSentNotification = (await AuthorizationService.AuthorizeAsync(authState.User, IdentityModule.Domain.Constants.Permissions.Users.CanSendMessage)).Succeeded;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
