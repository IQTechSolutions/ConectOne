using BusinessModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.BusinessListings
{
    /// <summary>
    /// Represents a business directory component that manages navigation and category data retrieval.
    /// </summary>
    /// <remarks>This class is designed to interact with a backend service to retrieve paginated category data
    /// and provides functionality for navigating to specific pages within the application.</remarks>
    public partial class ListingCategories
    {
        private ICollection<CategoryDto> _categories = new List<CategoryDto>();

        /// <summary>
        /// Gets or sets the service responsible for executing commands related to the business directory.
        /// </summary>
        [Inject] public IBusinessDirectoryCategoryService BusinessDirectoryCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI in the application.
        /// </summary>
        /// <remarks>The navigation manager provides methods for navigating to different URIs and for
        /// handling navigation events within a Blazor application. This property is typically injected by the
        /// framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        /// <remarks>This property is typically set by dependency injection. Use the provided ISnackbar
        /// instance to show transient messages or alerts within the application's user interface.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The configuration is typically populated by the dependency injection system and
        /// provides access to key-value pairs for application settings. Modifying this property is not recommended
        /// unless you need to replace the configuration source.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the parent category.
        /// </summary>
        [Parameter] public string? ParentCategoryId { get; set; }

        /// <summary>
        /// Navigates the application to the specified URL.
        /// </summary>
        /// <remarks>The navigation is performed using the application's <see cref="NavigationManager" />.
        /// Ensure that the URL provided is valid and accessible within the application's context.</remarks>
        /// <param name="url">The URL to navigate to. This must be a valid, absolute or relative URL.</param>
        public void NavigateToPage(string url)
        {
            NavigationManager.NavigateTo(url, true);
        }
        
        /// <summary>
        /// Asynchronously initializes the component by retrieving a paged list of categories and handling the response.
        /// </summary>
        /// <remarks>This method fetches a paged list of categories from the specified provider and
        /// updates the local category data.  If the operation fails, error messages are displayed using the configured
        /// snackbar.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var parameters = new CategoryPageParameters() { PageSize = 100 };
            if (!string.IsNullOrEmpty(ParentCategoryId))
                parameters.ParentId = ParentCategoryId;
            
            var pagingResponse = await BusinessDirectoryCategoryService.PagedCategoriesAsync(parameters);
            if (!pagingResponse.Succeeded)
            {
                SnackBar.AddErrors(pagingResponse.Messages);
            }
            _categories = pagingResponse.Data;

            await base.OnInitializedAsync();
        }
    }
}
