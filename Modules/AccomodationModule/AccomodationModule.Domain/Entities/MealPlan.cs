using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a meal plan associated with a room and rate, including details such as type, description, and
    /// pricing.
    /// </summary>
    /// <remarks>A meal plan defines the type of meals provided (e.g., Bed and Breakfast, Full Board) and
    /// includes pricing information. It can be marked as the default option and may have an original rate before
    /// discounts or adjustments.</remarks>
    public class MealPlan : EntityBase<string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier of the associated room.
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated rate.
        /// </summary>
        public int RateId { get; set; }

        /// <summary>
        /// Gets or sets the type of the meal plan (e.g., Bed and Breakfast, Full Board).
        /// </summary>
        public MealPlanTypes PartnerMealPlanId { get; set; }

        /// <summary>
        /// Gets or sets the description of the meal plan.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether this meal plan is the default option.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Gets or sets the rate of the meal plan.
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// Gets or sets the original rate of the meal plan before any discounts or adjustments.
        /// </summary>
        public double OriginalRate { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the meal plan.
        /// </summary>
        /// <returns>A string indicating the meal plan.</returns>
        public override string ToString()
        {
            return $"Meal Plan";
        }

        #endregion
    }
}
