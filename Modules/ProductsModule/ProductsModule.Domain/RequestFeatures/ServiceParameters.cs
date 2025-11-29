using ConectOne.Domain.RequestFeatures;

namespace ProductsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a set of parameters used to filter and paginate service-related data.
    /// </summary>
    /// <remarks>This class provides properties for specifying filtering criteria, such as price range,
    /// activity status,  featured status, and text-based search, as well as pagination and sorting options. It is
    /// designed to  facilitate querying and retrieving service data in a structured and customizable manner.</remarks>
    public class ServiceParameters : RequestParameters
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the minimum price allowed for a product or transaction.
        /// </summary>
        public double MinPrice { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowable price for a product or service.
        /// </summary>
        public double MaxPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
        public bool? Featured { get; set; }

        /// <summary>
        /// Gets or sets the department filter used to narrow down results.
        /// </summary>
        public string? DepartmentFilter { get; set; }

        /// <summary>
        /// Gets or sets the category filter used to limit results to a specific category.
        /// </summary>
        public string? CategoryFilter { get; set; }
    }
}
