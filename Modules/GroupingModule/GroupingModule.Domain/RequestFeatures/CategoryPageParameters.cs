using ConectOne.Domain.RequestFeatures;

namespace GroupingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used to filter and retrieve category pages in a request.
    /// </summary>
    /// <remarks>This class provides options for specifying parent category relationships,  filtering by
    /// active status, and optionally filtering by featured status.</remarks>
    public class CategoryPageParameters : RequestParameters
    {
        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
        public bool? Featured { get; set; }
    }
}
