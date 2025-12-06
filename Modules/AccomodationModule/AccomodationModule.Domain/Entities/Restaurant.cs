using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a restaurant entity, including its name, optional comments, and relationships to other entities.
    /// </summary>
    /// <remarks>The <see cref="Restaurant"/> class models a restaurant with properties for its name, optional
    /// comments,  and relationships to associated areas and meal additions. It supports one-to-many and many-to-one
    /// relationships  with other entities, such as <see cref="Area"/> and <see cref="MealAddition"/>.</remarks>
    public class Restaurant : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Restaurant"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see cref="Restaurant"/> class
        /// with no predefined values. Use this constructor when you need to create a restaurant object without
        /// initializing specific properties.</remarks>
        public Restaurant() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Restaurant"/> class using the specified data transfer object.
        /// </summary>
        /// <remarks>This constructor sets the <see cref="Name"/> and <see cref="Comments"/> properties of
        /// the restaurant based on the values provided in the <paramref name="dto"/> parameter.</remarks>
        /// <param name="dto">A <see cref="RestaurantDto"/> object containing the initial values for the restaurant's properties. The <see
        /// cref="RestaurantDto.Name"/> and <see cref="RestaurantDto.Comments"/> properties must not be null.</param>
        public Restaurant(RestaurantDto dto)
        {
            Name = dto.Name;
            Comments = dto.Comments;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the comments associated with the entity.
        /// </summary>
        public string? Comments { get; set; }

        #endregion

        #region Many-to-one Relationships

        /// <summary>
        /// Gets or sets the list of meal additions associated with the meal.
        /// </summary>
        public List<MealAddition> MealAdditions { get; set; } = [];

        #endregion
    }
}
