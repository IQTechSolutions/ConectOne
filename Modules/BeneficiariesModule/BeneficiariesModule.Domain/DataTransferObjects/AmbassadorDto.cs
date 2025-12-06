using BeneficiariesModule.Domain.Entities;

namespace BeneficiariesModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for an ambassador, encapsulating key information such as identification,
    /// contact details, and commission percentage.
    /// </summary>
    /// <remarks>This class is designed to facilitate the transfer of ambassador-related data between
    /// different layers of an application, such as the presentation layer and the domain layer. It provides
    /// constructors for initializing the object from various sources, including view models and domain
    /// entities.</remarks>
    public record AmbassadorDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmbassadorDto"/> class.
        /// </summary>
        public AmbassadorDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmbassadorDto"/> class using the specified <see
        /// cref="Ambassador"/> view model.
        /// </summary>
        /// <remarks>This constructor maps the properties of the <see cref="Ambassador"/> view model to
        /// the corresponding properties of the <see cref="AmbassadorDto"/>. If <paramref name="viewModel"/> is <see
        /// langword="null"/>, the <see cref="CommissionPercentage"/> property will default to 0.</remarks>
        /// <param name="viewModel">The <see cref="Ambassador"/> view model containing the data to populate the DTO.  If <paramref
        /// name="viewModel"/> is <see langword="null"/>, default values will be assigned.</param>
        public AmbassadorDto(Ambassador viewModel)
        {
            AmbassadorId = viewModel?.Id;
            Name = viewModel?.Name;
            Surname = viewModel?.Surname;
            PhoneNr = viewModel?.PhoneNr;
            Email = viewModel?.Email;
            CommissionPercentage = viewModel != null ?  viewModel.CommissionPercentage : 0;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the ambassador.
        /// </summary>
        public string? AmbassadorId { get; set; }

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
        /// Converts the current object to an <see cref="Ambassador"/> instance.
        /// </summary>
        /// <remarks>This method creates a new <see cref="Ambassador"/> object and populates its
        /// properties based on the current object's state. If <c>AmbassadorId</c> is null or empty, a new GUID is
        /// generated and used as the <c>Id</c> of the resulting <see cref="Ambassador"/>.</remarks>
        /// <returns>A new <see cref="Ambassador"/> instance with properties mapped from the current object.</returns>
        public Ambassador ToAmbassador()
        {
            return new Ambassador()
            {
                Id = string.IsNullOrEmpty(AmbassadorId) ? Guid.NewGuid().ToString() : AmbassadorId,
                Name = Name,
                Surname = Surname,
                PhoneNr = PhoneNr,
                Email = Email,
                CommissionPercentage = CommissionPercentage
            };
        }
    }
}
