using BusinessModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;

namespace BusinessModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a view model for listing product information, including details such as name, description, price, and
    /// associated company.
    /// </summary>
    /// <remarks>This class is typically used to transfer product data between the application layers, such as
    /// from a database or API to a user interface.</remarks>
    public record ListingProductDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingProductDto"/> class.
        /// </summary>
        public ListingProductDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingProductDto"/> class using the specified service data.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="ListingProduct"/>
        /// object to the corresponding properties of the <see cref="ListingProductDto"/> instance.</remarks>
        /// <param name="service">The <see cref="ListingProduct"/> instance containing the data to populate the DTO.</param>
        public ListingProductDto(ListingProduct service)
        {
            Id = service.Id;
            Name = service.Name;
            Description = service.Description;
            Price = service.Price;
            ListingId = service.ListingId;
            Images = service.Images is not null ? service.Images.Select(c => ImageDto.ToDto(c.Image)).ToList() : [];
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public decimal Price { get; init; }

        /// <summary>
        /// Gets or sets the unique identifier for the listing.
        /// </summary>
        public string ListingId { get; init; }

        /// <summary>
        /// Gets or sets the file path of the cover image to be uploaded.
        /// </summary>
        public string? CoverImageToUpload { get; init; }

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; init; } = [];
    }
}