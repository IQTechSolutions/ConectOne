using ConectOne.Blazor.Components;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.ResultWrappers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using SchoolsModule.Blazor.Components.Learners.Tables;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Parents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Components.Parents
{
    /// <summary>
    /// The ParentsTableV2 component is responsible for displaying and managing a table of parents.
    /// It provides functionality to delete parents, link parents to learners, and handle pagination and search.
    /// </summary>
    public partial class ParentsTableV2
    {
        #region Injected Services

        /// <summary>
        /// Injected HTTP provider for making API calls.
        /// </summary>
        [Inject] public IParentQueryService ParentQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage parent commands within the application.
        /// </summary>
        [Inject] public IParentCommandService ParentCommandService { get; set; } = null!;
        
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
        /// Indicates whether to show the "Link Parents" button.
        /// </summary>
        [Parameter] public bool ShowLinkParentsButton { get; set; } = true;

        /// <summary>
        /// The ID of the learner to which parents can be linked.
        /// </summary>
        [Parameter] public string? LearnerId { get; set; }

        /// <summary>
        /// List of selected parents.
        /// </summary>
        [Parameter] public List<ParentDto>? SelectedParents { get; set; } = new();

        /// <summary>
        /// Event callback for handling selection changes.
        /// </summary>
        [Parameter] public EventCallback<CustomCheckValueChangedEventArgs<ParentDto>> OnSelectionChange { get; set; }

        /// <summary>
        /// Indicates whether to show the "Link Learners" button.
        /// </summary>
        [Parameter] public bool ShowLinkLearnersButton { get; set; } = true;

        /// <summary>
        /// Indicates whether multiple selection is enabled.
        /// </summary>
        [Parameter] public bool MultiSelection { get; set; } = false;

        #endregion

        #region Fields and Properties

        /// <summary>
        /// Parameters for paging and filtering parents.
        /// </summary>
        private readonly ParentPageParameters _args = new();

        /// <summary>
        /// List of parents displayed in the table.
        /// </summary>
        private List<TableSelectionItem<ParentDto>> _parents = new();

        /// <summary>
        /// Indicates whether to prevent the default action for certain key events.
        /// </summary>
        private bool _preventDefault;

        /// <summary>
        /// The total number of items in the current page.
        /// </summary>
        private int _pageItemCount;

        /// <summary>
        /// The total number of pages.
        /// </summary>
        private int _pageCount;

        private bool _allSelected;

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a parent by ID after user confirmation.
        /// </summary>
        /// <param name="parentId">The ID of the parent to delete.</param>
        private async Task DeleteParent(string parentId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this parent from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deleteResult = await ParentCommandService.RemoveAsync(parentId);
                if (deleteResult.Succeeded)
                {
                    await LoadData();
                }
                else
                {
                    SnackBar.AddErrors(deleteResult.Messages);
                }
            }
        }

        /// <summary>
        /// Loads the data for the parents table.
        /// </summary>
        private async Task LoadData()
        {
            if (LearnerId == null || MultiSelection)
            {
                var result = await ParentQueryService.PagedParentsAsync(_args);
                if (result.Succeeded)
                {
                    _pageCount = result.TotalPages;
                    _pageItemCount = result.TotalCount;
                    _parents = result.Data.Select(b => new TableSelectionItem<ParentDto>(SelectedParents.Any(c => c.ParentId == b.ParentId), b)).ToList();
                }
            }
            else
            {
                var dneLearners = PaginatedResult<ParentDto>.Success(SelectedParents.ToList(), SelectedParents.Count, _args.PageNr, _args.PageSize);
                _pageCount = dneLearners.TotalPages;
                _pageItemCount = dneLearners.TotalCount;
                _parents = dneLearners.Data.Select(b => new TableSelectionItem<ParentDto>(SelectedParents.Any(c => c.ParentId == b.ParentId), b)).ToList();
            }

            StateHasChanged();
        }

        /// <summary>
        /// Handles the event when the value selection is changed for parents.
        /// </summary>
        /// <param name="args">The event arguments containing the selected parent.</param>
        private async Task OnValueSelectionChanged(CustomCheckValueChangedEventArgs<ParentDto> args)
        {
            if (args.IsChecked)
            {
                SelectedParents.Add(args.Item);
                _parents.FirstOrDefault(c => c.RowItem.ParentId == args.Item.ParentId).IsChecked = true;
            }
            else
            {
                SelectedParents.Remove(SelectedParents.FirstOrDefault(c => c.ParentId == args.Item.ParentId));
                _parents.FirstOrDefault(c => c.RowItem.ParentId == args.Item.ParentId).IsChecked = false;
            }

            await OnSelectionChange.InvokeAsync(args);
            await InvokeAsync(StateHasChanged);
        }

        public async Task OnAllSelectedChanged(bool value)
        {
            _allSelected = value;
            var parentResult = await ParentQueryService.AllParentsAsync();
            if (parentResult.Succeeded)
            {
                if (value)
                {
                    foreach (var parent in parentResult.Data.Where(c => SelectedParents.All(g => g.ParentId != c.ParentId)))
                    {
                        SelectedParents = parentResult.Data.ToList();
                        foreach (var item in _parents)
                        {
                            item.IsChecked = true;
                        }
                    }
                }
                else
                {
                    SelectedParents.Clear();
                    foreach (var item in _parents)
                    {
                        item.IsChecked = false;
                    }
                }
            }
            StateHasChanged();
        }

        /// <summary>
        /// Reloads the data for the parents table.
        /// </summary>
        private async Task Reload()
        {
            _args.PageNr = 1;
            _args.SearchText = "";
            await LoadData();
        }

        /// <summary>
        /// Handles the search event and updates the data based on the search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        private async Task OnSearch(string searchText)
        {
            _args.SearchText = searchText;
            await LoadData();
        }

        /// <summary>
        /// Toggles the multi-selection mode and reloads the data.
        /// </summary>
        private async Task OnLinkParents()
        {
            MultiSelection = !MultiSelection;
            await LoadData();
        }

        /// <summary>
        /// Handles the page change event and updates the data based on the new page number.
        /// </summary>
        /// <param name="pageNr">The new page number.</param>
        private async Task OnPageChanged(int pageNr)
        {
            _args.PageNr = pageNr;
            await LoadData();
        }

        /// <summary>
        /// Handles key down events to prevent default actions for certain keys.
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

        #endregion

        #region Overrides

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await LoadData();
                var parentResult = await ParentQueryService.AllParentsAsync();
                if (parentResult.Succeeded)
                {
                    if (SelectedParents is not null && SelectedParents.Any())
                        _allSelected = SelectedParents.SequenceEqual(parentResult.Data);
                }
                
                StateHasChanged();
            }
        }

        #endregion
    }
}
