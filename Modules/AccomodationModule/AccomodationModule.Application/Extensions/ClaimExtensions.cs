using System.Reflection;
using IdentityModule.Domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace AccomodationModule.Application.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring claims-based authorization policies.
    /// </summary>
    /// <remarks>This class contains methods that extend the functionality of <see
    /// cref="AuthorizationOptions"> to simplify the registration of claims-based policies, particularly for
    /// permissions.</remarks>
    public static class ClaimsExtensions
    {
        /// <summary>
        /// Registers authorization policies for accommodation-related permission claims.
        /// </summary>
        /// <remarks>This method dynamically registers authorization policies based on the static fields
        /// defined in the nested types of <see cref="Permissions"/>. Each policy requires a claim of type
        /// <see cref="ApplicationClaimTypes.Permission"/> with a value matching the corresponding permission.</remarks>
        /// <param name="options">The <see cref="AuthorizationOptions"/> instance to which the policies will be added.</param>
        /// <returns>The updated <see cref="AuthorizationOptions"/> instance containing the registered policies.</returns>
        public static AuthorizationOptions RegisterAccomodationPermissionClaims(this AuthorizationOptions options)
        {
            List<FieldInfo> list = typeof(Domain.Constants.Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)).ToList();

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
    }
}
