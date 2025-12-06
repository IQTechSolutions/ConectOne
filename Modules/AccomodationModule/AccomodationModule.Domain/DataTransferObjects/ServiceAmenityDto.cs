using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a service amenity, providing information about its name, associated
    /// room, and unique identifier.
    /// </summary>
    /// <remarks>This record is designed to facilitate the transfer of service amenity data between different
    /// layers of the application, such as the domain model, view model, and external services. It includes multiple
    /// constructors for creating instances from various related types, such as <see cref="ServiceAmenity"/></remarks>
    public record ServiceAmenityDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAmenityDto"/> class.
        /// </summary>
        public ServiceAmenityDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAmenityDto"/> class with the specified name and room
        /// identifier.
        /// </summary>
        /// <param name="name">The name of the service amenity. Cannot be null or empty.</param>
        /// <param name="roomId">The identifier of the room associated with the service amenity. Must be a positive integer.</param>
        public ServiceAmenityDto(string name, int roomId)
        {
            Name = name;
            RoomId = roomId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAmenityDto"/> class,  mapping data from a <see
        /// cref="ServiceAmenity"/> object.
        /// </summary>
        /// <remarks>This constructor extracts the ID, name, and room ID from the provided <see
        /// cref="ServiceAmenity"/>  and assigns them to the corresponding properties of the DTO.</remarks>
        /// <param name="amenity">The <see cref="ServiceAmenity"/> object containing the data to initialize the DTO.</param>
        public ServiceAmenityDto(ServiceAmenity amenity)
        {
            ServiceAmenityId = amenity.Id;
            Name = amenity.Name;
            RoomId = amenity.RoomId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the service amenity.
        /// </summary>
        public int ServiceAmenityId { get; init; }

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets the unique identifier for the room.
        /// </summary>
        public int RoomId { get; init; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance to a <see cref="ServiceAmenity"/> object.
        /// </summary>
        /// <remarks>The resulting <see cref="ServiceAmenity"/> object will have its <c>Name</c> and
        /// <c>RoomId</c> properties populated based on the corresponding properties of the current instance.</remarks>
        /// <returns>A new <see cref="ServiceAmenity"/> object with values copied from the current instance.</returns>
        public ServiceAmenity ToServiceAmenity()
        {
            return new ServiceAmenity()
            {
                Name = this.Name,
                RoomId = this.RoomId
            };
        }

        #endregion

    }
}
