using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Interfaces;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProductsModule.Domain.Interfaces;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Commerce
{
    /// <summary>
    /// The Categories component displays a loading state for a short period,
    /// then shows the category listings (or other content).
    /// It also allows navigation back to the commerce index or to a wallet page.
    /// </summary>
    public partial class Categories
    {
        public bool _loaded;
        private List<CategoryDto> _categories;

        /// <summary>
        /// Gets the column size based on the number of categories.
        /// </summary>
        private int _columnSize => _categories.Count() > 3 ? 6 : 12;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        /// <remarks>The provider is typically injected and used to abstract HTTP communication, allowing
        /// for easier testing and decoupling of HTTP-related logic.</remarks>
        [Inject] public IProductCategoryService ProductCategoryService { get; set; }

        /// <summary>
        /// Gets or sets the service used to display transient messages to the user.
        /// </summary>
        /// <remarks>Typically used to show notifications, alerts, or brief feedback in the application's
        /// user interface. The implementation of ISnackbar determines how messages are presented and managed.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatic navigation and
        /// for retrieving information about the current URI. This property is typically set by the Blazor framework via
        /// dependency injection.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Called by the Blazor runtime when the component initializes.
        /// Simulates a loading/data retrieval process via Task.Delay.
        /// </summary>
        protected override async Task OnInitializedAsync()
        { 
            var pagingResponse = await ProductCategoryService.PagedCategoriesAsync(new CategoryPageParameters() { PageSize = 100 });
            if (!pagingResponse.Succeeded)
            {
                SnackbarExtensions.AddErrors(SnackBar, pagingResponse.Messages);
            }
            else
            {
                _categories = pagingResponse.Data;
            }
            _loaded = true;
        }

        /// <summary>
        /// Navigates the user back to the commerce index page.
        /// </summary>
        public void NavigateToCategoryPage(string categoryId)
        {
            NavigationManager.NavigateTo($"/products/{categoryId}");
        }

    }
}
