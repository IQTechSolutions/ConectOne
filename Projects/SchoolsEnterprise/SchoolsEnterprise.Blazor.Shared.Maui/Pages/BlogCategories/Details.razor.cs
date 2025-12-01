using BloggingModule.Domain.DataTransferObjects;
using BloggingModule.Domain.Interfaces;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.BlogCategories
{
    /// <summary>
    /// The Details component is responsible for displaying the details of a specific blog post.
    /// It fetches the blog post details from the server and updates the notification state.
    /// </summary>
    public partial class Details
    {
        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IBlogPostService Provider { get; set; } = null!;

        /// <summary>
        /// The ID of the blog post to be displayed.
        /// </summary>
        [Parameter] public string BlogId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the notification.
        /// </summary>
        [Parameter] public string NotificationId { get; set; } = null!;

        /// <summary>
        /// The blog post details.
        /// </summary>
        private BlogPostDto _blogPost = null!;

        /// <summary>
        /// Indicates whether the component has completed loading and can render content.
        /// </summary>
        private bool _loaded;

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
                var userId = authState.User.GetUserId();

                var blogEntryResult = await Provider.GetBlogEntryAsync(BlogId, userId);
                if (blogEntryResult.Succeeded)
                {
                    _blogPost = blogEntryResult.Data;
                    _loaded = true;
                    //NotificationStateManager.NotifyStateChanged();
                }

                _loaded = true;

                StateHasChanged();
            }
        }
    }
}
