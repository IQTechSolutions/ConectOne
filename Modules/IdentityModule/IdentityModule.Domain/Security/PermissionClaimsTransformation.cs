using System.Security.Claims;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityModule.Domain.Security;

/// <summary>
/// Provides claims transformation that adds permission claims to a principal based on the user's roles and direct
/// claims.
/// </summary>
/// <remarks>Permission claims are aggregated from both direct user claims and claims assigned to the user's
/// roles. Claims are cached to reduce repeated lookups and improve performance. This class is typically used in
/// authentication pipelines to ensure that permission claims are present on the principal for authorization checks. The
/// transformation does not modify the original principal instance.</remarks>
/// <param name="userManager">The user manager used to retrieve user information and claims.</param>
/// <param name="roleManager">The role manager used to retrieve role information and associated claims.</param>
/// <param name="memoryCache">The memory cache used to store and retrieve permission claims for improved performance.</param>
public sealed class PermissionClaimsTransformation(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMemoryCache memoryCache) : IClaimsTransformation
{
    /// <summary>
    /// Transforms the specified principal by adding permission claims based on the user's roles and direct claims.
    /// </summary>
    /// <remarks>If the principal is not authenticated or already contains permission claims, no changes are
    /// made. Permission claims are retrieved from both the user's direct claims and the claims associated with the
    /// user's roles. Claims are cached for improved performance. The original principal is not modified.</remarks>
    /// <param name="principal">The principal to transform. Must represent an authenticated user.</param>
    /// <returns>A new ClaimsPrincipal instance with permission claims added if applicable; otherwise, the original principal if
    /// no transformation is performed.</returns>
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
        {
            return principal;
        }

        // Clone the principal so the original identity used to create the auth cookie isn't mutated.
        var transformedPrincipal = principal.Clone();
        if (transformedPrincipal.Identity is not ClaimsIdentity identity)
        {
            return principal;
        }

        if (identity.HasClaim(c => c.Type == ApplicationClaimTypes.Permission))
        {
            return transformedPrincipal;
        }

        var user = await userManager.GetUserAsync(transformedPrincipal);
        if (user is null)
        {
            return transformedPrincipal;
        }

        var cacheKey = $"permission-claims:{user.Id}:{user.SecurityStamp}";
        var permissionClaims = await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var claims = new HashSet<Claim>(ClaimComparer.Instance);

            if (userManager.SupportsUserClaim)
            {
                var userClaims = await userManager.GetClaimsAsync(user);
                foreach (var claim in userClaims.Where(c => c.Type == ApplicationClaimTypes.Permission))
                {
                    claims.Add(claim);
                }
            }

            if (userManager.SupportsUserRole)
            {
                var roles = await userManager.GetRolesAsync(user);
                foreach (var roleName in roles)
                {
                    var role = await roleManager.FindByNameAsync(roleName);
                    if (role is null)
                    {
                        continue;
                    }

                    var roleClaims = await roleManager.GetClaimsAsync(role);
                    foreach (var claim in roleClaims.Where(c => c.Type == ApplicationClaimTypes.Permission))
                    {
                        claims.Add(claim);
                    }
                }
            }

            return claims.ToArray();
        }) ?? Array.Empty<Claim>();

        foreach (var claim in permissionClaims)
        {
            if (!identity.HasClaim(claim.Type, claim.Value))
            {
                identity.AddClaim(claim);
            }
        }

        return transformedPrincipal;
    }

    /// <summary>
    /// Provides a case-insensitive equality comparer for <see cref="Claim"/> objects based on their type and value.
    /// </summary>
    /// <remarks>This comparer considers two <see cref="Claim"/> instances equal if their <see
    /// cref="Claim.Type"/> properties are equal using ordinal case-insensitive comparison and their <see
    /// cref="Claim.Value"/> properties are equal using ordinal comparison. This class is sealed and intended for use
    /// where claims need to be compared or used as keys in collections such as dictionaries or hash sets.</remarks>
    private sealed class ClaimComparer : IEqualityComparer<Claim>
    {
        /// <summary>
        /// Gets a thread-safe, shared instance of the ClaimComparer class.
        /// </summary>
        /// <remarks>Use this property to access a default, singleton instance of ClaimComparer without
        /// creating a new object. This instance can be used wherever a ClaimComparer is required.</remarks>
        public static ClaimComparer Instance { get; } = new();

        /// <summary>
        /// Determines whether two Claim objects are equal based on their type and value.
        /// </summary>
        /// <remarks>Two Claim objects are considered equal if their Type properties are equal using a
        /// case-insensitive comparison and their Value properties are equal using a case-sensitive comparison. If both
        /// parameters refer to the same object or are both null, the method returns true.</remarks>
        /// <param name="x">The first Claim to compare, or null.</param>
        /// <param name="y">The second Claim to compare, or null.</param>
        /// <returns>true if both Claim objects are equal; otherwise, false.</returns>
        public bool Equals(Claim? x, Claim? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            return string.Equals(x.Type, y.Type, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(x.Value, y.Value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns a hash code for the specified claim object.
        /// </summary>
        /// <remarks>The hash code is based on the claim's type (compared in a case-insensitive manner)
        /// and value. This ensures that claims with the same type and value, regardless of the case of the type,
        /// produce the same hash code.</remarks>
        /// <param name="obj">The claim for which to compute the hash code. Cannot be null.</param>
        /// <returns>A 32-bit signed integer hash code for the specified claim.</returns>
        public int GetHashCode(Claim obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            return HashCode.Combine(obj.Type?.ToUpperInvariant(), obj.Value);
        }
    }
}
