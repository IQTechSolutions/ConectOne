using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a request to list a lodging property, including contact information, location details, and status.
    /// </summary>
    /// <remarks>This class is used to encapsulate the details required for submitting a lodging listing
    /// request. It includes information about the lodging property, such as its name, address, and geographic
    /// coordinates, as well as contact details for the person submitting the request.</remarks>
    public class LodgingListingRequest : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the ID of the lodging listing request.
        /// </summary>
        public string? LodgingListingRequestId { get; set; }

        /// <summary>
        /// Gets or sets the name of the contact person associated with this entity.
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// Gets or sets the contact number associated with the entity.
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Gets or sets the contact email address.
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Gets or sets the URL of the website associated with the entity.
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address associated with the entity.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude component of a geographic coordinate.
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// Gets or sets the current status of the booking.
        /// </summary>
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        /// <summary>
        /// Returns a string representation of the lodging listing request.
        /// </summary>
        /// <returns>A string that represents the lodging listing request.</returns>
        public override string ToString()
        {
            return $"Lodging Listing Request";
        }
    }
}
