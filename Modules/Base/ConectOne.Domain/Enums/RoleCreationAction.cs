namespace ConectOne.Domain.Enums
{
    /// <summary>
    /// Represents the action to perform when creating a role with a name that already exists.
    /// </summary>
    public enum RoleCreationAction
    {
        /// <summary>
        /// No special action requested; the API should determine the appropriate behaviour.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Indicates that an existing soft-deleted role should be restored instead of creating a new entry.
        /// </summary>
        Restore = 1,

        /// <summary>
        /// Indicates that a previously existing role should be removed and recreated from scratch.
        /// </summary>
        Recreate = 2
    }
}
