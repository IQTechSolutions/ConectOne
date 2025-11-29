using BloggingModule.Domain.Entities;
using GroupingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace BloggingModule.Blazor.Pages.BlogPosts
{
    /// <summary>
    /// The BlogPosts component is responsible for displaying blog posts for a specific category.
    /// </summary>
    public partial class BlogPosts
    {
        #region Injections

        /// <summary>
        /// Injected HTTP provider for making API calls.
        /// </summary>
        [Inject] public ICategoryService<BlogPost> CategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        /// <remarks>This property is typically populated by dependency injection and provides access to
        /// configuration values such as connection strings, application settings, and environment variables.</remarks>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        #endregion

        #region Parameters

        /// <summary>
        /// The ID of the category for which to display blog posts.
        /// </summary>
        [Parameter] public string CategoryId { get; set; } = null!;

        #endregion

        #region Fields

        /// <summary>
        /// The name of the category.
        /// </summary>
        private string? _categoryName;

        /// <summary>
        /// The title of the page, which includes the category name.
        /// </summary>
        private string PageTitle => $"{_categoryName}";

        #endregion

        #region Methods

        /// <summary>
        /// Lifecycle method that is called when the component is initialized.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var categoryResult = await CategoryService.CategoryAsync(CategoryId);
            if (categoryResult.Succeeded)
                _categoryName = categoryResult.Data.Name;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
