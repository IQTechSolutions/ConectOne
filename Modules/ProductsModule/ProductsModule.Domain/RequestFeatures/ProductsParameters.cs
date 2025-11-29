using ConectOne.Domain.RequestFeatures;

namespace ProductsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a set of parameters used to filter and paginate product queries.
    /// </summary>
    /// <remarks>This class provides properties for specifying filtering criteria such as price range, active
    /// status, featured status,  and text-based search, as well as options for sorting and pagination. It is designed
    /// to be used in scenarios where  products need to be queried based on various criteria.</remarks>
    public class ProductsParameters : RequestParameters
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets the minimum price allowed for a product or service.
        /// </summary>
        public double MinPrice { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowable price for a product or service.
        /// </summary>
        public double MaxPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
        public bool Featured { get; set; } = true;

        /// <summary>
        /// Gets or sets the filter for department IDs.
        /// </summary>
        public string? DepartmentIdFilter { get; set; }

        /// <summary>
        /// Gets or sets the filter for category IDs.
        /// </summary>
        public string? CategoryIdFilter { get; set; }

        /// <summary>
        /// Gets or sets the filter for brand identifiers.
        /// </summary>
        public string? BrandIdFilter { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the shop owner.
        /// </summary>
        public string? ShopOwnerId { get; set; }
    }
}
