using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents an amenity or service associated with a room.
    /// </summary>
    /// <remarks>A <see cref="ServiceAmenity"/> is used to define specific features or services available for
    /// a room. It includes a name describing the amenity and an identifier for the associated room.</remarks>
    public class ServiceAmenity : EntityBase<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAmenity"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see cref="ServiceAmenity"/>
        /// class. Use this constructor when no specific initialization is required.</remarks>
        public ServiceAmenity() { } 

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAmenity"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the service amenity. Cannot be null or empty.</param>
        public ServiceAmenity(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the room.
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// Returns a string representation of the service amenity.
        /// </summary>
        /// <returns>A string that represents the service amenity.</returns>
        public override string ToString()
        {
            return $"Service Amenity";
        }
    }
}
