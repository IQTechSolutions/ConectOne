using ConectOne.Domain.Entities;

namespace LocationModule.Domain.Entities
{
    /// <summary>
    /// Represents a country with associated metadata and a collection of provinces.
    /// </summary>
    /// <remarks>The <see cref="Country"/> class provides properties for storing country-specific information,
    /// such as its code, name, short name, and description. It also maintains a collection of  associated provinces,
    /// allowing hierarchical geographic data representation.</remarks>
    public class Country : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the code associated with this instance.
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the short name associated with the entity.
        /// </summary>
        public string ShortName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of provinces associated with this entity.
        /// </summary>
        public virtual ICollection<City> Cities { get; set; } = new List<City>();
    }
}
