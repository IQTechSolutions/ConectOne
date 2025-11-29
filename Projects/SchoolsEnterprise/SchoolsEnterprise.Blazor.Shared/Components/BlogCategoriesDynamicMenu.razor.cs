using BloggingModule.Domain.Entities;
using GroupingModule.Application.ViewModels;
using GroupingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolsEnterprise.Blazor.Shared.Modals;

namespace SchoolsEnterprise.Blazor.Shared.Components
{
    /// <summary>
    /// The BlogCategoriesDynamicMenu component is responsible for displaying a dynamic menu of blog categories.
    /// It fetches the categories from the server and allows navigation to the selected category.
    /// </summary>
    public partial class BlogCategoriesDynamicMenu
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ICategoryService<BlogPost> Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="NavigationManager"/> instance used for handling navigation in the application.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be manually set
        /// in most scenarios.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Collection of categories to be displayed in the menu.
        /// </summary>
        private ICollection<CategoryViewModel> _categories = [];

        /// <summary>
        /// Navigates to the selected category.
        /// </summary>
        /// <param name="categoryId">The ID of the category to navigate to.</param>
        private void NavigateToCategory(string categoryId)
        {
            NavigationManager.NavigateTo($"/evers/list/{categoryId}", true);
        }

        /// <summary>
        /// Displays a dialog for selecting a category when creating a new blog entry.
        /// </summary>
        /// <remarks>This method asynchronously opens a modal dialog to allow the user to select a
        /// category  for a new blog entry. The dialog is displayed using the <see cref="DialogService"/>.</remarks>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task CreateNewBlogEntry()
        {
            await DialogService.ShowAsync<CategorySelectionForNewBlogEntryModal>("Confirm");
        }

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Loads the categories from the server.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await Provider.CategoriesAsync();
            if (result.Succeeded)
            {
                _categories = result.Data.Where(c => c.DisplayCategoryInMainMenu).Select(c => new CategoryViewModel(c)).ToList();
            }
            await base.OnInitializedAsync();
        }
    }
}
