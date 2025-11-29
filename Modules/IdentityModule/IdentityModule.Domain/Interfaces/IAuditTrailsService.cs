using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;

namespace IdentityModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for managing audit trails.
    /// </summary>
    public interface IAuditTrailsService
    {
        /// <summary>
        /// Retrieves the audit trails for the specified user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A task representing the asynchronous operation, containing a result with a list of audit entries.</returns>
        Task<IBaseResult<IEnumerable<AuditEntryDto>>> GetCurrentUserTrailsAsync(string userId);

        /// <summary>
        /// Exports the audit trails to an Excel file.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="searchString">The search string to filter audit trails.</param>
        /// <param name="searchInOldValues">Indicates whether to search in old values.</param>
        /// <param name="searchInNewValues">Indicates whether to search in new values.</param>
        /// <returns>A task representing the asynchronous operation, containing a result with the exported Excel file as a string.</returns>
        Task<IBaseResult<string>> ExportToExcelAsync(string userId, string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false);
    }
}
