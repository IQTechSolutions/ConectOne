using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for an area, containing details such as its identifier, name, description,  and average
    /// temperature data.
    /// </summary>
    /// <remarks>This class is typically used to transfer area-related data between different layers of an
    /// application,  such as from a data access layer to a presentation layer. It provides properties for basic area 
    /// information and a collection of average temperature data.</remarks>
    public class AreaViewModel 
    {
        #region Contractor

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaViewModel"/> class.
        /// </summary>
        public AreaViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaViewModel"/> class using the specified area data transfer
        /// object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="Dtos.AreaDto"/> to the
        /// corresponding properties of the <see cref="AreaViewModel"/>. If <see cref="Dtos.AreaDto.AverageTemperatures"/> is
        /// <see langword="null"/>, an empty array is assigned to <see cref="AverageTemperatures"/>.</remarks>
        /// <param name="areaDto">The data transfer object containing information about the area.  Must not be <see langword="null"/>.</param>
        public AreaViewModel(AreaDto areaDto)
        {
            Id = areaDto.Id;
            Name = areaDto.Name;
            Description = areaDto.Description;
            AverageTemperatures = areaDto.AverageTemperatures ?? [];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the object.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public string? Description { get; set; }

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the collection of average temperature data.
        /// </summary>
        public virtual List<AverageTemperatureDto> AverageTemperatures { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the area to its corresponding data transfer object (DTO).
        /// </summary>
        /// <returns>An <see cref="AreaDto"/> object that represents the current area, including its identifier, name, 
        /// description, and average temperatures. If <c>AverageTemperatures</c> is <c>null</c>, an empty array is
        /// returned.</returns>
        public AreaDto ToDto()
        {
            return new AreaDto
            {
                Id = Id,
                Name = Name,
                Description = Description,

                AverageTemperatures = AverageTemperatures ?? []
            };
        }

        #endregion

    }
}
