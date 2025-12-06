using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for an amenity, providing essential information such as its identifier,
    /// icon, name, and description.
    /// </summary>
    /// <remarks>This class is designed to facilitate the transfer of amenity-related data between different
    /// layers of an application, such as the presentation layer and the domain layer. It includes constructors for
    /// initializing the object from various models, making it adaptable to different contexts.</remarks>
    public record AmenityDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenityDto"/> class.
        /// </summary>
        public AmenityDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenityDto"/> class using the specified <see cref="Amenity"/>
        /// object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the <see cref="Amenity"/> object to the
        /// corresponding properties of the <see cref="AmenityDto"/>.</remarks>
        /// <param name="amenity">The <see cref="Amenity"/> object containing the data to populate the DTO. Cannot be null.</param>
        public AmenityDto(Amenity amenity)
        {
            AmenityId = amenity.Id.ToString();
            Icon = amenity.IconClass;
            Name = amenity.Name;
            Description = amenity.Description;
        }

		#endregion

        /// <summary>
        /// Gets the unique identifier for the amenity.
        /// </summary>
		public string? AmenityId { get; init; }

        /// <summary>
        /// Gets or sets the icon associated with the object.
        /// </summary>
        public string Icon { get; set; } = null!;

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets the description associated with the current object.
        /// </summary>
		public string? Description { get; init; }

        /// <summary>
        /// Converts the current object to an <see cref="Amenity"/> instance.
        /// </summary>
        /// <remarks>This method creates a new <see cref="Amenity"/> object and populates its properties
        /// based on the corresponding values of the current object.</remarks>
        /// <returns>An <see cref="Amenity"/> object initialized with the values of the current object's <c>Icon</c>,
        /// <c>Name</c>, and <c>Description</c> properties.</returns>
        public Amenity ToAmenity()
        {
            return new Amenity()
            {
                IconClass = this.Icon,
                Name = this.Name,
                Description = this.Description
            };
        }
    }
}
