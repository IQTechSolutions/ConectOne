using AdvertisingModule.Domain.DataTransferObjects;
using AdvertisingModule.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;

namespace AdvertisingModule.Application.ViewModels
{
    /// <summary>
    /// Represents a tier of advertisement with specific attributes such as name, description, availability, and
    /// duration.
    /// </summary>
    /// <remarks>This class is used to define the characteristics of an advertisement tier, including its
    /// name, a description of the tier,  the number of advertisements available in the tier, and the duration (in days)
    /// for which the tier is valid.</remarks>
    public class AdvertisementTierViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertisementTierViewModel"/> class.
        /// </summary>
        /// <remarks>This constructor initializes the instance with default values.  The <see cref="Id"/>
        /// property is set to a new GUID string,  and other properties are initialized to their default
        /// values.</remarks>
        public AdvertisementTierViewModel()
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            Description = string.Empty;
            AvailabilityCount = 0;
            Days = 0;
            Order = 0;
            Price = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertisementTierViewModel"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see
        /// cref="AdvertisementTierDto"/> to the corresponding properties of the <see
        /// cref="AdvertisementTierViewModel"/>.</remarks>
        /// <param name="dto">The data transfer object containing the advertisement tier details.</param>
        public AdvertisementTierViewModel(AdvertisementTierDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            ShortDescription = dto.ShortDescription;
            Description = dto.Description;
            AvailabilityCount = dto.AvailabilityCount;
            Days = dto.Days;
            Order = dto.Order;
            AdvertisementType = dto.AdvertisementType;
            Price = dto.Price;
            Images = dto.Images;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
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
        /// Gets or sets the collection of images associated with the entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AdvertisementTierDto ToDto()
        {
            return new AdvertisementTierDto()
            {
                Id = Id,
                Name = Name,
                ShortDescription = ShortDescription,
                Description = Description,
                AvailabilityCount = AvailabilityCount,
                Days = Days,
                Order = Order,
                AdvertisementType = AdvertisementType,
                Price = Price
            };
        }

        #endregion
    }

}
