using System.ComponentModel.DataAnnotations;
using CalendarModule.Domain.Enums;
using ConectOne.Domain.Extensions;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Enums;

namespace ProductsModule.Domain.Entities
{
    /// <summary>
    /// Represents a service in the application
    /// </summary>
    public class OfferedService : FileCollection<OfferedService, string>
    {
        private string? _displayName;
        private string _shortDescription;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferedService"/> class.
        /// </summary>
        public OfferedService() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfferedService"/> class using the specified service data
        /// transfer object (DTO).
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="ServiceDto"/> to the
        /// corresponding properties of the <see cref="OfferedService"/> instance. It is intended to simplify the
        /// creation of an <see cref="OfferedService"/> object from a DTO representation.</remarks>
        /// <param name="dto">The <see cref="ServiceDto"/> containing the data used to initialize the properties of the <see
        /// cref="OfferedService"/> instance.</param>
        public OfferedService(ServiceDto dto)
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
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the service
        /// </summary>
        [MaxLength(200, ErrorMessage = "Maximum length for the name is 50 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// The display name of the service
        /// </summary>
        [MaxLength(200, ErrorMessage = "Maximum length for the display name is 20 characters.")]
        public string DisplayName
        {
            get => string.IsNullOrEmpty(_displayName) ? Name : _displayName;
            set => _displayName = value;
        }

        /// <summary>
        /// A brief description of the service
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Maximum length for the short description is 200 characters.")]
        public string? ShortDescription
        {
            get => string.IsNullOrEmpty(_shortDescription) ? Description.HtmlToPlainText().TruncateLongString(150) : _shortDescription;
            set => _shortDescription = value;
        }

        /// <summary>
        /// A full description of the service
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        /// <summary>
        /// The frequency at which this service is billed for
        /// </summary>        
        public BillingFrequency BillingFrequency { get; set; } = BillingFrequency.Monthly;

        /// <summary>
        /// The frequency at which this service is preformed
        /// </summary>
        public Recurrence ServiceFrequency { get; set; } = Recurrence.None;

        /// <summary>
        /// Flag to indicate if this item should be displayed in lists on the website
        /// </summary>
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
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Flag to indicate if this service should be displayed in any catalogs
        /// </summary>
        public bool DoNotDisplayInCatalogs { get; set; } = false;

        #endregion

        #region Collections

        /// <summary>
        /// The collection of categories this service belongs to
        /// </summary>
        public virtual ICollection<EntityCategory<OfferedService>> Categories { get; set; } = new List<EntityCategory<OfferedService>>();

        #endregion
    }
}
