using ConectOne.Blazor.Extensions;
using ConectOne.Domain.ResultWrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Blazor.Extensions;
using SchoolsModule.Blazor.Modals;
using SchoolsModule.Blazor.StateManagers;
using SchoolsModule.Domain.Constants;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Components.Learners
{
    /// <summary>
    /// Displays a table of learners, supporting searching, paging, 
    /// filtering by grade/class/parent/activity group,
    /// and multi-selection modes. It integrates with various services 
    /// (e.g., <see cref="EventStateManager"/>) to fetch learners 
    /// and manage their association with activity groups or events.
    ///
    /// <para>
    /// <strong>Key Features:</strong>
    /// <list type="bullet">
    ///   <item><description>Server-side paging and searching using <see cref="MudTable{T}"/>.</description></item>
    ///   <item><description>Optional filters such as <c>ParentId</c>, <c>GradeId</c>, <c>SchoolClassId</c>, or <c>ActivityGroup</c>.</description></item>
    ///   <item><description>Multi-selection for linking learners to events or activity groups.</description></item>
    ///   <item><description>Event callbacks for removing consents, resending permission requests, and creating new learners.</description></item>
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// This component typically displays a list of learners in a <see cref="MudTable{T}"/>. 
    /// The table can handle large data sets via paging and sorting, 
    /// with optional advanced filtering if <see cref="ActivityGroup"/> or <see cref="AgeGroup"/> is provided.
    /// </para>
    /// </summary>
	public partial class LearnersTable 
    {
        #region Fields

        public MudTable<LearnerDto> _table { get; set; } = null!;
        private readonly LearnerPageParameters _args = new() { PageSize = 100 };

        private bool _searching;
        private bool _canCreate;
        private bool _canAddParent;
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;
        private bool _preventDefault;

        #endregion

        #region Injected Services

        /// <summary>
        /// Manages event-related state (e.g., selected event, activity groups, team members).
        /// Provides context when linking learners to events or sending permission requests.
        /// </summary>
        [Inject] public SchoolEventStateManager EventStateManager { get; set; } = null!;

        /// <summary>
        /// A provider abstraction for sending HTTP requests (GET, POST, DELETE, etc.) to the server.
        /// </summary>
        [Inject] public ILearnerQueryService LearnerQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to execute learner-related commands.
        /// </summary>
        [Inject] public ILearnerCommandService LearnerCommandService { get; set; } = null!;

        [Inject] public IActivityGroupQueryService ActivityGroupQueryService { get; set; } = null!;

        [Inject] public ISchoolEventCategoryService SchoolEventCategoryService { get; set; } = null!;

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
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        #endregion

        #region Parameters & Injected Services

        /// <summary>
        /// The cascading authentication state, used to determine 
        /// the current user's permissions (e.g., create/edit/delete learners).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
        
        /// <summary>
        /// Optional ParentId to filter learners by their associated parent.
        /// </summary>
        [Parameter] public string? ParentId { get; set; }

        /// <summary>
        /// Optional SchoolClassId to filter learners by their class.
        /// </summary>
        [Parameter] public string? SchoolClassId { get; set; }

        /// <summary>
        /// ActivityGroup context. If provided, filters learners related to this group or event activity group.
        /// </summary>
        [Parameter] public ActivityGroupDto? ActivityGroup { get; set; }

        /// <summary>
        /// Age group view model to filter learners by minimum and maximum age.
        /// </summary>
        [Parameter] public AgeGroupDto? AgeGroup { get; set; }

        /// <summary>
        /// Event callback triggered when learner selection changes (if multi-selection is enabled).
        /// </summary>
        [Parameter] public EventCallback<LearnerSelectionChangedEventArgs> LearnerSelectionChanged { get; set; }

        /// <summary>
        /// Event callback for resending attendance permission requests.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendAttendanceRequest { get; set; }

        /// <summary>
        /// Event callback for resending transport permission requests.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendTransportRequest { get; set; }

        /// <summary>
        /// Event callback for toggling multi-selection mode from external contexts.
        /// </summary>
        [Parameter] public EventCallback<bool> MultiSelectionChanged { get; set; }

        /// <summary>
        /// Event callback for removing consent for a given learner.
        /// </summary>
        [Parameter] public EventCallback<RemoveConsentArgs> RemoveConsent { get; set; }

        /// <summary>
        /// Event callback triggered when a new learner is created.
        /// </summary>
        [Parameter] public EventCallback<LearnerViewModel> LearnerCreated { get; set; }

        /// <summary>
        /// Event callback triggered when a search is performed.
        /// </summary>
        [Parameter] public EventCallback<string> Search { get; set; }

        /// <summary>
        /// Event callback triggered when a learner is removed.
        /// </summary>
        [Parameter] public EventCallback<string> LearnerRemoved { get; set; }

        /// <summary>
        /// Indicates whether to use a modal for creating new learners.
        /// </summary>
        [Parameter] public bool UseModal { get; set; } = false;

        /// <summary>
        /// Function to provide server-side data for the table.
        /// </summary>
        [Parameter] public Func<TableState, CancellationToken, Task<TableData<LearnerDto>>>? ServerData { get; set; }

        /// <summary>
        /// Collection of selected learners in the current page.
        /// </summary>
        [Parameter] public HashSet<LearnerDto> SelectedPagedLearners { get; set; } = new();

        /// <summary>
        /// Indicates whether to show the link learners table.
        /// </summary>
        [Parameter] public bool ShowLinkLearnersTable { get; set; } = true;

        /// <summary>
        /// Indicates whether multi-selection mode is enabled.
        /// </summary>
        [Parameter] public bool MultiSelection { get; set; } = false;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the component has finished loading data.
        /// </summary>
        public bool Loaded { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Triggered when the user performs a search. Updates _args.SearchText and reloads data.
        /// </summary>
        /// <param name="text">The search text entered by the user.</param>
        private async Task OnSearch(string text)
        {
            _args.SearchText = text;
            await Search.InvokeAsync(text);
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Deletes a learner, showing a confirmation dialog first.
        /// The parameter <paramref name="learnerId"/> is interpreted 
        /// as the learner's unique identifier.
        /// </summary>
        /// <param name="learnerId">The ID of the learner to delete.</param>
        private async Task DeleteLearner(string learnerId)
        {
            if (!await DialogService.ConfirmAction("Are you sure you want to remove this learner from this application?"))
                return;

            if (string.IsNullOrEmpty(ParentId))
            {
                var deleteResult = await LearnerCommandService.RemoveAsync(learnerId);

                async void SuccessAction()
                {
                    await LearnerRemoved.InvokeAsync(learnerId);
                    await _table.ReloadServerData();
                }

                deleteResult.ProcessResponseForDisplay(SnackBar, SuccessAction);
            }
            else
            {
                await LearnerRemoved.InvokeAsync(learnerId);
            }
        }

        /// <summary>
        /// Invoked whenever learners are selected or deselected, 
        /// typically in multi-selection mode. Fires the <see cref="LearnerSelectionChanged"/> callback 
        /// so parent components can respond (e.g., linking selected learners to an event).
        /// </summary>
        /// <param name="team">The newly selected set of learners in the table.</param>
        private async Task OnLearnerSelectionChanged(HashSet<LearnerDto> team)
        {
            if (Loaded)
                await LearnerSelectionChanged.InvokeAsync(new LearnerSelectionChangedEventArgs(team, ActivityGroup));
        }

        /// <summary>
        /// Handles the key down event for the table.
        /// </summary>
        /// <param name="e">The keyboard event arguments</param>
        private void HandleKeyDown(KeyboardEventArgs e)
        {
            _preventDefault = e.Key == "Enter" && !e.ShiftKey;
        }

        #endregion

        #region Helper Extensions

        /// <summary>
        /// Creates a filter for activity groups based on the specified table state.
        /// </summary>
        /// <param name="state">The current state of the table, including pagination and sorting information.</param>
        /// <returns>A <see cref="LearnerPageParameters"/> object configured with the activity group filter settings.</returns>
        private LearnerPageParameters CreateActivityGroupFilter(TableState state)
        {
            return new LearnerPageParameters
            {
                ActivityGroupId = ActivityGroup?.ActivityGroupId,
                PageSize = state.PageSize,
                PageNr = _searching ? 1 : state.Page + 1,
                OrderBy = state.GetSortOrder()
            };
        }

        #endregion

        #region Table: Paging & Loading

        /// <summary>
        /// Provides data to the MudTable via server-side paging. This method is called by MudTable to load data.
        /// </summary>
        /// <param name="state">The current table state (page, sort, etc.).</param>
        /// <param name="token">Cancellation token for async operations.</param>
        /// <returns>The table data containing the learners.</returns>
        private async Task<TableData<LearnerDto>> ServerReload(TableState state, CancellationToken token)
        {
            Loaded = false;

            if (ServerData is not null)
            {
                var tableData = await ServerData.Invoke(state, token);
                Loaded = true;
                return tableData;
            }

            if (ActivityGroup is not null && !MultiSelection)
            {
                if (EventStateManager.HasEvent())
                {
                    var participatingTeamMemberResult = await SchoolEventCategoryService.ParticipatingActivityGroupTeamMembers(EventStateManager.SchoolEvent!.EventId, ActivityGroup.ActivityGroupId);
                    var teamMembers = PaginatedResult<LearnerDto>.Success(SelectedPagedLearners.ToList(), SelectedPagedLearners.Count, state.Page + 1, state.PageSize);
                    Loaded = true;
                    return new TableData<LearnerDto> { TotalItems = teamMembers.TotalCount, Items = teamMembers.Data };
                }

                var args = CreateActivityGroupFilter(state);
                var teamMemberResult = PaginatedResult<LearnerDto>.Success(SelectedPagedLearners.ToList(), SelectedPagedLearners.Count, args.PageNr, args.PageSize);
                Loaded = true;
                return new TableData<LearnerDto> { TotalItems = teamMemberResult.TotalCount, Items = teamMemberResult.Data };
            }

            SetPageArgs(state);
            var request = await LearnerQueryService.PagedLearnersAsync(_args);

            if (ActivityGroup is not null && MultiSelection)
            {
                var tempSelected = SelectedPagedLearners.ToList();
                foreach (var item in request.Data.ToList().Where(c => tempSelected.Any(g => g.LearnerId == c.LearnerId)))
                {
                    SelectedPagedLearners = new HashSet<LearnerDto>();
                    SelectedPagedLearners.Add(item);
                }
            }

            Loaded = true;

            if (!request.Succeeded)
                return new TableData<LearnerDto> { TotalItems = 0, Items = new List<LearnerDto>() };

            return new TableData<LearnerDto> { TotalItems = request.TotalCount, Items = request.Data };
        }

        /// <summary>
        /// Adjusts <see cref="_args"/> to remove <c>ActivityGroupId</c> 
        /// and apply the gender/age constraints from <see cref="ActivityGroup"/> instead.
        /// Used when multi-selection is enabled so we can show a broader list of learners.
        /// </summary>
        private void SetLearnerListArgs()
        {
            _args.ActivityGroupId = null;
            _args.Gender = ActivityGroup?.Gender;
            _args.MinAge = ActivityGroup?.AgeGroup?.MinAge ?? 0;
            _args.MaxAge = ActivityGroup?.AgeGroup?.MaxAge ?? 100;
        }

        /// <summary>
        /// Updates the internal <see cref="_args"/> based on the table's current state.
        /// This includes sorting labels and directions, as well as the page size and index.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        private void SetPageArgs(TableState state)
        {
            _args.OrderBy = state.GetSortOrder();
            _args.PageSize = state.PageSize;
            _args.PageNr = _searching ? 1 : state.Page + 1;
        }

        /// <summary>
        /// Toggles multi-selection mode (linking multiple learners). 
        /// Updates the filters and re-synchronizes selected items after reload.
        /// </summary>
        private async void OnLinkTeamMembers()
        {
            MultiSelection = !MultiSelection;
            await MultiSelectionChanged.InvokeAsync(MultiSelection);

            if (!MultiSelection)
                _args.ActivityGroupId = ActivityGroup?.ActivityGroupId;

            SetLearnerListArgs();

            await _table.ReloadServerData();
            await ConfigureSelectedItems();
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Ensures that any learners previously selected remain selected if they appear 
        /// in the current table page after a data reload.
        /// </summary>
        private async Task ConfigureSelectedItems()
        {
            if (MultiSelection)
            {
                var tempSelected = SelectedPagedLearners.ToList();
                // Re-add them to ensure we use the exact instances from the newly loaded data
                foreach (var item in _table.FilteredItems.Where(c => tempSelected.Any(g => g.LearnerId == c.LearnerId)))
                {
                    SelectedPagedLearners.RemoveWhere(c => c.LearnerId == item.LearnerId);
                    SelectedPagedLearners.Add(item);
                }
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Creates a new learner. If <see cref="UseModal"/> is true, this opens a modal dialog; 
        /// otherwise, navigates to a separate page. After a successful creation, 
        /// triggers the <see cref="LearnerCreated"/> callback.
        /// </summary>
        private async Task CreateNewLearnerAsync()
        {
            if (!UseModal)
            {
                // Navigate to a "create learner" page
                NavigationManager.NavigateTo("/learners/create");
            }
            else
            {
                // Open a modal dialog for learner creation
                var dialog = await DialogService.ShowAsync<LearnerCreatorModal>("Create New Learner");
                var result = await dialog.Result;

                // If creation is confirmed in the dialog
                if (!result!.Canceled)
                {
                    // Cast the returned data to a LearnerViewModel
                    var learner = result.Data as LearnerViewModel;
                    // Map and send it to the server via a PUT call
                    var creationResult = await LearnerCommandService.CreateAsync(learner.ToDto());

                    // Display any errors or success messages
                    async void SuccessAction()
                    {
                        SnackBar.Add("Learner created successfully", Severity.Success);
                        // Fire the event callback so parent components can handle the new learner
                        await LearnerCreated.InvokeAsync(learner);
                        // Reload the table to show the newly created learner
                        await _table.ReloadServerData();
                    }

                    creationResult.ProcessResponseForDisplay(SnackBar, SuccessAction);
                }
            }
        }

        /// <summary>
        /// Reloads the server data for the table.
        /// </summary>
        public async Task ReloadServerData()
        {
            await _table.ReloadServerData();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Lifecycle method triggered once the component is initialized.
        /// Checks user permissions (e.g., can create learners?), 
        /// sets up filters in <see cref="_args"/>, and if <see cref="ActivityGroup"/> is present, 
        /// fetches preselected learners if an event is also active.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            EventStateManager.OnNotificationsChanged += async () => { await _table.ReloadServerData(); };
            Loaded = false;

            var authState = await AuthenticationStateTask;

            _args.SchoolClassId = SchoolClassId;
            _args.MinAge = AgeGroup?.MinAge ?? 0;
            _args.MaxAge = AgeGroup?.MaxAge ?? 100;

            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ParentPermissions.Create)).Succeeded;
            _canAddParent = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.LearnerPermissions.AddParent)).Succeeded;

            if (ActivityGroup is not null)
            {
                if (EventStateManager.HasEvent())
                {
                    var args = new LearnerPageParameters() { ActivityGroupId = ActivityGroup.ActivityGroupId };
                    var teamMemberResult = await SchoolEventCategoryService.ParticipatingActivityGroupTeamMembers(EventStateManager.SchoolEvent.EventId, ActivityGroup.ActivityGroupId);

                    if (teamMemberResult.Succeeded)
                    {
                        SelectedPagedLearners = teamMemberResult.Data.ToHashSet();
                        await ReloadServerData();
                    }
                }
                else
                {
                    var args = new LearnerPageParameters() { ActivityGroupId = ActivityGroup.ActivityGroupId };
                    var teamMemberResult = await ActivityGroupQueryService.ActivityGroupTeamMembersAsync(args);

                    if (teamMemberResult.Succeeded)
                    {
                        SelectedPagedLearners = teamMemberResult.Data.ToHashSet();
                        await ReloadServerData();
                    }
                }
            }

            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Lifecycle method triggered after the component has rendered.
        /// </summary>
        /// <param name="firstRender">Flag to indicate if this is the first time the page renders</param>
        /// <returns></returns>
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                Loaded = true;
            }
        }

        #endregion

    }
}
