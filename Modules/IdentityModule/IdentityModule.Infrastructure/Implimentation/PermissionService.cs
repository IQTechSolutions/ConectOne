using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides services for managing role-based permissions and claims within the application, including retrieving,
    /// creating, updating, and deleting role claims.
    /// </summary>
    /// <remarks>This service encapsulates common operations for role claim management, including asynchronous
    /// retrieval, modification, and deletion of claims. Changes to claims are not persisted until the appropriate save
    /// method is called. Thread safety depends on the underlying context and role manager implementations.</remarks>
    /// <param name="context">The database context used to access and manipulate role claim data. Cannot be null.</param>
    /// <param name="roleManager">The role manager used to manage roles and their associated claims. Cannot be null.</param>
    public class PermissionService(GenericDbContext context, RoleManager<ApplicationRole> roleManager) : IPermissionService
    {
        /// <summary>
        /// Retrieves the list of claims associated with the specified role identifier asynchronously.
        /// </summary>
        /// <param name="roleId">The unique identifier of the role for which to retrieve claims. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IBaseResult with a list of
        /// RoleClaimResponse objects for the specified role. If no claims are found, the list will be empty.</returns>
        public async Task<IBaseResult<List<RoleClaimResponse>>> ClaimsByRoleIdAsync(string roleId)
        {
            var roleClaims = context.Set<ApplicationRoleClaim>()
                .Include(x => x.Role)
                .Where(x => x.RoleId == roleId);

            return await Result<List<RoleClaimResponse>>.SuccessAsync(await roleClaims.Select(c => new RoleClaimResponse(c)).ToListAsync());
        }

        /// <summary>
        /// Creates a new role claim or updates an existing one based on the specified request.
        /// </summary>
        /// <remarks>If the request does not specify an ID, a new claim is created unless an identical
        /// claim already exists. If an ID is provided, the corresponding claim is updated if found; otherwise, no
        /// action is taken. Changes are not persisted to the database until the appropriate save method is
        /// called.</remarks>
        /// <param name="request">The role claim request containing the claim details to create or update. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        public async Task<IBaseResult> SaveRoleClaimsAsync(RoleClaimRequest request)
        {
            // If no ID, we assume it's a new claim
            if (request.Id == null)
            {
                var existingRoleClaim = await context.Set<ApplicationRoleClaim>()
                    .SingleOrDefaultAsync(x => x.RoleId == request.RoleId && x.ClaimType == request.Type && x.ClaimValue == request.Value);

                // If the claim already exists, no need to duplicate
                if (existingRoleClaim != null) return await Result.FailAsync("Role Claim does not exist"); 

                await context.Set<ApplicationRoleClaim>().AddAsync(new ApplicationRoleClaim(request.Description, request.Group));
                //await _dbContext.SaveChangesAsync(_currentUserService.UserId);
            }
            else
            {
                // Otherwise update the existing claim
                var existingRoleClaim = await context.Set<ApplicationRoleClaim>()
                    .Include(x => x.Role)
                    .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (existingRoleClaim == null) return await Result.FailAsync("Role Claim does not exist");

                existingRoleClaim.ClaimType = request.Type;
                existingRoleClaim.ClaimValue = request.Value;
                existingRoleClaim.Group = request.Group;
                existingRoleClaim.Description = request.Description;
                existingRoleClaim.RoleId = request.RoleId;

                context.Set<ApplicationRoleClaim>().Update(existingRoleClaim);
                //await _dbContext.SaveChangesAsync(_currentUserService.UserId);
            }

            return await Result.SuccessAsync("Role Claim Successfully Removed");
        }

        /// <summary>
        /// Updates the claims associated with a role based on the specified permission request.
        /// </summary>
        /// <remarks>Claims present in the request but not currently assigned to the role are added, while
        /// claims assigned to the role but not present in the request are removed. The operation is performed
        /// asynchronously.</remarks>
        /// <param name="request">The permission request containing the role identifier and the set of claims to be assigned to the role.
        /// Cannot be null.</param>
        /// <returns>A result indicating the outcome of the update operation. The result contains success or failure information
        /// and any relevant error messages.</returns>
        public async Task<IBaseResult> UpdateRoleClaimsAsync(PermissionRequest request)
        {
            var role = await roleManager.FindByIdAsync(request.RoleId);
            var existingClaims = await roleManager.GetClaimsAsync(role);

            try
            {
                // Add new claims that aren't in existingClaims
                foreach (var claim in request.RoleClaims.Where(c => existingClaims.All(g => g.Value != c.Value)))
                {
                    var result = await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, claim.Value));
                    if (!result.Succeeded)
                        return await Result.FailAsync(result.Errors.Select(c => c.Description).ToList());
                }
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                // Remove claims that exist in the role but not in the request
                foreach (var claim in existingClaims.Where(c => request.RoleClaims.All(g => g.Value != c.Value)))
                {
                    var result = await roleManager.RemoveClaimAsync(role, claim);
                    if (!result.Succeeded)
                        return await Result.FailAsync(result.Errors.Select(c => c.Description).ToList());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return await Result.SuccessAsync("Role Claims updated successfully");
        }

        /// <summary>
        /// Deletes the role claim with the specified identifier if it exists.
        /// </summary>
        /// <remarks>If no role claim with the specified identifier exists, the method completes without
        /// performing any action. This method does not save changes to the database; callers are responsible for
        /// persisting changes if required.</remarks>
        /// <param name="id">The unique identifier of the role claim to delete.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task<IBaseResult> DeleteRoleClaimAsync(int id)
        {
            var existingRoleClaim = await context.Set<ApplicationRoleClaim>()
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingRoleClaim != null)
            {
                context.Set<ApplicationRoleClaim>().Remove(existingRoleClaim);
                //await _dbContext.SaveChangesAsync(_currentUserService.UserId);
            }

            return await Result.SuccessAsync("Role Claim Successfully Removed");
        }
    }
}
