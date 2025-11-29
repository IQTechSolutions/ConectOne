using ConectOne.Domain.RequestFeatures;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Parameters for paging and filtering activity groups.
    /// </summary>
    public class ActivityGroupPageParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ActivityGroupPageParameters() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityGroupPageParameters"/> class with specified parameters.
        /// </summary>
        /// <param name="sortOrder">The sort order for the activity groups.</param>
        /// <param name="pageNr">The page number for pagination.</param>
        /// <param name="pageSize">The size of each page for pagination.</param>
        /// <param name="searchString">The search text for filtering activity groups.</param>
        public ActivityGroupPageParameters(string? sortOrder, int pageNr = 1, int pageSize = 50, string searchString = "") : base(pageNr, pageSize, sortOrder)
        {
            SearchText = searchString;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the event ID for filtering activity groups.
        /// </summary>
        public string? EventId { get; set; }

        /// <summary>
        /// Gets or sets the learner ID for filtering activity groups.
        /// </summary>
        public string? LearnerId { get; set; }

        /// <summary>
        /// Gets or sets the category IDs for filtering activity groups.
        /// </summary>
        public string? CategoryIds { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the coach.
        /// </summary>
        public string? CoachEmail { get; set; }

        #endregion
    }
}
