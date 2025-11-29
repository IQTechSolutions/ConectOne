using System.ComponentModel;

namespace IdentityModule.Domain.Constants
{
    /// <summary>
    /// Provides constant claim type names used for application-specific identity claims.
    /// </summary>
    /// <remarks>These claim type names can be used when creating or evaluating claims in authentication and
    /// authorization scenarios within the application. They are intended to supplement or extend the standard claim
    /// types defined in the .NET framework.</remarks>
    public static class ApplicationClaimTypes
    {
        /// <summary>
        /// Represents the permission claim type name used for authorization policies.
        /// </summary>
        [Description("Permission")]
        public const string Permission = "Permission";

        /// <summary>
        /// Represents the field name for the first name property in data models or serialization contexts.
        /// </summary>
        [Description("First Name")]
        public const string FirstName = "FirstName";

        /// <summary>
        /// Represents the key name for the last name field.
        /// </summary>
        [Description("Last Name")]
        public const string LastName = "LastName";
    }
}
