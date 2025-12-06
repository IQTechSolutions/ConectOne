using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents the guest information for a vacation booking.
    /// </summary>
    public class VacationGuestInfo : EntityBase<string>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the first name of the guest.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the surname of the guest.
        /// </summary>
        public string? Surname { get; set; }

        /// <summary>
        /// Gets or sets the email address of the guest.
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the guest.
        /// </summary>
        public string? PhoneNr { get; set; }

        /// <summary>
        /// Gets or sets the address of the guest.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the city of the guest.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the state of the guest.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets the zip code of the guest.
        /// </summary>
        public string? ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the country of the guest.
        /// </summary>
        public string? Country { get; set; }

        #endregion

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the ID of the associated vacation booking.
        /// </summary>
        [ForeignKey(nameof(VacationBooking))]  public string? VacationBookingId { get; set; }

        /// <summary>
        /// Gets or sets the associated vacation booking.
        /// </summary>
        public VacationBooking? VacationBooking { get; set; }

        #endregion
    }
}
