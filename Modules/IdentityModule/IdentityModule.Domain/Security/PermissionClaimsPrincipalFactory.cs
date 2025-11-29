using System.Security.Claims;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace IdentityModule.Domain.Security;

/// <summary>
/// Builds a <see cref="ClaimsPrincipal"/> that includes permission claims sourced from the
/// signed-in user's direct assignments and their associated roles.
/// </summary>
public sealed class PermissionClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
{
    private readonly IMemoryCache memoryCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionClaimsPrincipalFactory"/> class.
    /// </summary>
    public PermissionClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor,
        IMemoryCache memoryCache)
        : base(userManager, roleManager, optionsAccessor)
    {
        this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    /// <summary>
    /// Generates a <see cref="ClaimsIdentity"/> for the specified user, including permission claims from both user and
    /// role assignments.
    /// </summary>
    /// <remarks>Permission claims are aggregated from both the user's direct claims and any roles assigned to
    /// the user. Claims are cached for improved performance; changes to user or role claims may not be immediately
    /// reflected until the cache expires.</remarks>
    /// <param name="user">The user for whom to generate the claims identity. Cannot be null.</param>
    /// <returns>A <see cref="ClaimsIdentity"/> containing the user's claims, including permission claims from user and role
    /// claims. If the user is null, returns the base identity.</returns>
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        if (user is null)
        {
            return identity;
        }

        var cacheKey = $"permission-claims:{user.Id}:{user.SecurityStamp}";
        var permissionClaims = await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var claims = new HashSet<Claim>(ClaimComparer.Instance);

            if (UserManager.SupportsUserClaim)
            {
                var userClaims = await UserManager.GetClaimsAsync(user);
                foreach (var claim in userClaims.Where(c => c.Type == ApplicationClaimTypes.Permission))
                {
                    claims.Add(claim);
                }
            }

            if (UserManager.SupportsUserRole)
            {
                var roles = await UserManager.GetRolesAsync(user);

                foreach (var roleName in roles)
                {
                    if (!RoleManager.SupportsRoleClaims)
                    {
                        continue;
                    }

                    var role = await RoleManager.FindByNameAsync(roleName);
                    if (role is null)
                    {
                        continue;
                    }

                    var roleClaims = await RoleManager.GetClaimsAsync(role);
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

        return identity;
    }

    /// <summary>
    /// Provides a case-insensitive equality comparer for <see cref="Claim"/> objects based on their type and value.
    /// </summary>
    /// <remarks>This comparer considers two <see cref="Claim"/> instances equal if their <see
    /// cref="Claim.Type"/> properties are equal using ordinal case-insensitive comparison and their <see
    /// cref="Claim.Value"/> properties are equal using ordinal comparison. This is useful for collections or operations
    /// that require claim deduplication or lookup based on these fields.</remarks>
    private sealed class ClaimComparer : IEqualityComparer<Claim>
    {
        /// <summary>
        /// Gets a singleton instance of the ClaimComparer class.
        /// </summary>
        /// <remarks>Use this property to access a shared, thread-safe instance of ClaimComparer without
        /// creating a new object.</remarks>
        public static ClaimComparer Instance { get; } = new();

        /// <summary>
        /// Determines whether two Claim objects are equal based on their type and value.
        /// </summary>
        /// <remarks>Two Claim objects are considered equal if their Type properties are equal using a
        /// case-insensitive comparison and their Value properties are equal using a case-sensitive
        /// comparison.</remarks>
        /// <param name="x">The first Claim to compare, or null.</param>
        /// <param name="y">The second Claim to compare, or null.</param>
        /// <returns>true if both Claim objects are equal or both are null; otherwise, false.</returns>
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
        /// <remarks>The hash code is computed using the claim's type (converted to uppercase using the
        /// invariant culture) and value. This ensures that claims with type strings differing only in case produce the
        /// same hash code.</remarks>
        /// <param name="obj">The claim for which to compute the hash code. Cannot be null.</param>
        /// <returns>A 32-bit signed integer hash code for the specified claim.</returns>
        public int GetHashCode(Claim obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            return HashCode.Combine(obj.Type?.ToUpperInvariant(), obj.Value);
        }
    }
}
