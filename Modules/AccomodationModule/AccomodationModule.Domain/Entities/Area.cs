using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a geographical or logical area with associated metadata, including its name, description,  and
    /// average temperature data.
    /// </summary>
    /// <remarks>The <see cref="Area"/> class provides properties to store information about an area, such as
    /// its name,  description, and a collection of average temperature records. It is designed to be used in scenarios 
    /// where areas need to be modeled with associated data.</remarks>
    public class Area : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Area"/> class.
        /// </summary>
        public Area() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Area"/> class using the specified <see cref="AreaDto"/> object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="AreaDto"/> to the
        /// corresponding properties of the <see cref="Area"/> instance. If the <c>AverageTemperatures</c> property of
        /// the <paramref name="area"/> is null, an empty list is assigned to the <c>AverageTemperatures</c> property of
        /// the <see cref="Area"/> instance.</remarks>
        /// <param name="area">The data transfer object containing the area details. Cannot be null.</param>
        public Area(AreaDto area)
        {
            Id = area.Id;
            Name = area.Name;
            Description = area.Description;

            AverageTemperatures = area.AverageTemperatures?.Select(at => new AverageTemperature(at)).ToList() ?? [];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description associated with the current object.
        /// </summary>
        public string? Description { get; set; }

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the collection of average temperature records.
        /// </summary>
        public virtual ICollection<AverageTemperature>? AverageTemperatures { get; set; } = [];

        #endregion
    }
}
