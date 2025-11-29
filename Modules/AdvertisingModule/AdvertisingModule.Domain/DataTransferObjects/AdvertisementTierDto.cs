using AdvertisingModule.Domain.Entities;
using AdvertisingModule.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;

namespace AdvertisingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a tier of advertisement with specific attributes such as name, description, availability, and
    /// duration.
    /// </summary>
    /// <remarks>This class is used to define the characteristics of an advertisement tier, including its
    /// name, a description of the tier,  the number of advertisements available in the tier, and the duration (in days)
    /// for which the tier is valid.</remarks>
    public class AdvertisementTierDto 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertisementTierDto"/> class with default values.
        /// </summary>
        /// <remarks>The default constructor initializes the <see cref="Id"/> property to a new unique
        /// identifier and sets all other properties to their default values. The <see cref="Selector"/> property is
        /// initialized to null.</remarks>
        public AdvertisementTierDto()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            ShortDescription = string.Empty;
            Description = string.Empty;
            AvailabilityCount = 0;
            Days = 0;
            Order = 0;
            Price = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertisementTierDto"/> class using the specified advertisement
        /// tier.
        /// </summary>
        /// <param name="tier">The advertisement tier from which to initialize the DTO. Must not be <see langword="null"/>.</param>
        public AdvertisementTierDto(AdvertisementTier tier)
        {
            Id = tier.Id;
            Name = tier.Name;
            ShortDescription = tier.ShortDescription;
            Description = tier.Description;
            AvailabilityCount = tier.AvailabilityCount;
            Days = tier.Days;
            Order = tier.Order;
            AdvertisementType = tier.AdvertisementType;
            Price = tier.Price;
            Images = tier.Images.Select(ImageDto.ToDto).ToList();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets or sets the description of the object.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets a brief description of the object.
        /// </summary>
        public string? ShortDescription { get; init; }

        /// <summary>
        /// Gets or sets the number of items currently available.
        /// </summary>
        public int AvailabilityCount { get; init; }

        /// <summary>
        /// Gets the number of days represented by this instance.
        /// </summary>
        public int Days { get; init; }

        /// <summary>
        /// Gets or sets the order identifier associated with the current operation.
        /// </summary>
        public int Order { get; init; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price { get; init; }

        /// <summary>
        /// Gets or sets the CSS selector used to identify elements in the DOM.
        /// </summary>
        /// <remarks>The selector must be a valid CSS selector. If the value is null or empty, no elements
        /// will be matched.</remarks>
        public AdvertisementType AdvertisementType { get; init; }

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance to an <see cref="AdvertisementTier"/> entity.
        /// </summary>
        /// <returns>A new <see cref="AdvertisementTier"/> instance populated with the values from the current object.</returns>
        public AdvertisementTier ToEntity()
        {
            return new AdvertisementTier
            {
                Id = this.Id,
                Name = this.Name,
                ShortDescription = this.ShortDescription,
                Description = this.Description,
                AvailabilityCount = this.AvailabilityCount,
                Days = this.Days,
                Order = this.Order,
                Price = this.Price,
                AdvertisementType = this.AdvertisementType
            };
        }

        #endregion
    }
}
