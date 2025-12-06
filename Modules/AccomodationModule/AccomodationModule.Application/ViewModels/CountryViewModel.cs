using System.ComponentModel.DataAnnotations;
using LocationModule.Domain.DataTransferObjects;
using LocationModule.Domain.Entities;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a country, containing its code, name,
    /// short name and description.
    /// </summary>
    public class CountryViewModel 
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryViewModel"/> class.
        /// </summary>
        public CountryViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryViewModel"/> class
        /// using the specified country data transfer object.
        /// </summary>
        /// <param name="dto">The <see cref="CountryDto"/> to map values from.</param>
        public CountryViewModel(CountryDto dto)
        {
            CountryId = dto.CountryId;
            Code = dto.Code;
            Name = dto.Name;
            ShortName = dto.ShortName;
            Description = dto.Description;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the country.
        /// </summary>
        public string CountryId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the country name.
        /// </summary>
        [Required] public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the short name for the country.
        /// </summary>
        public string? ShortName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description associated with the country.
        /// </summary>
        public string? Description { get; set; } = string.Empty;

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current <see cref="Country"/> instance to a <see cref="CountryDto"/>.
        /// </summary>
        /// <returns>A <see cref="CountryDto"/> object containing the data from the current <see cref="Country"/> instance.</returns>
        public CountryDto ToDto()
        {
            return new CountryDto
            {
                CountryId = this.CountryId,
                Code = this.Code,
                Name = this.Name,
                ShortName = this.ShortName,
                Description = this.Description
            };
        }

        #endregion
    }
}
