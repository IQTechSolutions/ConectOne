using AccomodationModule.Domain.Arguments;

namespace AccomodationModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters required to query lodging availability.
    /// </summary>
    /// <remarks>This class encapsulates the location and date range information needed to perform a lodging
    /// availability search.</remarks>
    public class LodgingAvailabilityParameters
    {
        /// <summary>
        /// Gets or sets the unique identifier for the location.
        /// </summary>
        public string LocationId { get; set; } = "";

        /// <summary>
        /// Gets or sets the date range for lodging availability.
        /// </summary>
        public LodgingAvailabilityDateRange DateRange { get; set; } = new LodgingAvailabilityDateRange();

    }
}
