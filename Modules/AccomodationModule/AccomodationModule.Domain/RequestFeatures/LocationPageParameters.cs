using ConectOne.Domain.RequestFeatures;

namespace AccomodationModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used for paginated requests to retrieve location data.
    /// </summary>
    /// <remarks>This class provides properties for specifying filtering, sorting, and pagination options when
    /// querying location-related data. It includes parameters such as location and destination identifiers, sorting
    /// order, page number, page size, and whether to include only active locations.</remarks>
    public class LocationPageParameters : RequestParameters
    {
        #region Consturctors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationPageParameters"/> class with default values.
        /// </summary>
        /// <remarks>The default values are: <list type="bullet"> <item><description><c>OrderBy</c>: "Code
        /// asc"</description></item> <item><description><c>PageSize</c>: 25</description></item> </list> These defaults
        /// can be modified after initialization by setting the corresponding properties.</remarks>
        public LocationPageParameters()
        {
            OrderBy = "Code asc";
            PageSize = 25;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationPageParameters"/> class with the specified parameters
        /// for paginated location data retrieval.
        /// </summary>
        /// <remarks>This constructor is typically used to configure parameters for paginated queries
        /// involving location data. Ensure that <paramref name="locationId"/> and <paramref name="destinationId"/> are
        /// valid identifiers.</remarks>
        /// <param name="locationId">The unique identifier of the location. Cannot be null or empty.</param>
        /// <param name="destinationId">The unique identifier of the destination. Cannot be null or empty.</param>
        /// <param name="sortOrder">The sort order for the results, specified as a string. Defaults to "Code asc".</param>
        /// <param name="pageNr">The page number to retrieve. Defaults to 1. Must be greater than or equal to 1.</param>
        /// <param name="pageSize">The number of items per page. Defaults to 25. Must be greater than 0.</param>
        /// <param name="active">A value indicating whether to filter results by active status. Defaults to <see langword="true"/>.</param>
        /// <param name="searchText">An optional search text to filter results. Can be null or empty.</param>
        public LocationPageParameters(string locationId, string destinationId, string sortOrder = "Code asc", int pageNr = 1, int pageSize = 25, bool active = true, string? searchText = "")
        {
            OrderBy = sortOrder;
            PageNr = pageNr;
            PageSize = pageSize;
            Active = active;
            SearchText = searchText;
            LocationId = locationId;
            DestinationId = destinationId;
        }

        #endregion

        /// <summary>
        /// Gets or sets the identifier for the location.
        /// </summary>
        public string? LocationId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the destination.
        /// </summary>
        public string? DestinationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; } = true;
    }
}