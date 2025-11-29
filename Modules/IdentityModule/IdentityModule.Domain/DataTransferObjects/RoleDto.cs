using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Enums;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for roles.
    /// This class is used to transfer role-related data between layers or systems.
    /// </summary>
    public record RoleDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDto"/> class with default values.
        /// </summary>
        public RoleDto()
        {
            Id = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            CreationAction = RoleCreationAction.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDto"/> class using an <see cref="ApplicationRole"/> entity.
        /// </summary>
        /// <param name="role">The <see cref="ApplicationRole"/> entity containing role details.</param>
        public RoleDto(ApplicationRole role)
        {
            Id = role?.Id;
            Name = role?.Name;
            Description = role?.Description;
            NotAvailableForRegistrationSelection = role is not null ? role.NotAvailableForRegistrationSelection : false;
            CanDeleteRole = role?.CannotDelete ?? false;
            AdvertiseRegistration = role.AdvertiseRegistration;
            AdvertiseOnlyToMembers = role.AdvertiseOnlyToMembers;
            AdministrativeRole = role.AdministrativeRole;
            CreationAction = RoleCreationAction.Default;
            ProductManager = role.ProductManager;
            ChildRoles = role.ChildRoles.Select(c => new RoleDto(c)).ToList();
        }

        /// <summary>
        /// Gets the unique identifier of the role.
        /// </summary>
        public string? Id { get; init; }

        /// <summary>
        /// Gets the identifier of the parent role, if any.
        /// </summary>
        public string? ParentRoleId { get; init; }

        /// <summary>
        /// Gets the name of the role.
        /// </summary>
        [Required(ErrorMessage = "Name is a required field")]
        public string? Name { get; init; }

        /// <summary>
        /// Gets the description of the role.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the registration process should be advertised.
        /// </summary>
        public bool AdvertiseRegistration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the advertisement is restricted to members only.
        /// </summary>
        public bool AdvertiseOnlyToMembers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is unavailable for selection during registration.
        /// </summary>
        public bool NotAvailableForRegistrationSelection { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has administrative privileges.
        /// </summary>
        public bool AdministrativeRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user has permission to delete roles.
        /// </summary>
        public bool CanDeleteRole { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the advertisement is visible only to members.
        /// </summary>
        public bool ProductManager { get; init; }

        /// <summary>
        /// Gets or sets the requested action to take when a role with the same name already exists.
        /// </summary>
        public RoleCreationAction CreationAction { get; set; } = RoleCreationAction.Default;

        /// <summary>
        /// Gets or sets the collection of roles that are considered children of this role.
        /// </summary>
        /// <remarks>Child roles typically inherit permissions or characteristics from their parent role.
        /// Modifying this collection affects the role hierarchy and may impact authorization logic.</remarks>
        public virtual ICollection<RoleDto> ChildRoles { get; set; } = [];

        /// <summary>
        /// Converts the current <see cref="RoleDto"/> instance to an <see cref="ApplicationRole"/> entity.
        /// </summary>
        /// <returns>An <see cref="ApplicationRole"/> entity with the same properties as the current <see cref="RoleDto"/>.</returns>
        public ApplicationRole ToRole()
        {
            string bb = "0";
            var c = Int32.TryParse(bb, out int v);

            return new ApplicationRole
            {
                Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id,
                ParentRoleId = ParentRoleId,
                Name = Name,
                Description = Description,
                NotAvailableForRegistrationSelection = NotAvailableForRegistrationSelection,
                CannotDelete = CanDeleteRole,
                AdvertiseRegistration = AdvertiseRegistration,
                AdvertiseOnlyToMembers = AdvertiseOnlyToMembers,
                AdministrativeRole = AdministrativeRole,
                ProductManager = ProductManager
            };
        }
    }
}
