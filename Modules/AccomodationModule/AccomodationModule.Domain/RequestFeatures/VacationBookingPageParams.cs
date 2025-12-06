using ConectOne.Domain.RequestFeatures;

namespace AccomodationModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters for vacation booking page requests.
    /// Inherits from <see cref="RequestParameters"/> to include pagination, sorting, and searching capabilities.
    /// </summary>
    public class VacationBookingPageParams : RequestParameters
    {
        /// <summary>
        /// Gets or sets the user ID associated with the vacation booking.
        /// </summary>
        public string? UserId { get; set; }
    }
}
