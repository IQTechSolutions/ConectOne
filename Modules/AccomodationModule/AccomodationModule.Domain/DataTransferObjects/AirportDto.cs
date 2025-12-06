using AccomodationModule.Domain.Entities;
using LocationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents an airport data transfer object.
    /// </summary>
    public record AirportDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AirportDto"/> class.
        /// </summary>
        public AirportDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AirportDto"/> class using an <see cref="Airport"/> entity.
        /// </summary>
        /// <param name="airport">The airport entity.</param>
        public AirportDto(Airport airport)
        {
            Id = airport.Id;
            Name = airport.Name;
            Code = airport.Code;
            Description = airport.Description;
            City = airport.City == null ? new CityDto() : new CityDto(airport.City);
        }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public string? Id { get; init; } 

        /// <summary>
        /// Gets or sets the airport name.
        /// </summary>
        public string? Name { get; init; } 

        /// <summary>
        /// Gets or sets the airport code.
        /// </summary>
        public string? Code { get; init; } 

        /// <summary>
        /// Gets or sets the airport description.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Gets or sets the city associated with the airport.
        /// </summary>
        public CityDto? City { get; set; }
    }
}
