using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;

namespace IdentityModule.Application.RestServices
{
    /// <summary>
    /// The AuditTrialsService provides methods to query and export audit trails related to user actions.
    /// Audit trails track changes (old/new values) to database records, who made them, and when.
    /// This service can return audit history for a given user, and export those logs to Excel for review.
    /// </summary>
    public sealed class AuditTrialsRestService(IBaseHttpProvider provider) : IAuditTrailsService
    {
        /// <summary>
        /// Exports audit trails of a specific user to an Excel file.
        /// Fetches all trails for the given user, orders them by DateTime (descending),
        /// and applies column mappings for the export. Returns a Base64 string of the Excel file.
        /// Optional parameters (searchString, searchInOldValues, searchInNewValues) are currently not applied.
        /// </summary>
        /// <param name="userId">The ID of the user whose trails are being exported.</param>
        /// <param name="searchString">An optional string to filter records, not currently implemented.</param>
        /// <param name="searchInOldValues">An optional bool to filter by old values, not currently implemented.</param>
        /// <param name="searchInNewValues">An optional bool to filter by new values, not currently implemented.</param>
        /// <returns>An IBaseResult containing either a Base64 Excel file or an error message.</returns>
        public async Task<IBaseResult<string>> ExportToExcelAsync(string userId, string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false)
        {
            var result = await provider.GetAsync<string>("audittrails/export");
            return result;
        }

        /// <summary>
        /// Retrieves the latest 250 audit trail entries for the given user.
        /// Maps each Audit entity to an AuditEntryDto for easier consumption by clients.
        /// Useful for displaying a user's recent changes, allowing an admin or user to review activity history.
        /// </summary>
        /// <param name="userId">The ID of the user whose trails should be retrieved.</param>
        /// <returns>A result containing an enumerable of AuditEntryDto or an error message.</returns>
        public async Task<IBaseResult<IEnumerable<AuditEntryDto>>> GetCurrentUserTrailsAsync(string userId)
        {
            var result = await provider.GetAsync<IEnumerable<AuditEntryDto>>($"audittrails/{userId}");
            return result;
        }
    }
}
