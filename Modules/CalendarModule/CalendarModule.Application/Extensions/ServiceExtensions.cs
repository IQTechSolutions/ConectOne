using System.Reflection;
using IdentityModule.Domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace CalendarModule.Application.Extensions
{
    /// <summary>
    /// Registers permission-based authorization policies for the Products module.
    /// </summary>
    /// <remarks>This method dynamically retrieves all permission constants defined in the nested types of
    /// <c>Constants.Permissions</c> and registers an authorization policy for each permission. Each policy requires a
    /// claim of type <see cref="ApplicationClaimTypes.Permission"/> with a value matching the permission.</remarks>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers permission claims for the Products module in the provided <see cref="AuthorizationOptions"/>.
        /// </summary>
        /// <remarks>This method dynamically retrieves all permission constants defined in the nested
        /// types of <c>Constants.Permissions</c> and registers them as authorization policies. Each policy requires a
        /// claim of type <c>ApplicationClaimTypes.Permission</c> with a value matching the corresponding permission
        /// constant.</remarks>
        /// <param name="options">The <see cref="AuthorizationOptions"/> to which the permission claims will be added.</param>
        /// <returns>The updated <see cref="AuthorizationOptions"/> with the registered permission claims.</returns>
        public static AuthorizationOptions RegisterCalendarModulePermissionClaims(this AuthorizationOptions options)
        {
            List<FieldInfo> list = typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList();
            foreach (var prop in list)
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
                }
            }
            return options;
        }
    }
}
