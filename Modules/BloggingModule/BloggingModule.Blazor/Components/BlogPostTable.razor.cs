using BloggingModule.Application.ViewModels;
using BloggingModule.Domain.Constants;
using BloggingModule.Domain.Interfaces;
using BloggingModule.Domain.RequestFeatures;
using ConectOne.Blazor.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace BloggingModule.Blazor.Components
{
    /// <summary>
    /// The BlogPostTable component is responsible for displaying a table of blog posts.
    /// It fetches the blog posts from the server and supports pagination, sorting, and deletion.
    /// </summary>
    public partial class BlogPostTable
    {
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered;
        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;

        /// <summary>
        /// The cascading authentication state, used to determine 
        /// the current user's permissions (e.g., create/edit/delete learners).
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IBlogPostService BlogPostService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// The ID of the category to filter blog posts by.
        /// </summary>
        [Parameter] public string? CategoryId { get; set; }
        
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

            var pagingResponse = await BlogPostService.PagedBlogEntriesAsync(_pageParameters);

            return new TableData<BlogPostViewModel>() { TotalItems = pagingResponse.Data.Count, Items = pagingResponse.Data.Select(c => new BlogPostViewModel(c)) };
        }

        /// <summary>
        /// Deletes a specific blog entry after user confirmation.
        /// </summary>
        /// <param name="blogId">The ID of the blog entry to delete.</param>
        private async Task DeleteBlogEntry(string blogId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this blog entry from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var removalResult = await BlogPostService.DeleteBlogEntryAsync(blogId);
                if (removalResult.Succeeded)
                    await _table.ReloadServerData();
            }
        }

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Sets the category ID in the page parameters if provided.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BlogPermissions.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BlogPermissions.Edit)).Succeeded; 
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.BlogPermissions.Edit)).Succeeded;

            _pageParameters.CategoryId = CategoryId;

            await base.OnInitializedAsync();
        }
    }
}