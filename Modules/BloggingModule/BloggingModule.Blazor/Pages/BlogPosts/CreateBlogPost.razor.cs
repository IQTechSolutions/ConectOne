using BloggingModule.Application.ViewModels;
using BloggingModule.Domain.Entities;
using BloggingModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using FilingModule.Application.ViewModels;
using FilingModule.Blazor.Modals;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.Interfaces;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Color = System.Drawing.Color;

namespace BloggingModule.Blazor.Pages.BlogPosts
{
    /// <summary>
    /// The CreateBlogPost component allows users to create or configure a new blog entry.
    /// It handles:
    /// 1) Adding a title, description, cover image, and optional documents or links.
    /// 2) Saving the post via <see cref="IBlogPostProvider"/>.
    /// 3) Sending out a notification after a successful post creation.
    /// 
    /// This component demonstrates how to gather input, manage local state (BlogPost), 
    /// perform async server calls, and optionally display user feedback with MudBlazor.
    /// </summary>
    public partial class CreateBlogPost
    {
        private CategoryDto _category;
        private bool _sendNotification = true;

        /// <summary>
        /// Injected provider for creating a blog post entry.
        /// </summary>
        [Inject] public IBlogPostService BlogPostService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user service used to manage user-related operations.
        /// </summary>
        [Inject] public IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage categories for blog posts.
        /// </summary>
        [Inject] public ICategoryService<BlogPost> CategoryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs, retrieving the current URI, and handling navigation events. It is typically injected by
        /// the Blazor framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        // Holds the source/path for the post’s cover image, if any.
        private string? _imageSource;

        /// <summary>
        /// The local BlogPost data being constructed or edited.
        /// </summary>
        public BlogPostViewModel BlogPost { get; set; } = new() { BlogPostId = Guid.NewGuid().ToString() };

        /// <summary>
        /// Optional parameter for the category of the blog post, if known.
        /// </summary>
        [Parameter] public string? CategoryId { get; set; }        

        /// <summary>
        /// Opens a modal dialog for creating or attaching documents/links to the current post.
        /// If documents are created, they are added to the local BlogPost.Documents collection;
        /// if a single URL is provided, it's added to BlogPost.DocumentLinks.
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
                    // Add multiple documents to the post if returned from the dialog
                    foreach (var item in model.Documents)
                    {
                        BlogPost.Documents.Add(item);
                    }
                }
                else if (!string.IsNullOrEmpty(model.Url))
                {
                    // If only a single URL was provided, add it to DocumentLinks
                    BlogPost.DocumentLinks.Add(model.Url);
                }
            }

            StateHasChanged();
        }

        /// <summary>
        /// Called when the user selects/changes the cover image for the blog post,
        /// updating local state accordingly.
        /// </summary>
        public void CoverImageChanged(MudCropperResponse coverImage)
        {
            _imageSource = coverImage.Base64String;
        }

        /// <summary>
        /// Saves the blog post asynchronously by calling the BlogPostProvider.
        /// Once saved, navigates to the evers list page for the specific category.
        /// A notification is also shown upon success.
        /// </summary>
        public async Task SaveAsync()
        {
            var result = await BlogPostService.CreateBlogEntryAsync(BlogPost.ToDto());
            result.ProcessResponseForDisplay(SnackBar, () =>
            {
                NavigationManager.NavigateTo($"evers/list/{BlogPost.CategoryId}");
                SnackBar.AddSuccess("You have successfully added this blog entry");
            });
        }

        /// <summary>
        /// Creates the blog post, then triggers the <see cref="SuccessfullyCreatedAction"/>
        /// which sends a push notification after success.
        /// </summary>
        public async Task CreateAsync()
        {
            var result = await BlogPostService.CreateBlogEntryAsync(BlogPost.ToDto());
            result.ProcessResponseForDisplay(SnackBar, async () => await SuccessfullyCreatedAction());
        }

        /// <summary>
        /// Called on a successful blog post creation.
        /// Navigates back to a list of evers, displays a success message, 
        /// and sends a blog-post notification using NotificationStateManager.
        /// </summary>
        private async Task SuccessfullyCreatedAction()
        {
            NavigationManager.NavigateTo($"evers/list/{BlogPost.CategoryId}");
            SnackBar.AddSuccess("You have successfully added this blog entry");

            if (_sendNotification)
            {
                try
                {
                    var userListResult = await UserService.GlobalNotificationsUserList();
                    var userList = !userListResult.Succeeded ? new List<RecipientDto>() : userListResult.Data.ToList();
                    //var notificationResult = await NotificationStateManager.SendBlogPostNotification(BlogPost.BlogPostId, userList);

                    //if (notificationResult.Succeeded)
                    //{
                    //    SnackBar.AddSuccess("Event created notification successfully sent");
                    //}
                    //else
                    //{
                    //    SnackBar.AddErrors(notificationResult.Messages);
                    //}
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    SnackBar.Add("An error occurred while sending notifications", Severity.Error);
                }
            }
        }

        /// <summary>
        /// Removes a document link from the BlogPost.DocumentLinks collection 
        /// after confirming the removal with a confirmation dialog.
        /// </summary>
        private async Task RemoveDocumentLink(string link)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this learner from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, MudBlazor.Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var linkToRemove = BlogPost.DocumentLinks.FirstOrDefault(c => c == link);

                if (linkToRemove != null)
                    BlogPost.DocumentLinks.Remove(linkToRemove);
            }
        }

        /// <summary>
        /// Cancels creation and navigates back to the blog listing page without saving changes.
        /// </summary>
        public void Cancel()
        {
            NavigationManager.NavigateTo($"/blog/list/{CategoryId}");
        }

        /// <summary>
        /// Lifecycle method that runs when the component is initialized.
        /// If a CategoryId parameter is supplied, it's assigned to the BlogPost.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(CategoryId))
            {
                BlogPost.CategoryId = CategoryId;
            }
            
            await base.OnInitializedAsync();
        }
    }
}
