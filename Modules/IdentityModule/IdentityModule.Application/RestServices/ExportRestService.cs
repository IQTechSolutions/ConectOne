using ConectOne.Domain.Extensions;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Application.RestServices
{
    /// <summary>
    /// Provides functionality to export user data to Excel files based on specified filtering and pagination criteria.
    /// </summary>
    /// <remarks>This service supports exporting user details such as ID, name, email, phone number, status,
    /// and creation dates. Filtering by user role is supported if specified. The export operation is performed
    /// asynchronously and may take longer for large datasets.</remarks>
    /// <param name="userManager">The user manager used to retrieve and manage user information for export operations. Cannot be null.</param>
    public class ExportRestService : IExportService
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the ExportRestService class using the specified HTTP client for REST API
        /// communication.
        /// </summary>
        /// <remarks>The provided HttpClient will have its timeout set to 30 seconds and its default
        /// request headers cleared upon initialization. Ensure that the client is not shared with other components that
        /// rely on different timeout or header configurations.</remarks>
        /// <param name="client">The HttpClient instance used to send HTTP requests to the export REST service. Must not be null.</param>
        public ExportRestService(HttpClient client)
        {
            _client = client;
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// Exports a filtered list of users to an Excel file asynchronously.
        /// </summary>
        /// <remarks>The exported Excel file includes user details such as ID, name, email, phone number,
        /// status, and creation dates. If a role is specified in <paramref name="pageParameters"/>, only users in that
        /// role are exported; otherwise, all users are included. The operation is performed asynchronously and may take
        /// longer for large datasets.</remarks>
        /// <param name="pageParameters">The parameters used to filter and paginate the user list. The role filter is applied if specified;
        /// otherwise, all users are included.</param>
        /// <returns>A string containing the file path or identifier of the generated Excel file.</returns>
        public async Task<string> ExportToExcelAsync(UserPageParameters pageParameters)
        {
            var response = await _client.GetAsync($"account/exportNow?{pageParameters.GetQueryString()}");
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
    }
}
