using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a response containing details about a role claim.
    /// This class is used to transfer role claim data between layers or systems.
    /// </summary>
    public class RoleClaimResponse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimResponse"/> class with default values.
        /// </summary>
        public RoleClaimResponse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimResponse"/> class using an <see cref="ApplicationRoleClaim"/> entity.
        /// </summary>
        /// <param name="roleClaim">The <see cref="ApplicationRoleClaim"/> entity containing role claim details.</param>
        public RoleClaimResponse(ApplicationRoleClaim roleClaim)
        {
            Id = roleClaim.Id;
            RoleId = roleClaim.RoleId;
            Description = roleClaim.Description;
            Value = roleClaim.ClaimValue;
            Group = roleClaim.Group;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier of the role claim.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the role associated with the claim.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the type of the claim (e.g., permission type).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the claim.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the description of the claim.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the group to which the claim belongs.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the claim is selected.
        /// </summary>
        public bool Selected { get; set; }

        #endregion
    }
}

