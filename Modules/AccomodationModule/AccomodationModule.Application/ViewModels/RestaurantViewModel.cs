using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a restaurant, containing its name, comments, and associated area.
    /// </summary>
    /// <remarks>This class is typically used to transfer restaurant-related data between layers in an
    /// application. It provides properties for the restaurant's name, comments, and area information.</remarks>
    public class RestaurantViewModel : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RestaurantViewModel"/> class.
        /// </summary>
        public RestaurantViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestaurantViewModel"/> class using the specified restaurant
        /// data transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="RestaurantDto"/> to
        /// the corresponding properties of the <see cref="RestaurantViewModel"/> instance.</remarks>
        /// <param name="dto">The <see cref="RestaurantDto"/> containing the restaurant's data, including its name, comments, and area.</param>
        public RestaurantViewModel(RestaurantDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Comments = dto.Comments;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the object.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the comments associated with the entity.
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Gets or sets the area associated with the current object.
        /// </summary>
        public AreaDto Area { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current restaurant entity to a data transfer object (DTO).
        /// </summary>
        /// <returns>A <see cref="RestaurantDto"/> instance containing the restaurant's ID, name, and comments.</returns>
        public RestaurantDto ToDto()
        {
            return new RestaurantDto()
            {
                Id = Id,
                Name = Name,
                Comments = Comments
            };
        }

        #endregion
    }
}
