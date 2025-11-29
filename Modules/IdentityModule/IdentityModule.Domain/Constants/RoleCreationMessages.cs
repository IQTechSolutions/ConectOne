namespace IdentityModule.Domain.Constants
{
    /// <summary>
    /// Contains constants that describe role creation scenarios requiring additional user action.
    /// </summary>
    public static class RoleCreationMessages
    {
        /// <summary>
        /// Message code returned when a soft-deleted role with the requested name already exists.
        /// </summary>
        public const string DeletedRoleExists = "roles.deleted.exists";
    }
}
