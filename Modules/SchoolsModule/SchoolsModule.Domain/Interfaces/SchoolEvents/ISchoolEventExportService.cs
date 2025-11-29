using ConectOne.Domain.ResultWrappers;
using MessagingModule.Domain.Enums;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Interfaces.SchoolEvents
{
    /// <summary>
    /// Defines methods for exporting school event data and consent information to Excel.
    /// </summary>
    public interface ISchoolEventExportService
    {
        /// <summary>
        /// Exports the parent consent status for all learners participating in a specific event.
        /// </summary>
        /// <param name="eventId">The unique identifier of the event.</param>
        /// <param name="consentType">The type of consent to evaluate (e.g., Attendance, Transport).</param>
        /// <param name="cancellationToken">Optional token to cancel the operation.</param>
        /// <returns>
        /// A result containing the path or identifier of the exported Excel file,
        /// or error messages if the operation fails.
        /// </returns>
        Task<IBaseResult<string>> ExportEventConsents(string eventId, ConsentTypes consentType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Exports a list of upcoming school events to an Excel file.
        /// </summary>
        /// <param name="pageParameters">Parameters to filter or page through event data (currently unused).</param>
        /// <param name="trackChanges">Indicates whether EF Core should track changes to the retrieved entities.</param>
        /// <param name="cancellationToken">Optional token to cancel the operation.</param>
        /// <returns>
        /// A result containing the path or identifier of the exported Excel file,
        /// or error messages if the operation fails.
        /// </returns>
        Task<IBaseResult<string>> ExportEvents(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default);
    }

}
