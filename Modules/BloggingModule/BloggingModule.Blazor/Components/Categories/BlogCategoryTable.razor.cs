using BloggingModule.Domain.Constants;
using BloggingModule.Domain.Entities;
using ConectOne.Blazor.Modals;
using GroupingModule.Application.ViewModels;
using GroupingModule.Domain.Interfaces;
using GroupingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BloggingModule.Blazor.Components.Categories
{
    public partial class BlogCategoryTable
    {
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;
        private bool _canCreate = false;
        private bool _canEdit = false;
        private bool _canDelete = false;

        /// <summary>
        /// Reference to the MudTable component for categories.
        /// </summary>
        private MudTable<CategoryViewModel> table;

        /// <summary>
        /// Parameters for paging and filtering categories.
        /// </summary>
        private CategoryPageParameters pageParameters = new CategoryPageParameters();

        /// <summary>
        /// The ID of the parent category, if any.
        /// </summary>
        [Parameter] public string ParentId { get; set; }

        /// <summary>
        /// The URL part for the parent domain.
        /// </summary>
        [Parameter, EditorRequired] public string ParentDomainUrlPart { get; set; } = "blogging";

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ICategoryService<BlogPost> CategoryService { get; set; }

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// The cascading authentication state, used to determine 
        /// the current user's permissions (e.g., create/edit/delete learners).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Fetches the categories from the server based on the current table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A TableData object containing the total items and the items to display.</returns>
        public async Task<TableData<CategoryViewModel>> GetCategoriesAsync(TableState state, CancellationToken token)
        {
            pageParameters.PageNr = state.Page + 1;
            pageParameters.PageSize = state.PageSize;

            if (state.SortDirection == SortDirection.Ascending)
                pageParameters.OrderBy = $"{state.SortLabel} desc";
            else if (state.SortDirection == SortDirection.Descending)
                pageParameters.OrderBy = $"{state.SortLabel} asc";
            else pageParameters.OrderBy = null;

            var pagingResponse = await CategoryService.PagedCategoriesAsync(pageParameters);

            return new TableData<CategoryViewModel>() { TotalItems = pagingResponse.Data.Count(), Items = pagingResponse.Data.Select(c => new CategoryViewModel(c)) };
        }

        /// <summary>
        /// Deletes a specific category after user confirmation.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete.</param>
        private async Task DeleteCategory(string categoryId)
        {
            var parameters = new DialogParameters<ConformtaionModal>();
            parameters.Add(x => x.ContentText, "Are you sure you want to remove this product from this application?");
            parameters.Add(x => x.ButtonText, "Yes");
            parameters.Add(x => x.Color, Color.Success);

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var removalResult = await CategoryService.DeleteCategoryAsync(categoryId);
                if (removalResult.Succeeded)
                    await table.ReloadServerData();
            }
        }
        

        #region LifeCycle Methods

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
                pageParameters.ParentId = ParentId;

                if (table is not null)
                    await table.ReloadServerData();

                StateHasChanged();
            }
        }

        /// <summary>
        /// Asynchronously initializes the component and determines the user's authorization permissions for creating,
        /// editing, and deleting blog posts.
        /// </summary>
        /// <remarks>This method evaluates the user's authorization state and sets internal flags
        /// indicating whether the user has permissions to create, edit, or delete blog posts.  It also invokes the base
        /// class's initialization logic.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BlogCategoryPermissions.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BlogCategoryPermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BlogCategoryPermissions.Delete)).Succeeded;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
