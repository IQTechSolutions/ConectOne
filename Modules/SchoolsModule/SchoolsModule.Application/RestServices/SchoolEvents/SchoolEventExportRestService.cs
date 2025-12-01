using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using MessagingModule.Domain.Enums;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices.SchoolEvents
{
    /// <summary>
    /// Provides functionality to export school event data, including event consents and event details, through REST API
    /// calls.
    /// </summary>
    /// <remarks>This service interacts with a REST API to retrieve and export data related to school events. 
    /// It supports exporting event consents for attendance or transport, as well as exporting event details  based on
    /// specified pagination parameters.</remarks>
    /// <param name="provider"></param>
    public class SchoolEventExportRestService(IBaseHttpProvider provider) : ISchoolEventExportService
    {
        /// <summary>
        /// Exports consents for a specified event based on the consent type.
        /// </summary>
        /// <remarks>The method retrieves the consents for the specified event and consent type from the
        /// provider. The result is returned as a string, which may represent a file path, serialized data, or another
        /// format depending on the implementation.</remarks>
        /// <param name="eventId">The unique identifier of the event for which consents are to be exported.</param>
        /// <param name="consentType">The type of consent to export, such as attendance or transport.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string representing the
        /// exported consents.</returns>
        public async Task<IBaseResult<string>> ExportEventConsents(string eventId, ConsentTypes consentType, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<string>(consentType == ConsentTypes.Attendance ? $"schoolevents/exportAttendance/{eventId}" : $"schoolevents/exportTransport/{eventId}");
            return result;
        }

        /// <summary>
        /// Exports school event attendance data based on the specified parameters.
        /// </summary>
        /// <remarks>This method sends a request to export attendance data for school events based on the
        /// provided query parameters. The exported data is returned as a string.</remarks>
        /// <param name="pageParameters">The parameters used to filter and paginate the school event data. This must not be null.</param>
        /// <param name="trackChanges">A value indicating whether to track changes during the export operation. The default is <see
        /// langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a string representing the
        /// exported data.</returns>
        public async Task<IBaseResult<string>> ExportEvents(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<string>($"schoolevents/exportAttendance?{pageParameters.GetQueryString()}");
            return result;
        }
    }
}
