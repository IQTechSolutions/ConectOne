using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Defines the types of gifts that can be categorized.
    /// </summary>
    /// <remarks>The <see cref="GiftType"/> enumeration provides predefined categories for gifts, such as
    /// rooms, persons, or couples. It can be used in applications where gift categorization or identification is
    /// required.</remarks>
    public enum GiftType
    {
        /// <summary>
        /// Represents a room within a building or structure.
        /// </summary>
        /// <remarks>This enumeration value is used to identify or categorize a room.</remarks>
        [Description("Room")] Room,

        /// <summary>
        /// Represents an individual person with associated properties and behaviors.
        /// </summary>
        /// <remarks>This class is used to model a person entity, typically including attributes such as
        /// name, age, and other personal details. It can be extended or utilized in scenarios where person-related data
        /// needs to be managed.</remarks>
        [Description("Person")] Person,

        /// <summary>
        /// Represents a pair or couple of related items.
        /// </summary>
        /// <remarks>This type is typically used to group two related objects or values
        /// together.</remarks>
        [Description("Couple")] Couple
    }
}
