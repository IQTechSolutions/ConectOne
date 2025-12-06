using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a template for adding a meal, including details such as the date, time, guest type, meal type, and
    /// associated restaurant.
    /// </summary>
    /// <remarks>This class is used to define the structure and properties of a meal addition, including
    /// optional notes and relationships to a restaurant. It supports specifying the type of guest and meal, as well as
    /// the timing details.</remarks>
    public class MealAdditionTemplate : EntityBase<string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the type of guest for whom the meal is intended.
        /// </summary>
        public GuestType GuestType { get; set; }

        /// <summary>
        /// Gets or sets the type of meal (e.g., breakfast, lunch, dinner).
        /// </summary>
        public MealType MealType { get; set; } = MealType.Dinner;

        /// <summary>
        /// Gets or sets additional notes or details about the meal addition.
        /// </summary>
        [MaxLength(5000)] public string? Notes { get; set; } = null!;

        #endregion

        #region One-to-Many Relationships

        /// <summary>
        /// Gets or sets the identifier of the associated restaurant.
        /// </summary>
        [ForeignKey(nameof(Restaurant))] public string? RestaurantId { get; set; }

        /// <summary>
        /// Gets or sets the restaurant associated with the current context.
        /// </summary>
        public Restaurant? Restaurant { get; set; }

        #endregion
    }
}
