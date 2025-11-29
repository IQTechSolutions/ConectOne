using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CalendarModule.Domain.Enums;
using FilingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.DataTransferObjects;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Enums;

namespace ProductsModule.Application.ViewModels
{
    /// <summary>
    /// Represents a service in the application
    /// </summary>
    public class ServiceViewModel 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceViewModel"/> class.
        /// </summary>
        public ServiceViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceViewModel"/> class using the specified service data
        /// transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="ServiceDto"/> to the
        /// corresponding properties of the <see cref="ServiceViewModel"/>. It is typically used to create a view model
        /// representation of a service for display or interaction in a user interface.</remarks>
        /// <param name="dto">The <see cref="ServiceDto"/> containing the data to initialize the view model.  This parameter must not be
        /// <c>null</c>.</param>
        public ServiceViewModel(ServiceDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            DisplayName = dto.DisplayName;
            ShortDescription = dto.ShortDescription;
            Description = dto.Description;
            BillingFrequency = dto.BillingFrequency;
            ServiceFrequency = dto.ServiceFrequency;
            PriceTableItem = dto.PriceTableItem;
            Featured = dto.Featured;
            Active = dto.Active;
            Tags = dto.Tags;
            Price = dto.Price;
            DoNotDisplayInCatalogs = dto.DoNotDisplayInCatalogs;
            Categories = dto.Categories;
            Images = dto.Images;
            Videos = dto.Videos;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The name of the service
        /// </summary>
        [Required(ErrorMessage = "Name is a required field"), MaxLength(200, ErrorMessage = "Maximum length for the name is 50 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The display name of the service
        /// </summary>
        [DisplayName(@"Display Name"), MaxLength(200, ErrorMessage = "Maximum length for the display name is 20 characters.")]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// A brief description of the service
        /// </summary>
        [DisplayName(@"Short Description"), DataType(DataType.MultilineText), MaxLength(1000, ErrorMessage = "Maximum length for the short description is 200 characters.")]
        public string? ShortDescription { get; set; }

        /// <summary>
        /// A full description of the service
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        /// <summary>
        /// The frequency at which this service is billed for
        /// </summary>        
        [DisplayName(@"Billing Frequency")]
        public BillingFrequency BillingFrequency { get; set; } = BillingFrequency.Monthly;

        /// <summary>
        /// The frequency at which this service is preformed
        /// </summary>
        [DisplayName(@"Service Frequency")]
        public Recurrence ServiceFrequency { get; set; } = Recurrence.None;

        /// <summary>
        /// Flag to indicate if this item should be displayed in lists on the website
        /// </summary>
        [DisplayName(@"Price Table Item")]
        public bool PriceTableItem { get; set; }

        /// <summary>
        /// Flag to indicate if this is a featured service
        /// </summary>
        public bool Featured { get; set; }

        /// <summary>
        /// Flag to indicate if this is a active service
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Any featured tags for this service
        /// </summary>
        public string? Tags { get; set; }

        /// <summary>
        /// The price of this service
        /// </summary>
        public double Price { get; set; } 

        /// <summary>
        /// Flag to indicate if this service should be displayed in any catalogs
        /// </summary>
        [DisplayName(@"Do Not Display")]
        public bool DoNotDisplayInCatalogs { get; set; } 

        #endregion

        #region Collections

        /// <summary>
        /// The collection of categories this service belongs to
        /// </summary>
        public virtual ICollection<CategoryDto> Categories { get; set; } = new List<CategoryDto>();

        /// <summary>
        /// Gets or sets the collection of image files associated with the current entity.
        /// </summary>
        public ICollection<ImageDto> Images { get; set; } = new List<ImageDto>();

        /// <summary>
        /// Gets or sets the collection of videos associated with the entity.
        /// </summary>
        public ICollection<VideoDto> Videos { get; set; } = new List<VideoDto>();

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current service instance to a <see cref="ServiceDto"/>.
        /// </summary>
        /// <remarks>This method creates a new <see cref="ServiceDto"/> object and populates its
        /// properties with the corresponding values from the current service instance.</remarks>
        /// <returns>A <see cref="ServiceDto"/> object containing the data from the current service instance.</returns>
        public ServiceDto ToDto()
        {
            return new ServiceDto
            {
                Id = Id,
                Name = Name,
                DisplayName = DisplayName,
                ShortDescription = ShortDescription,
                Description = Description,
                BillingFrequency = BillingFrequency,
                ServiceFrequency = ServiceFrequency,
                PriceTableItem = PriceTableItem,
                Featured = Featured,
                Active = Active,
                Tags = Tags,
                Price = Price,
                DoNotDisplayInCatalogs = DoNotDisplayInCatalogs
            };
        }

        #endregion
    }
}
