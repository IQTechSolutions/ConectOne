namespace ConectOne.Domain.Interfaces
{
    /// <summary>
    /// Interface for entities that require auditing.
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        /// The username or identifier of the user who created the entity.
        /// Allows tracking of the entity’s origin and helps with auditing or troubleshooting changes.
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// The timestamp when the entity was first created.
        /// Useful for displaying creation dates in the UI or for debugging and audit logs.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// The username or identifier of the user who last modified the entity.
        /// Allows auditing of changes, letting administrators or support teams see who made the latest update.
        /// </summary>
        public string? LastModifiedBy { get; set; }

        /// <summary>
        /// The timestamp when the entity was last modified.
        /// Useful for showing last update times to users and maintaining an audit trail of changes.
        /// </summary>
        public DateTime? LastModifiedOn { get; set; }

        /// <summary>
        /// Property indicating whether the entity has been soft-deleted.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Property containing the date and time the entity has been soft-deleted.
        /// </summary>
        DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Row‑version column used by EF Core to detect “lost updates”.
        /// Updated automatically by the database each time the row changes.
        /// </summary>
        byte[] RowVersion { get; set; }
    }

    /// <summary>
    /// Interface for entities that require auditing and have a specific type of identifier.
    /// </summary>
    /// <typeparam name="TId">The type of the entity's primary key (e.g., int, Guid, string).</typeparam>
    public interface IAuditableEntity<TId> : IAuditableEntity
    {
        /// <summary>
        /// The primary key of the entity, decorated with [Key] and configured to use database-generated identity.
        /// 'TId' makes it flexible, allowing different entity classes to use different key types.
        /// </summary>
        TId Id { get; set; }
    }

}
