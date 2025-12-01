using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.Interfaces.ActivityGroups;

namespace SchoolsModule.Application.RestServices.ActivityGroups
{
    /// <summary>
    /// Provides functionality to export activity group data via REST API.
    /// </summary>
    /// <remarks>This service interacts with a REST API to export the data of a specified activity
    /// group.</remarks>
    /// <param name="provider"></param>
    public class ActivityGroupExportRestService(IBaseHttpProvider provider) : IActivityGroupExportService
    {
        /// <summary>
        /// Exports the specified activity group and returns the result as a string.
        /// </summary>
        /// <remarks>This method sends a request to export the activity group identified by <paramref
        /// name="activityGroupId"/>. The result of the export operation is returned as a string. Ensure that the
        /// provided activity group ID is valid.</remarks>
        /// <param name="activityGroupId">The unique identifier of the activity group to export. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the exported activity group data
        /// as a string.</returns>
        public async Task<IBaseResult<string>> ExportActivityGroup(string activityGroupId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<string>($"activitygroups/exportactivitygroup/{activityGroupId}");
            return result;
        }
    }
}
