using ConectOne.Domain.ResultWrappers;
using GroupingModule.Application.ViewModels;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces.ActivityGroups;

namespace SchoolsModule.Blazor.Modals
{
    /// <summary>
    /// The AddEventToActivityCategoryModal component is responsible for selecting a category
    /// to which an event will be added. It provides a list of categories and handles the selection process.
    /// </summary>
    public partial class AddEventToActivityCategoryModal
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IActivityGroupCategoryService ActivityGroupCategoryService { get; set; } = null!;

        /// <summary>
        /// The parent category for which the event is being added.
        /// </summary>
        [Parameter] public CategoryViewModel? ParentCategory { get; set; }

        /// <summary>
        /// The MudDialog instance for managing the dialog state.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Indicates whether the component has completed loading and can render content.
        /// </summary>
        private bool _loaded = false;

        /// <summary>
        /// The selected category to which the event will be added.
        /// </summary>
        private CategoryViewModel? _selectedCategory = null!;

        /// <summary>
        /// Collection of category view models.
        /// </summary>
        private IEnumerable<CategoryViewModel> _categories = [];

        /// <summary>
        /// Converter function for displaying category names.
        /// </summary>
        Func<CategoryViewModel, string> categoryConverter = p => p?.Name;

        /// <summary>
        /// Submits the selected category and closes the dialog.
        /// </summary>
        public void SubmitAsync()
        {
            MudDialog.Close(_selectedCategory);
        }

        /// <summary>
        /// Cancels the selection process and closes the dialog.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        #region Overrides

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Loads the categories from the server.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            IBaseResult<IEnumerable<CategoryDto>> categoriesResult = null!;

            if (ParentCategory is not null)
                categoriesResult = await ActivityGroupCategoryService.CategoriesAsync(ParentCategory.CategoryId);
            else
                categoriesResult = await ActivityGroupCategoryService.CategoriesAsync();

            if (categoriesResult.Succeeded)
            {
                _categories = categoriesResult.Data.Select(c => new CategoryViewModel(c));
                _selectedCategory = _categories.FirstOrDefault();
            }

            _loaded = true;
        }

        #endregion
    }
}
