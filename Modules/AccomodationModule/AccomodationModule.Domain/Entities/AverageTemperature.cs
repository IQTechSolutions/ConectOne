using System.ComponentModel.DataAnnotations.Schema;
using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents the average high and low temperatures for a specific month.
    /// </summary>
    /// <remarks>This class provides properties to store the average high and low temperatures, as well as the
    /// month they correspond to. It is intended for use in applications that analyze or display temperature data over
    /// time.</remarks>
    public class AverageTemperature : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AverageTemperature"/> class.
        /// </summary>
        public AverageTemperature() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AverageTemperature"/> class using the specified data transfer
        /// object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see
        /// cref="AverageTemperatureDto"/> to the corresponding properties of the <see cref="AverageTemperature"/>
        /// instance.</remarks>
        /// <param name="dto">The data transfer object containing the average temperature details, including the identifier, month,
        /// average high temperature, and average low temperature.</param>
        public AverageTemperature(AverageTemperatureDto dto)
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
        /// Gets or sets the month component of a date.
        /// </summary>
        public DateTime Month { get; set; }

        /// <summary>
        /// Gets or sets the average high temperature.
        /// </summary>
        public double AvgHigh { get; set; }

        /// <summary>
        /// Gets or sets the average low temperature.
        /// </summary>
        public double AvgLow { get; set; }

        #endregion

        #region One-to-Many Relationships

        /// <summary>
        /// Gets or sets the unique identifier for the associated area.
        /// </summary>
        [ForeignKey(nameof(Area))] public string? AreaId { get; set; }

        /// <summary>
        /// Gets or sets the area associated with the object.
        /// </summary>
        public Area? Area { get; set; }

        #endregion
    }
}
