using ConectOne.Domain.Enums;
using ConectOne.Domain.RequestFeatures;

namespace AdvertisingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used to request a paginated list of business listings.
    /// </summary>
    /// <remarks>This class is typically used to specify filtering options, such as the category of
    /// businesses, when retrieving business listings from an API or data source.</remarks>
    public class AdvertisementListingPageParameters : RequestParameters
    {
        /// <summary>
        /// Gets or sets the identifier of the category.
        /// </summary>
        public string? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the approval status as a string.
        /// </summary>
        public ReviewStatus? Status { get; set; } 
    }
}
