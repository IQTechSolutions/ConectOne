using System.ComponentModel.DataAnnotations.Schema;
using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;
using LocationModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// The <see cref="Airport"/> class represents an airport entity with properties for its name, code, and description.
    /// </summary>
public class Airport : EntityBase<string>
{
        /// <summary>
        /// Initializes a new instance of the <see cref="Airport"/> class.
        /// </summary>
        public Airport() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Airport"/> class using an <see cref="AirportDto"/>.
        /// </summary>
        /// <param name="dto">Airport dto.</param>
        public Airport(AirportDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Code = dto.Code;
            Description = dto.Description ?? string.Empty;
            CityId = dto.City.CityId;
        }

        /// <summary>
        /// Gets o sets the name of the airport name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the airport code.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Gets or sets the airport description.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated city.
        /// </summary>
        [ForeignKey(nameof(City))] public string? CityId { get; set; }

        /// <summary>
        /// Gets or sets the city associated with the current entity.
        /// </summary>
        public City? City { get; set; }

        /// <summary>
        /// Gets or sets the collection of arriving flights.
        /// </summary>
        public ICollection<Flight> Arrivals { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of flights scheduled to depart.
        /// </summary>
        public ICollection<Flight> Departures { get; set; } = [];
}
}
