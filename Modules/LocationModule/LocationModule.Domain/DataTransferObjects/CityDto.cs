using LocationModule.Domain.Entities;

namespace LocationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a city.
    /// </summary>
    public record CityDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CityDto"/> class.
        /// </summary>
        public CityDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CityDto"/> class using the specified entity.
        /// </summary>
        /// <param name="city">The city entity used to populate this DTO.</param>
        public CityDto(City city)
        {
            CityId = city.Id;
            Name = city.Name;
            Code = city.Code;
            ShortName = city.ShortName;
            Description = city.Description;
            CountryId = city.CountryId;
        }

        /// <summary>
        /// Gets the unique identifier of the city.
        /// </summary>
        public string? CityId { get; init; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Gets or sets the city code.
        /// </summary>
        public string? Code { get; init; }

        /// <summary>
        /// Gets or sets the short name of the city.
        /// </summary>
        public string? ShortName { get; init; }

        /// <summary>
        /// Gets or sets the description of the city.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets the identifier of the country that the city belongs to.
        /// </summary>
        public string? CountryId { get; init; }
    }
}
