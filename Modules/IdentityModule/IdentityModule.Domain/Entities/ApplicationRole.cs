using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IdentityModule.Domain.Entities
{
    /// <summary>
    /// Represents a role in the application.
    /// This class extends <see cref="IdentityRole"/> to include additional metadata for auditing and role claims.
    /// </summary>
    public class ApplicationRole : IdentityRole, IAuditableEntity<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole"/> class with default values.
        /// </summary>
        public ApplicationRole() : base()
        {
            CreatedBy = string.Empty;
            RoleClaims = new HashSet<ApplicationRoleClaim>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole"/> class with a specified role name and description.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <param name="roleDescription">The description of the role.</param>
        public ApplicationRole(string roleName, string roleDescription = null, bool cannotDelete = false, bool advertiseRegistration = false,  bool advertiseOnlyToMembers = false, bool productManager = false) : base(roleName)
        {
            Description = roleDescription;
            CreatedBy = string.Empty;
            RoleClaims = new HashSet<ApplicationRoleClaim>();
            CannotDelete = cannotDelete;
            AdvertiseOnlyToMembers = advertiseOnlyToMembers;
            AdvertiseRegistration = advertiseRegistration;
            ProductManager=productManager;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is unavailable for selection during registration.
        /// </summary>
        public bool NotAvailableForRegistrationSelection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has administrative privileges.
        /// </summary>
        public bool AdministrativeRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is protected from deletion.
        /// </summary>
        public bool CannotDelete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the registration process should be advertised.
        /// </summary>
        public bool AdvertiseRegistration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the advertisement is restricted to members only.
        /// </summary>
        public bool AdvertiseOnlyToMembers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the advertisement is visible only to members.
        /// </summary>
        public bool ProductManager { get; set; }

        /// <summary>
        /// Gets or sets the username or identifier of the user who created the role.
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the role was created.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the username or identifier of the user who last modified the role.
        /// </summary>
        public string? LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the role was last modified.
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
        /// Gets or sets the collection of claims associated with this role.
        /// </summary>
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent role associated with this role.
        /// </summary>
        /// <remarks>This property is used to establish a hierarchical relationship between roles.  The
        /// value corresponds to the primary key of the parent role entity.</remarks>
        [ForeignKey(nameof(ParentRole))] public string? ParentRoleId { get; set; }

        /// <summary>
        /// Gets or sets the parent role associated with this application role.
        /// </summary>
        /// <remarks>This property represents the hierarchical relationship between roles in the
        /// application.  Setting this property to <see langword="null"/> removes the parent role association.</remarks>
        public ApplicationRole? ParentRole { get; set; }

        /// <summary>
        /// Gets or sets the collection of child roles associated with this role.
        /// </summary>
        /// <remarks>This property establishes an inverse relationship with the <see cref="ParentRole"/>
        /// property.</remarks>
        [InverseProperty(nameof(ParentRole))] public virtual ICollection<ApplicationRole> ChildRoles { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the role.
        /// </summary>
        /// <returns>A string indicating the type of the role.</returns>
        public override string ToString()
        {
            return "Application Role";
        }

        #endregion
    }
}

