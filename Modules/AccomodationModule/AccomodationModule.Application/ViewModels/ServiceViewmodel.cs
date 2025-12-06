using AccomodationModule.Domain.DataTransferObjects;
using ProductsModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for service amenities.
    /// </summary>
    /// <remarks>
    /// This class is used to transfer data related to service amenities between the application layer and the presentation layer.
    /// </remarks>
    public class ServiceViewmodel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceViewmodel"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see cref="ServiceViewmodel"/>
        /// class. Use this constructor when no specific initialization parameters are required.</remarks>
        public ServiceViewmodel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceViewmodel"/> class using the specified service data
        /// transfer object.
        /// </summary>
        /// <param name="service">The service data transfer object containing the data to initialize the view model. Cannot be null.</param>
        public ServiceViewmodel(ServiceDto service) { }

        /// <summary>
        /// Gets or sets the unique identifier for the service.
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the location associated with the current object.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        public string? ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the display name associated with the object.
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
        /// Gets or sets the code associated with the entity.
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
        /// Gets or sets the current rate value.
        /// </summary>
        public double? CurrentRate { get; set; }

        /// <summary>
        /// Gets or sets the number of rooms currently available.
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
        /// Gets or sets the policy applied to child elements within the current context.
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
    }
}
