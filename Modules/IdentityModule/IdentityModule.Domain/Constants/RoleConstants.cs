namespace IdentityModule.Domain.Constants
{
    /// <summary>
    /// Provides a centralized set of constant values for well-known user role names used throughout the application.
    /// </summary>
    /// <remarks>Use these constants to avoid hard-coding role names when performing role-based authorization
    /// or access checks. This helps ensure consistency and reduces the risk of typographical errors.</remarks>
    public static class RoleConstants
    {
        /// <summary>
        /// Represents the role name for a user with full administrative privileges.
        /// </summary>
        public const string SuperUser = "Super User";

        /// <summary>
        /// Represents the role name for an administrator user.
        /// </summary>
        public const string Administrator = "Administrator";

        /// <summary>
        /// Represents the role name for a company administrator.
        /// </summary>
        public const string CompanyAdmin = "Company Admin";

        /// <summary>
        /// Represents the user role name for a guest user account.
        /// </summary>
        public const string Guest = "Guest";

        /// <summary>
        /// Represents the name of the Basic authentication scheme.
        /// </summary>
        public const string Basic = "Basic";

        /// <summary>
        /// Represents the key name for the driver setting or configuration value.
        /// </summary>
        public const string Driver = "Driver";

        /// <summary>
        /// Represents the string value "Parent" as a constant.
        /// </summary>
        public const string Parent = "Parent";

        /// <summary>
        /// Represents the string value used to identify a learner role or type.
        /// </summary>
        public const string Learner = "Learner";

        /// <summary>
        /// Represents the role name for a teacher user.
        /// </summary>
        public const string Teacher = "Teacher";

        /// <summary>
        /// Represents the key name for specifying a provider in a configuration or connection string.
        /// </summary>
        public const string Provider = "Provider";

        /// <summary>
        /// Represents the string value "Consumer" used to identify a consumer role or type.
        /// </summary>
        public const string Consumer = "Consumer";
    }
}
