using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Entities;
using LocationModule.Domain.DataTransferObjects;

namespace LocationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a city containing its basic details and associated country.
    /// </summary>
    public class CityViewModel : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CityViewModel"/> class.
        /// </summary>
        public CityViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CityViewModel"/> class using the specified data transfer object.
        /// </summary>
        /// <param name="dto">The DTO used to populate this view model.</param>
        public CityViewModel(CityDto dto)
        {
            CityId = dto.CityId ?? string.Empty;
            Name = dto.Name ?? string.Empty;
            Code = dto.Code ?? string.Empty;
            ShortName = dto.ShortName;
            Description = dto.Description;
            CountryId = dto.CountryId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the city.
        /// </summary>
        public string CityId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the city name.
        /// </summary>
        [Required] public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the city code.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the short name of the city.
        /// </summary>
        public string? ShortName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description for the city.
        /// </summary>
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the country the city belongs to.
        /// </summary>
        public string? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the selected country information when used in the UI.
        /// </summary>
        public CountryDto? Country { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current <see cref="City"/> instance to a <see cref="CityDto"/>.
        /// </summary>
        /// <returns>A <see cref="CityDto"/> object containing the data from the current <see cref="City"/> instance.</returns>
        public CityDto ToDto()
        {
            return new CityDto
            {
                CityId = CityId,
                Name = Name,
                Code = Code,
                ShortName = ShortName,
                Description = Description,
                CountryId = CountryId
            };
        }

        #endregion

    }
}
