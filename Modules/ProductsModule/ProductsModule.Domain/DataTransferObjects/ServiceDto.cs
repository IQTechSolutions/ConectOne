using CalendarModule.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Entities;
using ProductsModule.Domain.Enums;

namespace ProductsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a service, encapsulating its details, metadata, and associated
    /// collections.
    /// </summary>
    /// <remarks>This class is designed to provide a simplified representation of a service entity for use in
    /// data transfer scenarios,  such as API responses or inter-layer communication. It includes properties for the
    /// service's core attributes,  such as its name, description, and pricing, as well as collections for related
    /// categories, images, and videos.</remarks>
    public class ServiceDto 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDto"/> class.
        /// </summary>
        public ServiceDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDto"/> class using the specified <see cref="Service"/>
        /// object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="Service"/> object to
        /// the corresponding properties of the <see cref="ServiceDto"/>. Collections such as categories, images, and
        /// videos are converted to their respective DTO representations.</remarks>
        /// <param name="service">The <see cref="Service"/> object from which to populate the properties of the <see cref="ServiceDto"/>
        /// instance.</param>
        public ServiceDto(OfferedService service)
        {
             Id = service.Id;
             Name = service.Name;
             DisplayName = service.DisplayName;
             ShortDescription = service.ShortDescription;
             Description = service.Description;
             BillingFrequency = service.BillingFrequency;
             ServiceFrequency = service.ServiceFrequency;
             PriceTableItem = service.PriceTableItem;
             Featured = service.Featured;
             Active = service.Active;
             Tags = service.Tags;
             Price = service.Price;
             DoNotDisplayInCatalogs = service.DoNotDisplayInCatalogs;
             Categories = service.Categories.Select(c => CategoryDto.ToCategoryDto(c.Category)).ToList();
             Images = service.Images.Select(f => ImageDto.ToDto(f)).ToList();
             Videos = service.Videos.Select(v => VideoDto.ToDto(v)).ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// The name of the service
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// The display name of the service
        /// </summary>
        public string DisplayName { get; init; }

        /// <summary>
        /// A brief description of the service
        /// </summary>
        public string? ShortDescription { get; init; }

        /// <summary>
        /// A full description of the service
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// The frequency at which this service is billed for
        /// </summary>        
        public BillingFrequency BillingFrequency { get; init; }

        /// <summary>
        /// The frequency at which this service is preformed
        /// </summary>
        public Recurrence ServiceFrequency { get; init; } 

        /// <summary>
        /// Flag to indicate if this item should be displayed in lists on the website
        /// </summary>
        public bool PriceTableItem { get; init; }

        /// <summary>
        /// Flag to indicate if this is a featured service
        /// </summary>
        public bool Featured { get; set; }

        /// <summary>
        /// Flag to indicate if this is a active service
        /// </summary>
        public bool Active { get; init; }

        /// <summary>
        /// Any featured tags for this service
        /// </summary>
        public string? Tags { get; init; }

        /// <summary>
        /// The price of this service
        /// </summary>
        public double Price { get; init; }

        /// <summary>
        /// Flag to indicate if this service should be displayed in any catalogs
        /// </summary>
        public bool DoNotDisplayInCatalogs { get; init; } 

        #endregion

        #region Collections

        /// <summary>
        /// The collection of categories this service belongs to
        /// </summary>
        public virtual ICollection<CategoryDto> Categories { get; init; } = [];

        /// <summary>
        /// Gets the collection of image files associated with the current object.
        /// </summary>
        public ICollection<ImageDto> Images { get; init; } = [];

        /// <summary>
        /// Gets the collection of videos associated with the current entity.
        /// </summary>
        public ICollection<VideoDto> Videos { get; init; } = [];

        #endregion

        /// <summary>
        /// Converts the current instance to an <see cref="OfferedService"/> object.
        /// </summary>
        /// <remarks>This method creates a new <see cref="OfferedService"/> instance and maps the
        /// properties of the current object to the corresponding properties of the <see cref="OfferedService"/>. The
        /// returned object is independent of the current instance.</remarks>
        /// <returns>An <see cref="OfferedService"/> object populated with the values from the current instance.</returns>
        public OfferedService ToService()
        {
            return new OfferedService()
            {
                Id = Id,
                Name = Name,
                DisplayName = DisplayName,
                ShortDescription = ShortDescription,
                Description = Description,
                BillingFrequency = BillingFrequency,
                ServiceFrequency = ServiceFrequency,
                Price = Price,
                PriceTableItem = PriceTableItem,
                Featured = Featured,
                Active = Active,
                Tags = Tags,
                DoNotDisplayInCatalogs = DoNotDisplayInCatalogs
            };
        }
    }
}
