namespace AccomodationModule.Domain.Arguments.Requests
{
    /// <summary>
    /// Represents a request to create a vacation destination, including the identifiers for the destination and the
    /// vacation.
    /// </summary>
    /// <remarks>This class is used to encapsulate the data required to associate a destination with a
    /// vacation. Both <see cref="DestinationId"/> and <see cref="VacationId"/> must be provided to ensure the request
    /// is valid.</remarks>
    /// <param name="destinationId"></param>
    /// <param name="vacationId"></param>
    public class CreateVacationDestinationRequest(string destinationId, string vacationId)
    {
        /// <summary>
        /// Gets or sets the unique identifier for the destination.
        /// </summary>
        public string DestinationId { get; set; } = destinationId;

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        public string VacationId { get; set; } = vacationId;
    }
}
