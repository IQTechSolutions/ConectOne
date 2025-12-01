using System.Reflection;
using IdentityModule.Domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace SchoolsModule.Application.Extensions
{
    /// <summary>
    /// Registers permission claims for the School module into the provided <see cref="AuthorizationOptions"/>.
    /// </summary>
    /// <remarks>This method dynamically retrieves all public, static fields from nested types within the 
    /// <c>Identity.Shared.Constants.Permissions</c> and <c>Permissions</c> classes. Each field value is used to create 
    /// a policy that requires a claim of type <c>ApplicationClaimTypes.Permission</c> with the corresponding field
    /// value.</remarks>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers permission-based authorization policies for the school module.
        /// </summary>
        /// <remarks>This method dynamically retrieves all permission constants defined in the nested
        /// types of the  <c>Identity.Shared.Constants.Permissions</c> and <c>Permissions</c> classes. For each
        /// permission,  it adds an authorization policy that requires the corresponding permission claim.</remarks>
        /// <param name="options">The <see cref="AuthorizationOptions"/> to which the policies will be added.</param>
        /// <returns>The updated <see cref="AuthorizationOptions"/> instance with the registered policies.</returns>
        public static AuthorizationOptions RegisterSchoolModulePermissionClaims(this AuthorizationOptions options)
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
