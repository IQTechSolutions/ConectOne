namespace ConectOne.Domain.ResultWrappers
{
    /// <summary>
    /// Represents a paginated result of an operation, including success status, messages, and pagination details.
    /// </summary>
    /// <typeparam name="T">The type of the data payload.</typeparam>
    public class PaginatedResult<T> : Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResult{T}"/> class with the specified data.
        /// </summary>
        /// <param name="data">The data payload.</param>
        public PaginatedResult(List<T> data)
        {
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResult{T}"/> class with the specified parameters.
        /// </summary>
        /// <param name="succeeded">Indicates whether the operation succeeded.</param>
        /// <param name="data">The data payload.</param>
        /// <param name="messages">The messages associated with the result.</param>
        /// <param name="count">The total count of items.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        internal PaginatedResult(bool succeeded, List<T>? data = default, List<string>? messages = null, int count = 0, int page = 1, int pageSize = 10)
        {
            Data = data?.Skip((page - 1) * pageSize).Take(pageSize).ToList() ?? new List<T>();
            CurrentPage = page;
            Succeeded = succeeded;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Messages = messages ?? new List<string>();
        }

        /// <summary>
        /// Gets or sets the data payload.
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total count of items.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the size of each page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;

        /// <summary>
        /// Creates a failed paginated result with the specified messages.
        /// </summary>
        /// <param name="messages">The failure messages.</param>
        /// <returns>A failed paginated result with the specified messages.</returns>
        public static PaginatedResult<T> Failure(List<string> messages)
        {
            return new PaginatedResult<T>(false, default, messages);
        }

        /// <summary>
        /// Creates a successful paginated result with the specified data, count, page, and page size.
        /// </summary>
        /// <param name="data">The data payload.</param>
        /// <param name="count">The total count of items.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <returns>A successful paginated result with the specified data, count, page, and page size.</returns>
        public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize)
        {
            return new PaginatedResult<T>(true, data, null, count, page, pageSize);
        }
    }
}
