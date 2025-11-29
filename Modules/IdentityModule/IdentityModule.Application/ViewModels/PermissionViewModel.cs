using IdentityModule.Domain.Interfaces;

namespace IdentityModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for managing permissions associated with a role.
    /// This class is used to bind role and claim data in the UI.
    /// </summary>
    public class PermissionViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionViewModel"/> class with default values.
        /// </summary>
        public PermissionViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionViewModel"/> class using a <see cref="PermissionResponse"/>.
        /// </summary>
        /// <param name="response">The <see cref="PermissionResponse"/> containing role and claim details.</param>
        public PermissionViewModel(PermissionResponse response)
        {
            RoleId = response.RoleId;
            RoleName = response.RoleName;
            RoleClaims = response.RoleClaims.Select(c => new RoleClaimViewModel(c)).ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the list of claims associated with the role.
        /// </summary>
        public List<RoleClaimViewModel> RoleClaims { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current object to a <see cref="PermissionRequest"/> instance.
        /// </summary>
        /// <remarks>The method creates a new <see cref="PermissionRequest"/> object, populating its
        /// properties with the values of the current object's <c>RoleId</c> and a transformed list of
        /// <c>RoleClaims</c>. Each <c>RoleClaim</c> is converted using its <c>ToRoleClaimRequest</c> method.</remarks>
        /// <returns>A <see cref="PermissionRequest"/> instance containing the <c>RoleId</c> and the transformed
        /// <c>RoleClaims</c> from the current object.</returns>
        public PermissionRequest ToRequest()
        {
            return new PermissionRequest()
            {
                RoleId = this.RoleId,
                RoleClaims = this.RoleClaims.Select(c => c.ToRoleClaimRequest()).ToList()
            };
        }

        #endregion
    }
}