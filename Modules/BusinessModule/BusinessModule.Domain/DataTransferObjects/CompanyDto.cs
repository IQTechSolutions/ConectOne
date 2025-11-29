using BusinessModule.Domain.Entities;
using ConectOne.Domain.DataTransferObjects;

namespace BusinessModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a company, encapsulating its details, address, industries, theme
    /// preferences, and other related information.
    /// </summary>
    /// <remarks>This class is designed to facilitate the transfer of company-related data between different
    /// layers of an application, such as the presentation layer and the domain layer. It provides constructors for
    /// initializing the DTO from various sources, including a <see cref="CompanyDto"/> or a domain <see
    /// cref="Company"/> entity.</remarks>
    public record CompanyDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyDto"/> class.
        /// </summary>
        public CompanyDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyDto"/> class using the specified <see cref="Company"/>
        /// object.
        /// </summary>
        /// <param name="company">The <see cref="Company"/> object containing the data to initialize the DTO. Cannot be <see
        /// langword="null"/>.</param>
        public CompanyDto(Company company)
        {
            Id = company.Id;
            OwnerId = company.OwnerId;
            Details = new CompanyDetailsDto(company);
            Address = new AddressDto(company.Address);
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the owner associated with this entity.
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the details of the company.
        /// </summary>
        public CompanyDetailsDto Details { get; set; }

        /// <summary>
        /// Gets the address associated with the current entity.
        /// </summary>
        public AddressDto Address { get; init; }
    }
}
