using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace BeneficiariesModule.Domain.Entities
{
    /// <summary>
    /// Represents the association between a user and a beneficiary in the system.
    /// </summary>
    /// <remarks>This class establishes a relationship between a user and a beneficiary, allowing the system
    /// to track which beneficiaries are linked to specific users. It includes foreign key references to both the user
    /// and the beneficiary entities.</remarks>
    public class UserBenificiary : EntityBase<int>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the associated beneficiary.
        /// </summary>
        [ForeignKey(nameof(Benificiary))] public int BenificiaryId { get; set; }

        /// <summary>
        /// Gets or sets the beneficiary associated with the current operation.
        /// </summary>
        public Benificiary Benificiary { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated user.
        /// </summary>
        [ForeignKey(nameof(User))] public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the current application context.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns>A string that represents the user beneficiary.</returns>
        public override string ToString()
        {
            return $"User Beneficiary";
        }
    }
}
