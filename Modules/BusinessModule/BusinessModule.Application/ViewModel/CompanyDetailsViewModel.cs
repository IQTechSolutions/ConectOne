using BusinessModule.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations;

namespace BusinessModule.Application.ViewModel
{
	/// <summary>
	/// Represents the details of a company, including its name, description, registration information, and associated
	/// industries.
	/// </summary>
	/// <remarks>This view model is designed to encapsulate the key details of a company for display purposes in a
	/// user interface. It can be initialized using a <see cref="CompanyDto"/> or <see cref="CompanyDetailsDto"/>
	/// object.</remarks>
	public class CompanyDetailsViewModel
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyDetailsViewModel"/> class.
		/// </summary>
		public CompanyDetailsViewModel() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyDetailsViewModel"/> class using the specified company data
		/// transfer object.
		/// </summary>
		/// <remarks>This constructor extracts relevant details from the provided <see cref="CompanyDto"/>  and
		/// initializes the properties of the <see cref="CompanyDetailsViewModel"/> accordingly.</remarks>
		/// <param name="company">The <see cref="CompanyDto"/> containing the company details used to populate the view model.</param>
		public CompanyDetailsViewModel(CompanyDto company)
		{
			CoverImageUrl = company.Details.CoverImageUrl;
			CompanyName = company.Details.CompanyName;
			DisplayName = company.Details.DisplayName;
			UniqueUrl = company.Details.UniqueUrl;
			RegistrationNr = company.Details.RegistrationNr;
			Description = company.Details.Description;
			VatNr = company.Details.VatNr;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyDetailsViewModel"/> class  using the specified company
		/// details.
		/// </summary>
		/// <remarks>This constructor maps the properties of the provided <see cref="CompanyDetailsDto"/>  to the
		/// corresponding properties of the <see cref="CompanyDetailsViewModel"/>.</remarks>
		/// <param name="details">An instance of <see cref="CompanyDetailsDto"/> containing the company details  to populate the view model.</param>
		public CompanyDetailsViewModel(CompanyDetailsDto details)
		{
			CoverImageUrl = details.CoverImageUrl;
			CompanyName = details.CompanyName;
			DisplayName = details.DisplayName;
			UniqueUrl = details.UniqueUrl;
			RegistrationNr = details.RegistrationNr;
			Description = details.Description;
			VatNr = details.VatNr;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the URL of the cover image associated with the item.
        /// </summary>
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        [Required, MaxLength(255, ErrorMessage = "Company name has a max charachter count of 255")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the display name of the company.
        /// </summary>
        [Required, MaxLength(255, ErrorMessage = "Company name has a max charachter count of 255")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the unique URL associated with the company.
        /// </summary>
        [Required, MaxLength(300, ErrorMessage = "Company name has a max charachter count of 300")]
        public string UniqueUrl { get; set; }

        /// <summary>
        /// Gets or sets the registration number associated with the entity.
        /// </summary>
        public string? RegistrationNr { get; set; }

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the VAT (Value-Added Tax) number associated with the entity.
        /// </summary>
        public string? VatNr { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the company details into a <see cref="CompanyDetailsDto"/>.
        /// </summary>
        /// <returns>A <see cref="CompanyDetailsDto"/> containing the company details, including the cover image URL,  company name,
        /// unique URL, display name, registration number, description, and VAT number.</returns>
        public CompanyDetailsDto ToDto()
        {
            return new CompanyDetailsDto()
            {
                CoverImageUrl = CoverImageUrl,
                CompanyName = CompanyName,
                UniqueUrl = UniqueUrl,
                DisplayName = DisplayName,
                RegistrationNr = RegistrationNr,
                Description = Description,
                VatNr = VatNr
            };
        }

        #endregion
    }
}
