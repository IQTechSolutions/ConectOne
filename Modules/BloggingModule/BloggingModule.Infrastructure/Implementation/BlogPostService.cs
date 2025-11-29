using BloggingModule.Domain.DataTransferObjects;
using BloggingModule.Domain.Entities;
using BloggingModule.Domain.Interfaces;
using BloggingModule.Domain.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using BloggingModule.Infrastructure.Specifications;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using GroupingModule.Domain.Entities;
using MessagingModule.Domain.Entities;

namespace BloggingModule.Infrastructure.Implementation
{
    /// <summary>
    /// Service implementation for managing blog posts.
    /// </summary>
    public sealed class BlogPostService : IBlogPostService
    {
        private readonly IRepository<BlogPost, string> _repository;
        private readonly IRepository<Notification, string> _notificationRepository;
        private readonly IRepository<EntityCategory<BlogPost>, string> _blogPostCategoryRepository;
        private readonly IRepository<BlogEntryView, string> _blogEntryViewRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostService"/> class.
        /// </summary>
        /// <param name="repository">The repository for blog posts.</param>
        /// <param name="notificationRepository">The repository for notifications.</param>
        /// <param name="blogPostCategoryRepository">The repository for blog post categories.</param>
        /// <param name="blogEntryViewRepository">The repository for blog entry views.</param>
        public BlogPostService(IRepository<BlogPost, string> repository, IRepository<Notification, string> notificationRepository,
            IRepository<EntityCategory<BlogPost>, string> blogPostCategoryRepository, IRepository<BlogEntryView, string> blogEntryViewRepository)
        {
            _repository = repository;
            _notificationRepository = notificationRepository;
            _blogPostCategoryRepository = blogPostCategoryRepository;
            _blogEntryViewRepository = blogEntryViewRepository;
        }

        #region Public Methods

