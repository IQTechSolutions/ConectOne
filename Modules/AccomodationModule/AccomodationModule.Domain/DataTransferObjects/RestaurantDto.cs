using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a restaurant, containing essential information such as its
    /// identifier, name, comments, and associated area.
    /// </summary>
    /// <remarks>This class is designed to facilitate the transfer of restaurant-related data between
    /// different layers of an application, such as between the domain model and the presentation layer. It provides
    /// constructors for initializing the DTO from various source types, including domain models and view
    /// models.</remarks>
    public record RestaurantDto 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RestaurantDto"/> class.
        /// </summary>
        public RestaurantDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestaurantDto"/> class using the specified <see
        /// cref="Restaurant"/> object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="Restaurant"/> object
        /// to the corresponding properties of the <see cref="RestaurantDto"/>.</remarks>
        /// <param name="restaurant">The <see cref="Restaurant"/> object containing the data to initialize the DTO. Cannot be <see
        /// langword="null"/>.</param>
        public RestaurantDto(Restaurant restaurant)
        {
            Id = restaurant?.Id;
            Name = restaurant?.Name;
            Comments = restaurant?.Comments;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public string Id { get; init; } = null!;

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets the comments associated with the current object.
        /// </summary>
        public string? Comments { get; init; }

        #endregion
    }
}