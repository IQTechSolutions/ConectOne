using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Represents the type of amenity available in a facility or service.
    /// </summary>
    /// <remarks>This enumeration categorizes amenities into distinct types, such as lodging or room-specific
    /// amenities. Use this type to specify or filter amenities based on their category.</remarks>
    public enum AmenityType
    {
        /// <summary>
        /// Represents a lodging category, typically used to classify accommodations such as hotels, motels, or other
        /// places of stay.
        /// </summary>
        /// <remarks>This enumeration value is commonly used in systems that categorize or manage
        /// different types of accommodations.</remarks>
        [Description("Lodging")] Lodging,

        /// <summary>
        /// Represents a room within a building or structure.
        /// </summary>
        /// <remarks>This class can be used to model physical spaces, such as rooms in a house, office, or
        /// other structures. Additional properties and methods may be added to define specific characteristics or
        /// behaviors of the room.</remarks>
        [Description("Room")] Room
    }
}
