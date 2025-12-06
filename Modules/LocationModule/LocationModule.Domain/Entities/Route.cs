using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Entities;

namespace LocationModule.Domain.Entities
{
    /// <summary>
    /// Represents a route associated with a specific entity type, including details such as the supervisor, driver,
    /// name, description, and associated addresses.
    /// </summary>
    /// <remarks>This class provides properties to define and manage a route, including its name, description,
    /// and active status.  It also maintains a collection of addresses associated with the route.</remarks>
    /// <typeparam name="T">The type of entity associated with the route.</typeparam>
    public class Route<T> : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the identifier of the supervisor associated with this entity.
        /// </summary>
        public string SupervisorId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the driver.
        /// </summary>
        public string DriverId { get; set; }

        /// <summary>
        /// Gets or sets the name of the route.
        /// </summary>
        /// <remarks>This property is required and must be set to a valid value. If the value is not
        /// provided or exceeds the maximum length,  validation errors will occur.</remarks>
        [Required(ErrorMessage = @"Route Name is Required")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description text associated with the entity.
        /// </summary>
        [DataType(DataType.MultilineText)]
        [MaxLength(500, ErrorMessage = "Maximum length for the Description is 500 characters.")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets the collection of route addresses associated with the current instance.
        /// </summary>
        public ICollection<Address<Route<T>>> RouteAddresses { get; set; } = new List<Address<Route<T>>>();

        /// <summary>
        /// Returns a string representation of the route associated with the specified type.
        /// </summary>
        /// <returns>A string that represents the route for the type <typeparamref name="T"/>.</returns>
        public override string ToString()
        {
            return $"Route for {typeof(T).Name}";
        }
    }
}
