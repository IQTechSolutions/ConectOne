using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Specifies the types of vacation inclusions to be displayed.
    /// </summary>
    /// <remarks>This enumeration is used to categorize and display different aspects of a vacation package, 
    /// such as general information, accommodations, meals and activities, golf rounds, and transportation or
    /// flights.</remarks>
    public enum VacationInclusionDisplayTypes
    {
        /// <summary>
        /// Represents general information or settings for the application.
        /// </summary>
        /// <remarks>This enumeration value is typically used to categorize or identify general-purpose
        /// data or settings.</remarks>
        [Description("General Info")] General,

        /// <summary>
        /// Represents an accommodation entity, typically used to describe lodging or housing options.
        /// </summary>
        /// <remarks>This enumeration value is associated with the description "Accommodation." It can be
        /// used in contexts where accommodations need to be categorized or identified.</remarks>
        [Description("Accommodation")] Accommodation,

        /// <summary>
        /// Represents the category for meals and activities.
        /// </summary>
        /// <remarks>This enumeration value is typically used to classify items or events related to meals
        /// and activities.</remarks>
        [Description("Meals & Activities")] MealsAndActivities,

        /// <summary>
        /// Represents the number of golf rounds played.
        /// </summary>
        /// <remarks>This property is typically used to track or display the count of golf rounds in a
        /// given context.</remarks>
        [Description("Golf Rounds")] GolfRounds,

        /// <summary>
        /// Represents the transport and flights category.
        /// </summary>
        /// <remarks>This category is typically used to group items or services related to transportation
        /// and air travel.</remarks>
        [Description("Transport & Flights")] TransportAndFlights
    }
}
