using LocationModule.Domain.Entities;

namespace LocationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a country, including its key details
    /// and associated provinces.
    /// </summary>
    public record CountryDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryDto"/> class.
        /// </summary>
        public CountryDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryDto"/> class using
        /// the specified <see cref="Country"/> entity.
        /// </summary>
        /// <param name="country">The source entity used to populate this DTO.</param>
        public CountryDto(Country country)
        {
            CountryId = country.Id;
            Code = country.Code;
            Name = country.Name;
            ShortName = country.ShortName;
            Description = country.Description;
        }

        /// <summary>
        /// Gets the unique identifier for the country.
        /// </summary>
        public string CountryId { get; init; }

        /// <summary>
        /// Gets the code associated with this instance.
        /// </summary>
        public string? Code { get; init; }

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Gets the short name associated with the entity.
        /// </summary>
        public string? ShortName { get; init; }

        /// <summary>
        /// Gets the description associated with the object.
        /// </summary>
        public string? Description { get; init; }
    }
}
