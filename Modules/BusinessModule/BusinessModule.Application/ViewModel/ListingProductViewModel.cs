using BusinessModule.Domain.DataTransferObjects;
using FilingModule.Domain.DataTransferObjects;

namespace BusinessModule.Application.ViewModel
{
    /// <summary>
    /// Represents a view model for listing product information, including details such as name, description, price, and
    /// associated company.
    /// </summary>
    /// <remarks>This class is typically used to transfer product data between the application layers, such as
    /// from a database or API to a user interface.</remarks>
    public class ListingProductViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingProductViewModel"/> class.
        /// </summary>
        public ListingProductViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingProductViewModel"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <remarks>The <paramref name="dto"/> parameter must not be null. The properties of the view
        /// model are populated based on the corresponding properties of the provided <see
        /// cref="ListingProductDto"/>.</remarks>
        /// <param name="dto">The data transfer object containing the product details to initialize the view model.</param>
        public ListingProductViewModel(ListingProductDto dto)
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
        public string Id { get; set; }

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
        public ICollection<ImageDto>? Images { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the product to a <see cref="ListingProductDto"/>.
        /// </summary>
        /// <returns>A <see cref="ListingProductDto"/> representing the current product, including its identifier, name,
        /// description, price, associated listing ID, and cover image to upload.</returns>
        public ListingProductDto ToDto()
        {
            return new ListingProductDto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Price = Price,
                ListingId = ListingId,
                CoverImageToUpload = CoverImageToUpload
            };
        }

        #endregion
    }
}
