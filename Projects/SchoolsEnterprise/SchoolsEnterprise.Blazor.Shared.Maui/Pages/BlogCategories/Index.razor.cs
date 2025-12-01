using BloggingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.RequestFeatures;
using IdentityModule.Domain.Extensions;
using Microsoft.Extensions.Configuration;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.BlogCategories
{
    /// <summary>
    /// The Index component displays a list of blog categories (via <see cref="IBlogCategoryProvider"/>),
    /// along with any unread notifications or posts for the current user.
    /// </summary>
    public partial class Index
    {
        private string _userId = null!;
        private List<CategoryDto> _categories = [];
        private bool _loaded;

        /// <summary>
        /// Retrieves the current authentication state, used for identifying the user and 
        /// filtering data relevant to that user (e.g., unread blog entries).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Provider for fetching category data from the blogging system.
        /// </summary>
        [Inject] public IBlogPostCategoryService BlogPostCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage blog posts.
        /// </summary>
        [Inject] public IBlogPostService BlogPostService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The configuration is typically populated by the dependency injection system and
        /// provides access to key-value application settings. Use this property to retrieve configuration values such
        /// as connection strings, feature flags, or other environment-specific data.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs, retrieving the current URI, and handling navigation events. This property is typically
        /// injected by the Blazor framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets the label text for the "Evers" configuration from the application settings.
        /// </summary>
        private string _everLabelText => Configuration["ApplicationConfiguration:EversLabelText"];

        /// <summary>
        /// Navigates to a particular blog category’s page (e.g., "/evers/{blogId}").
        /// </summary>
        /// <param name="blogId">The unique identifier of the blog category.</param>
        public void NavigateToCategory(string blogId)
        {
            NavigationManager.NavigateTo($"/evers/{blogId}");
        }

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
                var authState = await AuthenticationStateTask;
                _userId = authState.User.GetUserId();

                // Request a maximum of 100 categories (arbitrary upper limit).
                var result = await BlogPostCategoryService.PagedCategoriesAsync(new CategoryPageParameters { PageSize = 100 });

                if (result.Succeeded)
                {
                    var categories = result.Data.ToList();

                    

                    _categories = categories;

                    _loaded = true;

                    var tasks = categories.Select(async category =>
                    {
                        var countResult = await BlogPostService.UnreadBlogEntryCount(category.CategoryId);
                        if (countResult.Succeeded)
                            category.UnreadNotifications = countResult.Data;
                    });

                    await Task.WhenAll(tasks);
                }

                StateHasChanged();
            }
        }
    }
}
