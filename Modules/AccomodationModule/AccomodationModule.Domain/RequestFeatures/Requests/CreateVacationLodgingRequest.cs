namespace AccomodationModule.Domain.Arguments.Requests
{
    /// <summary>
    /// Represents a request to associate a lodging with a vacation.    
    /// </summary>
    /// <remarks>This class is used to create a request for linking a specific lodging to a vacation. Both the
    /// lodging and vacation must be identified by their respective IDs.</remarks>
    /// <param name="lodgingId"></param>
    /// <param name="vacationId"></param>
    public class CreateVacationLodgingRequest(string lodgingId, string vacationId)
    {
        /// <summary>
        /// Gets or sets the unique identifier for the lodging.
        /// </summary>
        public string LodgingId { get; set; } = lodgingId;

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        public string VacationId { get; set; } = vacationId;
    }
}
