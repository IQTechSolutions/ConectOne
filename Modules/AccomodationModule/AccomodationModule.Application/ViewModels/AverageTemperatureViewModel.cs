using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the average high and low temperatures for a specific month.
    /// </summary>
    /// <remarks>This class provides properties to store the average high and low temperatures, as well as the
    /// month they correspond to. It is intended for use in applications that analyze or display temperature data over
    /// time.</remarks>
    public class AverageTemperatureViewModel 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AverageTemperatureViewModel"/> class.
        /// </summary>
        public AverageTemperatureViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AverageTemperatureViewModel"/> class using the data provided in
        /// the specified <see cref="AverageTemperatureDto"/>.
        /// </summary>
        /// <param name="dto">The data transfer object containing the average temperature data. Must not be <see langword="null"/>.</param>
        public AverageTemperatureViewModel(AverageTemperatureDto dto)
        {
            Id = dto.Id;
            AreaId = dto.AreaId;
            Month = dto.Month;
            AvgHigh = dto.AvgHigh;
            AvgLow = dto.AvgLow;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the unique identifier for the area.
        /// </summary>
        public string AreaId { get; set; }

        /// <summary>
        /// Gets or sets the month component of a date.
        /// </summary>
        public DateTime? Month { get; set; }

        /// <summary>
        /// Gets or sets the average high temperature.
        /// </summary>
        public double AvgHigh { get; set; }

        /// <summary>
        /// Gets or sets the average low temperature.
        /// </summary>
        public double AvgLow { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Converts the current instance to an <see cref="AverageTemperatureDto"/>.
        /// </summary>
        /// <returns>An <see cref="AverageTemperatureDto"/> representing the current instance,  including its identifier, area
        /// ID, month, average high temperature, and average low temperature.</returns>
        public AverageTemperatureDto ToDto()
        {
            return new AverageTemperatureDto
            {
                Id = Id,
                AreaId = AreaId,
                Month = Month.Value,
                AvgHigh = AvgHigh,
                AvgLow = AvgLow
            };
        }

        #endregion
    }
}
