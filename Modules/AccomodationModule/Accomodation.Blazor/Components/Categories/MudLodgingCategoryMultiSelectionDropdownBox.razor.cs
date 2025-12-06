using AccomodationModule.Application.ViewModels;
using AccomodationModule.Domain.Interfaces;
using GroupingModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Accomodation.Blazor.Components.Categories
{
    /// <summary>
    /// Represents a dropdown box for selecting multiple lodging categories.
    /// </summary>
    /// <remarks>This component allows users to select multiple lodging categories from a predefined list. The
    /// list of categories is fetched asynchronously during initialization. The selected categories are synchronized
    /// with the <see cref="Lodging"/> model.</remarks>
    public partial class MudLodgingCategoryMultiSelectionDropdownBox
    {
        private bool _loaded = false;
        private CategoryViewModel _category;
        private IEnumerable<CategoryViewModel> _categories;

        /// <summary>
        /// Gets or sets the lodging information for the current view.
        /// </summary>
        [Parameter] public LodgingViewModel Lodging { get; set; }

        /// <summary>
        /// Gets or sets the variant style of the component.
        /// </summary>
        [Parameter] public Variant Variant { get; set; } = Variant.Text;

        [Inject] public ILodgingCategoryService LodgingCategoryService { get; set; }

        /// <summary>
        /// Converts a <see cref="CategoryViewModel"/> instance to its corresponding string representation.
        /// </summary>
        /// <remarks>This delegate is used to extract the <see cref="CategoryViewModel.Name"/> property
        /// from a given <see cref="CategoryViewModel"/> instance. If the input is <see langword="null"/>, the result
        /// will also be <see langword="null"/>.</remarks>
        private Func<CategoryViewModel, string> categoryConverter = p => p?.Name;

        /// <summary>
        /// Asynchronously initializes the component and loads category data.
        /// </summary>
        /// <remarks>This method retrieves parent category data from the provider and maps it to the local
        /// category view models. It also synchronizes the lodging's selected categories with the loaded categories. The
        /// method sets the component's loaded state to <see langword="true"/> after initialization.</remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            //var categoryResult = await LodgingCategoryService.ca() Provider.GetAsync<IEnumerable<CategoryDto>>("lodgings/categories/parents");
            //_categories = categoryResult.Data.Select(c => new CategoryViewModel(c)).ToList();

            //var selectionResult = Lodging.Categories;
            //var selectionList = new List<CategoryViewModel>();

            //if (selectionResult is not null)
            //{
            //    foreach (var category in selectionResult)
            //    {
            //        if (_categories.Any(c => c.CategoryId == category.CategoryId))
            //        {
            //            selectionList.Add(_categories.FirstOrDefault(c => c.CategoryId == category.CategoryId));
            //        }
            //    }
            //}

            //Lodging.Categories = selectionList;

            await base.OnInitializedAsync();
            _loaded = true;
        }
    }
}
