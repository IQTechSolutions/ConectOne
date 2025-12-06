using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the settings and configuration for lodging properties, including booking options, amenities, and
    /// operational details.
    /// </summary>
    /// <remarks>This view model is designed to encapsulate various settings related to lodging properties,
    /// such as booking preferences,  check-in and check-out times, and available amenities. It can be initialized with
    /// default values or populated using  data from a <see cref="LodgingSettingsDto"/> object.</remarks>
    public class LodgingSettingsViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingSettingsViewModel"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see
        /// cref="LodgingSettingsViewModel"/> class. Use this constructor when no specific initialization is
        /// required.</remarks>
        public LodgingSettingsViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingSettingsViewModel"/> class using the specified lodging
        /// settings data.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="LodgingSettingsDto"/>
        /// to the corresponding properties of the <see cref="LodgingSettingsViewModel"/>. It ensures that the view
        /// model is initialized with the appropriate values for lodging settings such as booking options, amenities,
        /// and operational details.</remarks>
        /// <param name="contentResponse">An instance of <see cref="LodgingSettingsDto"/> containing the lodging settings data to initialize the view
        /// model.</param>
        public LodgingSettingsViewModel(LodgingSettingsDto contentResponse) 
        {
            ApiPartner = contentResponse.ApiPartner;
            Active = contentResponse.Active;
            Featured = false;
            AllowBookings = contentResponse.AllowBookings;
            AllowLiveBookings = contentResponse.AllowLiveBookings;
            UniquePartnerId = contentResponse.UniquePartnerId!;
            MinAdvanceBookingDays = contentResponse.MinAdvanceBookingDays;
            AllowSameday = contentResponse.AllowSameday;
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
        /// Gets or sets the API partner configuration.
        /// </summary>
        public ApiPartners ApiPartner { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a partner.
        /// </summary>
        public string UniquePartnerId { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
        public bool Featured { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether bookings are allowed.
        /// </summary>
        public bool AllowBookings { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether live bookings are allowed.
        /// </summary>
        public bool AllowLiveBookings { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of days in advance that a booking can be made.
        /// </summary>
        public int MinAdvanceBookingDays { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether same-day processing is allowed.
        /// </summary>
        public bool AllowSameday { get; set; }

        /// <summary>
        /// Gets or sets the cutoff time for processing operations.
        /// </summary>
        /// <remarks>The cutoff time determines the latest time at which certain operations can be
        /// processed. Ensure the value is in the "HH:mm" format (24-hour clock) to avoid parsing errors.</remarks>
        public string? CutOffTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool OneNightStayRefundable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user's cell phone number should be displayed.
        /// </summary>
        public bool ShowCellPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether smoking is allowed.
        /// </summary>
        public bool AllowSmoking { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multiple meal plans can be assigned to a single user.
        /// </summary>
        public bool AllowMultipleMealPlans { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is VAT registered.
        /// </summary>
        public bool VatRegistered { get; set; }

        /// <summary>
        /// Gets or sets the VAT (Value Added Tax) number associated with the entity.
        /// </summary>
        public string VatNr { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the check-in time for the reservation.
        /// </summary>
        public string? CheckInTime { get; set; }

        /// <summary>
        /// Gets or sets the checkout time for a transaction.
        /// </summary>
        public string? CheckoutTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pets are allowed.
        /// </summary>
        public string? AllowPets { get; set; }

        /// <summary>
        /// Gets or sets the parking information associated with the location.
        /// </summary>
        public string? Parking { get; set; }

        /// <summary>
        /// Gets or sets the Wi-Fi network name.
        /// </summary>
        public string? Wifi { get; set; }

        /// <summary>
        /// Gets or sets the cost associated with using Wi-Fi.
        /// </summary>
        public string? WifiCost { get; set; }

        #region Methods

        /// <summary>
        /// Converts the current lodging settings to a <see cref="LodgingSettingsDto"/> object.
        /// </summary>
        /// <remarks>This method creates a new instance of <see cref="LodgingSettingsDto"/> and populates
        /// it with the values from the current lodging settings. Default values are applied for <see
        /// cref="CheckInTime"/> and  <see cref="CheckoutTime"/> if they are not set.</remarks>
        /// <returns>A <see cref="LodgingSettingsDto"/> object containing the lodging settings data.</returns>
        public LodgingSettingsDto ToDto()
        {
            return new LodgingSettingsDto
            {
                ApiPartner = ApiPartner,
                UniquePartnerId = UniquePartnerId,
                Active = Active,
                Featured = false,
                AllowBookings = AllowBookings,
                AllowLiveBookings = AllowLiveBookings,

                MinAdvanceBookingDays = MinAdvanceBookingDays,
                AllowSameday = AllowSameday,
                CutOffTime = CutOffTime,
                OneNightStayRefundable = OneNightStayRefundable,
                ShowCellPhoneNumber = ShowCellPhoneNumber,
                AllowSmoking = AllowSmoking,
                AllowMultipleMealPlans = AllowMultipleMealPlans,

                CheckInTime = string.IsNullOrEmpty(CheckInTime) ? "14:00" : CheckInTime,
                CheckoutTime = string.IsNullOrEmpty(CheckoutTime) ? "10:00" : CheckoutTime,

                AllowPets = AllowPets,
                Parking = Parking,
                Wifi = Wifi,
                WifiCost = WifiCost
            };
        }

        #endregion
    }
}
