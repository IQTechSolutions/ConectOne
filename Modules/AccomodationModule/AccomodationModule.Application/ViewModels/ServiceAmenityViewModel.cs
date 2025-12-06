using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a service amenity, providing details such as its identifier, name, and associated
    /// room.
    /// </summary>
    /// <remarks>This class is typically used to transfer data between the application layers, such as from a
    /// data source to the UI. It can be initialized using a <see cref="ServiceAmenityDto"/> object or directly with
    /// specific values.</remarks>
	public class ServiceAmenityViewModel
    {
        #region Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAmenityViewModel"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see
        /// cref="ServiceAmenityViewModel"/> class. Use this constructor when no initial data needs to be
        /// provided.</remarks>
        public ServiceAmenityViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAmenityViewModel"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <param name="dto">The <see cref="ServiceAmenityDto"/> containing the data to initialize the view model. <paramref name="dto"/>
        /// must not be <see langword="null"/>.</param>
        public ServiceAmenityViewModel(ServiceAmenityDto dto)
        {
            ServiceAmentityId = dto.ServiceAmenityId;
            Name = dto.Name;
            RoomId = dto.RoomId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAmenityViewModel"/> class with the specified name and
        /// room ID.
        /// </summary>
        /// <param name="name">The name of the service amenity. Cannot be null or empty.</param>
        /// <param name="roomId">The unique identifier of the room associated with the service amenity. Must be a positive integer.</param>
        public ServiceAmenityViewModel(string name, int roomId)
        {
            Name = name;
            RoomId = roomId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the service amenity.
        /// </summary>
        public int ServiceAmentityId { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the room.
        /// </summary>
        public int RoomId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the <see cref="ServiceAmenity"/> class to a  <see
        /// cref="ServiceAmenityDto"/> object.
        /// </summary>
        /// <returns>A <see cref="ServiceAmenityDto"/> object containing the data from the current instance.</returns>
        public ServiceAmenityDto ToDto()
        {
            return new ServiceAmenityDto
            {
                ServiceAmenityId = this.ServiceAmentityId,
                Name = this.Name,
                RoomId = this.RoomId
            };
        }

        #endregion
    }
}
