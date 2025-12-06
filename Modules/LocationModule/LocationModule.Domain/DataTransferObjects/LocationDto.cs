using LocationModule.Domain.Entities;

namespace LocationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a location, containing details such as its identifier, description,
    /// code, and associated destination.
    /// </summary>
    /// <remarks>This DTO is used to encapsulate location data for transfer between application layers or
    /// external systems.  It includes properties for the location's unique identifier, descriptive information, and its
    /// associated destination.</remarks>
    public record LocationDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationDto"/> class.
        /// </summary>
        public LocationDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationDto"/> class using the specified location entity.
        /// </summary>
        /// <param name="location">The location entity from which to populate the DTO. Must not be <see langword="null"/>.</param>
        public LocationDto(Location location)
        {
            LocationId = location.Id;
            LocationDesc = location.LocationDesc;
            Code = location.Code;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for a location.
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Gets the description of the location.
        /// </summary>
        public string LocationDesc { get; init; }

        /// <summary>
        /// Gets the unique code associated with this instance.
        /// </summary>
        public string Code { get; init; }
    }
}
