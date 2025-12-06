using BeneficiariesModule.Domain.Entities;
using FilingModule.Domain.Enums;

namespace BeneficiariesModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a beneficiary, encapsulating details such as personal information, 
    /// contact details, bank account information, and associated ambassador data.
    /// </summary>
    /// <remarks>This DTO is designed to facilitate the transfer of beneficiary-related data between different
    /// layers of the application. It includes properties for identifying the beneficiary, describing their purpose, and
    /// providing contact and financial details. Additionally, it supports optional ambassador information and image
    /// data.</remarks>
    public record BeneficiaryDto
	{
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BeneficiaryDto"/> class.
        /// </summary>
        /// <remarks>This constructor is typically used for creating an empty DTO instance, which can be populated later.</remarks>
        public BeneficiaryDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeneficiaryDto"/> class, mapping data from a given <see
        /// cref="Benificiary"/> object.
        /// </summary>
        /// <remarks>This constructor extracts relevant information from the provided <see
        /// cref="Benificiary"/> object, including its ID, name, description, contact details, commission percentage,
        /// registration reason, and banking information. Additionally, if the <see cref="Benificiary"/> has an
        /// associated ambassador, the <see cref="AmbassadorDto"/> is initialized with the ambassador's data.</remarks>
        /// <param name="benificiary">The <see cref="Benificiary"/> object containing the source data to populate the <see cref="BeneficiaryDto"/>
        /// instance.</param>
        public BeneficiaryDto(Benificiary benificiary)
        {
            BenificiaryId = benificiary.Id;
            ImgPath = benificiary.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath;
            Name = benificiary.Name;
            Description = benificiary.Description;
            ContactPerson = benificiary.ContactPerson;
            ContactNumber = benificiary.ContactNumber;
            ContactEmail = benificiary.ContactEmail;
            CommissionPercentage = benificiary.CommissionPercentage;
            ReasonForRegistration = benificiary.ReasonForRegistration;

            BranchCode = benificiary.BranchCode;
            BankName = benificiary.BankName;
            BankAccountType = benificiary.AccountType;
            BankAccountNr = benificiary.AccountNr;

            if (benificiary.Ambassador is not null)
                Ambassador = new AmbassadorDto(benificiary.Ambassador);
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier of the beneficiary.
        /// </summary>
        /// <remarks>This property is optional and can be null if the beneficiary has not been registered yet.</remarks>
        /// </summary>
        public int? BenificiaryId { get; init; }

        /// <summary>
        /// Gets the file path to the image associated with the current object.
        /// </summary>
        public string? ImgPath { get; init; }

        /// <summary>
        /// Gets the name associated with the current object.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets the name of the contact person associated with this entity.
        /// </summary>
        public string ContactPerson { get; init; }

        /// <summary>
        /// Gets the contact number associated with the entity.
        /// </summary>
		public string ContactNumber { get; init; }

        /// <summary>
        /// Gets the contact email address associated with the entity.
        /// </summary>
		public string ContactEmail { get; init; }

        /// <summary>
        /// Gets the commission percentage applied to a transaction.
        /// </summary>
		public double CommissionPercentage { get; init; }

        /// <summary>
        /// Gets the reason provided for registration.
        /// </summary>
		public string? ReasonForRegistration { get; init; }

        /// <summary>
        /// Gets the name of the bank associated with the account.
        /// </summary>
		public string? BankName { get; init; }

        /// <summary>
        /// Gets the type of bank account (e.g., savings, checking).
        /// </summary>
		public string? BankAccountType { get; init; }

        /// <summary>
        /// Gets the bank account number associated with the entity.
        /// </summary>
		public string? BankAccountNr { get; init; }

        /// <summary>
        /// Gets the branch code associated with the entity.
        /// </summary>
		public string? BranchCode { get; init; }

        /// <summary>
        /// Gets the ambassador associated with the current context.
        /// </summary>
		public AmbassadorDto? Ambassador { get; init; }
	}
}