        /// <summary>
        /// Retrieves a paginated list of blog entries based on the specified parameters.
        /// </summary>
        /// <param name="blogListPageParameters">The parameters for pagination and filtering.</param>
        /// <param name="trackChanges">Indicates whether to track changes in the retrieved entities.</param>
        /// <returns>A paginated result containing the blog entries.</returns>
        public async Task<PaginatedResult<BlogPostDto>> PagedBlogEntriesAsync(BlogPostPageParameters blogListPageParameters, bool trackChanges)
        {
            // Create specifications for count and page retrieval
            var pageSpec = new PagedBlogPostSpecification(blogListPageParameters, applyPaging: true);
            var countSpec = new PagedBlogPostSpecification(blogListPageParameters);

            var result = await _repository.ListAsync(pageSpec, trackChanges);
            if (!result.Succeeded) return PaginatedResult<BlogPostDto>.Failure(result.Messages);

            var countResult = await _repository.CountAsync(countSpec);
            if (!countResult.Succeeded) return PaginatedResult<BlogPostDto>.Failure(countResult.Messages);

            var blogEntries = result.Data.Select(c => new BlogPostDto(c)).ToList();

            return PaginatedResult<BlogPostDto>.Success(
                blogEntries,
                countResult.Data,
                blogListPageParameters.PageNr,
                blogListPageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a list of popular blog entries.
        /// </summary>
        /// <param name="trackChanges">Indicates whether to track changes in the retrieved entities.</param>
        /// <returns>A result containing the popular blog entries.</returns>
        public async Task<IBaseResult<IEnumerable<BlogPostDto>>> PopularBlogEntriesAsync(bool trackChanges)
        {
            var spec = new LambdaSpec<BlogPost>(c => c.Featured);
            spec.AddInclude(g => g.Include(c => c.Images));

            var result = await _repository.ListAsync(spec, trackChanges);
            if (!result.Succeeded) return await Result<IEnumerable<BlogPostDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<BlogPostDto>>.SuccessAsync(result.Data.Select(c => new BlogPostDto(c)));
        }

        /// <summary>
        /// Retrieves all blog entries based on the specified parameters.
        /// </summary>
        /// <param name="blogListPageParameters">The parameters for pagination and filtering.</param>
        /// <param name="trackChanges">Indicates whether to track changes in the retrieved entities.</param>
        /// <returns>A result containing all blog entries.</returns>
        public async Task<IBaseResult<IEnumerable<BlogPostDto>>> GetAllBlogEntriesAsync(BlogPostPageParameters blogListPageParameters, bool trackChanges)
        {
            var spec = new LambdaSpec<BlogPost>(c => true);
            spec.AddInclude(g => g.Include(c => c.Images));

            var result = await _repository.ListAsync(spec, trackChanges);
            if (!result.Succeeded) return await Result<IEnumerable<BlogPostDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<BlogPostDto>>.SuccessAsync(result.Data.Select(c => new BlogPostDto(c)));
        }

        /// <summary>
        /// Retrieves the next blog entry based on the specified blog ID.
        /// </summary>
        /// <param name="blogId">The ID of the current blog entry.</param>
        /// <param name="trackChanges">Indicates whether to track changes in the retrieved entities.</param>
        /// <returns>A result containing the next blog entry.</returns>
        public async Task<IBaseResult<BlogPostDto>> GetNextBlogEntryAsync(string blogId, bool trackChanges)
        {
            var spec = new LambdaSpec<BlogPost>(c => true);
            var result = await _repository.ListAsync(spec, trackChanges);
            if (!result.Succeeded) return await Result<BlogPostDto>.FailAsync(result.Messages);

            var item = result.Data.FirstOrDefault(c => c.Id.Equals(blogId));
            if (item is not null)
            {
                var list = result.Data.ToList();
                var index = list.IndexOf(item) + 1;

                if (index == list.IndexOf(list.First()))
                    index = list.IndexOf(list.Last());
                else if (index == list.IndexOf(list.Last()))
                    index = list.IndexOf(list.First());
                return await Result<BlogPostDto>.SuccessAsync(new BlogPostDto(list[index]));
            }
            return await Result<BlogPostDto>.FailAsync($"No blog post found matching id '{blogId}' was found in the database");
        }

        /// <summary>
        /// Retrieves a specific blog entry based on the specified blog ID and user ID.
        /// </summary>
        /// <param name="blogId">The ID of the blog entry.</param>
        /// <param name="userId">The ID of the user (optional).</param>
        /// <param name="trackChanges">Indicates whether to track changes in the retrieved entities.</param>
        /// <returns>A result containing the blog entry.</returns>
        public async Task<IBaseResult<BlogPostDto>> GetBlogEntryAsync(string blogId, string? userId = null, bool trackChanges = false)
        {
            var spec = new LambdaSpec<BlogPost>(c => c.Id.Equals(blogId));
            spec.AddInclude(g => g.Include(c => c.Images).Include(c => c.Categories));

            var result = await _repository.FirstOrDefaultAsync(spec, trackChanges);
            if (!result.Succeeded) return await Result<BlogPostDto>.FailAsync(result.Messages);

            var response = result.Data;
            var blogPost = new BlogPostDto(response!);

            var notificationsResult = _notificationRepository.FindByCondition(c => c.EntityId!.Equals(blogId), true);
            if (notificationsResult.Succeeded)
            {
                foreach (var notification in notificationsResult.Data)
                {
                    notification.OpenedDate = DateTime.UtcNow;
                    _notificationRepository.Update(notification);
                }
                await _notificationRepository.SaveAsync();
            }

            if (string.IsNullOrEmpty(userId)) return await Result<BlogPostDto>.SuccessAsync(blogPost);

            var view = new BlogEntryView() { UserId = userId, BlogPostId = blogId };
            await _blogEntryViewRepository.CreateAsync(view);

            var saveResult = await _blogEntryViewRepository.SaveAsync();
            if (!saveResult.Succeeded) return await Result<BlogPostDto>.FailAsync(saveResult.Messages);
            return await Result<BlogPostDto>.SuccessAsync(blogPost);
        }

        /// <summary>
        /// Retrieves the previous blog entry based on the specified blog ID.
        /// </summary>
        /// <param name="blogId">The ID of the current blog entry.</param>
        /// <param name="trackChanges">Indicates whether to track changes in the retrieved entities.</param>
        /// <returns>A result containing the previous blog entry.</returns>
        public async Task<IBaseResult<BlogPostDto>> GetPrevBlogEntryAsync(string blogId, bool trackChanges)
        {
            var spec = new LambdaSpec<BlogPost>(c => true);
            spec.AddInclude(g => g.Include(c => c.Images)
                .Include(c => c.Categories)
                .Include(c => c.Comments).ThenInclude(c => c.Comments));

            var result = await _repository.ListAsync(spec, trackChanges);
            if (!result.Succeeded) return await Result<BlogPostDto>.FailAsync(result.Messages);

            var list = result.Data.ToList();
            var item = list.FirstOrDefault(c => c.Id.Equals(blogId));

            if (item != null)
            {
                var index = list.IndexOf(item) - 1;

                if (index == list.IndexOf(list.First()))
                    index = list.IndexOf(list.Last());
                else if (index == list.IndexOf(list.Last()))
                    index = list.IndexOf(list.First());
                return await Result<BlogPostDto>.SuccessAsync(new BlogPostDto(list[index]));
            }
            return await Result<BlogPostDto>.FailAsync($"No blog post matching id '{blogId}' was found in the database");
        }

        /// <summary>
        /// Creates a new blog entry.
        /// </summary>
        /// <param name="blogEntry">The blog entry to create.</param>
        /// <returns>A result containing the created blog entry.</returns>
        public async Task<IBaseResult<BlogPostDto>> CreateBlogEntryAsync(BlogPostDto blogEntry)
        {
            var result = await _repository.CreateAsync(blogEntry.ToBlogPost());
            if (!result.Succeeded) return await Result<BlogPostDto>.FailAsync(result.Messages);

            if (!string.IsNullOrEmpty(blogEntry.CategoryId))
            {
                var categoryResult = await _blogPostCategoryRepository.CreateAsync(new EntityCategory<BlogPost>(blogEntry.BlogPostId!, blogEntry.CategoryId));
                if (!categoryResult.Succeeded) return await Result<BlogPostDto>.FailAsync(categoryResult.Messages);
            }

            var saveResult = await _repository.SaveAsync();
            if (!saveResult.Succeeded) return await Result<BlogPostDto>.FailAsync(saveResult.Messages);
            return await Result<BlogPostDto>.SuccessAsync(new BlogPostDto(result.Data));
        }

        /// <summary>
        /// Updates an existing blog entry.
        /// </summary>
        /// <param name="blogEntry">The blog entry to update.</param>
        /// <returns>A result indicating the success or failure of the update operation.</returns>
        public async Task<IBaseResult<BlogPostDto>> UpdateBlogEntryAsync(BlogPostDto blogEntry)
        {
            var spec = new LambdaSpec<BlogPost>(c => c.Id.Equals(blogEntry.BlogPostId));
            var result = await _repository.FirstOrDefaultAsync(spec, true);

            if (result.Succeeded)
            {
                var response = result.Data;

                if (response != null)
                {
                    response.Title = blogEntry.Title;
                    response.Featured = blogEntry.Featured;
                    response.Description = blogEntry.Description;
                    response.Content = blogEntry.Content;
                    response.DocumentLinks = string.Join(";", blogEntry.DocumentLinks);

                    _repository.Update(response);

                    var saveResult = await _repository.SaveAsync();
                    if (saveResult.Succeeded)
                    {
                        return await Result<BlogPostDto>.SuccessAsync($"Blog post with id '{blogEntry.BlogPostId} was updated successfully'");
                    }
                    return await Result<BlogPostDto>.FailAsync(saveResult.Messages);
                }
                return await Result<BlogPostDto>.FailAsync($"No entry with id '{blogEntry.BlogPostId}' was found in the database");
            }
            return await Result<BlogPostDto>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Deletes a blog entry based on the specified blog ID.
        /// </summary>
        /// <param name="blogId">The ID of the blog entry to delete.</param>
        /// <returns>A result indicating the success or failure of the delete operation.</returns>
        public async Task<IBaseResult> DeleteBlogEntryAsync(string blogId)
        {
            var spec = new LambdaSpec<BlogPost>(c => c.Id.Equals(blogId));
            var result = await _repository.FirstOrDefaultAsync(spec, true);
            if (!result.Succeeded) return await Result.FailAsync(result.Messages);

            var deleteResult = await _repository.DeleteAsync(blogId);
            if (!deleteResult.Succeeded) return await Result<BlogPostDto>.FailAsync(deleteResult.Messages);

            var saveResult = await _repository.SaveAsync();
            if (!saveResult.Succeeded) return await Result<BlogPostDto>.FailAsync(saveResult.Messages);

            return await Result<BlogPostDto>.SuccessAsync($"Blog post with id '{blogId}'");
        }

        /// <summary>
        /// Retrieves the count of unread blog entries for a specific user and category.
        /// </summary>
        /// <param name="userId">The ID of the user (optional).</param>
        /// <param name="categoryId">The ID of the category (optional).</param>
        /// <returns>A result containing the count of unread blog entries.</returns>
        public async Task<IBaseResult<int>> UnreadBlogEntryCount(string? userId = null, string? categoryId = null)
        {
            try
            {
                var spec = string.IsNullOrEmpty(categoryId)
                    ? new LambdaSpec<BlogPost>(c => true)
                    : new LambdaSpec<BlogPost>(c => c.Categories.Any(cat => cat.CategoryId == categoryId));
                spec.AddInclude(g => g.Include(c => c.Categories).Include(c => c.Views));

                var blogPostResult = await _repository.ListAsync(spec, false);
                if (!blogPostResult.Succeeded) return await Result<int>.FailAsync(blogPostResult.Messages);

                var blogEntries = blogPostResult.Data;

                if (!string.IsNullOrEmpty(userId))
                    return await Result<int>.SuccessAsync(blogEntries.Count(c => c.Views.Count(blogEntry => blogEntry.UserId == userId) == 0));
                return await Result<int>.SuccessAsync(blogEntries.Count(c => !c.Views.Any()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the count of views for a specific blog entry.
        /// </summary>
        /// <param name="blogEntryId">The ID of the blog entry.</param>
        /// <param name="userId">The ID of the user (optional).</param>
        /// <returns>A result containing the count of views for the blog entry.</returns>
        public async Task<IBaseResult<int>> BlogEntryViews(string blogEntryId, string? userId = null)
        {
            var spec = new LambdaSpec<BlogEntryView>(c => c.BlogPostId == blogEntryId);
            var viewResult = await _blogEntryViewRepository.ListAsync(spec, false);
            if (!viewResult.Succeeded) return await Result<int>.FailAsync();

            if (!string.IsNullOrEmpty(userId))
                return await Result<int>.SuccessAsync(viewResult.Data.Count(c => c.UserId == userId));
            return await Result<int>.SuccessAsync(viewResult.Data.Count());
        }

        #endregion
    }
}

