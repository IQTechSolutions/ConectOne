using GroupingModule.Application.ViewModels;
using GroupingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using SchoolsModule.Domain.Entities;

namespace SchoolsEnterprise.Blazor.Shared.Components
{
    /// <summary>
    /// Represents a dynamic menu component for activity groups.
    /// This component fetches and displays categories that are marked to be shown in the main menu.
    /// </summary>
    public partial class ActivityGroupsDynamicMenu
    {
        private ICollection<CategoryViewModel> _categories = new List<CategoryViewModel>();

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        [Inject] public ICategoryService<ActivityGroup> Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be manually set
        /// in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;        

        /// <summary>
        /// Navigates to the category update page.
        /// </summary>
        /// <param name="categoryId">The ID of the category to update.</param>
        private void NavigateToCategory(string categoryId)
        {
            NavigationManager.NavigateTo($"/activities/categories/update/{categoryId}", true);
        }

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Fetches the categories from the server and filters them to include only those that should be displayed in the main menu.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Fetch the categories from the server
            var result = await Provider.CategoriesAsync();
            if (result.Succeeded)
            {
                // Filter and map the categories to the view model
                _categories = result.Data
                    .Where(c => c.DisplayCategoryInMainMenu)
                    .Select(c => new CategoryViewModel(c))
                    .ToList();
            }

            await base.OnInitializedAsync();
        }
    }
}
