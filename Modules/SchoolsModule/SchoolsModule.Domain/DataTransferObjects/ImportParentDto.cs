using ConectOne.Domain.Entities;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object for importing parent information.
    /// </summary>
    public record ImportParentDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportParentDto"/> class.
        /// </summary>
        public ImportParentDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportParentDto"/> class with specified details.
        /// </summary>
        /// <param name="name">The first name of the parent.</param>
        /// <param name="surname">The surname of the parent.</param>
        /// <param name="mobileNo">The mobile number of the parent.</param>
        /// <param name="email">The email address of the parent.</param>
        /// <param name="idNumber">The identification number of the parent.</param>
        /// <param name="addressLine1">The primary address line of the parent.</param>
        public ImportParentDto(string name, string surname, string mobileNo, string email, string idNumber, string addressLine1)
        {
            Id = idNumber;
            Name = name;
            Surname = surname;
            MobileNo = mobileNo;
            Email = email;
            IdNumber = idNumber;
            AddressLine1 = addressLine1;
        }

        #endregion

        /// <summary>
        /// Gets the unique identifier of the parent.
        /// </summary>
        public string Id { get; init; } = null!;

        /// <summary>
        /// Gets the first name of the parent.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets the surname of the parent.
        /// </summary>
        public string Surname { get; init; } = null!;

        /// <summary>
        /// Gets the mobile number of the parent.
        /// </summary>
        public string MobileNo { get; init; } = null!;

        /// <summary>
        /// Gets the email address of the parent.
        /// </summary>
        public string Email { get; init; } = null!;

        /// <summary>
        /// Gets the identification number of the parent.
        /// </summary>
        public string? IdNumber { get; init; } = null!;

        /// <summary>
        /// Gets the primary address line of the parent.
        /// </summary>
        public string? AddressLine1 { get; init; }

        /// <summary>
        /// Converts the <see cref="ImportParentDto"/> to a <see cref="Parent"/> entity.
        /// </summary>
        /// <returns>A <see cref="Parent"/> entity populated with the DTO's data.</returns>
        public Parent CreateParent()
        {
            return new Parent
            {
                Id = string.IsNullOrEmpty(IdNumber) ? Guid.NewGuid().ToString() : IdNumber,
                FirstName = Name,
                LastName = Surname,
                ParentIdNumber = IdNumber,
                ContactNumbers = new List<ContactNumber<Parent>>() { new ContactNumber<Parent>(MobileNo, true) },
                EmailAddresses = new List<EmailAddress<Parent>>() { new EmailAddress<Parent>(Email, true) },
                Addresses = !string.IsNullOrEmpty(AddressLine1) ? new List<Address<Parent>>() { new Address<Parent>() { StreetName = AddressLine1, Country = "ZA" } } : [],
            };
        }
    }
}
