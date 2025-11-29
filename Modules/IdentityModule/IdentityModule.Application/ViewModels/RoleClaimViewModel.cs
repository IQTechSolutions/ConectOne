using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.RequestFeatures;

namespace IdentityModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a role claim.
    /// This class is used to bind role claim data in the UI.
    /// </summary>
    public class RoleClaimViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimViewModel"/> class with default values.
        /// </summary>
        public RoleClaimViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimViewModel"/> class using a <see cref="RoleClaimResponse"/>.
        /// </summary>
        /// <param name="response">The <see cref="RoleClaimResponse"/> containing role claim details.</param>
        public RoleClaimViewModel(RoleClaimResponse response)
        {
            Id = response.Id;
            RoleId = response.RoleId;
            Type = response.Type;
            Value = response.Value;
            Description = response.Description + " " + response.Value.Split(".").Last();
            Group = response.Group;
            Selectede = response.Selected;
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
        public bool Selectede { get; set; } = false;

        #endregion

        #region Method

        /// <summary>
        /// Converts the current object to a <see cref="RoleClaimRequest"/> instance.
        /// </summary>
        /// <remarks>This method maps the properties of the current object to a new <see
        /// cref="RoleClaimRequest"/> object.</remarks>
        /// <returns>A <see cref="RoleClaimRequest"/> instance containing the mapped values from the current object.</returns>
        public RoleClaimRequest ToRoleClaimRequest()
        {
            return new RoleClaimRequest()
            {
                Id = this.Id,
                RoleId = this.RoleId,
                Type = this.Type,
                Value = this.Value,
                Description = this.Description,
                Group = this.Group,
                Selected = this.Selectede
            };
        }

        /// <summary>
        /// Converts the current object to a <see cref="RoleClaimRequest"/> instance.
        /// </summary>
        /// <param name="roleId">The unique identifier of the role to associate with the claim.</param>
        /// <returns>A new <see cref="RoleClaimRequest"/> instance populated with the current object's properties and the
        /// specified <paramref name="roleId"/>.</returns>
        public RoleClaimRequest ToRoleClaimRequest(string roleId)
        {
            return new RoleClaimRequest()
            {
                Id = this.Id,
                RoleId = roleId,
                Type = this.Type,
                Value = this.Value,
                Description = this.Description,
                Group = this.Group,
                Selected = this.Selectede
            };
        }

        #endregion
    }
}

