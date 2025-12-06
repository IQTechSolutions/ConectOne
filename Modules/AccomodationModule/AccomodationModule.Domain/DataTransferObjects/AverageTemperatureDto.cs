using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the average high and low temperatures for a specific month.
    /// </summary>
    /// <remarks>This class provides properties to store the average high and low temperatures, as well as the
    /// month they correspond to. It is intended for use in applications that analyze or display temperature data over
    /// time.</remarks>
    public record AverageTemperatureDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AverageTemperatureDto"/> class.
        /// </summary>
        public AverageTemperatureDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AverageTemperatureDto"/> class using the specified <see
        /// cref="AverageTemperature"/> object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the <see cref="AverageTemperature"/> object
        /// to the corresponding properties of the <see cref="AverageTemperatureDto"/>.</remarks>
        /// <param name="averageTemperature">The <see cref="AverageTemperature"/> object containing the data to initialize the DTO.</param>
        public AverageTemperatureDto(AverageTemperature averageTemperature)
        {
            Id = averageTemperature.Id;
            AreaId = averageTemperature.AreaId;
            Month = averageTemperature.Month;
            AvgHigh = averageTemperature.AvgHigh;
            AvgLow = averageTemperature.AvgLow;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; init; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the area.
        /// </summary>
        public string AreaId { get; set; }

        /// <summary>
        /// Gets or sets the month component of a date.
        /// </summary>
        public DateTime Month { get; init; }

        /// <summary>
        /// Gets or sets the average high temperature.
        /// </summary>
        public double AvgHigh { get; init; }

        /// <summary>
        /// Gets or sets the average low temperature.
        /// </summary>
        public double AvgLow { get; init; }

        #endregion
    }
}
