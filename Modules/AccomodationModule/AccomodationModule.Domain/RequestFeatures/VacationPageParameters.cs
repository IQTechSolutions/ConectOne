using ConectOne.Domain.RequestFeatures;

namespace AccomodationModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents parameters for filtering and retrieving vacation host-related data.
    /// Inherits pagination, sorting, and searching capabilities from RequestParameters.
    /// </summary>
    public class VacationPageParameters : RequestParameters
    {

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// Can be null if filtering by vacation ID is not required.
        /// </summary>
        public string? VacationHostId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a vacation extension.
        /// Can be null if filtering by vacation ID is not required.
        /// </summary>
        public string? VacationExtensionParentId { get; set; }
    }
}