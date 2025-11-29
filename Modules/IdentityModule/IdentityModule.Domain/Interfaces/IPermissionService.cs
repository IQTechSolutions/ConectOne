using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for managing role claims/permissions.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Retrieves claims associated with the specified role.
        /// </summary>
        Task<IBaseResult<List<RoleClaimResponse>>> ClaimsByRoleIdAsync(string roleId);

        /// <summary>
        /// Saves or updates a role claim.
        /// </summary>
        Task<IBaseResult> SaveRoleClaimsAsync(RoleClaimRequest request);

        /// <summary>
        /// Updates role claims according to the provided request.
        /// </summary>
        Task<IBaseResult> UpdateRoleClaimsAsync(PermissionRequest request);

        /// <summary>
        /// Deletes a role claim by ID.
        /// </summary>
        Task<IBaseResult> DeleteRoleClaimAsync(int id);
    }
}
