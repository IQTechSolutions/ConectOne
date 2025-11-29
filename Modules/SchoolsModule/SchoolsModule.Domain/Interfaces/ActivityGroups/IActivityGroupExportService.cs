using ConectOne.Domain.ResultWrappers;

namespace SchoolsModule.Domain.Interfaces.ActivityGroups
{
    /// <summary>
    /// Defines the contract for exporting activity group-related data.
    /// </summary>
    public interface IActivityGroupExportService
    {
        /// <summary>
        /// Exports learner data for a specific activity group into an Excel file.
        /// </summary>
        /// <param name="activityGroupId">The unique identifier of the activity group to export.</param>
        /// <param name="cancellationToken">An optional token to cancel the operation.</param>
        /// <returns>
        /// A task that returns a result containing a base64-encoded string of the Excel file 
        /// or error messages if the operation fails.
        /// </returns>
        Task<IBaseResult<string>> ExportActivityGroup(string activityGroupId, CancellationToken cancellationToken = default);
    }

}
