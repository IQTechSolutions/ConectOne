using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a service, encapsulating key details about the service such as its
    /// identifier, location, name, supplier information, pricing, availability, and policies.
    /// </summary>
    /// <remarks>This record is designed to facilitate the transfer of service-related data between different
    /// layers of an application or across systems. It provides a comprehensive view of a service's attributes,
    /// including metadata, pricing details, availability, and associated policies.  Use this DTO to represent service
    /// information in scenarios such as API responses, data persistence, or inter-service communication.</remarks>
    public record LodgingServiceDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the LodgingServiceDto class.
        /// </summary>
        public LodgingServiceDto() { }

        /// <summary>
        /// Initializes a new instance of the LodgingServiceDto class using the specified LodgingService data.
        /// </summary>
        /// <remarks>All properties of the LodgingServiceDto are set based on the corresponding values
        /// from the provided LodgingService. This constructor is typically used to convert a domain model to a data
        /// transfer object for serialization or transport.</remarks>
        /// <param name="service">The LodgingService instance containing the data to populate the DTO. Cannot be null.</param>
        public LodgingServiceDto(LodgingService service)
        {
            ServiceId = service.Id.ToString();
            Location = service.Location;
            ServiceName = service.ServiceName;
            DisplayName = service.DisplayName;
            Supplier = service.Supplier;
            SupplierName = service.SupplierName;
            Code = service.Code;
            UniqueId = service.UniqueId;
            CommPerc = service.CommPerc;
            Display = service.Display;
            IsPublished = service.IsPublished;
            RatePeriodStart = service.RatePeriodStart;
            RatePeriodEnd = service.RatePeriodEnd;
            MarkupPerc = service.MarkupPerc;
            CurrentRate = service.CurrentRate;
            RoomsAvailable = service.RoomsAvailable;
            RoomRateTypeDescription = service.RoomRateTypeDescription;
            IsPackage = service.IsPackage;
            RateCode = service.RateCode;
            AvailablePartnerUid = service.AvailablePartnerUid;
            Includes = service.Includes;
            Excludes = service.Excludes;
            RoomInformation = service.RoomInformation;
            ChildPolicy = service.ChildPolicy;
            CancellationPolicy = service.CancellationPolicy;
            BookingTerms = service.BookingTerms;
            UseLocalData = service.UseLocalData;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the service.
        /// </summary>
        public string ServiceId { get; init; }

        /// <summary>
        /// Gets the location associated with the current object.
        /// </summary>
        public string? Location { get; init; }

        /// <summary>
        /// Gets the name of the service associated with this instance.
        /// </summary>
        public string? ServiceName { get; init; }

        /// <summary>
        /// Gets the display name associated with the object.
        /// </summary>
        public string DisplayName { get; init; }

        /// <summary>
        /// Gets the name of the supplier associated with the current entity.
        /// </summary>
        public string? Supplier { get; init; }

        /// <summary>
        /// Gets the name of the supplier.
        /// </summary>
        public string? SupplierName { get; init; }

        /// <summary>
        /// Gets the code associated with the current instance.
        /// </summary>
        public string? Code { get; init; }

        /// <summary>
        /// Gets the unique identifier for the object.
        /// </summary>
        public string? UniqueId { get; init; }

        /// <summary>
        /// Gets the commission percentage associated with the transaction.
        /// </summary>
        public double? CommPerc { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the display is enabled.
        /// </summary>
        public bool? Display { get; init; }

        /// <summary>
        /// Gets a value indicating whether the item is published.
        /// </summary>
        public bool? IsPublished { get; init; }

        /// <summary>
        /// Gets the start date and time of the rate period.
        /// </summary>
        public DateTime? RatePeriodStart { get; init; }

        /// <summary>
        /// Gets the end date and time of the rate period.
        /// </summary>
        public DateTime? RatePeriodEnd { get; init; }

        /// <summary>
        /// Gets the percentage markup applied to the base price.
        /// </summary>
        public double? MarkupPerc { get; init; }

        /// <summary>
        /// Gets the current rate applied to the calculation or operation.
        /// </summary>
        public double? CurrentRate { get; init; }

        /// <summary>
        /// Gets the number of rooms currently available for booking.
        /// </summary>
        public string? RoomsAvailable { get; init; }

        /// <summary>
        /// Gets or sets the description of the room rate type.
        /// </summary>
        public string? RoomRateTypeDescription { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current item is a package.
        /// </summary>
        public bool? IsPackage { get; init; }

        /// <summary>
        /// Gets the rate code associated with the current entity.
        /// </summary>
        public string? RateCode { get; init; }

        /// <summary>
        /// Gets the unique identifier of the available partner.
        /// </summary>
        public string? AvailablePartnerUid { get; init; }

        /// <summary>
        /// Gets or initializes the value that specifies additional items or conditions to include in an operation.
        /// </summary>
        public string? Includes { get; init; }

        /// <summary>
        /// Gets the exclusion criteria used to filter results.
        /// </summary>
        public string? Excludes { get; init; }

        /// <summary>
        /// Gets information about the room, such as its name, description, or other relevant details.
        /// </summary>
        public string? RoomInformation { get; init; }

        /// <summary>
        /// Gets the policy applied to child elements within the current context.
        /// </summary>
        public string? ChildPolicy { get; init; }

        /// <summary>
        /// Gets the cancellation policy associated with the current booking or reservation.
        /// </summary>
        public string? CancellationPolicy { get; init; }

        /// <summary>
        /// Gets the terms and conditions associated with the booking.
        /// </summary>
        public string? BookingTerms { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether local data should be used.
        /// </summary>
        public bool? UseLocalData { get; init; }

        #endregion

    }
}
