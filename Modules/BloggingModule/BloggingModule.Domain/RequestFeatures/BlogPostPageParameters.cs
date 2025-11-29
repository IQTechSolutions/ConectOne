using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.RequestFeatures;

namespace BloggingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used for paginated requests to retrieve blog posts.
    /// </summary>
    /// <remarks>This class provides filtering, sorting, and pagination options for querying blog posts. It
    /// includes properties for specifying date ranges, featured status, search text, and category filters.</remarks>
    public class BlogPostPageParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostPageParameters"/> class.
        /// </summary>
        /// <remarks>By default, the <see cref="OrderBy"/> property is set to "Title".</remarks>
        public BlogPostPageParameters()
        {
            OrderBy = "Title";
        }

        /// <summary>
        /// Represents the parameters used for paginated blog post queries, including sorting, filtering, and search
        /// options.
        /// </summary>
        /// <param name="sortOrder">The order in which blog posts should be sorted. Typically "asc" for ascending or "desc" for descending.</param>
        /// <param name="pageNr">The page number to retrieve. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of blog posts to include per page. Must be greater than 0.</param>
        /// <param name="startDateFilter">An optional filter specifying the earliest publication date for blog posts. Pass <see langword="null"/> to
        /// omit this filter.</param>
        /// <param name="endDateFilter">An optional filter specifying the latest publication date for blog posts. Pass <see langword="null"/> to
        /// omit this filter.</param>
        /// <param name="featured">A value indicating whether to filter for featured blog posts. <see langword="true"/> to include only
        /// featured posts; otherwise, <see langword="false"/>.</param>
        /// <param name="searchText">An optional text string to search for within blog post content or titles. Pass <see langword="null"/> or an
        /// empty string to omit this filter.</param>
        public BlogPostPageParameters(string sortOrder, int pageNr, int pageSize, DateTime? startDateFilter, DateTime? endDateFilter, bool featured, string? searchText) : base(pageNr, pageSize, sortOrder)
        {           
            StartDateFilter = startDateFilter;
            EndDateFilter = endDateFilter;
            Featured = featured;
            SearchText = searchText;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the category.
        /// </summary>
		public string? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the start date used to filter results.
        /// </summary>
		[DataType(DataType.Date)] public DateTime? StartDateFilter { get; set; }

        /// <summary>
        /// Gets or sets the end date used to filter results.
        /// </summary>
        [DataType(DataType.Date)] public DateTime? EndDateFilter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
        public bool Featured { get; set; } = true;
    }
}
