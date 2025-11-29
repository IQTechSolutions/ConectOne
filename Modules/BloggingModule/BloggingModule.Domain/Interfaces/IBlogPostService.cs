using BloggingModule.Domain.DataTransferObjects;
using BloggingModule.Domain.RequestFeatures;
using ConectOne.Domain.ResultWrappers;

namespace BloggingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for managing blog post entries, including retrieval, creation, updating, and deletion
    /// operations.
    /// </summary>
    /// <remarks>This interface provides methods for interacting with blog post entries in various ways, such
    /// as paginated retrieval,  fetching popular entries, and managing individual blog posts. It supports operations
    /// that can optionally track changes  for entity state management and includes functionality for counting unread
    /// entries and tracking views.</remarks>
    public interface IBlogPostService
    {
        /// <summary>
        /// Retrieves a paginated list of blog entries based on the specified parameters.
        /// </summary>
        /// <remarks>Use this method to retrieve blog entries in a paginated format, which is useful for
        /// scenarios such as displaying a list of blog posts in a UI with pagination controls.</remarks>
        /// <param name="blogListPageParameters">The parameters that define the pagination and filtering criteria for the blog entries.</param>
        /// <param name="trackChanges">A value indicating whether the retrieved blog entries should be tracked for changes. <see langword="true"/>
        /// to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="PaginatedResult{BlogPostDto}"/> containing the paginated list of blog entries. The result
        /// includes metadata such as the total count and current page information.</returns>
        Task<PaginatedResult<BlogPostDto>> PagedBlogEntriesAsync(BlogPostPageParameters blogListPageParameters, bool trackChanges = false);

        /// <summary>
        /// Retrieves a collection of popular blog entries.
        /// </summary>
        /// <param name="trackChanges">A value indicating whether the retrieved blog entries should be tracked for changes.  <see langword="true"/>
        /// to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// object with a collection of <see cref="BlogPostDto"/>  representing the popular blog entries.</returns>
        Task<IBaseResult<IEnumerable<BlogPostDto>>> PopularBlogEntriesAsync(bool trackChanges);

        /// <summary>
        /// Retrieves all blog entries based on the specified pagination parameters.
        /// </summary>
        /// <param name="blogListPageParameters">The pagination parameters used to filter and retrieve the blog entries. Must not be <see langword="null"/>.</param>
        /// <param name="trackChanges">A value indicating whether the retrieved blog entries should be tracked for changes. <see langword="true"/>
        /// to enable change tracking; otherwise, <see langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{BlogPostDto}"/> representing the retrieved blog entries.</returns>
        Task<IBaseResult<IEnumerable<BlogPostDto>>> GetAllBlogEntriesAsync(BlogPostPageParameters blogListPageParameters, bool trackChanges = false);

        /// <summary>
        /// Retrieves the next blog entry for the specified blog.
        /// </summary>
        /// <remarks>Use this method to retrieve the next blog entry in a sequence for a given blog. The
        /// <paramref name="trackChanges"/> parameter can be used to enable or disable change tracking for the returned
        /// blog entry.</remarks>
        /// <param name="blogId">The unique identifier of the blog for which the next entry is requested. Must not be <see langword="null"/>
        /// or empty.</param>
        /// <param name="trackChanges">A value indicating whether changes to the blog entry should be tracked. <see langword="true"/> to track
        /// changes; otherwise, <see langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see cref="IBaseResult{T}"/>
        /// of type <see cref="BlogPostDto"/> representing the next blog entry. If no blog entry is available, the
        /// result may indicate an empty or null state.</returns>
        Task<IBaseResult<BlogPostDto>> GetNextBlogEntryAsync(string blogId, bool trackChanges = false);

        /// <summary>
        /// Retrieves a blog entry based on the specified blog ID.
        /// </summary>
        /// <remarks>If <paramref name="trackChanges"/> is set to <see langword="true"/>, the returned
        /// blog entry will be tracked  for changes, which may impact performance in scenarios involving large
        /// datasets.</remarks>
        /// <param name="blogId">The unique identifier of the blog entry to retrieve. This parameter cannot be null or empty.</param>
        /// <param name="userId">The optional identifier of the user requesting the blog entry. If provided, the result may be tailored to
        /// the user's context.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved blog entry.  <see langword="true"/> to enable
        /// change tracking; otherwise, <see langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// of type <see cref="BlogPostDto"/> representing the requested blog entry.</returns>
        Task<IBaseResult<BlogPostDto>> GetBlogEntryAsync(string blogId, string? userId = null, bool trackChanges = false);

        /// <summary>
        /// Retrieves the previous blog entry for the specified blog.
        /// </summary>
        /// <remarks>Use this method to navigate to the previous blog entry in a sequence. The <paramref
        /// name="trackChanges"/> parameter  is useful for scenarios where change tracking is required, such as in
        /// Entity Framework.</remarks>
        /// <param name="blogId">The unique identifier of the blog. This parameter cannot be null or empty.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved blog entry.  If <see langword="true"/>, the
        /// returned object will be tracked for changes; otherwise, it will not.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// object wrapping the previous blog entry as a <see cref="BlogPostDto"/>. If no previous entry exists, the
        /// result may be empty.</returns>
        Task<IBaseResult<BlogPostDto>> GetPrevBlogEntryAsync(string blogId, bool trackChanges = false);

        /// <summary>
        /// Creates a new blog entry asynchronously.
        /// </summary>
        /// <remarks>Ensure that the <paramref name="blogEntry"/> contains all required fields before
        /// calling this method. The operation may fail if the blog entry violates validation rules or if there are
        /// issues with the underlying data store.</remarks>
        /// <param name="blogEntry">The blog entry to create. Must contain valid data for the blog post, including title and content.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the created <see cref="BlogPostDto"/> if the operation succeeds, or error details if it fails.</returns>
        Task<IBaseResult<BlogPostDto>> CreateBlogEntryAsync(BlogPostDto blogEntry);

        /// <summary>
        /// Updates an existing blog entry with the provided data.
        /// </summary>
        /// <param name="blogEntry">The blog entry data to update. Must include valid identifiers and content.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IBaseResult{T}> object with
        /// the updated <BlogPostDto> if the operation succeeds.</returns>
        Task<IBaseResult<BlogPostDto>> UpdateBlogEntryAsync(BlogPostDto blogEntry);

        /// <summary>
        /// Deletes a blog entry with the specified identifier.
        /// </summary>
        /// <remarks>Use this method to remove a blog entry from the system. Ensure that the <paramref
        /// name="blogId"/>  corresponds to an existing blog entry. The operation may fail if the blog entry does not
        /// exist or  if there are insufficient permissions.</remarks>
        /// <param name="blogId">The unique identifier of the blog entry to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the success or failure of the deletion operation.</returns>
        Task<IBaseResult> DeleteBlogEntryAsync(string blogId);

        /// <summary>
        /// Retrieves the count of unread blog entries for the specified user and category.
        /// </summary>
        /// <remarks>This method can be used to determine the number of unread blog entries for a specific
        /// user and/or category. If both <paramref name="userId"/> and <paramref name="categoryId"/> are <see
        /// langword="null"/>, the method  returns the total count of unread blog entries across all users and
        /// categories.</remarks>
        /// <param name="userId">The unique identifier of the user for whom unread blog entries are being counted. If <see langword="null"/>,
        /// the count is calculated for all users.</param>
        /// <param name="categoryId">The unique identifier of the blog category to filter unread entries. If <see langword="null"/>, the count
        /// includes all categories.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/> 
        /// with the count of unread blog entries as an integer.</returns>
        Task<IBaseResult<int>> UnreadBlogEntryCount(string? userId = null, string? categoryId = null);

        /// <summary>
        /// Retrieves the total number of views for a specified blog entry, optionally filtered by a specific user.
        /// </summary>
        /// <param name="blogEntryId">The unique identifier of the blog entry. Cannot be null or empty.</param>
        /// <param name="userId">The optional identifier of the user. If provided, the views will be filtered to include only those by the
        /// specified user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with the total number of views for the blog entry. If the blog entry does not exist, the result may indicate
        /// an error.</returns>
        Task<IBaseResult<int>> BlogEntryViews(string blogEntryId, string? userId = null);
    }
}
