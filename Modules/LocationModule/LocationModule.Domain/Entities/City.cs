using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace LocationModule.Domain.Entities
{
    /// <summary>
    /// Represents a city, including its name, code, and optional metadata such as a short name and description.
    /// </summary>
    /// <remarks>The <see cref="City"/> class is designed to model a city entity, including its relationship
    /// to a country. It includes properties for the city's name, code, and optional details such as a short name and
    /// description. Additionally, it supports a one-to-many relationship with the <see cref="Country"/> class, allowing
    /// a city to be associated with a specific country.</remarks>
    public class City : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the code associated with this instance.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Gets or sets the short name associated with the entity.
        /// </summary>
        public string? ShortName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; set; } = null!;

        #region One-To-Many Relationship

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        [ForeignKey(nameof(Country))] public string? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the country associated with the entity.
        /// </summary>
        public Country? Country { get; set; }

        #endregion
    }
}
