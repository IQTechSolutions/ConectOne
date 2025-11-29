using BloggingModule.Application.ViewModels;
using BloggingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FilingModule.Application.ViewModels;
using FilingModule.Blazor.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using MudBlazor;

namespace BloggingModule.Blazor.Pages.BlogPosts
{
    /// <summary>
    /// Code-behind for the UpdateBlogPost component, handling the logic for updating a blog post.
    /// </summary>
    public partial class UpdateBlogPost
    {
        private string? _imageSource;

        /// <summary>
        /// The ID of the blog post to update.
        /// </summary>
        [Parameter] public string BlogId { get; set; } = null!;

        /// <summary>
        /// The ID of the category to which the blog post belongs.
        /// </summary>
        [Parameter] public string CategoryId { get; set; } = null!;

        /// <summary>
        /// Injected service for managing blog post-related operations.
        /// </summary>
        [Inject] public IBlogPostService BlogPostService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the application configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current navigation state.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// ViewModel for handling the blog post data.
        /// </summary>
        public BlogPostViewModel BlogPost { get; set; } = new();

        /// <summary>
        /// Creates a notification link for the blog post.
        /// </summary>
        public async Task CreateNotificationLinkAsync()
        {
            var parameters = new DialogParameters<CreateDocumentLinkUrlModal>
            {
                { x => x.EntityId, BlogPost.BlogPostId }
            };

            var dialog = await DialogService.ShowAsync<CreateDocumentLinkUrlModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var model = (CreateDocumentLinkUrlViewModel)result.Data!;
                if (model.Documents.Any())
                {
                    foreach (var item in model.Documents)
                    {
                        BlogPost.Documents.Add(item);
                    }
                }
                else if (!string.IsNullOrEmpty(model.Url))
                {
                    BlogPost.DocumentLinks.Add(model.Url);
                }
            }

            StateHasChanged();
        }

        /// <summary>
        /// Updates the cover image URL when the cover image is changed.
        /// </summary>
        /// <param name="coverImage">The new cover image URL.</param>
        public void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Removes a document link from the blog post.
        /// </summary>
        /// <param name="link">The document link to remove.</param>
        private async Task RemoveDocumentLink(string link)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this link from this blog post?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var linkToRemove = BlogPost.DocumentLinks.FirstOrDefault(c => c == link);
                if (linkToRemove != null)
                {
                    BlogPost.DocumentLinks.Remove(linkToRemove);
                }
            }
        }

        /// <summary>
        /// Updates the blog post with the current data.
        /// </summary>
        public async Task UpdateAsync()
        {
            var updateResult = await BlogPostService.UpdateBlogEntryAsync(BlogPost.ToDto());
            updateResult.ProcessResponseForDisplay(SnackBar,
                () =>
                {
                    NavigationManager.NavigateTo($"/evers/list/{CategoryId}");
                    SnackBar.Add("Operation Completed Successfully, blog post was updated", Severity.Success);
                });
        }

        /// <summary>
        /// Cancels the update operation and navigates back to the blog post list.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo($"/evers/list/{BlogPost.CategoryId}");
        }

        /// <summary>
        /// Initializes the component and loads the blog post data.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var blogResult = await BlogPostService.GetBlogEntryAsync(BlogId);
            BlogPost = new BlogPostViewModel(blogResult.Data);
            if (!string.IsNullOrWhiteSpace(BlogPost.CoverImageUrl))
            {
                _imageSource =
                    $"{Configuration.GetValue<string>("ImagesUrl")}/{BlogPost.CoverImageUrl?.TrimStart('/')}";
            }
            else
            {
                _imageSource = "/_content/Blogging.Blazor/images/NoImage.jpg";
            }
        }
    }
}