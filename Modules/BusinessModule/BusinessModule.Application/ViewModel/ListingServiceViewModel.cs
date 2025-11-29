using BusinessModule.Domain.DataTransferObjects;
using FilingModule.Domain.DataTransferObjects;

namespace BusinessModule.Application.ViewModel
{
    /// <summary>
    /// Represents a view model for a listing service, encapsulating details such as  the service's identifier, name,
    /// description, price, and associated listing identifier.
    /// </summary>
    /// <remarks>This class is typically used to transfer data between the application and the UI layer  for
    /// operations related to listing services. It can be initialized directly or populated  using a <see
    /// cref="ListingServiceDto"/>.</remarks>
    public class ListingServiceViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingServiceViewModel"/> class.
        /// </summary>
        public ListingServiceViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingServiceViewModel"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="ListingServiceDto"/> 
        /// to the corresponding properties of the <see cref="ListingServiceViewModel"/>.</remarks>
        /// <param name="dto">The <see cref="ListingServiceDto"/> containing the data to initialize the view model.</param>
        public ListingServiceViewModel(ListingServiceDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            Price = dto.Price;
            ListingId = dto.ListingId;
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
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the listing.
        /// </summary>
        public string ListingId { get; set; }

        /// <summary>
        /// Gets or sets the file path of the cover image to be uploaded.
        /// </summary>
        public string? CoverImageToUpload { get; set; }

        /// <summary>
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the listing service to a <see cref="ListingServiceDto"/> object.
        /// </summary>
        /// <returns>A <see cref="ListingServiceDto"/> object containing the data from the current instance.</returns>
        public ListingServiceDto ToDto()
        {
            return new ListingServiceDto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Price = Price,
                ListingId = ListingId,
                Images = Images
            };
        }

        #endregion
    }
}
