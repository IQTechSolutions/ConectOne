using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a service entity, including details about its location, name, supplier, pricing, availability, and
    /// associated policies.
    /// </summary>
    /// <remarks>The <see cref="LodgingService"/> class provides properties to describe a service, such as its name,
    /// location, supplier information,  pricing details, availability, and associated policies. It also includes
    /// relationships to other entities, such as packages,  lodging, and available partners. This class is commonly used
    /// in scenarios where detailed information about a service is required,  such as booking systems or inventory
    /// management.</remarks>
    public class LodgingService : EntityBase<int>
    {
        /// <summary>
        /// Gets or sets the location associated with the current object.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        public string? ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the display name of the entity.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the name of the supplier.
        /// </summary>
        public string? Supplier { get; set; }

        /// <summary>
        /// Gets or sets the name of the supplier.
        /// </summary>
        public string? SupplierName { get; set; }

        /// <summary>
        /// Gets or sets the code associated with the current object.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the object.
        /// </summary>
        public string? UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the commission percentage.
        /// </summary>
        public double? CommPerc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the display is enabled.
        /// </summary>
        public bool? Display { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is published.
        /// </summary>
        public bool? IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the start date and time of the rate period.
        /// </summary>
        public DateTime? RatePeriodStart { get; set; }

        /// <summary>
        /// Gets or sets the end date and time of the rate period.
        /// </summary>
        public DateTime? RatePeriodEnd { get; set; }

        /// <summary>
        /// Gets or sets the markup percentage applied to the base price.
        /// </summary>
        public double? MarkupPerc { get; set; }

        /// <summary>
        /// Gets or sets the current rate applied to calculations or operations.
        /// </summary>
        public double? CurrentRate { get; set; }

        /// <summary>
        /// Gets or sets the number of rooms available for booking.
        /// </summary>
        public string? RoomsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the description of the room rate type.
        /// </summary>
        public string? RoomRateTypeDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current item is a package.
        /// </summary>
        public bool? IsPackage { get; set; }

        /// <summary>
        /// Gets or sets the rate code associated with the current transaction or entity.
        /// </summary>
        public string? RateCode { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the available partner.
        /// </summary>
        public string? AvailablePartnerUid { get; set; }

        /// <summary>
        /// Gets or sets the list of included items or criteria as a string.
        /// </summary>
        public string? Includes { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of items to exclude from processing.
        /// </summary>
        public string? Excludes { get; set; }

        /// <summary>
        /// Gets or sets information about the room, such as its name, description, or other relevant details.
        /// </summary>
        public string? RoomInformation { get; set; }

        /// <summary>
        /// Gets or sets the policy applied to child elements.
        /// </summary>
        public string? ChildPolicy { get; set; }

        /// <summary>
        /// Gets or sets the cancellation policy for the associated service or booking.
        /// </summary>
        public string? CancellationPolicy { get; set; }

        /// <summary>
        /// Gets or sets the terms and conditions associated with the booking.
        /// </summary>
        public string? BookingTerms { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether local data should be used.
        /// </summary>
        public bool? UseLocalData { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated package.
        /// </summary>
        [ForeignKey(nameof(Package))] public int? PackageId { get; set; }

        /// <summary>
        /// Gets or sets the package associated with the current operation.
        /// </summary>
        public LodgingPackage? Package { get; set; }


        /// <summary>
        /// Gets or sets the unique identifier for the associated lodging entity.
        /// </summary>
        [ForeignKey(nameof(Lodging))] public string LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the lodging details associated with the current entity.
        /// </summary>
        public Lodging Lodging { get; set; }

        /// <summary>
        /// Gets or sets the collection of child age parameters.
        /// </summary>
        public virtual ICollection<ChildAgeParams> ChildAgeParams { get; set; } = new List<ChildAgeParams>();

        /// <summary>
        /// Gets or sets the collection of rate entities associated with this instance.
        /// </summary>
        public virtual ICollection<Rates> Rates { get; set; } = new List<Rates>();
    }
}
