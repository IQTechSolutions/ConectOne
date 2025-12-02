using ConectOne.Blazor.Extensions;
using FilingModule.Blazor.Managers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Blazor.Extensions;
using SchoolsModule.Domain.Constants;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Components.Parents
{
    /// <summary>
    /// Represents a Blazor component for managing a table of parents with optional learner context.
    /// This component allows CRUD-like interactions including displaying, selecting, searching, exporting,
    /// and removing parents. It adapts based on whether it's used within a learner's context or standalone.
    /// </summary>
    public partial class ParentsTable
    {
        private bool _searching, _dense, _bordered, _loaded;
        private bool _striped = true;
        private bool _canCreate;
        private bool _canExport;
        private bool _canAddLearners;
        private int _totalItems;
        private List<ParentDto> _parents = new();
        private HashSet<ParentDto> _selectedParents = new();
        private MudTable<ParentDto> _table = null!;
        private readonly ParentPageParameters _args = new();

        #region Dependencies

        /// <summary>
        /// Provides the current user's authentication state, used to evaluate access permissions.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Interface for performing backend HTTP requests to fetch and manipulate parent data.
        /// </summary>
        [Inject] public IParentService ParentService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to query learner information.
        /// </summary>
        [Inject] public ILearnerQueryService LearnerQueryService { get; set; } = null!;

        /// <summary>
        /// Service used to trigger file downloads in the browser.
        /// </summary>
        [Inject] public IBlazorDownloadFileManager BlazorDownloadFileManager { get; set; } = null!;

        /// <summary>
        /// Displays confirmation dialogs and modals to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Snackbar for showing user notifications, including errors and confirmations.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Facilitates page navigation and URI manipulation.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// Optional identifier for a learner; filters parents to only those associated with this learner.
        /// </summary>
        [Parameter] public string? LearnerId { get; set; }

        /// <summary>
        /// If true, enables multi-selection mode allowing multiple parents to be selected at once.
        /// </summary>
        [Parameter] public bool MultiSelection { get; set; }

        /// <summary>
        /// Event callback triggered when the selection of parents changes.
        /// </summary>
        [Parameter] public EventCallback<HashSet<ParentDto>> ParentSelectionChanged { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Determines whether the "Link Parents" button should be visible based on LearnerId presence.
        /// </summary>
        private bool ShowLinkParentsButton => !string.IsNullOrWhiteSpace(LearnerId);

        #endregion

        #region Methods

        /// <summary>
        /// Prompts the user with a confirmation dialog and deletes a parent if confirmed.
        /// </summary>
        /// <param name="parentId">The ID of the parent to delete.</param>
        internal async Task DeleteParent(string parentId)
        {
            if (await DialogService.ConfirmAction("Are you sure you want to remove this parent from this application?"))
            {
                var deleteResult = await ParentService.RemoveAsync(parentId);
                if (deleteResult.Succeeded)
                    await ReloadTableAsync();
                else
                    SnackBar.AddErrors(deleteResult.Messages);
            }
        }

        /// <summary>
        /// Toggles between single and multi-selection modes and reloads the table data.
        /// </summary>
        internal async Task ToggleMultiSelectionModeAsync()
        {
            MultiSelection = !MultiSelection;
            await ReloadTableAsync();
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Triggers a search operation by reloading the MudTable.
        /// </summary>
        /// <param name="text">The user-provided search term.</param>
        private async Task OnSearch(string? text)
        {
            _searching = true;
            _args.SearchText = text;
            await ReloadTableAsync();
            _searching = false;
        }

        /// <summary>
        /// Fires an event when the selected parents set changes.
        /// </summary>
        /// <param name="team">The currently selected parents.</param>
        private async Task OnParentSelectionChanged(HashSet<ParentDto> team)
        {
            await ParentSelectionChanged.InvokeAsync(team);
        }

        /// <summary>
        /// Exports the current parent list as an Excel file and triggers a browser download.
        /// </summary>
        internal async Task ExportToExcel()
        {
            var response = await ParentService.ExportParents();
            if (!response.Succeeded && response.Messages != null)
                SnackBar.AddErrors(response.Messages);

            await BlazorDownloadFileManager.DownloadFile($"Parents_{DateTime.Now:ddMMyyyyHHmmss}.xlsx", Convert.FromBase64String(response.Data),
                "application/octet-stream");
            SnackBar.Add("School Events Exported", Severity.Success);
        }

        /// <summary>
        /// Loads the parent data from the server based on the current table state.
        /// </summary>
        /// <param name="state">The table state used for sorting and pagination.</param>
        /// <param name="token">Cancellation token for the async operation.</param>
        private async Task<TableData<ParentDto>> ServerReload(TableState state, CancellationToken token)
        {
            SetPageArgs(state);
            try
            {
                await LoadData(state);
                await ConfigureSelectedItems();
            }
            catch (Exception ex)
            {
                SnackBar.AddError(ex.Message);
            }
            return new TableData<ParentDto> { TotalItems = _totalItems, Items = _parents };
        }

        /// <summary>
        /// Fetches parent data either globally or scoped to a specific learner.
        /// </summary>
        private async Task LoadData(TableState state)
        {
            if (LearnerId == null)
            {
                var request = await ParentService.PagedParentsAsync(_args);
                if (request.Succeeded)
                {
                    _totalItems = request.TotalCount;
                    _parents = request.Data;
                    SyncSelectedParents();
                }
                if (request.Messages != null)
                    SnackBar.AddErrors(request.Messages);
            }
            else
            {
                var request = await LearnerQueryService.LearnerParentsAsync(LearnerId);
                if (request.Succeeded)
                {
                    _totalItems = request.Data.Count();
                    _parents = request.Data.ToList();
                    SyncSelectedParents();
                }
                if (request.Messages != null)
                    SnackBar.AddErrors(request.Messages);
            }
        }

        /// <summary>
        /// Reloads the data for the table asynchronously.
        /// </summary>
        /// <remarks>This method refreshes the table's data by reloading it from the server.  If the table
        /// is null, the method completes immediately without performing any action.</remarks>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.  If the table is null, the returned task is
        /// already completed.</returns>
        protected virtual Task ReloadTableAsync()
        {
            return _table?.ReloadServerData() ?? Task.CompletedTask;
        }

        /// <summary>
        /// Keeps selected parents in sync with the currently loaded data.
        /// </summary>
        private void SyncSelectedParents()
        {
            if (!MultiSelection && _selectedParents.Any())
            {
                var selectedIds = _selectedParents.Select(c => c.ParentId).ToHashSet();
                _parents = _parents.Where(c => selectedIds.Contains(c.ParentId)).ToList();
            }
        }

        /// <summary>
        /// Updates the selection set to reflect changes during data reloads in multi-selection mode.
        /// </summary>
        private async Task ConfigureSelectedItems()
        {
            if (MultiSelection)
            {
                var result = await LearnerQueryService.LearnerParentsAsync(LearnerId);
                if (result.Succeeded && result.Data.Any())
                {
                    var tempSelectedParents = _selectedParents.ToList();
                    foreach (var item in _parents.Where(c => tempSelectedParents.Any(g => g.ParentId == c.ParentId)))
                    {
                        _selectedParents.RemoveWhere(c => c.ParentId == item.ParentId);
                        _selectedParents.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Maps table state (sort and pagination) to server query parameters.
        /// </summary>
        private void SetPageArgs(TableState state)
        {
            _args.OrderBy = state.GetSortOrder();
            _args.PageSize = state.PageSize;
            _args.PageNr = state.Page + 1;
            _args.LearnerId = MultiSelection ? null : LearnerId;
        }

        /// <summary>
        /// Lifecycle method for initializing the component, setting permissions, and preloading data.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            if (!string.IsNullOrWhiteSpace(LearnerId))
            {
                _args.LearnerId = LearnerId;
                
                var result = await LearnerQueryService.LearnerParentsAsync(LearnerId);
                if (result.Succeeded)
                {
                    foreach (var item in result.Data!)
                    {
                        if (_selectedParents.All(c => c.ParentId != item.ParentId))
                            _selectedParents.Add(item);
                    }
                }
            }

            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ParentPermissions.Create)).Succeeded;
            _canAddLearners = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ParentPermissions.AddLeaner)).Succeeded;
            _canExport = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ParentPermissions.Export)).Succeeded;
            _loaded = true;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
