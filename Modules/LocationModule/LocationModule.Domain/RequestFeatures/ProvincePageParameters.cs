using ConectOne.Domain.RequestFeatures;

namespace LocationModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used for paginated requests to retrieve province data.
    /// </summary>
    /// <remarks>This class provides options for specifying sorting, pagination, filtering by active status, 
    /// and search text when querying province data. By default, the results are sorted by "Id"  and include 25 items
    /// per page.</remarks>
    public class ProvincePageParameters : RequestParameters
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; } = true;
    }
}