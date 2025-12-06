using ConectOne.Domain.Entities;

namespace LocationModule.Domain.Entities
{
    /// <summary>
    /// Represents a physical or logical location with an optional association to a destination.
    /// </summary>
    /// <remarks>The <see cref="Location"/> class provides properties to describe a location, including a
    /// description,  a code, and an optional reference to a related destination. This class inherits from <see
    /// cref="EntityBase{TId}"/>  with an integer identifier.</remarks>
    public class Location : EntityBase<int>
    {
        /// <summary>
        /// Gets or sets the description of the location.
        /// </summary>
        public string LocationDesc { get; set; }

        /// <summary>
        /// Gets or sets the code associated with this instance.
        /// </summary>
        public string Code { get; set; }
    }
}
