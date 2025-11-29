using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Interfaces;

namespace ConectOne.Domain.Entities
{
    /// <summary>
    /// Provides a base class for all entities in the system, defining common fields like Id, 
    /// creation/modification timestamps, and user tracking. Implements IAuditableEntity for 
    /// consistent auditing across the domain.
    /// </summary>
    /// <typeparam name="TId">The type of the entity's primary key (e.g., int, Guid, string).</typeparam>
    public abstract class EntityBase<TId> : IAuditableEntity<TId>
    {
        /// <summary>
        /// The primary key of the entity, decorated with [Key] and configured to use database-generated identity.
        /// 'TId' makes it flexible, allowing different entity classes to use different key types.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key, DisplayName("Id"), Column("Id")]
        public TId Id { get; set; } = default!;

        /// <summary>
        /// The username or identifier of the user who created the entity.
        /// Allows tracking of the entity’s origin and helps with auditing or troubleshooting changes.
        /// </summary>
        public string? CreatedBy { get; set; } = "";

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
        /// Property containing the date and time the entity has been soft-deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Property containing the date and time the entity has been soft-deleted.
        /// </summary>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Row‑version column used by EF Core to detect “lost updates”.
        /// Updated automatically by the database each time the row changes.
        /// </summary>
        [Timestamp] public byte[] RowVersion { get; set; } = null!;
    }
}