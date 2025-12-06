using ConectOne.Domain.RequestFeatures;

namespace AccomodationModule.Domain.Arguments
{
    /// <summary>
    /// Represents the parameters used for paginated requests to retrieve destination pages.
    /// </summary>
    /// <remarks>This class provides properties for specifying sorting, pagination, and filtering criteria
    /// when querying destination pages. It includes default values for common use cases.</remarks>
    public class DestinationPageParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DestinationPageParameters"/> class with default values for
        /// pagination and sorting.
        /// </summary>
        /// <remarks>The default values are: <list type="bullet"> <item><description><c>OrderBy</c>:
        /// "Name"</description></item> <item><description><c>PageSize</c>: 12</description></item> </list> These
        /// defaults can be modified after initialization by setting the corresponding properties.</remarks>
        public DestinationPageParameters()
        {
            OrderBy = "Name";
            PageSize = 12;
        }

        /// <summary>
        /// Represents the parameters required for paginated navigation and sorting on a destination page.
        /// </summary>
        /// <param name="sortOrder">The sorting order to apply to the data. Typically specifies the field and direction (e.g., "Name ASC").</param>
        /// <param name="pageNr">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of items to include per page. Defaults to 50.</param>
        /// <param name="searchText">Optional search text used to filter the results. Can be null if no filtering is required.</param>
        public DestinationPageParameters(string sortOrder, int pageNr = 1, int pageSize = 50, string? searchText = null) : base(pageNr, pageSize, sortOrder)
        {
            OrderBy = sortOrder;
            SearchText= searchText;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        public string? VacationId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a vacation extension.
        /// </summary>
        public string? VacationExtensionId { get; set; }
    }
}
