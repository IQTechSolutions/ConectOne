using BusinessModule.Domain.Entities;
using FilingModule.Domain.DataTransferObjects;

namespace BusinessModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object for a listing service, containing details about a specific listing.
    /// </summary>
    /// <remarks>This DTO is used to encapsulate information about a listing, including its unique identifier,
    /// name,  description, price, and associated listing ID. It is typically used for transferring data between 
    /// application layers or services.</remarks>
    public record ListingServiceDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingServiceDto"/> class.
        /// </summary>
        public ListingServiceDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingServiceDto"/> class using the specified <see
        /// cref="ListingService"/> object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="ListingService"/>
        /// object to the corresponding properties of the <see cref="ListingServiceDto"/> instance.</remarks>
        /// <param name="service">The <see cref="ListingService"/> object containing the data to initialize the DTO.</param>
        public ListingServiceDto(ListingService service)
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
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string? Name { get; init; } 

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
        public string? ListingId { get; init; }

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