using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Domain.Extensions
{
    /// <summary>
    /// Provides extension methods for managing claims, permissions, and user-related data.
    /// </summary>
    public static class ClaimsExtensions
    {
        /// <summary>
        /// Registers all permission claims defined in the <see cref="Permissions"/> class as authorization policies.
        /// </summary>
        /// <param name="options">The <see cref="AuthorizationOptions"/> to configure.</param>
        /// <returns>The updated <see cref="AuthorizationOptions"/>.</returns>
        public static AuthorizationOptions RegisterIdentityPermissionClaims(this AuthorizationOptions options)
        {
            List<FieldInfo> list = typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList();

            foreach (var prop in list)
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    options.AddPolicy(propertyValue.ToString(), policy =>
                        policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
                }
            }
            return options;
        }

        /// <summary>
        /// Retrieves all permissions defined in the <see cref="Permissions"/> class and adds them to the provided list.
        /// </summary>
        /// <param name="allPermissions">The list to populate with permissions.</param>
        public static void GetAllPermissions(this List<RoleClaimResponse> allPermissions)
        {
            var modules = typeof(Permissions).GetNestedTypes();

            foreach (var module in modules)
            {
                var moduleName = string.Empty;
                var moduleDescription = string.Empty;

                if (module.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
                    moduleName = displayNameAttribute.DisplayName;

                if (module.GetCustomAttributes(typeof(DescriptionAttribute), true)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                    moduleDescription = descriptionAttribute.Description;

                var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                foreach (var fi in fields)
                {
                    var propertyValue = fi.GetValue(null);

                    if (propertyValue is not null)
                        allPermissions.Add(new RoleClaimResponse
                        {
                            Value = propertyValue.ToString(),
                            Type = ApplicationClaimTypes.Permission,
                            Group = moduleName,
                            Description = moduleDescription
                        });
                }
            }
        }

        /// <summary>
        /// Adds a permission claim to a role if it does not already exist.
        /// </summary>
        /// <param name="roleManager">The <see cref="RoleManager{T}"/> to manage roles.</param>
        /// <param name="role">The role to which the claim will be added.</param>
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

        #region User Claims Retrieval

        /// <summary>
        /// Retrieves the user ID from the claims principal.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <returns>The user ID.</returns>
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        /// <summary>
        /// Retrieves the user's email address from the claims principal.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <returns>The user's email address.</returns>
        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email);
        }

        /// <summary>
        /// Retrieves the user's first name from the claims principal.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <returns>The user's first name.</returns>
        public static string GetUserFirstName(this ClaimsPrincipal user)
        {
            return $"{user.FindFirstValue(ClaimTypes.GivenName)}";
        }

        /// <summary>
        /// Retrieves the user's last name from the claims principal.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <returns>The user's last name.</returns>
        public static string GetUserLastName(this ClaimsPrincipal user)
        {
            return $"{user.FindFirstValue(ClaimTypes.Surname)}";
        }

        /// <summary>
        /// Retrieves the user's display name (first and last name) from the claims principal.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <returns>The user's display name.</returns>
        public static string GetUserDisplayName(this ClaimsPrincipal user)
        {
            return $"{user.FindFirstValue(ClaimTypes.GivenName)} {user.FindFirstValue(ClaimTypes.Surname)}";
        }

        /// <summary>
        /// Retrieves the user's phone number from the claims principal.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <returns>The user's phone number.</returns>
        public static string GetUserPhoneNumber(this ClaimsPrincipal user)
        {
            return $"{user.FindFirstValue(ClaimTypes.MobilePhone)}";
        }

        /// <summary>
        /// Retrieves the user's country from the claims principal.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <returns>The user's country.</returns>
        public static string GetUserCountry(this ClaimsPrincipal user)
        {
            return $"{user.FindFirstValue(ClaimTypes.Country)}";
        }

        /// <summary>
        /// Retrieves the user's province or state from the claims principal.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <returns>The user's province or state.</returns>
        public static string GetUserProvince(this ClaimsPrincipal user)
        {
            return $"{user.FindFirstValue(ClaimTypes.StateOrProvince)}";
        }

        /// <summary>
        /// Retrieves the user's street address from the claims principal.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <returns>The user's street address.</returns>
        public static string GetUserStreetAddress(this ClaimsPrincipal user)
        {
            return $"{user.FindFirstValue(ClaimTypes.StreetAddress)}";
        }

        /// <summary>
        /// Retrieves the user's postal code from the claims principal.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the user.</param>
        /// <returns>The user's postal code.</returns>
        public static string GetUserPostalCode(this ClaimsPrincipal user)
        {
            return $"{user.FindFirstValue(ClaimTypes.PostalCode)}";
        }

        #endregion
    }
}
