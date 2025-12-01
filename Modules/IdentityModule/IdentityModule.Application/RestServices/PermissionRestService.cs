using IdentityModule.Domain.Interfaces;
using System.Net.Http.Json;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Application.RestServices
{
    /// <summary>
    /// Provides REST-based operations for managing role claims and permissions within the application.
    /// </summary>
    /// <remarks>This service implements the IPermissionService interface to support asynchronous operations
    /// for retrieving, updating, saving, and deleting role claims. It is typically used by administrative components to
    /// control access rights and permissions for user roles.</remarks>
    public class PermissionRestService : IPermissionService
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the PermissionRestService class using the specified HTTP client for REST API
        /// communication.
        /// </summary>
        /// <remarks>The provided HttpClient will have its timeout set to 30 seconds and its default
        /// request headers cleared upon initialization. Ensure that the client is not shared with other services that
        /// rely on custom timeout or headers.</remarks>
        /// <param name="client">The HTTP client instance used to send requests to the permission service. Must not be null.</param>
        public PermissionRestService(HttpClient client)
        {
            _client = client;
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// Retrieves the list of claims associated with the specified role identifier.
        /// </summary>
        /// <param name="roleId">The unique identifier of the role for which to retrieve claims. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult with a list of
        /// RoleClaimResponse objects representing the claims for the specified role. The list will be empty if the role
        /// has no claims.</returns>
        public async Task<IBaseResult<List<RoleClaimResponse>>> ClaimsByRoleIdAsync(string roleId)
        {
            var path = $"account/roles/role/permissions/{roleId}";
            var response = await _client.GetAsync(path);
            return await response.ToResultAsync<List<RoleClaimResponse>>(); 
        }

        /// <summary>
        /// Saves the specified role claims to the server asynchronously.
        /// </summary>
        /// <param name="request">The request containing the role claims to be saved. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the save operation.</returns>
        public async Task<IBaseResult> SaveRoleClaimsAsync(RoleClaimRequest request)
        {
            var response = await _client.PostAsJsonAsync("account/roles/permissions/save", request);
            return await response.ToResultAsync();
        }

        /// <summary>
        /// Updates the claims associated with a role based on the specified permission request.
        /// </summary>
        /// <param name="request">An object containing the role and the permissions to be updated. Cannot be null.</param>
        /// <returns>A result object indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateRoleClaimsAsync(PermissionRequest request)
        {
            var response = await _client.PostAsJsonAsync("account/roles/permissions/update", request);
            return await response.ToResultAsync();
        }

        /// <summary>
        /// Deletes a role claim identified by the specified ID asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the role claim to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating whether the deletion was successful.</returns>
        public async Task<IBaseResult> DeleteRoleClaimAsync(int id)
        {
            var response = await _client.DeleteAsync($"account/roles/permissions/{id}");
            return await Result.SuccessAsync("Role Claim Successfully Removed");
        }
    }
}
