using ConectOne.Domain.RequestFeatures;

namespace CalendarModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used for paginated calendar requests, including optional date range filtering and
    /// search functionality.
    /// </summary>
    /// <remarks>This class extends <see cref="RequestParameters"/> to include additional properties specific
    /// to calendar-related queries,  such as filtering by a date range or searching by a specific text.</remarks>
    public class CalendarPageParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarPageParameters"/> class.
        /// </summary>
        public CalendarPageParameters() { }

        /// <summary>
        /// Represents the parameters used for paginated and sorted calendar data retrieval.
        /// </summary>
        /// <param name="sortOrder">The order in which the calendar data should be sorted. This can be a field name or a predefined sort key.</param>
        /// <param name="pageNr">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of items to include per page. Defaults to 50.</param>
        /// <param name="searchString">The search text used to filter calendar data. Defaults to an empty string, which means no filtering is
        /// applied.</param>
        public CalendarPageParameters(string sortOrder, int pageNr = 1, int pageSize = 50, string searchString = "") : base(pageNr, pageSize, sortOrder)
        {
            SearchText = searchString;
        }

        #endregion

        /// <summary>
        /// Gets or sets the start date of the event or process.
        /// </summary>
        public DateTime StartDate { get; set; } = new DateTime(1900, 1,1);

        /// <summary>
        /// Gets or sets the end date of the event or time period.
        /// </summary>
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
    }
}
