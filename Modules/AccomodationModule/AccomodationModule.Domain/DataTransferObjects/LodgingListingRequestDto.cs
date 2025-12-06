using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for lodging listing requests.
    /// </summary>
    /// <remarks>This class is used to encapsulate the details of a lodging listing request, including contact
    /// information, location details, and the current status of the request. It provides constructors for initializing
    /// the DTO from different models and includes a method to convert the DTO back into a domain model.</remarks>
	public class LodgingListingRequestDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingListingRequestDto"/> class.
        /// </summary>
        public LodgingListingRequestDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingListingRequestDto"/> class,  mapping properties from the
        /// specified <see cref="LodgingListingRequest"/> model.
        /// </summary>
        /// <remarks>This constructor copies relevant properties from the provided <see
        /// cref="LodgingListingRequest"/>  instance to create a data transfer object (DTO) representation. It is
        /// typically used to convert  domain models into a format suitable for external communication or API
        /// responses.</remarks>
        /// <param name="model">The <see cref="LodgingListingRequest"/> model containing the data to initialize the DTO.</param>
        public LodgingListingRequestDto(LodgingListingRequest model)
        {
            LodgingListingRequestId = model.Id;
            ContactPerson = model.ContactPerson;
            ContactNumber = model.ContactNumber;
            ContactEmail = model.ContactEmail;
            Name = model.Name;
            WebsiteUrl = model.WebsiteUrl;
            Address = model.Address;
            Lat = model.Lat;
            Lng = model.Lng;
            Status = model.Status;
        }

        #endregion

        /// <summary>
        /// Gets the unique identifier for a lodging listing request.
        /// </summary>
        public string? LodgingListingRequestId { get; init; }      
        
        /// <summary>
        /// Gets the name of the contact person associated with this entity.
        /// </summary>
        public string ContactPerson { get; init; }

        /// <summary>
        /// Gets the contact number associated with the entity.
        /// </summary>
        public string ContactNumber { get; init; }

        /// <summary>
        /// Gets the contact email address associated with the entity.
        /// </summary>
        public string ContactEmail { get; init; }

        /// <summary>
        /// Gets the URL of the website associated with the current entity.
        /// </summary>
        public string WebsiteUrl { get; init; }

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the address associated with the current object.
        /// </summary>
        public string Address { get; init; }

        /// <summary>
        /// Gets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; init; }

        /// <summary>
        /// Gets the longitude component of a geographic coordinate.
        /// </summary>
        public double Lng { get; init; }

        /// <summary>
        /// Gets the current status of the booking.
        /// </summary>
        public BookingStatus Status { get; init; } = BookingStatus.Pending;

        /// <summary>
        /// Converts the current instance to a <see cref="LodgingListingRequest"/> object.
        /// </summary>
        /// <remarks>This method creates a new <see cref="LodgingListingRequest"/> object and copies the
        /// relevant properties from the current instance. Use this method to transform the current object into a format
        /// suitable for lodging listing requests.</remarks>
        /// <returns>A <see cref="LodgingListingRequest"/> object populated with the values from the current instance.</returns>
        public LodgingListingRequest ToLodgingListingRequest()
        {
            return new LodgingListingRequest()
			{
				LodgingListingRequestId = this.LodgingListingRequestId,
				ContactPerson = this.ContactPerson,
				ContactNumber = this.ContactNumber,
				ContactEmail = this.ContactEmail,
				Name = this.Name,
				Address = this.Address,
				Lat = this.Lat,
				Lng = this.Lng,
				Status = this.Status
			};
		}
    }
}
