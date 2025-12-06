using System.Reflection;
using System.Security.Claims;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Permissions = FilingModule.Domain.Constants.Permissions;

namespace KiburiOnline.Blazor.Shared.Extensions
{
    /// <summary>
    /// Provides extension methods for managing claims in a <see cref="ClaimsPrincipal"/>.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Adds a permission claim to a role if it does not already exist.
        /// </summary>
        /// <param name="roleManager">The role manager to use for managing roles.</param>
        /// <param name="role">The role to add the permission claim to.</param>
        /// <param name="permission">The permission to add as a claim.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the result of the operation.</returns>
        public static async Task<IdentityResult> AddPermissionClaim(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
            {
                return await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
            }

            return IdentityResult.Failed();
        }

        /// <summary>
        /// Gets a list of all registered permissions.
        /// </summary>
        /// <param name="all">If true, gets all permissions; otherwise, gets only administrator permissions.</param>
        /// <returns>A list of registered permissions.</returns>
        public static List<string> GetRegisteredPermissions(bool all = true)
        {
            var permissions = new List<string>();

            List<FieldInfo> modules;
            if (all)
                modules = GetAllPermissionTypesAsync().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList();
            else
            {
                modules = GetAdmininstratorPermissionsTypesAsync().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList();
            }

            foreach (var module in modules)
            {
                var propertyValue = module.GetValue(null);
                if (propertyValue is not null)
                    permissions.Add(propertyValue.ToString());
            }
            return permissions;
        }

        /// <summary>
        /// Gets all permission types.
        /// </summary>
        /// <returns>A list of all permission types.</returns>
        public static List<Type> GetAllPermissionTypesAsync()
        {
            List<Type> modules = new List<Type>();
            modules.AddRange(typeof(IdentityModule.Domain.Constants.Permissions).GetNestedTypes());
            modules.AddRange(typeof(AccomodationModule.Domain.Constants.Permissions).GetNestedTypes());
            modules.AddRange(typeof(ProductsModule.Domain.Constants.Permissions).GetNestedTypes());
            modules.AddRange(typeof(CalendarModule.Domain.Constants.Permissions).GetNestedTypes());
            modules.AddRange(typeof(FilingModule.Domain.Constants.Permissions).GetNestedTypes());
            return modules;
        }

        /// <summary>
        /// Gets administrator permission types.
        /// </summary>
        /// <returns>A list of administrator permission types.</returns>
        public static List<Type> GetAdmininstratorPermissionsTypesAsync()
        {
            List<Type> modules = new List<Type>();
            modules.AddRange(typeof(IdentityModule.Domain.Constants.Permissions).GetNestedTypes());
            modules.AddRange(typeof(AccomodationModule.Domain.Constants.Permissions).GetNestedTypes());
            modules.AddRange(typeof(ProductsModule.Domain.Constants.Permissions).GetNestedTypes());
            modules.AddRange(typeof(CalendarModule.Domain.Constants.Permissions).GetNestedTypes());
            modules.AddRange(typeof(CalendarModule.Domain.Constants.Permissions).GetNestedTypes());
            modules.AddRange(typeof(FilingModule.Domain.Constants.Permissions).GetNestedTypes());
            return modules;
        }
    }
}
