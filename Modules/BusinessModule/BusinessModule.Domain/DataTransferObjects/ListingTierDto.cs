using BusinessModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;

namespace BusinessModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a listing tier, which defines the properties and pricing details of
    /// a specific tier in a listing system.
    /// </summary>
    /// <remarks>This class is used to transfer data between different layers of the application, such as
    /// between the domain model and the presentation layer. It includes properties for the tier's unique identifier,
    /// name, descriptions, order, price, and associated images.</remarks>
    public class ListingTierDto 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingTierDto"/> class with default values.
        /// </summary>
        /// <remarks>The <see cref="Id"/> property is initialized to a new unique identifier as a string.
        /// The <see cref="Name"/> and <see cref="Description"/> properties are initialized to empty strings. The <see
        /// cref="Order"/> and <see cref="Price"/> properties are initialized to 0.</remarks>
        public ListingTierDto()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            Description = string.Empty;
            Order = 0;
            Price = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingTierDto"/> class using the specified <see
        /// cref="ListingTier"/>.
        /// </summary>
        /// <param name="tier">The <see cref="ListingTier"/> object containing the data to initialize the DTO. Cannot be <see
        /// langword="null"/>.</param>
        public ListingTierDto(ListingTier tier)
        {
            Id = tier.Id;
            Name = tier.Name;
            Description = tier.Description;
            Order = tier.Order;
            Price = tier.Price;
            AllowServiceAndProductListing = tier.AllowServiceAndProductListing;
            Images = tier.Images.Select(ImageDto.ToDto).ToList();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Gets the unique identifier for the object.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets a brief description or summary of the object.
        /// </summary>
        public string? ShortDescription { get; init; }

        /// <summary>
        /// Gets the description associated with the object.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets the order in which this item should be processed or displayed.
        /// </summary>
        public int Order { get; init; } = 0;

        /// <summary>
        /// Gets the price of the item.
        /// </summary>
        public double Price { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether services and products can be listed.
        /// </summary>
        public bool AllowServiceAndProductListing { get; init; }

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public List<ImageDto> Images { get; init; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance to a <see cref="ListingTier"/> entity.
        /// </summary>
        /// <returns>A <see cref="ListingTier"/> object populated with the values from the current instance.</returns>
        public ListingTier ToEntity()
        {
            return new ListingTier
            {
                Id = this.Id,
                Name = this.Name,
                ShortDescription = this.ShortDescription,
                Description = this.Description,
                Order = this.Order,
                Price = this.Price,
                AllowServiceAndProductListing = this.AllowServiceAndProductListing
            };
        }

        #endregion
    }
}