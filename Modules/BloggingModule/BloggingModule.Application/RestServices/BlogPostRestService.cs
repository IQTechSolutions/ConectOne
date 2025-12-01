using BloggingModule.Domain.DataTransferObjects;
using BloggingModule.Domain.Interfaces;
using BloggingModule.Domain.RequestFeatures;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;

namespace BloggingModule.Application.RestServices
{
    /// <summary>
    /// Provides RESTful operations for managing blog posts, including retrieving, creating, updating, and deleting blog
    /// entries.
    /// </summary>
    /// <remarks>This service supports operations such as paginated retrieval of blog posts, fetching popular
    /// entries, and managing individual blog entries. It also provides functionality to track changes, retrieve
    /// adjacent blog entries, and count unread entries or views.</remarks>
    public class BlogPostRestService(IBaseHttpProvider provider) : IBlogPostService
    {
        /// <summary>
        /// Retrieves a paginated list of blog entries based on the specified page parameters.
        /// </summary>
        /// <remarks>The method fetches blog entries from the "blog" resource and applies the specified 
        /// pagination parameters. The <paramref name="trackChanges"/> parameter determines whether  the retrieved
        /// entities are tracked for changes in the underlying data context.</remarks>
        /// <param name="blogListPageParameters">The parameters that define the pagination and filtering options for the blog entries.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved entities.  Defaults to <see langword="false"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of blog entries as  <see
        /// cref="BlogPostDto"/> objects.</returns>
        public async Task<PaginatedResult<BlogPostDto>> PagedBlogEntriesAsync(BlogPostPageParameters blogListPageParameters, bool trackChanges = false)
        {
            var result = await provider.GetPagedAsync<BlogPostDto, BlogPostPageParameters>("blog", blogListPageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a collection of the most popular blog entries.
        /// </summary>
        /// <remarks>The method fetches the popular blog entries from the "blog/popular" endpoint. The
        /// returned  collection may be empty if no popular blog entries are available.</remarks>
        /// <param name="trackChanges">A boolean value indicating whether the retrieved entities should be tracked for changes. This parameter is
        /// currently not used in the implementation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}> of
        /// <IEnumerable{T}> containing the popular blog entries  as <BlogPostDto> objects.</returns>
        public async Task<IBaseResult<IEnumerable<BlogPostDto>>> PopularBlogEntriesAsync(bool trackChanges)
        {
            var result = await provider.GetAsync<IEnumerable<BlogPostDto>>("blog/popular");
            return result;
        }

        /// <summary>
        /// Retrieves all blog entries based on the specified pagination parameters.
        /// </summary>
        /// <remarks>The returned blog entries are filtered and paginated based on the provided  <paramref
        /// name="blogListPageParameters"/>. If no entries are found, the result may contain an  empty
        /// collection.</remarks>
        /// <param name="blogListPageParameters">The pagination parameters, including page number and page size, used to filter the blog entries.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved entities.  Defaults to <see
        /// langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing the blog entries.</returns>
        public async Task<IBaseResult<IEnumerable<BlogPostDto>>> GetAllBlogEntriesAsync(BlogPostPageParameters blogListPageParameters, bool trackChanges = false)
        {
            var result = await provider.GetAsync<IEnumerable<BlogPostDto>>($"blog/all?{blogListPageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves the next blog entry for the specified blog.
        /// </summary>
        /// <remarks>This method asynchronously retrieves the next blog entry based on the provided blog
        /// identifier.  The caller can specify whether to track changes to the retrieved entry.</remarks>
        /// <param name="blogId">The unique identifier of the blog for which the next entry is requested. Cannot be null or empty.</param>
        /// <param name="trackChanges">A boolean value indicating whether changes to the retrieved blog entry should be tracked.  Defaults to <see
        /// langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object wrapping the next blog entry as a <see cref="BlogPostDto"/>. If no next entry exists, the result may
        /// be empty.</returns>
        public async Task<IBaseResult<BlogPostDto>> GetNextBlogEntryAsync(string blogId, bool trackChanges = false)
        {
            var result = await provider.GetAsync<BlogPostDto>($"blog/next/{blogId}");
            return result;
        }

        /// <summary>
        /// Retrieves a blog entry based on the specified blog ID.
        /// </summary>
        /// <remarks>This method retrieves a blog entry from the underlying data provider. If <paramref
        /// name="userId"/> is specified,  the returned data may include user-specific details. The <paramref
        /// name="trackChanges"/> parameter determines  whether the retrieved entity is tracked for changes.</remarks>
        /// <param name="blogId">The unique identifier of the blog entry to retrieve. Cannot be null or empty.</param>
        /// <param name="userId">An optional identifier for the user making the request. If provided, the response may include user-specific
        /// data.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved blog entry.  Set to <see
        /// langword="true"/> to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object wrapping the <see cref="BlogPostDto"/> for the requested blog entry.</returns>
        public async Task<IBaseResult<BlogPostDto>> GetBlogEntryAsync(string blogId, string? userId = null, bool trackChanges = false)
        {
            var result = await provider.GetAsync<BlogPostDto>($"blog/{blogId}/{userId}");
            return result;
        }

        /// <summary>
        /// Retrieves the previous blog entry for the specified blog ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the previous blog entry based
        /// on the provided blog ID. The <paramref name="trackChanges"/> parameter determines whether the retrieved
        /// entry is tracked for changes.</remarks>
        /// <param name="blogId">The unique identifier of the blog entry for which the previous entry is to be retrieved.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved blog entry. The default value is <see
        /// langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the <see cref="BlogPostDto"/> of the previous blog entry, or <see langword="null"/> if no
        /// previous entry exists.</returns>
        public async Task<IBaseResult<BlogPostDto>> GetPrevBlogEntryAsync(string blogId, bool trackChanges = false)
        {
            var result = await provider.GetAsync<BlogPostDto>($"blog/prev/{blogId}");
            return result;
        }

        /// <summary>
        /// Creates a new blog entry asynchronously.
        /// </summary>
        /// <remarks>This method sends the provided blog entry to the underlying data provider for
        /// creation.  Ensure that the <paramref name="blogEntry"/> contains all required fields before calling this
        /// method.</remarks>
        /// <param name="blogEntry">The blog entry to create, represented as a <see cref="BlogPostDto"/> object. This parameter cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object that includes the created blog entry as a <see cref="BlogPostDto"/> or details about the operation's
        /// outcome.</returns>
        public async Task<IBaseResult<BlogPostDto>> CreateBlogEntryAsync(BlogPostDto blogEntry)
        {
            var result = await provider.PutAsync<BlogPostDto, BlogPostDto>($"blog", blogEntry);
            return result;
        }

        /// <summary>
        /// Updates an existing blog entry with the provided data.
        /// </summary>
        /// <remarks>The method sends the updated blog entry to the underlying provider for processing.
        /// Ensure that the <paramref name="blogEntry"/> parameter contains valid data before calling this
        /// method.</remarks>
        /// <param name="blogEntry">The <see cref="BlogPostDto"/> object containing the updated data for the blog entry.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object wrapping the updated <see cref="BlogPostDto"/>.</returns>
        public async Task<IBaseResult<BlogPostDto>> UpdateBlogEntryAsync(BlogPostDto blogEntry)
        {
            var result = await provider.PostAsync<BlogPostDto, BlogPostDto>($"blog", blogEntry);
            return result;
        }

        /// <summary>
        /// Deletes a blog entry with the specified identifier.
        /// </summary>
        /// <remarks>This method sends a delete request to the underlying provider to remove the specified
        /// blog entry.  Ensure the <paramref name="blogId"/> corresponds to an existing blog entry to avoid unexpected
        /// results.</remarks>
        /// <param name="blogId">The unique identifier of the blog entry to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the delete operation.</returns>
        public async Task<IBaseResult> DeleteBlogEntryAsync(string blogId)
        {
            var result = await provider.DeleteAsync($"blog", blogId);
            return result;
        }

        /// <summary>
        /// Retrieves the count of unread blog entries for the specified user and category.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the unread blog entry count 
        /// from the underlying data provider. The result may vary depending on the provided  <paramref name="userId"/>
        /// and <paramref name="categoryId"/> parameters.</remarks>
        /// <param name="userId">The identifier of the user for whom to retrieve the unread blog entry count.  If <see langword="null"/>, the
        /// count is retrieved for all users.</param>
        /// <param name="categoryId">The identifier of the blog category to filter the unread entries.  If <see langword="null"/>, the count is
        /// retrieved across all categories.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="int"/> representing  the count of unread blog entries.</returns>
        public async Task<IBaseResult<int>> UnreadBlogEntryCount(string? userId = null, string? categoryId = null)
        {
            var result = await provider.GetAsync<int>($"blog/unread/count/{userId}/{categoryId}");
            return result;
        }

        /// <summary>
        /// Retrieves the number of unread views for a specific blog entry.
        /// </summary>
        /// <remarks>This method calls an external provider to retrieve the unread view count. Ensure that
        /// the <paramref name="blogEntryId"/> is valid and corresponds to an existing blog entry.</remarks>
        /// <param name="blogEntryId">The unique identifier of the blog entry.</param>
        /// <param name="userId">An optional user identifier. If provided, the count is specific to the given user;  otherwise, the count is
        /// for all users.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number  of unread views for
        /// the specified blog entry.</returns>
        public async Task<IBaseResult<int>> BlogEntryViews(string blogEntryId, string? userId = null)
        {
            var result = await provider.GetAsync<int>($"blog/unread/count/{blogEntryId}/{userId}");
            return result;
        }
    }
}
