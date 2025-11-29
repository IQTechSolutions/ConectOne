using AdvertisingModule.Domain.Enums;
using FilingModule.Domain.Entities;

namespace AdvertisingModule.Domain.Entities
{
    /// <summary>
    /// Represents a tier of advertisement with specific attributes such as name, description, availability, and
    /// duration.
    /// </summary>
    /// <remarks>This class is used to define the characteristics of an advertisement tier, including its
    /// name, a description of the tier,  the number of advertisements available in the tier, and the duration (in days)
    /// for which the tier is valid.</remarks>
    public class AdvertisementTier : FileCollection<AdvertisementTier, string>
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a brief description of the object.
        /// </summary>
        public string? ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the description of the object.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the number of items currently available.
        /// </summary>
        public int AvailabilityCount { get; set; }

        /// <summary>
        /// Gets or sets the number of days.
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Gets or sets the order identifier associated with the current operation.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the CSS selector used to identify elements in the DOM.
        /// </summary>
        /// <remarks>The selector must be a valid CSS selector. If the value is null or empty, no elements
        /// will be matched.</remarks>
        public AdvertisementType AdvertisementType { get; set; }

        /// <summary>
        /// Gets or sets the collection of advertisements associated with this entity.
        /// </summary>
        public virtual ICollection<Advertisement> Advertisements { get; set; } = [];
    }
}
