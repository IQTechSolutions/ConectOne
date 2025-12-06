using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;
using LocationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for an airport, containing its name, code and description.
    /// </summary>
    public class AirportViewModel : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AirportViewModel"/> class.
        /// </summary>
        public AirportViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AirportViewModel"/> class using the specified airport data transfer object.
        /// </summary>
        /// <param name="dto">The <see cref="AirportDto"/> containing the airport's data.</param>
        public AirportViewModel(AirportDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Code = dto.Code;
            Description = dto.Description;
            City = dto.City ?? new CityDto();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the object.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the airport name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the airport code.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Gets or sets the airport description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the city information associated with the current entity.
        /// </summary>
        public CityDto City { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current airport entity to its corresponding data transfer object (DTO).
        /// </summary>
        /// <returns>An <see cref="AirportDto"/> instance containing the data of the current airport entity.</returns>
        public AirportDto ToDto()
        {
            return new AirportDto
            {
                Id = this.Id,
                Name = this.Name,
                Code = this.Code,
                Description = this.Description,
                City = this.City
            };
        }

        #endregion
    }
}
