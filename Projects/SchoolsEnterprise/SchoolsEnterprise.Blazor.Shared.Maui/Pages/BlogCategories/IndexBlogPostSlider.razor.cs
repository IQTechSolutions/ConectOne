using BloggingModule.Domain.DataTransferObjects;
using BloggingModule.Domain.Interfaces;
using BloggingModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.BlogCategories
{
    /// <summary>
    /// The IndexBlogPostSlider component is responsible for displaying a slider of featured blog posts
    /// for a specific category. It fetches the blog posts from the server and allows navigation to the
    /// details of each blog post.
    /// </summary>
    public partial class IndexBlogPostSlider
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IBlogPostService BlogPostService { get; set; } = null!;

        /// <summary>
        /// Injected Logger for logging errors and information.
        /// </summary>
        [Inject] public ILogger<Home> Logger { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>The configuration provides access to key-value pairs and other settings used to
        /// configure the application's behavior. This property is typically populated by the dependency injection
        /// system.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// The ID of the category to filter blog posts by.
        /// </summary>
        [Parameter] public string CategoryId { get; set; } = null!;

        /// <summary>
        /// List of blog posts to be displayed in the slider.
        /// </summary>
        private List<BlogPostDto>? _feed = [];

        /// <summary>
        /// Navigates to the details page of a specific blog post.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post to view.</param>
        private void NavigateToFeed(string blogPostId)
        {
            NavigationManager.NavigateTo($"/blog/details/{blogPostId}");
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
                try
                {
                    var feedResult = await BlogPostService.PagedBlogEntriesAsync(new BlogPostPageParameters() { CategoryId = CategoryId, Featured = true });
                    if (feedResult.Succeeded)
                    {
                        _feed = feedResult.Data;
                    }

                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred while fetching featured categories.");
                    SnackBar.Add("An error occurred while fetching featured categories for newsfeed.");
                }
            }
        }
    }
}
