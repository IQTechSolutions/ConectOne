using BusinessModule.Domain.DataTransferObjects;
using FilingModule.Domain.DataTransferObjects;

namespace BusinessModule.Application.ViewModel
{
    /// <summary>
    /// Represents a view model for a listing tier, including details such as name, description, price, and associated
    /// images.
    /// </summary>
    /// <remarks>This class is typically used to transfer data between the application and the presentation
    /// layer. It provides a structured representation of a listing tier, which may include optional descriptions and
    /// images.</remarks>
    public class ListingTierViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingTierViewModel"/> class with default values.
        /// </summary>
        /// <remarks>The <see cref="Id"/> property is initialized to a new unique identifier as a string.
        /// The <see cref="Name"/> and <see cref="Description"/> properties are initialized to empty strings. The <see
        /// cref="Order"/> property is set to 0, and the <see cref="Price"/> property is set to 0.</remarks>
        public ListingTierViewModel()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            Description = string.Empty;
            Order = 0;
            Price = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingTierViewModel"/> class using the specified data transfer
        /// object.
        /// </summary>
        /// <remarks>The constructor maps the properties of the provided <see cref="ListingTierDto"/> to
        /// the corresponding properties of the <see cref="ListingTierViewModel"/>. Ensure that the <paramref
        /// name="dto"/> parameter is properly populated before calling this constructor.</remarks>
        /// <param name="dto">The data transfer object containing the listing tier details. Cannot be null.</param>
        public ListingTierViewModel(ListingTierDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            Order = dto.Order;
            Price = dto.Price;
            AllowServiceAndProductListing = dto.AllowServiceAndProductListing;
            Images = dto.Images;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a brief description of the object.
        /// </summary>
        public string? ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the order in which this item is processed or displayed.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether services and products can be listed.
        /// </summary>
        public bool AllowServiceAndProductListing { get; set; }

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public List<ImageDto> Images { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the <see cref="ListingTier"/> class to a <see cref="ListingTierDto"/>.
        /// </summary>
        /// <returns>A <see cref="ListingTierDto"/> object that represents the current instance, including its identifier, name,
        /// description, order, price, and other associated properties.</returns>
        public ListingTierDto ToDto()
        {
            return new ListingTierDto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Order = Order,
                Price = Price,
                AllowServiceAndProductListing = AllowServiceAndProductListing,
                Images = Images
            };
        }

        #endregion
    }
}
