using ConectOne.Domain.Entities;

namespace BeneficiariesModule.Domain.Entities
{
    /// <summary>
    /// Represents an ambassador entity with personal details, contact information, and associated beneficiaries.
    /// </summary>
    /// <remarks>The <see cref="Ambassador"/> class provides properties to store the ambassador's name,
    /// surname, phone number,  email address, and commission percentage. It also maintains a collection of
    /// beneficiaries associated with the ambassador.</remarks>
    public class Ambassador : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the surname of the individual.
        /// </summary>
        public string Surname { get; set; } = null!;

        /// <summary>
        /// Gets or sets the phone number associated with the entity.
        /// </summary>
        public string PhoneNr { get; set; } = null!; 

        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the commission percentage applied to transactions.
        /// </summary>
        public double CommissionPercentage { get; set; }

        /// <summary>
        /// Gets or sets the collection of beneficiaries associated with this entity.
        /// </summary>
        public ICollection<Benificiary> Beneficiaries { get; set; } = [];
    }
}
