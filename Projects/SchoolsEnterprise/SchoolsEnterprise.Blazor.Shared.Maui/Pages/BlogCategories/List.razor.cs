using BloggingModule.Domain.DataTransferObjects;
using BloggingModule.Domain.Interfaces;
using BloggingModule.Domain.RequestFeatures;
using GroupingModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.BlogCategories
{
    /// <summary>
    /// The List component is responsible for displaying a list of blog posts for a specific category.
    /// It fetches the blog posts and category details from the server and allows navigation to the
    /// details of each blog post.
    /// </summary>
    public partial class List
    {
        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IBlogPostCategoryService BlogPostCategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage blog posts.
        /// </summary>
        [Inject] public IBlogPostService BlogPostService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI in the application.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// It enables components to perform navigation actions and access the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The ID of the category to filter blog posts by.
        /// </summary>
        [Parameter] public string CategoryId { get; set; } = null!;

        /// <summary>
        /// The ID of the current user.
        /// </summary>
        private string _userId = null!;

        /// <summary>
        /// Indicates whether the component has completed loading and can render content.
        /// </summary>
        private bool _loaded;

        /// <summary>
        /// List of blog posts to be displayed.
        /// </summary>
        private List<BlogPostDto> _blogPosts = [];

        /// <summary>
        /// The category details.
        /// </summary>
        private CategoryDto _category = null!;

        /// <summary>
        /// Navigates to the details page of a specific blog post.
        /// </summary>
        /// <param name="blogPostId">The ID of the blog post to view.</param>
        public void NavigateToBlogPost(string blogPostId)
        {
            NavigationManager.NavigateTo($"/blog/details/{blogPostId}");
        }

        #region Overrides

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

                var categoryResult = await BlogPostCategoryService.CategoryAsync(CategoryId);
                if (categoryResult.Succeeded)
                {
                    _category = categoryResult.Data;

                    var blogEntriesResult = await BlogPostService.PagedBlogEntriesAsync(new BlogPostPageParameters() { OrderBy = "CreatedOn desc", PageSize = 100, CategoryId = _category.CategoryId });
                    if (blogEntriesResult.Succeeded)
                    {
                        foreach (var blogEntry in blogEntriesResult.Data)
                        {
                            var viewCountResult = await BlogPostService.BlogEntryViews(blogEntry.BlogPostId, _userId);
                            if (viewCountResult.Succeeded)
                                blogEntry.Read = viewCountResult.Data > 0;

                            _blogPosts.Add(blogEntry);
                        }
                    }
                }

                _loaded = true;

                StateHasChanged();
            }
        }

        #endregion
    }
}
