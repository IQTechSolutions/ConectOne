using ConectOne.Domain.Enums;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Enums;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeuralTech.Base.Enums;

namespace IdentityModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides services for managing application roles and their relationships with users and other roles.
    /// </summary>
    /// <remarks>This service offers a variety of role-related operations, including retrieving roles,
    /// managing role-user assignments, creating and updating roles, and handling hierarchical role relationships. It
    /// leverages <see cref="UserManager{TUser}"/> and <see cref="RoleManager{TRole}"/> for user and role management,
    /// and integrates with a database context and repository for additional data operations.</remarks>
    /// <param name="userManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="context"></param>
    /// <param name="userInfoRepo"></param>
    public class RoleService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, GenericDbContextFactory contextFactory, IRepository<UserInfo, string> userInfoRepo) : IRoleService
    {
        private GenericDbContext context => contextFactory.CreateDbContext();

        /// <summary>
        /// Retrieves all application roles as <see cref="RoleDto"/> objects.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<RoleDto>>> AllRoles()
        {
            try
            {
                return await Result<IEnumerable<RoleDto>>.SuccessAsync(roleManager.Roles.Select(c => new RoleDto(c)));
            }
            catch (Exception e)
            {
                return await Result<IEnumerable<RoleDto>>.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Asynchronously retrieves all roles that are designated as product managers.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// with a collection of <see cref="RoleDto"/> objects representing product manager roles. If no product manager
        /// roles exist, the collection will be empty. If an error occurs, the result will indicate failure and include
        /// an error message.</returns>
        public async Task<IBaseResult<IEnumerable<RoleDto>>> ProductManagers()
        {
            try
            {
                return await Result<IEnumerable<RoleDto>>.SuccessAsync(context.Roles.Include(c => c.ChildRoles)
                    .Where(c => c.ProductManager).Select(c => new RoleDto(c)));
            }
            catch (Exception e)
            {
                return await Result<IEnumerable<RoleDto>>.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Retrieves the roles assigned to a specific user by ID.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<RoleDto>>> UserRolesAsync(string userId)
        {
            try
            {
                var user = userManager.Users.FirstOrDefault(c => c.Id == userId);
                var selectedRoles = await userManager.GetRolesAsync(user);
                var roles = new List<RoleDto>();

                foreach (var item in selectedRoles)
                {
                    var result = await RoleAsync(item);
                    roles.Add(result.Data);
                }

                return await Result<IEnumerable<RoleDto>>.SuccessAsync(roles);
            }
            catch (Exception e)
            {
                return await Result<IEnumerable<RoleDto>>.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Retrieves the number of users assigned to a specified role.
        /// </summary>
        /// <remarks>This method asynchronously retrieves the users associated with the specified role and
        /// returns their count. Ensure that the role name provided exists in the system to avoid unexpected
        /// results.</remarks>
        /// <param name="roleName">The name of the role for which to count the users. Cannot be null or empty.</param>
        /// <returns>The total number of users in the specified role.</returns>
        public async Task<IBaseResult<int>> RoleUserCount(string roleName)
        {
            try
            {
                var roleUsers = await userManager.GetUsersInRoleAsync(roleName);
                return await Result<int>.SuccessAsync(roleUsers.Count);
            }
            catch (Exception e)
            {
                return await Result<int>.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Retrieves the child roles associated with the specified parent role.
        /// </summary>
        /// <remarks>This method queries the role hierarchy to find all roles that have the specified role
        /// as their parent. The returned roles are projected into <see cref="RoleDto"/> objects for easier
        /// consumption.</remarks>
        /// <param name="roleId">The unique identifier of the parent role whose child roles are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing a collection of <see cref="RoleDto"/> objects representing the child roles. If the operation
        /// fails, the result includes an error message.</returns>
        public async Task<IBaseResult<IEnumerable<RoleDto>>> ChildrenAsync(string roleId)
        {
            try
            {
                var roles = roleManager.Roles.Include(c => c.ChildRoles).Where(c => c.ParentRoleId == roleId);
                return await Result<IEnumerable<RoleDto>>.SuccessAsync(roles.Select(c => new RoleDto(c)));
            }
            catch (Exception e)
            {
                return await Result<IEnumerable<RoleDto>>.FailAsync(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Retrieves detailed information about a role given its name.
        /// </summary>
        public async Task<IBaseResult<RoleDto>> RoleAsync(string roleName)
        {
            var role = roleManager.Roles.FirstOrDefault(c => c.Name == roleName);
            if (role is null)
                return await Result<RoleDto>.FailAsync($"No role with role name '{roleName}' found in database");

            return await Result<RoleDto>.SuccessAsync(new RoleDto(role));
        }

        /// <summary>
        /// Deletes an application role by ID.
        /// </summary>
        public async Task<IBaseResult> DeleteApplicationRole(string id)
        {
            var newRole = await roleManager.FindByIdAsync(id);

            if (newRole.CannotDelete)
                return await Result.FailAsync("This is a system role and cannot be deleted");

            var roleResult = await roleManager.DeleteAsync(newRole);

            if (roleResult.Succeeded)
                return await Result.SuccessAsync("Role Removed Successfully");

            return await Result.FailAsync(roleResult.Errors.Select(c => c.Description).ToList());
        }

        /// <summary>
        /// Creates a new application role with the specified <see cref="RoleDto"/>.
        /// </summary>
        public async Task<IBaseResult<RoleDto>> CreateApplicationRole(RoleDto dto)
        {
            if (dto == null)
            {
                return await Result<RoleDto>.FailAsync("Role payload cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return await Result<RoleDto>.FailAsync("Role name cannot be empty.");
            }

            var normalizedRoleName = roleManager.NormalizeKey(dto.Name);
            var existingRole = await context.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName);

            if (existingRole is null)
            {
                return await CreateRoleAsync(dto);
            }

            if (!existingRole.IsDeleted)
            {
                return await Result<RoleDto>.FailAsync($"A role with the name '{dto.Name}' already exists.");
            }

            if (dto.CreationAction == RoleCreationAction.Default)
            {
                return new Result<RoleDto>
                {
                    Succeeded = false,
                    Data = new RoleDto(existingRole),
                    Messages = new List<string>
                    {
                        RoleCreationMessages.DeletedRoleExists,
                        $"A role named '{dto.Name}' was previously deleted. Choose whether to restore it or create a new role."
                    }
                };
            }

            if (dto.CreationAction == RoleCreationAction.Restore)
            {
                existingRole.IsDeleted = false;
                existingRole.DeletedOn = null;
                existingRole.Description = dto.Description;
                existingRole.AdministrativeRole = dto.AdministrativeRole;
                existingRole.AdvertiseOnlyToMembers = dto.AdvertiseOnlyToMembers;
                existingRole.AdvertiseRegistration = dto.AdvertiseRegistration;
                existingRole.NotAvailableForRegistrationSelection = dto.NotAvailableForRegistrationSelection;

                var updateResult = await roleManager.UpdateAsync(existingRole);
                if (updateResult.Succeeded)
                {
                    return await Result<RoleDto>.SuccessAsync(new RoleDto(existingRole));
                }

                return await Result<RoleDto>.FailAsync(updateResult.Errors.Select(c => c.Description).ToList());
            }

            if (dto.CreationAction == RoleCreationAction.Recreate)
            {
                return await RecreateRoleAsync(existingRole, dto);
            }

            return await Result<RoleDto>.FailAsync("Unsupported role creation action.");
        }

        /// <summary>
        /// Creates a new role asynchronously based on the provided role data transfer object (DTO).
        /// </summary>
        /// <remarks>This method uses the role manager to create a new role in the system. The role's
        /// properties are initialized based on the values provided in the <paramref name="dto"/>.</remarks>
        /// <param name="dto">The <see cref="RoleDto"/> containing the details of the role to be created, including its name, description,
        /// and configuration options.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="RoleDto"/>: <list type="bullet"> <item> <description>A successful result containing the
        /// created role's details if the operation succeeds.</description> </item> <item> <description>A failed result
        /// containing a list of error descriptions if the operation fails.</description> </item> </list></returns>
        private async Task<IBaseResult<RoleDto>> CreateRoleAsync(RoleDto dto)
        {
            var newRole = new ApplicationRole(dto.Name, dto.Description)
            {
                AdministrativeRole = dto.AdministrativeRole,
                AdvertiseOnlyToMembers = dto.AdvertiseOnlyToMembers,
                AdvertiseRegistration = dto.AdvertiseRegistration,
                NotAvailableForRegistrationSelection = dto.NotAvailableForRegistrationSelection,
                ProductManager = dto.ProductManager
            };

            var roleResult = await roleManager.CreateAsync(newRole);

            if (roleResult.Succeeded)
            {
                return await Result<RoleDto>.SuccessAsync(new RoleDto(newRole));
            }

            return await Result<RoleDto>.FailAsync(roleResult.Errors.Select(c => c.Description).ToList());
        }

        /// <summary>
        /// Recreates an existing role by deleting its associated claims, user-role mappings, and other related data,
        /// and then creates a new role with the specified details.
        /// </summary>
        /// <remarks>This method performs the following steps: <list type="bullet">
        /// <item><description>Deletes all claims and user-role mappings associated with the existing
        /// role.</description></item> <item><description>Clears the change tracker to ensure no stale data is
        /// tracked.</description></item> <item><description>Attempts to create a new role using the provided <paramref
        /// name="dto"/>.</description></item> <item><description>Commits the transaction if the operation succeeds, or
        /// rolls back the transaction in case of failure.</description></item> </list> If the operation fails at any
        /// point, the transaction is rolled back, and an error result is returned.</remarks>
        /// <param name="existingRole">The existing role to be replaced. This role's data will be removed from the database.</param>
        /// <param name="dto">The data transfer object containing the details for the new role to be created.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="IBaseResult{RoleDto}"/> indicating the success or failure of the operation. If successful, the result
        /// includes the details of the newly created role.</returns>
        private async Task<IBaseResult<RoleDto>> RecreateRoleAsync(ApplicationRole existingRole, RoleDto dto)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var roleId = existingRole.Id;

                await context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM [Identity].[RoleClaims] WHERE [RoleId] = {0}", roleId);
                await context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM [AspNetUserRoles] WHERE [RoleId] = {0}", roleId);
                await context.Database.ExecuteSqlRawAsync(
                    "UPDATE [Identity].[UserRoles] SET [ParentRoleId] = NULL WHERE [ParentRoleId] = {0}", roleId);
                await context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM [Identity].[UserRoles] WHERE [Id] = {0}", roleId);

                context.ChangeTracker.Clear();

                var creationResult = await CreateRoleAsync(dto);
                if (!creationResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return creationResult;
                }

                await transaction.CommitAsync();
                return creationResult;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return await Result<RoleDto>.FailAsync(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new application role with the specified <see cref="RoleDto"/>.
        /// </summary>
        public async Task<IBaseResult<RoleDto>> UpdateApplicationRole(RoleDto dto)
        {
            var role = await roleManager.FindByIdAsync(dto.Id);
            if (role != null)
            {
                role.Name = dto.Name;
                role.Description = dto.Description;
                role.AdvertiseRegistration = dto.AdvertiseRegistration;
                role.AdvertiseOnlyToMembers = dto.AdvertiseOnlyToMembers;
                role.AdministrativeRole = dto.AdministrativeRole;
                role.NotAvailableForRegistrationSelection = dto.NotAvailableForRegistrationSelection;
                role.ProductManager = dto.ProductManager;

                var result = roleManager.UpdateAsync(role);
                
                if (result.Result.Succeeded)
                    return await Result<RoleDto>.SuccessAsync(dto);
            }
            return await Result<RoleDto>.FailAsync($"No role with role name: '{dto.Name}' found in the database");
        }

        /// <summary>
        /// Adds a user to a specified role by user ID and role name.
        /// </summary>
        public async Task<IBaseResult> AddUserToRoleAsync(string userId, string role)
        {
            try
            {
                var user = userManager.Users.FirstOrDefault(c => c.Id == userId);
                var result = await userManager.AddToRoleAsync(user, role);
                return await Result.SuccessAsync();
            }
            catch (Exception e)
            {
                return await Result.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Assigns a role to a parent role, establishing a hierarchical relationship between them.
        /// </summary>
        /// <remarks>This method updates the specified role to associate it with the given parent role. If
        /// the operation fails, the result will include the relevant error messages. Exceptions are also captured and
        /// returned as part of the result.</remarks>
        /// <param name="parentRoleId">The unique identifier of the parent role to which the role will be assigned.</param>
        /// <param name="roleToBeManagedId">The unique identifier of the role to be managed under the parent role.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the success or failure of the operation. On success, the result includes a success message; on
        /// failure, it includes error details.</returns>
        public async Task<IBaseResult> AddRoleToBeManagedToParentAsync(string parentRoleId, string roleToBeManagedId)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(roleToBeManagedId);
                role.ParentRoleId = parentRoleId;
                var result = await roleManager.UpdateAsync(role);
                if (!result.Succeeded) return await Result.FailAsync(result.Errors.Select(c => c.Description).ToList());
                return await Result.SuccessAsync("Role successfully assigned to parent role");
            }
            catch (Exception e)
            {
                return await Result.FailAsync(e.Message);
            }
        }

        /// <summary>
        /// Removes the specified role from a user.
        /// </summary>
        public async Task<IBaseResult> RemoveRoleAsync(string userId, string role)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(c => c.Id == userId);
                await userManager.RemoveFromRoleAsync(user, role);
                return await Result.SuccessAsync("User Role successfully removed");
            }
            catch (Exception ex)
            {
                return await Result.FailAsync(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of users associated with a specified role, including their details and notification
        /// preferences.
        /// </summary>
        /// <remarks>Each user in the returned list includes their first name, last name, email, profile
        /// image (if available), and notification preferences. If no users are found for the specified role, the result
        /// will contain an empty list.</remarks>
        /// <param name="roleName">The name of the role for which to retrieve the list of users. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{RecipientDto}"/>. The result includes the details of users in
        /// the specified role.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> RoleNotificationsUserList(string roleName)
        {
            var roleUsers = await userManager.GetUsersInRoleAsync(roleName);
            var list = new List<RecipientDto>();
            foreach (var user in roleUsers)
            {
                var spec = new LambdaSpec<UserInfo>(c => c.Id == user.Id);
                spec.AddInclude(c => c.Include(g => g.Images));
                var result = await userInfoRepo.FirstOrDefaultAsync(spec, false);
                if (result.Succeeded)
                    list.Add(new RecipientDto(user.Id, result.Data.FirstName, result.Data.LastName, new List<string>() { user.Email }, true, false, result.Data.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Profile)?.Image?.RelativePath, MessageType.RoleMessage));
            }

            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(list);
        }
    }
}
