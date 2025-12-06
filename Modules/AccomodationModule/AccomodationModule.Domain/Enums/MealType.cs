using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Represents the type of meal in a day, such as breakfast, lunch, or dinner.
    /// </summary>
    /// <remarks>This enumeration is commonly used to categorize meals in applications related to food
    /// services, meal planning, or dietary tracking.</remarks>
    public enum MealType
    {
        /// <summary>
        /// Represents the breakfast meal category.
        /// </summary>
        /// <remarks>This enumeration value is typically used to categorize items or activities related to
        /// breakfast.</remarks>
        [Description("Breakfast")] Breakfast,

        /// <summary>
        /// Represents the lunch meal option.
        /// </summary>
        /// <remarks>This enumeration value is typically used to specify a meal type in contexts such as
        /// menu selection or meal planning.</remarks>
        [Description("Lunch")] Lunch,

        /// <summary>
        /// Represents a meal categorized as dinner.
        /// </summary>
        /// <remarks>This enumeration value is used to specify the dinner meal type in contexts where meal
        /// categorization is required.</remarks>
        [Description("Dinner")] Dinner
    }
}
