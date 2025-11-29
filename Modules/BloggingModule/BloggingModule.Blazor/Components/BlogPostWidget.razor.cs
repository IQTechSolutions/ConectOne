using BloggingModule.Application.ViewModels;
using BloggingModule.Domain.DataTransferObjects;
using BloggingModule.Domain.RequestFeatures;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BloggingModule.Blazor.Components
{
    /// <summary>
    /// The BlogPostWidget component is responsible for displaying a table of blog posts.
    /// It fetches the blog posts from the server and supports pagination, sorting, and filtering.
    /// </summary>
    public partial class BlogPostWidget
    {
        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the title displayed for the widget.
        /// </summary>
        [Parameter] public string WidgetTitle { get; set; } = "Blog Posts";

        /// <summary>
        /// The ID of the category to filter blog posts by.
        /// </summary>
        [Parameter] public string? CategoryId { get; set; }

        /// <summary>
        /// Indicates whether the table rows are dense.
        /// </summary>
        private bool _dense = true;

        /// <summary>
        /// Indicates whether the table rows are striped.
        /// </summary>
        private bool _striped = true;

        /// <summary>
        /// Indicates whether the table has borders.
        /// </summary>
        private bool _bordered;

        /// <summary>
        /// Reference to the MudTable component for blog posts.
        /// </summary>
        private MudTable<BlogPostViewModel> _table = null!;

        /// <summary>
        /// Parameters for paging and filtering blog posts.
        /// </summary>
        private readonly BlogPostPageParameters _pageParameters = new();

        /// <summary>
        /// Fetches the blog posts from the server based on the current table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A TableData object containing the total items and the items to display.</returns>
        public async Task<TableData<BlogPostViewModel>> GetBlogPostsAsync(TableState state, CancellationToken token)
        {
            _pageParameters.PageNr = state.Page + 1;
            _pageParameters.PageSize = state.PageSize;

            if (state.SortDirection == SortDirection.Ascending)
                _pageParameters.OrderBy = $"{state.SortLabel} desc";
            else if (state.SortDirection == SortDirection.Descending)
                _pageParameters.OrderBy = $"{state.SortLabel} asc";
            else _pageParameters.OrderBy = null;

            var pagingResponse = await Provider.GetPagedAsync<BlogPostDto, BlogPostPageParameters>("blog", _pageParameters);

            return new TableData<BlogPostViewModel>() { TotalItems = pagingResponse.Data.Count, Items = pagingResponse.Data.Select(c => new BlogPostViewModel(c)) };
        }

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Sets the category ID in the page parameters if provided.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _pageParameters.CategoryId = CategoryId;

            await base.OnInitializedAsync();
        }
    }
}
