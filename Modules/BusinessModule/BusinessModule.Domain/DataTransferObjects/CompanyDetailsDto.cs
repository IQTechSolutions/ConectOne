using BusinessModule.Domain.Entities;
using FilingModule.Domain.Enums;

namespace BusinessModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the details of a company, including its name, registration information, and other descriptive data.
    /// </summary>
    /// <remarks>This data transfer object (DTO) is used to encapsulate company-related information for use in
    /// various application layers. It can be initialized using a <see cref="CompanyDetailsViewModel"/> or a <see
    /// cref="Company"/> entity.</remarks>
    public record CompanyDetailsDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyDetailsDto"/> class.
        /// </summary>
        public CompanyDetailsDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyDetailsDto"/> class using the specified company details.
        /// </summary>
        /// <remarks>The constructor extracts relevant information from the provided <see cref="Company"/>
        /// object,  including the cover image URL, company name, display name, unique URL, registration number, 
        /// description, and VAT number. The cover image URL is determined by selecting the first image  with an <see
        /// cref="UploadType"/> of <see cref="UploadType.Cover"/>.</remarks>
        /// <param name="details">The <see cref="Company"/> object containing the details to populate the DTO.  This parameter cannot be null.</param>
        public CompanyDetailsDto(Company details)
        {
            CoverImageUrl = details.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover)?.Image.RelativePath;
            CompanyName = details.Name;
            DisplayName = details.DisplayName;
            UniqueUrl = details.WebsiteUrl;
            RegistrationNr = details.RegistrationNr;
            Description = details.Description;
            VatNr = details.VatNr;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the company.
        /// </summary>
        public string CompanyName { get; init; }

        /// <summary>
        /// Gets the display name associated with the object.
        /// </summary>
        public string DisplayName { get; init; }

        /// <summary>
        /// Gets the unique URL associated with the entity.
        /// </summary>
        public string UniqueUrl { get; init; }

        /// <summary>
        /// Gets the registration number associated with the entity.
        /// </summary>
        public string? RegistrationNr { get; init; }

        /// <summary>
        /// Gets the VAT (Value-Added Tax) number associated with the entity.
        /// </summary>
        public string? VatNr { get; init; }

        /// <summary>
        /// Gets a description or additional information about the object.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets the URL of the cover image associated with the item.
        /// </summary>
        public string? CoverImageUrl { get; set; }

        #endregion
    }
}
