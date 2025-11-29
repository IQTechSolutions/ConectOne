using BloggingModule.Domain.DataTransferObjects;
using BloggingModule.Domain.Entities;
using BloggingModule.Domain.Interfaces;
using BloggingModule.Domain.RequestFeatures;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using GroupingModule.Domain.Interfaces;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuralTech.Base.Enums;

namespace BloggingModule.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for managing blog posts.
    /// Provides endpoints for CRUD operations, retrieving blog posts, and sending notifications.
    /// </summary>
    [Route("api/blog"), ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class BlogPostController(IBlogPostService service, ICategoryService<BlogPost> categoryService, IPushNotificationService pushNotificationService) : ControllerBase
    {
        #region Blog Post Retrieval

        /// <summary>
        /// Retrieves a paginated list of blog entries.
        /// </summary>
        [HttpGet] public async Task<IActionResult> PagedBlogEntries([FromQuery] BlogPostPageParameters blogListPageParameters)
        {
            var blogEntries = await service.PagedBlogEntriesAsync(blogListPageParameters);
            return Ok(blogEntries);
        }

        /// <summary>
        /// Retrieves a list of popular blog entries.
        /// </summary>
        [HttpGet("popular")] public async Task<IActionResult> PopularBlogEntries()
        {
            var blogEntries = await service.PopularBlogEntriesAsync(false);
            return Ok(blogEntries);
        }

        /// <summary>
        /// Retrieves all blog entries with optional pagination.
        /// </summary>
        [HttpGet("all")] public async Task<IActionResult> GetAllBlogEntries([FromQuery] BlogPostPageParameters blogListPageParameters)
        {
            var blogEntries = await service.GetAllBlogEntriesAsync(blogListPageParameters, false);
            return Ok(blogEntries);
        }

        /// <summary>
        /// Retrieves the next blog entry based on the current blog ID.
        /// </summary>
        [HttpGet("{id}/next")] public async Task<IActionResult> GetNextBlogEntry(string id)
        {
            var blogEntries = await service.GetNextBlogEntryAsync(id, trackChanges: false);
            return Ok(blogEntries);
        }

        /// <summary>
        /// Retrieves a specific blog entry by ID and optional user ID.
        /// </summary>
        [HttpGet("{id}/{userId?}")] public async Task<IActionResult> GetBlogEntry(string id, string? userId)
        {
            var blogEntries = await service.GetBlogEntryAsync(id, userId);
            return Ok(blogEntries);
        }

        /// <summary>
        /// Retrieves the previous blog entry based on the current blog ID.
        /// </summary>
        [HttpGet("{id}/prev")] public async Task<IActionResult> GetPrevBlogEntry(string id)
        {
            var blogEntries = await service.GetPrevBlogEntryAsync(id, trackChanges: false);
            return Ok(blogEntries);
        }

        #endregion

        #region Blog Post Management

        /// <summary>
        /// Creates a new blog entry.
        /// </summary>
        [HttpPut] public async Task<IActionResult> CreateBlogEntry([FromBody] BlogPostDto blogEntry)
        {
            var result = await service.CreateBlogEntryAsync(blogEntry);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing blog entry.
        /// </summary>
        [HttpPost] public async Task<IActionResult> UpdateBlogEntry([FromBody] BlogPostDto blogEntry)
        {
            var result = await service.UpdateBlogEntryAsync(blogEntry);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a blog entry by ID.
        /// </summary>
        [HttpDelete("{blogId}")] public async Task<IActionResult> DeleteBlogEntry(string blogId)
        {
            var result = await service.DeleteBlogEntryAsync(blogId);
            return Ok(result);
        }

        #endregion

        #region Blog Post Statistics

        /// <summary>
        /// Retrieves the count of unread blog entries for a user and optional category.
        /// </summary>
        [HttpGet("unread/count/{userId?}/{categoryId?}")] public async Task<IActionResult> BlogEntryViewCount(string? userId = null, string? categoryId = null)
        {
            var blogEntries = await service.UnreadBlogEntryCount(userId, categoryId);
            return Ok(blogEntries);
        }

        /// <summary>
        /// Retrieves the view count for a specific blog entry.
        /// </summary>
        [HttpGet("views/{blogId}/{userId?}")] public async Task<IActionResult> GetBlogEntryViews(string blogId, string? userId)
        {
            var blogEntries = await service.BlogEntryViews(blogId, userId);
            return Ok(blogEntries);
        }

        #endregion


        #region Notifications

        /// <summary>
        /// Sends a push notification for a blog post to specified users.
        /// </summary>
        [HttpPost("pushnotifications")] public async Task<IActionResult> SendPushNotification([FromBody] CreateBlogPostNotificationRequest request)
        {
            try
            {
                var errorList = new List<string>();
                var blogPostResult = await service.GetBlogEntryAsync(request.BlogPostId);
                if (!blogPostResult.Succeeded)
                {
                    errorList.AddRange(blogPostResult.Messages);
                }
                else
                {
                    var blogPost = blogPostResult.Data;

                    var documentLinks = new List<string>();
                    documentLinks.AddRange(blogPost.DocumentLinks);
                    foreach (var file in blogPost.Documents)
                    {
                        documentLinks.Add(file.Url);
                    }

                    var notification = new NotificationDto
                    {
                        EntityId = blogPost.BlogPostId,
                        MessageType = MessageType.BlogPost,
                        Title = blogPost.Title,
                        ShortDescription = blogPost.Description.TruncateLongString(55),
                        Message = blogPost.Description,
                        DocumentLinks = documentLinks,
                        Created = DateTime.Now,
                        NotificationUrl = $"/blog/details/{blogPost.BlogPostId}"
                    };

                    var categoryResult = await categoryService.CategoryAsync(blogPost.CategoryId);
                    if (categoryResult.Succeeded)
                    {
                        notification = notification with { Category = categoryResult.Data.Name };
                    }
                    else
                    {
                        errorList.AddRange(categoryResult.Messages);
                    }

                    var notificationResult = await pushNotificationService.SendNotifications(request.Users, notification);
                    if (notificationResult.Succeeded)
                    {
                       // foreach (var user in request.Users)
                       // {
                       //     await hub.Clients.User(user.Id).SendAsync("ReceivePushNotification", notification.Message, user.Id, User.GetUserId());
                       //
                       //     //if (user.ReceiveEmails)
                       //     //{
                       //     //    foreach (var emailAddress in user.EmailAddresses)
                       //     //    {
                       //     //        var emailResult = await emailSender.SendBlogPostNotificationEmailAsync(
                       //     //            $"{user.Name} {user.LastName}", emailAddress, notification.Title,
                       //     //            notification.Message, notification.MessageType, "", notification.Category,
                       //     //            configuration["EmailConfiguration:logoUrl"], configuration["EmailConfiguration:caption"],
                       //     //            configuration["EmailConfiguration:logoLink"], notification.DocumentLinks.ToList(), notification.Documents.ToList());
                       //     //        if (!emailResult.Succeeded)
                       //     //            errorList.AddRange(emailResult.Messages);
                       //     //    }
                       //     //}
                       // }
                    }
                    else
                    {
                        errorList.AddRange(notificationResult.Messages);
                    }
                }

                return errorList.Any() ? Ok(await Result.FailAsync(errorList)) : Ok(await Result.SuccessAsync("Blog Post Notification sent successfully"));
            }
            catch (Exception e)
            {
                return Ok(await Result.FailAsync(e.Message));
            }
        }

        #endregion
    }
}
