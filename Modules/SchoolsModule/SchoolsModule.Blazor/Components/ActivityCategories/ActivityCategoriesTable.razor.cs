
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.Interfaces;
using GroupingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Blazor.StateManagers;
using SchoolsModule.Domain.Constants;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Blazor.Components.ActivityCategories
{
    /// <summary>
    /// Represents a table component for displaying and managing activity categories.
    /// </summary>
    public partial class ActivityCategoriesTable
    {
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private bool _canSearch;
        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;
        private bool _canAddActivityGroup;
        private bool _canCreateEvent;
        private bool _canSendMessage;

        private CategoryDto _parentCategory = null!;
        private IEnumerable<CategoryDto> _categories = null!;
        private MudTable<CategoryDto> _table = null!;
        private readonly CategoryPageParameters _args = new();

        #region Injections & Parameters

        /// <summary>
        /// The cascading authentication state, used to determine 
        /// the current user's permissions (e.g., create/edit/delete learners).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

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
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ICategoryService<ActivityGroup> ActivityGroupCategoryService { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// URL for creating a new category.
        /// </summary>
        [Parameter] public string CreateUrl { get; set; } = "/products/categories/create";

        /// <summary>
        /// The ID of the parent category, if any.
        /// </summary>
        [Parameter] public string? ParentId { get; set; }

        /// <summary>
        /// Render fragment for additional toolbar buttons.
        /// </summary>
        [Parameter] public RenderFragment ToolbarButtons { get; set; } = null!;

        /// <summary>
        /// Indicates whether multi-selection is enabled.
        /// </summary>
        [Parameter] public bool MultiSelection { get; set; } = false;

        /// <summary>
        /// Indicates whether to show the link categories button.
        /// </summary>
        [Parameter] public bool ShowLinkCategories { get; set; } = true;

        /// <summary>
        /// The set of selected paged categories.
        /// </summary>
        [Parameter] public HashSet<CategoryDto> SelectedPagedCategories { get; set; } = new();

        /// <summary>
        /// Event callback for when the category selection changes.
        /// </summary>
        [Parameter] public EventCallback<HashSet<CategoryDto>> CategorySelectionChanged { get; set; }

        /// <summary>
        /// Function to fetch server data for the table.
        /// </summary>
        [Parameter] public Func<TableState, CancellationToken, Task<TableData<CategoryDto>>>? ServerData { get; set; }

        /// <summary>
        /// Event callback for when the multi-selection state changes.
        /// </summary>
        [Parameter] public EventCallback<bool> MultiSelectionChanged { get; set; }

        #endregion
        
        #region Methods

        /// <summary>
        /// Handles the event when the selected categories change.
        /// </summary>
        /// <param name="categories">The new set of selected categories.</param>
        private async Task OnSelectedCategoryChanged(HashSet<CategoryDto> categories)
        {
            await EventStateManager.SelectedCategoriesChanged(categories);
            await CategorySelectionChanged.InvokeAsync(categories);
        }

        /// <summary>
        /// Deletes a category by its ID after user confirmation.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete.</param>
        private async Task DeleteCategory(string categoryId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this activity category from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deleteResult = await ActivityGroupCategoryService.DeleteCategoryAsync(categoryId);

                async void SuccessAction()
                {
                    await _table.ReloadServerData();
                }

                deleteResult.ProcessResponseForDisplay(SnackBar, SuccessAction);
            }
        }

        /// <summary>
        /// Searches for categories based on the provided text.
        /// </summary>
        /// <param name="text">The search text.</param>
        private async Task OnSearch(string text)
        {
            _args.SearchText = text;
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Navigates to the category update page.
        /// </summary>
        /// <param name="categoryId">The ID of the category to update.</param>
        private void NavigateToCategory(string categoryId)
        {
            NavigationManager.NavigateTo($"/activities/categories/update/{categoryId}", true);
        }

        #endregion

        #region Table: Paging & Loading

        /// <summary>
        /// Sets the page parameters based on the current table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        private void SetPageParameters(TableState state)
        {
            _args.PageNr = state.Page + 1;
            _args.PageSize = state.PageSize;
            _args.ParentId = ParentId;

            _args.OrderBy = state.SortDirection switch
            {
                SortDirection.Ascending => $"{state.SortLabel} desc",
                SortDirection.Descending => $"{state.SortLabel} asc",
                _ => _args.OrderBy
            };
        }

        /// <summary>
        /// Fetches the categories asynchronously based on the current table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the table data.</returns>
        public async Task<TableData<CategoryDto>> GetCategoriesAsync(TableState state, CancellationToken token)
        {
            if (ServerData is not null)
            {
                var bb = await ServerData.Invoke(state, token);

                foreach (var item in bb.Items.Where(c => EventStateManager.SchoolEvent!.SelectedActivityCategories.Any(g => g.CategoryId == c.CategoryId)))
                {
                    EventStateManager.SchoolEvent!.SelectedActivityCategories.Remove(EventStateManager.SchoolEvent!.SelectedActivityCategories.FirstOrDefault(c => c.CategoryId == item.CategoryId)!);
                    EventStateManager.SchoolEvent!.SelectedActivityCategories.Add(item);
                }
                SelectedPagedCategories = EventStateManager.SchoolEvent!.SelectedActivityCategories.ToHashSet();
                return bb;
            }


            SetPageParameters(state);

            var pagingResponse = await ActivityGroupCategoryService.PagedCategoriesAsync(_args);

            if (!pagingResponse.Succeeded)
            {
                SnackBar.AddErrors(pagingResponse.Messages);
            }

            _categories = pagingResponse.Data;

            if (EventStateManager.HasEvent())
            {
                foreach (var item in _categories.Where(c => EventStateManager.SchoolEvent!.SelectedActivityCategories.Any(g => g.CategoryId == c.CategoryId)))
                {
                    EventStateManager.SchoolEvent!.SelectedActivityCategories.Remove(EventStateManager.SchoolEvent!.SelectedActivityCategories.FirstOrDefault(c => c.CategoryId == item.CategoryId)!);
                    EventStateManager.SchoolEvent!.SelectedActivityCategories.Add(item);
                }
                SelectedPagedCategories = EventStateManager.SchoolEvent!.SelectedActivityCategories.ToHashSet();
            }

            return new TableData<CategoryDto>() { TotalItems = pagingResponse.TotalCount, Items = _categories };
        }

        /// <summary>
        /// Toggles the multi-selection mode and reloads the server data.
        /// </summary>
        private async void OnLinkCategories()
        {
            MultiSelection = !MultiSelection;
            await MultiSelectionChanged.InvokeAsync(MultiSelection);

            await _table.ReloadServerData();
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Asynchronously reloads data from the server, replacing the current data set with the latest available from
        /// the server source.
        /// </summary>
        /// <returns>A task that represents the asynchronous reload operation.</returns>
        public async Task ReloadServerData()
        {
            await _table.ReloadServerData();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Initializes the component asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _args.OrderBy = "Name asc";

            _canSearch = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityCategoryPermissions.Search)).Succeeded;
            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityCategoryPermissions.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityCategoryPermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityCategoryPermissions.Delete)).Succeeded;
            _canAddActivityGroup = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.ActivityPermissions.LinkGroups)).Succeeded;
            _canCreateEvent = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.EventPermissions.Create)).Succeeded;
            _canSendMessage = (await AuthorizationService.AuthorizeAsync(authState.User, IdentityModule.Domain.Constants.Permissions.Users.CanSendMessage)).Succeeded;

            if (!string.IsNullOrEmpty(ParentId))
            {
                _args.ParentId = ParentId;
                var parentCategoryResult = await ActivityGroupCategoryService.CategoryAsync(ParentId);
                if (parentCategoryResult.Succeeded)
                {
                    _parentCategory = parentCategoryResult.Data;
                }
            }

            if (EventStateManager.HasEvent())
                SelectedPagedCategories = EventStateManager.SchoolEvent!.SelectedActivityCategories.ToHashSet();

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
