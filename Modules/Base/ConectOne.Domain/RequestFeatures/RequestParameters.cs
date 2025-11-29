namespace ConectOne.Domain.RequestFeatures
{
    /// <summary>
    /// Represents request parameters used for pagination, sorting, and searching in API requests.
    /// </summary>
    public class RequestParameters
    {
        private int _maxPageSize = 100;
        private int _pageSize = 12;

        #region Constructors

        /// <summary>
        /// Default constructor initializes the request parameters with default values.
        /// </summary>
        public RequestParameters() { }

        /// <summary>
        /// Parameterized constructor to initialize request parameters with specific values.
        /// </summary>
        /// <param name="pageNr">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="orderBy">The field by which to order the results.</param>
        public RequestParameters(int pageNr, int pageSize, string? orderBy)
        {
            PageNr = pageNr;
            PageSize = pageSize; // The setter ensures it does not exceed the max limit.
            OrderBy = orderBy;
        }

        #endregion

        /// <summary>
        /// Gets or sets the page number for pagination. Defaults to 1.
        /// </summary>
        public int PageNr { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of items per page. 
        /// Ensures that the page size does not exceed the maximum allowed size.
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                // Restrict the page size to not exceed the defined maximum.
                _pageSize = value > _maxPageSize ? _maxPageSize : value;
            }
        }

        /// <summary>
        /// Gets or sets the field name by which the results should be ordered.
        /// Can be null if no specific ordering is required.
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// Gets or sets the search text for filtering results.
        /// Can be null if no filtering is needed.
        /// </summary>
        public string? SearchText { get; set; }
    }
}
