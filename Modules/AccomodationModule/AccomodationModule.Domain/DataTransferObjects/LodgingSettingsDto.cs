using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the settings and configuration for lodging, including booking options, amenities, and operational
    /// details.
    /// </summary>
    /// <remarks>This data transfer object (DTO) is used to encapsulate lodging settings for integration with
    /// external systems or for use within the application. It provides properties to configure booking behavior,
    /// amenities, and other lodging-specific details.</remarks>
    public record LodgingSettingsDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingSettingsDto"/> class.
        /// </summary>
        public LodgingSettingsDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingSettingsDto"/> class using the specified <see
        /// cref="LodgingSettings"/> object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="LodgingSettings"/>
        /// object to the corresponding properties of the <see cref="LodgingSettingsDto"/>. It ensures that the DTO is
        /// populated with the necessary lodging settings for further processing or API responses.</remarks>
        /// <param name="contentResponse">The <see cref="LodgingSettings"/> object containing the lodging settings to initialize the DTO.</param>
        public LodgingSettingsDto(LodgingSettings contentResponse) 
        {
            ApiPartner = contentResponse.ApiPartner!.Value;
            UniquePartnerId = contentResponse.UniquePartnerId;
            Active = contentResponse.Active;
            Featured = false;
            AllowBookings = contentResponse.AllowBookings;
            AllowLiveBookings = contentResponse.AllowLiveBookings;

            MinAdvanceBookingDays = contentResponse.MinAdvanceBookingDays;
            AllowSameday = contentResponse.AllowSameDay;
            CutOffTime = contentResponse.CutOffTime;
            OneNightStayRefundable = contentResponse.OneNightStayRefundable;
            ShowCellPhoneNumber = contentResponse.ShowCellPhoneNumber;
            AllowSmoking = contentResponse.AllowSmoking;
            AllowMultipleMealPlans = contentResponse.AllowMultipleMealPlans;

            CheckInTime = contentResponse.CheckInTime;
            CheckoutTime = contentResponse.CheckoutTime;

            AllowPets = contentResponse.AllowPets;
            Parking = contentResponse.Parking;
            Wifi = contentResponse.Wifi;
            WifiCost = contentResponse.WifiCost;
        }

        #endregion

        /// <summary>
        /// Gets the API partner configuration associated with the current instance.
        /// </summary>
        public ApiPartners ApiPartner { get; init; }

        /// <summary>
        /// Gets the unique identifier for a partner.
        /// </summary>
        public string? UniquePartnerId { get; init; }

        /// <summary>
        /// Gets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; init; }

        /// <summary>
        /// Gets a value indicating whether the item is marked as featured.
        /// </summary>
        public bool Featured { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether bookings are allowed.
        /// </summary>
        public bool AllowBookings { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether live bookings are allowed.
        /// </summary>
        public bool AllowLiveBookings { get; set; }

        /// <summary>
        /// Gets the minimum number of days in advance that a booking can be made.
        /// </summary>
        public int MinAdvanceBookingDays { get; init; }

        /// <summary>
        /// Gets a value indicating whether same-day operations are allowed.
        /// </summary>
        public bool AllowSameday { get; init; }

        /// <summary>
        /// Gets the cutoff time for processing operations.
        /// </summary>
        public string? CutOffTime { get; init; }

        /// <summary>
        /// Gets a value indicating whether a one-night stay is refundable.
        /// </summary>
        public bool OneNightStayRefundable { get; init; }

        /// <summary>
        /// Gets a value indicating whether the user's cell phone number should be displayed.
        /// </summary>
        public bool ShowCellPhoneNumber { get; init; }

        /// <summary>
        /// Gets a value indicating whether smoking is allowed.
        /// </summary>
        public bool AllowSmoking { get; init; }

        /// <summary>
        /// Gets a value indicating whether multiple meal plans can be selected simultaneously.
        /// </summary>
        public bool AllowMultipleMealPlans { get; init; }

        /// <summary>
        /// Gets a value indicating whether the entity is registered for VAT (Value Added Tax).
        /// </summary>
        public bool VatRegistered { get; init; }

        /// <summary>
        /// Gets the VAT (Value Added Tax) number associated with the entity.
        /// </summary>
        public string? VatNr { get; init; } 

        /// <summary>
        /// Gets the check-in time for the reservation.
        /// </summary>
        public string? CheckInTime { get; init; }

        /// <summary>
        /// Gets the checkout time for the current transaction.
        /// </summary>
        public string? CheckoutTime { get; init; } 

        /// <summary>
        /// Gets or initializes a value indicating whether pets are allowed.
        /// </summary>
        public string? AllowPets { get; init; } 

        /// <summary>
        /// Gets the parking information associated with the entity.
        /// </summary>
        public string? Parking { get; init; }

        /// <summary>
        /// Gets the Wi-Fi network name associated with the current configuration.
        /// </summary>
        public string? Wifi { get; init; } 

        /// <summary>
        /// Gets the cost of Wi-Fi service, if available.
        /// </summary>
        public string? WifiCost { get; init; }

        /// <summary>
        /// Converts a <see cref="LodgingSettingsDto"/> instance to a <see cref="LodgingSettings"/> object.
        /// </summary>
        /// <remarks>This method maps properties from the <see cref="LodgingSettingsDto"/> to a new <see
        /// cref="LodgingSettings"/> instance. Certain properties are set to default values, such as <see
        /// cref="LodgingSettings.Featured"/> being set to <see langword="false"/> and <see
        /// cref="LodgingSettings.ApiPartner"/> being set to <see cref="ApiPartners.NightsBridge"/>.</remarks>
        /// <param name="contentResponse">The <see cref="LodgingSettingsDto"/> instance containing lodging settings data to be converted.</param>
        /// <returns>A <see cref="LodgingSettings"/> object populated with the corresponding values from the provided <paramref
        /// name="contentResponse"/>.</returns>
        public static LodgingSettings ToLodgingSettings(LodgingSettingsDto contentResponse)
        {
            return new LodgingSettings()
            {
                ApiPartner = ApiPartners.NightsBridge,
                Active = contentResponse.Active,
                Featured = false,
                AllowBookings = contentResponse.AllowBookings,
                AllowLiveBookings = contentResponse.AllowLiveBookings,
                UniquePartnerId = contentResponse.UniquePartnerId,

                MinAdvanceBookingDays = contentResponse.MinAdvanceBookingDays,
                AllowSameDay = contentResponse.AllowSameday,
                CutOffTime = contentResponse.CutOffTime,
                OneNightStayRefundable = contentResponse.OneNightStayRefundable,
                ShowCellPhoneNumber = contentResponse.ShowCellPhoneNumber,
                AllowSmoking = contentResponse.AllowSmoking,
                AllowMultipleMealPlans = contentResponse.AllowMultipleMealPlans,

                CheckInTime = contentResponse.CheckInTime!,
                CheckoutTime = contentResponse.CheckoutTime!,

                AllowPets = contentResponse.AllowPets,
                Parking = contentResponse.Parking,
                Wifi = contentResponse.Wifi,
                WifiCost = contentResponse.WifiCost,
            };
        }
    }
}
