using AccomodationModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using GroupingModule.Application.ViewModels;
using GroupingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Components.Categories
{
    /// <summary>
    /// Represents a table component for displaying and managing lodging categories.
    /// </summary>
    /// <remarks>This component provides functionality for displaying lodging categories in a paginated table
    /// with sorting and filtering capabilities. It interacts with the <see cref="ILodgingCategoryService"/> to fetch
    /// and manage category data.</remarks>
    public partial class MudLodgingCategoryTable
    {
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;
        private bool showTable = false;

        private MudTable<CategoryViewModel> table;
        private CategoryPageParameters pageParameters = new CategoryPageParameters();

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        [Parameter] public string ParentId { get; set; }

        /// <summary>
        /// Gets or sets the service responsible for managing lodging category operations.
        /// </summary>
        /// <remarks>This property is automatically injected and should be configured in the dependency
        /// injection container.</remarks>
        [Inject] public ILodgingCategoryService LodgingCategoryService { get; set; } = null!;

        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Retrieves a paginated and optionally sorted list of categories based on the specified table state.
        /// </summary>
        /// <remarks>The method applies the pagination and sorting parameters specified in the <paramref
        /// name="state"/> object to retrieve the appropriate subset of categories. Sorting is determined by the <see
        /// cref="TableState.SortDirection"/>  and <see cref="TableState.SortLabel"/> properties.</remarks>
        /// <param name="state">The state of the table, including pagination and sorting information.</param>
        /// <param name="token">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A <see cref="TableData{T}"/> containing the total number of items and a collection of <see
        /// cref="CategoryViewModel"/> objects representing the categories.</returns>
        public async Task<TableData<CategoryViewModel>> GetCategoriesAsync(TableState state, CancellationToken token)
        {
            pageParameters.PageNr = state.Page+1;
            pageParameters.PageSize = state.PageSize;

            if (state.SortDirection == SortDirection.Ascending)
                pageParameters.OrderBy = $"{state.SortLabel} desc";
            else if (state.SortDirection == SortDirection.Descending)
                pageParameters.OrderBy = $"{state.SortLabel} asc";
            else pageParameters.OrderBy = null;

            var pagingResponse = await LodgingCategoryService.PagedCategoriesAsync(pageParameters);

            return new TableData<CategoryViewModel>() { TotalItems = pagingResponse.Data.Count(), Items = pagingResponse.Data.Select(c => new CategoryViewModel(c)) };
        }

        /// <summary>
        /// Deletes a category after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// deletion.  If the user confirms, the category is deleted using the <see cref="LodgingCategoryService"/>.
        /// Upon successful deletion, the associated data table is reloaded to reflect the changes.</remarks>
        /// <param name="categoryId">The unique identifier of the category to be deleted. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task DeleteCategory(string categoryId)
        {
            var parameters = new DialogParameters<ConformtaionModal>();
            parameters.Add(x => x.ContentText, "Are you sure you want to remove this product from this application?");
            parameters.Add(x => x.ButtonText, "Yes");
            parameters.Add(x => x.Color, Color.Success);

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var deletionResult = await LodgingCategoryService.DeleteCategoryAsync(categoryId);
                deletionResult.ProcessResponseForDisplay(Snackbar, async () =>
                {
                    await table.ReloadServerData();
                });
                
            }
        }

        /// <summary>
        /// Asynchronously initializes the component and sets up the required parameters.
        /// </summary>
        /// <remarks>This method sets the <see cref="pageParameters.ParentId"/> property to the value of
        /// <see cref="ParentId"/> before invoking the base implementation. Override this method to perform additional
        /// initialization logic.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override Task OnInitializedAsync()
        {
            pageParameters.ParentId = ParentId;

            return base.OnInitializedAsync();
        }
    }
}
