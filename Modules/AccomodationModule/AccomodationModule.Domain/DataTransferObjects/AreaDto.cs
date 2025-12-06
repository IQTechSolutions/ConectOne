using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for an area, including its basic details and associated average
    /// temperatures.
    /// </summary>
    /// <remarks>This class is designed to facilitate the transfer of area-related data between different
    /// layers of an application. It includes properties for the area's identifier, name, description, and a collection
    /// of average temperature data.</remarks>
    public record AreaDto
    {
        #region Contractors

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaDto"/> class.
        /// </summary>
        public AreaDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaDto"/> class using the specified <see cref="Area"/> object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="Area"/> object to the
        /// corresponding properties of the <see cref="AreaDto"/> instance. If the <see
        /// cref="Area.AverageTemperatures"/> property is null, the <see cref="AverageTemperatures"/> property will be
        /// initialized as an empty list.</remarks>
        /// <param name="area">The <see cref="Area"/> object containing the data to initialize the DTO. Cannot be null.</param>
        public AreaDto(Area area)
        {
            Id = area.Id;
            Name = area.Name;
            Description = area.Description;

            AverageTemperatures = area.AverageTemperatures?.Select(at => new AverageTemperatureDto(at)).ToList() ?? [];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the object.
        /// </summary>
        public string? Id { get; init; }

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string? Name { get; init; } 

        /// <summary>
        /// Gets the description associated with the current object.
        /// </summary>
        public string? Description { get; init; }

        #endregion

        #region Collection

        /// <summary>
        /// Gets the collection of average temperature data.
        /// </summary>
        public List<AverageTemperatureDto> AverageTemperatures { get; init; } = [];

        #endregion
    }
}
