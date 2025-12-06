using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents the settings for a lodging establishment.
    /// Contains various configuration options and policies for the establishment.
    /// </summary>
    public class LodgingSettings : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the API partner associated with the lodging.
        /// </summary>
        public ApiPartners? ApiPartner { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the lodging from the API partner.
        /// </summary>
        [StringLength(100, ErrorMessage = "Unique Partner ID cannot exceed 100 characters.")]
        public string? UniquePartnerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the lodging is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the lodging is featured.
        /// </summary>
        public bool Featured { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether bookings are allowed.
        /// </summary>
        public bool AllowBookings { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether live bookings are allowed.
        /// </summary>
        public bool AllowLiveBookings { get; set; } = true;

        /// <summary>
        /// Gets or sets the minimum number of days in advance required for booking.
        /// </summary>
        [Range(0, 365, ErrorMessage = "Minimum advance booking days must be between 0 and 365.")]
        public int MinAdvanceBookingDays { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether same-day bookings are allowed.
        /// </summary>
        public bool AllowSameDay { get; set; }

        /// <summary>
        /// Gets or sets the cut-off time for same-day bookings.
        /// </summary>
        [StringLength(20, ErrorMessage = "Cut-off time cannot exceed 20 characters.")]
        public string? CutOffTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a one-night stay is refundable.
        /// </summary>
        public bool OneNightStayRefundable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell phone number is shown publicly.
        /// </summary>
        public bool ShowCellPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether smoking is allowed.
        /// </summary>
        public bool AllowSmoking { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multiple meal plans are allowed.
        /// </summary>
        public bool AllowMultipleMealPlans { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the lodging is VAT registered.
        /// </summary>
        public bool VatRegistered { get; set; }

        /// <summary>
        /// Gets or sets the VAT number for the lodging.
        /// </summary>
        [StringLength(50, ErrorMessage = "VAT number cannot exceed 50 characters.")]
        public string? VatNr { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the standard check-in time for the lodging.
        /// </summary>
        [Required(ErrorMessage = "Check-in time is required.")]
        [StringLength(20, ErrorMessage = "Check-in time cannot exceed 20 characters.")]
        public string CheckInTime { get; set; } = "14:00";

        /// <summary>
        /// Gets or sets the standard check-out time for the lodging.
        /// </summary>
        [Required(ErrorMessage = "Check-out time is required.")]
        [StringLength(20, ErrorMessage = "Check-out time cannot exceed 20 characters.")]
        public string CheckoutTime { get; set; } = "10:00";

        /// <summary>
        /// Gets or sets the policy for allowing pets.
        /// </summary>
        [StringLength(100, ErrorMessage = "Allow pets description cannot exceed 100 characters.")]
        public string? AllowPets { get; set; } = "No pets allowed";

        /// <summary>
        /// Gets or sets the parking information for the lodging.
        /// </summary>
        [StringLength(100, ErrorMessage = "Parking description cannot exceed 100 characters.")]
        public string? Parking { get; set; } = "Free";

        /// <summary>
        /// Gets or sets the Wi-Fi availability information for the lodging.
        /// </summary>
        [StringLength(200, ErrorMessage = "Wi-Fi description cannot exceed 100 characters.")]
        public string? Wifi { get; set; } = "Yes, on entire property";

        /// <summary>
        /// Gets or sets the cost of Wi-Fi for the lodging.
        /// </summary>
        [StringLength(100, ErrorMessage = "Wi-Fi cost description cannot exceed 100 characters.")]
        public string? WifiCost { get; set; } = "Free and unlimited";

        /// <summary>
        /// Returns a string representation of the lodging settings.
        /// </summary>
        /// <returns>A string representing the lodging settings.</returns>
        public override string ToString()
        {
            return $"Lodging Settings";
        }
    }
}

