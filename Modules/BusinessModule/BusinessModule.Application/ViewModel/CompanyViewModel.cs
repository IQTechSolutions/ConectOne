using BusinessModule.Domain.DataTransferObjects;
using ConectOne.Domain.DataTransferObjects;

namespace BusinessModule.Application.ViewModel
{
	/// <summary>
	/// Represents a view model for a company, encapsulating its details, address, theme preferences,  license information,
	/// and associated industries.
	/// </summary>
	/// <remarks>This class is designed to provide a structured representation of a company's data for use in 
	/// presentation layers, such as web or desktop applications. It includes properties for the company's  unique
	/// identifier, owner information, and various related models such as address, theme preferences,  and industry
	/// details.</remarks>
	public class CompanyViewModel
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyViewModel"/> class.
		/// </summary>
		public CompanyViewModel() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyViewModel"/> class using the specified company data.
		/// </summary>
		/// <remarks>This constructor initializes the <see cref="CompanyViewModel"/> with the provided company data,
		/// including its identifier, owner identifier, details, and address. The <paramref name="company"/> parameter must
		/// contain valid data, as it is used to populate the properties of the view model.</remarks>
		/// <param name="company">The data transfer object containing the company's information. Cannot be <c>null</c>.</param>
		public CompanyViewModel(CompanyDto company)
		{
			Id = company.Id;
			OwnerId = company.OwnerId;
			Details = new CompanyDetailsViewModel(company);
			Address = company.Address;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier of the owner associated with this entity.
		/// </summary>
		public string OwnerId { get; set; }

		/// <summary>
		/// Gets or sets the details of the company.
		/// </summary>
		public CompanyDetailsViewModel Details { get; set; }        

		/// <summary>
		/// Gets or sets the address information associated with the entity.
		/// </summary>
		public AddressDto Address { get; set; }

        #endregion

        #region Methods

		/// <summary>
		/// Converts the current <see cref="Company"/> instance to a <see cref="CompanyDto"/>.
		/// </summary>
		/// <returns>A <see cref="CompanyDto"/> object that represents the current <see cref="Company"/> instance.</returns>
        public CompanyDto ToDto()
        {
            return new CompanyDto()
            {
                Id = Id,
                OwnerId = OwnerId,
                Details = Details.ToDto(),
                Address = Address
            };
        }

        #endregion
    }
}
