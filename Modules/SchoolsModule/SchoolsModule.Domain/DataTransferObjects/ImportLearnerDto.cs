using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object for importing learner information.
    /// </summary>
    public record ImportLearnerDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportLearnerDto"/> class.
        /// </summary>
        public ImportLearnerDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportLearnerDto"/> class with specified details.
        /// </summary>
        /// <param name="name">The first name of the learner.</param>
        /// <param name="surname">The surname of the learner.</param>
        /// <param name="mobileNo">The mobile number of the learner.</param>
        /// <param name="email">The email address of the learner.</param>
        /// <param name="idNumber">The identification number of the learner.</param>
        /// <param name="addressLine1">The primary address line of the learner.</param>
        /// <param name="gender">The gender of the learner.</param>
        public ImportLearnerDto(string name, string surname, string mobileNo, string email, string idNumber, string addressLine1, Gender gender)
        {
            Id = idNumber;
            Name = name;
            Surname = surname;
            MobileNo = mobileNo;
            Email = email;
            IdNumber = idNumber;
            AddressLine1 = addressLine1;
            Gender = gender;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier of the learner.
        /// </summary>
        public string? Id { get; init; } = null!;

        /// <summary>
        /// Gets or sets the first name of the learner.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the surname of the learner.
        /// </summary>
        public string Surname { get; set; } = null!;

        /// <summary>
        /// Gets or sets the mobile number of the learner.
        /// </summary>
        public string? MobileNo { get; set; }

        /// <summary>
        /// Gets or sets the email address of the learner.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the identification number of the learner.
        /// </summary>
        public string IdNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the primary address line of the learner.
        /// </summary>
        public string? AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the gender of the learner.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the grade (or year level) in which the learner is enrolled.
        /// </summary>
        public string? Grade { get; set; }

        /// <summary>
        /// Gets or sets the class or homeroom identifier for the learner.
        /// </summary>
        public string? Class { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show parent details.
        /// </summary>
        public bool ShowParentDetails { get; set; }

        /// <summary>
        /// Gets or sets the collection of parent details.
        /// </summary>
        public ICollection<ImportParentDto> Parents { get; set; } = [];

        /// <summary>
        /// Creates a new Learner entity from the values in the ImportLearnerDto.
        /// </summary>
        /// <returns>A new Learner entity.</returns>
        public Learner CreateLearner()
        {
            return new Learner
            {
                Id = IdNumber,
                ChildGuid = IdNumber,
                FirstName = Name,
                SchoolUid = Guid.NewGuid().ToString(),
                DisplayName = $"{Name} {Surname}",
                LastName = Surname,
                IdNumber = IdNumber,
                Gender = Gender,
                ContactNumbers = !string.IsNullOrEmpty(MobileNo) ? new List<ContactNumber<Learner>>() { new ContactNumber<Learner>(MobileNo, true) } : [],
                EmailAddresses = !string.IsNullOrEmpty(Email) ? new List<EmailAddress<Learner>>() { new EmailAddress<Learner>(Email, true) } : []
            };
        }
    }
}
