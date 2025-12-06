using AccomodationModule.Domain.Enums;
using ConectOne.Domain.RequestFeatures;

namespace AccomodationModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used for requesting a paginated list of lodging listings.
    /// </summary>
    /// <remarks>This class provides options for filtering, sorting, and paginating lodging listings. It
    /// includes properties for specifying booking status, search text, and other criteria.</remarks>
    public class LodgingListingRequestParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingListingRequestParameters"/> class with default values
        /// for sorting and pagination.
        /// </summary>
        /// <remarks>The default values are: <list type="bullet"> <item><description><c>OrderBy</c>:
        /// "Name"</description></item> <item><description><c>PageSize</c>: 12</description></item> </list> These
        /// defaults can be modified after initialization by setting the corresponding properties.</remarks>
        public LodgingListingRequestParameters()
        {
            OrderBy = "Name";
            PageSize = 12;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingListingRequestParameters"/> class with specified
        /// parameters for filtering and sorting lodging listings.
        /// </summary>
        /// <remarks>This constructor allows you to specify parameters for paginated and filtered lodging
        /// listings. Use the <paramref name="sortOrder"/> parameter to define the sorting criteria, and the <paramref
        /// name="status"/> parameter to filter by booking status. The <paramref name="pageNr"/> and <paramref
        /// name="pageSize"/> parameters control pagination.</remarks>
        /// <param name="sortOrder">The order in which the results should be sorted. Typically, this is a field name followed by a direction
        /// (e.g., "Name ASC").</param>
        /// <param name="status">The booking status used to filter the listings. Defaults to <see cref="BookingStatus.Pending"/>.</param>
        /// <param name="pageNr">The page number of the results to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of items to include per page. Defaults to 50.</param>
        /// <param name="active">A value indicating whether to filter listings based on their active status. Defaults to <see
        /// langword="true"/>.</param>
        /// <param name="searchText">An optional text used to search within the listings. If <see langword="null"/>, no search filter is applied.</param>
        public LodgingListingRequestParameters(string sortOrder, BookingStatus status = BookingStatus.Pending, int pageNr = 1, int pageSize = 50, bool active = true, 
            string? searchText = null) : base(pageNr, pageSize, sortOrder)
        {
            OrderBy = sortOrder;
            Statuses = status;
            SearchText= searchText;
        }

        #endregion

        /// <summary>
        /// Gets or sets the current booking status.
        /// </summary>
        public BookingStatus Statuses { get; set; } = BookingStatus.Pending;
    }
}
