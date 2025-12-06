namespace AccomodationModule.Domain.Arguments.Requests
{
    /// <summary>
    /// Represents a request to create a vacation golf course associated with a specific vacation.
    /// </summary>
    /// <remarks>This class is used to encapsulate the data required to create a golf course entry for a
    /// vacation. It includes identifiers for both the golf course and the vacation.</remarks>
    /// <param name="vacationGolfCourseId"></param>
    /// <param name="vacationId"></param>
    public class CreateVacationGolfCourseRequest(string vacationGolfCourseId, string vacationId)
    {
        /// <summary>
        /// Gets or sets the unique identifier for the vacation golf course.
        /// </summary>
        public string VacationGolfCourseId { get; set; } = vacationGolfCourseId;

        /// <summary>
        /// Gets or sets the unique identifier for a vacation.
        /// </summary>
        public string VacationId { get; set; } = vacationId;
    }
}
