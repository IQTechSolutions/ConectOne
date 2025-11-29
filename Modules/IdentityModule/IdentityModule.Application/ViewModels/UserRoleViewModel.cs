using ConectOne.Domain.Enums;
using IdentityModule.Domain.DataTransferObjects;

namespace IdentityModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for user roles.
    /// This class is used to manage and display role-related data in the UI.
    /// </summary>
    public class UserRoleViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleViewModel"/> class with default values.
        /// </summary>
        public UserRoleViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleViewModel"/> class using a <see cref="RoleDto"/> and a selection state.
        /// </summary>
        /// <param name="roleDto">The <see cref="RoleDto"/> containing role details.</param>
        /// <param name="selectedItem">Indicates whether the role is selected.</param>
        public UserRoleViewModel(RoleDto roleDto, bool selectedItem)
        {
            RoleId = roleDto.Id;
            ParentRoleId = roleDto.ParentRoleId;
            RoleName = roleDto.Name;
            RoleDescription = roleDto.Description;
            Selected = selectedItem;
            NotAvailableForRegistrationSelection = roleDto.NotAvailableForRegistrationSelection;
            CanDeleteRole = roleDto.CanDeleteRole;
            AdvertiseRegistration = roleDto.AdvertiseRegistration;
            AdvertiseOnlyToMembers = roleDto.AdvertiseOnlyToMembers;
            AdministrativeRole = roleDto.AdministrativeRole;
            ProductManager = roleDto.ProductManager;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the parent role in the role hierarchy.
        /// </summary>
        public string ParentRoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the description of the role.
        /// </summary>
        public string? RoleDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is unavailable for selection during registration.
        /// </summary>
        public bool NotAvailableForRegistrationSelection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user has permission to delete roles.
        /// </summary>
        public bool CanDeleteRole { get; set; }

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
        /// Gets or sets a value indicating whether the user has administrative privileges.
        /// </summary>
        public bool AdministrativeRole { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current role entity to a <see cref="RoleDto"/> object.
        /// </summary>
        /// <remarks>This method maps the properties of the current role entity to a new <see
        /// cref="RoleDto"/> instance. The resulting DTO can be used for data transfer or presentation
        /// purposes.</remarks>
        /// <returns>A <see cref="RoleDto"/> object containing the mapped properties of the current role entity.</returns>
        public RoleDto ToDto()
        {
            return new RoleDto()
            {
                Id = this.RoleId,
                ParentRoleId = this.ParentRoleId,
                Name = this.RoleName,
                Description = this.RoleDescription,
                NotAvailableForRegistrationSelection = this.NotAvailableForRegistrationSelection,
                CanDeleteRole = this.CanDeleteRole,
                AdvertiseRegistration = this.AdvertiseRegistration,
                AdministrativeRole = this.AdministrativeRole,
                AdvertiseOnlyToMembers = this.AdvertiseOnlyToMembers,
                CreationAction = RoleCreationAction.Default,
                ProductManager = this.ProductManager
            };
        }

        #endregion
    }
}