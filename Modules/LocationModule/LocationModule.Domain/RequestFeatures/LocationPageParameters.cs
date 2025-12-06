using ConectOne.Domain.RequestFeatures;

namespace LocationModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used for paginated location-based requests.
    /// </summary>
    /// <remarks>This class provides properties to configure sorting, pagination, filtering, and search
    /// options for location-related API requests. By default, the results are sorted by "Id", with a page size of
    /// 25.</remarks>
    public class LocationPageParameters : RequestParameters
    {
        #region Consturctors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationPageParameters"/> class with default values.
        /// </summary>
        /// <remarks>The default values are: <list type="bullet"> <item><description><see cref="OrderBy"/>
        /// is set to "Id".</description></item> <item><description><see cref="PageSize"/> is set to
        /// 25.</description></item> </list> These defaults can be modified after initialization by setting the
        /// respective properties.</remarks>
        public LocationPageParameters()
        {
            OrderBy = "Id";
            PageSize = 25;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationPageParameters"/> class with the specified pagination,
        /// sorting, and filtering options.
        /// </summary>
        /// <param name="sortOrder">The field by which to sort the results. Defaults to "Id".</param>
        /// <param name="pageNr">The page number to retrieve. Must be 1 or greater. Defaults to 1.</param>
        /// <param name="pageSize">The number of items per page. Must be 1 or greater. Defaults to 25.</param>
        /// <param name="active">A value indicating whether to filter results to only active items. Defaults to <see langword="true"/>.</param>
        /// <param name="searchText">An optional search term to filter results. Defaults to an empty string.</param>
        public LocationPageParameters(string sortOrder = "Id", int pageNr = 1, int pageSize = 25, bool active = true, string? searchText = "")
        {
            OrderBy = sortOrder;
            PageNr = pageNr;
            PageSize = pageSize;
            Active = active;
            SearchText = searchText;
        }

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; } = true;
    }
}