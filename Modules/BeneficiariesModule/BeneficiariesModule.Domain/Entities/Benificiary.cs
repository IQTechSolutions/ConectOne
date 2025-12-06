using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BeneficiariesModule.Domain.Enums;
using FilingModule.Domain.Entities;

namespace BeneficiariesModule.Domain.Entities
{
    /// <summary>
    /// Represents a beneficiary entity, which includes details such as contact information,  commission percentage,
    /// registration status, and associated ambassador and user relationships.
    /// </summary>
    /// <remarks>The <see cref="Benificiary"/> class is designed to store and manage information about a
    /// beneficiary,  including their name, description, contact details, commission percentage, and banking
    /// information.  It also tracks the registration status and relationships with ambassadors and users.</remarks>
    public class Benificiary : FileCollection<Benificiary, int>
    {
        /// <summary>
        /// Gets or sets the URL of the cover image for the accommodation.
        /// </summary>
        public string CoverImageUrl { get; set; } = "_content/Accomodation.Blazor/images/NoImage.jpg";

        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// </summary>
        /// <remarks>The maximum length of the name is restricted to 1000 characters. Assigning a value
        /// longer than this will result in a validation error.</remarks>
        [MaxLength(1000)] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description text, limited to a maximum length of 5000 characters.
        /// </summary>
        [MaxLength(5000)] public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the contact person associated with the entity.
        /// </summary>
        [MaxLength(1000)] public string ContactPerson { get; set; } = null!;

        /// <summary>
        /// Gets or sets the contact number associated with the entity.
        /// </summary>
        [MaxLength(1000)] public string ContactNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the contact email address.
        /// </summary>
        [MaxLength(1000)] public string ContactEmail { get; set; } = null!;

        /// <summary>
        /// Gets or sets the commission percentage applied to a transaction.
        /// </summary>
        public double CommissionPercentage { get; set; }

        /// <summary>
        /// Gets or sets the reason provided for registration.
        /// </summary>
        public string? ReasonForRegistration { get; set; }

        /// <summary>
        /// Gets or sets the current status of the beneficiary.
        /// </summary>
        public BenificiaryStatus Status { get; set; } = BenificiaryStatus.Pending;

        /// <summary>
        /// Gets or sets the name of the bank associated with the account.
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Gets or sets the branch code associated with the entity.
        /// </summary>
        public string? BranchCode { get; set; }

        /// <summary>
        /// Gets or sets the account number associated with the entity.
        /// </summary>
        public string? AccountNr { get; set; }   

        /// <summary>
        /// Gets or sets the type of account associated with the user.
        /// </summary>
        public string? AccountType { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated Ambassador entity.
        /// </summary>
        [ForeignKey(nameof(Ambassador))] public string? AmbassadorId { get; set; }

        /// <summary>
        /// Gets or sets the ambassador associated with the current context.
        /// </summary>
        public Ambassador? Ambassador { get; set; }

        /// <summary>
        /// Gets or sets the collection of user beneficiaries associated with the current user.
        /// </summary>
        public ICollection<UserBenificiary> UserBeneficiaries { get; set; } = new List<UserBenificiary>();

        /// <summary>
        /// Returns a string representation of the current object.
        /// </summary>
        /// <returns>A string that represents the current object, specifically "Beneficiary".</returns>
        public override string ToString()
        {
            return $"Beneficiary";
        }
    }
}
