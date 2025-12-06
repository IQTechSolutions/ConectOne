namespace AccomodationModule.Domain.Arguments.Requests
{
    /// <summary>
    /// Represents a request to create a vacation extension for an existing vacation.
    /// </summary>
    /// <remarks>This class encapsulates the identifiers required to associate a vacation extension with a
    /// specific vacation. Use this type to pass data when creating a vacation extension.</remarks>
    /// <param name="vacationExtensionId"></param>
    /// <param name="vacationId"></param>
    public class CreateVacationExtensionForVacationRequest(string vacationExtensionId, string vacationId)
    {
        /// <summary>
        /// Gets or sets the unique identifier for the vacation extension.
        /// </summary>
        public string ExtensionId { get; set; } = vacationExtensionId;

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        public string VacationId { get; set; } = vacationId;
    }
}
