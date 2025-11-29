using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Domain.Entities
{
    /// <summary>
    /// Represents a claim associated with a role in the application.
    /// This class extends <see cref="IdentityRoleClaim{TKey}"/> to include additional metadata for auditing and grouping.
    /// </summary>
    public class ApplicationRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRoleClaim"/> class with default values.
        /// </summary>
        public ApplicationRoleClaim() : base()
        {
            CreatedBy = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRoleClaim"/> class with specified description and group.
        /// </summary>
        /// <param name="roleClaimDescription">The description of the role claim.</param>
        /// <param name="roleClaimGroup">The group to which the role claim belongs.</param>
        public ApplicationRoleClaim(string? roleClaimDescription = null, string? roleClaimGroup = null) : base()
        {
            Description = roleClaimDescription;
            Group = roleClaimGroup;
            CreatedBy = string.Empty;
        }

        #endregion

        /// <summary>
        /// Gets or sets the description of the role claim.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the group to which the role claim belongs.
        /// </summary>
        public string? Group { get; set; }

        /// <summary>
        /// Gets or sets the username or identifier of the user who created the role claim.
        /// </summary>
        public string? CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the role claim was created.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the username or identifier of the user who last modified the role claim.
        /// </summary>
        public string? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the role claim was last modified.
        /// </summary>
        public DateTime? LastModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role has been soft-deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the role was soft-deleted.
        /// </summary>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Gets or sets the row version for concurrency checks.
        /// </summary>
        [Timestamp] public byte[] RowVersion { get; set; } = null!;

        /// <summary>
        /// Gets or sets the role associated with this claim.
        /// </summary>
        public virtual ApplicationRole Role { get; set; }

        /// <summary>
        /// Returns a string representation of the role claim.
        /// </summary>
        /// <returns>A string indicating the type of the role claim.</returns>
        public override string ToString()
        {
            return "Application Role Claim";
        }
    }
}

