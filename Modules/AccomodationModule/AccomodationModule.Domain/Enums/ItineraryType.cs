using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Represents the type of itinerary for an event or activity.
    /// </summary>
    /// <remarks>This enumeration defines various categories of itineraries, such as meals, tours, and special
    /// events. Use this type to specify the nature of an activity in scheduling or event management systems.</remarks>
    public enum ItineraryType
    {
        /// <summary>
        /// Represents a gift or room drop event.
        /// </summary>
        /// <remarks>This enumeration value is typically used to identify events related to gifts or room
        /// drops in the context of a specific application or system.</remarks>
        [Description("Gift/Room Drop")] GiftRoomDrop,

        /// <summary>
        /// Represents the "Meet & Greet" event type.
        /// </summary>
        /// <remarks>This enumeration value is used to identify events categorized as "Meet &
        /// Greet."</remarks>
        [Description("Meet & Greet")] MeetAndGreet,

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
        /// <remarks>This enumeration value is used to specify the dinner category in meal-related
        /// contexts.</remarks>
        [Description("Dinner")] Dinner,

        /// <summary>
        /// Represents a day tour activity or event.
        /// </summary>
        /// <remarks>This enumeration value is typically used to categorize or identify activities that
        /// occur during the day.</remarks>
        [Description("Day Tour")] DayTour
    }
}
