using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Represents the type of reservation for accommodations.
    /// </summary>
    /// <remarks>This enumeration is used to specify the configuration of a reservation, such as the number 
    /// and arrangement of beds. It can be used in booking systems to differentiate between  single, double, and twin
    /// room types.</remarks>
    public enum ReservationType
    {
        /// <summary>
        /// Represents a single precision floating-point number.
        /// </summary>
        /// <remarks>Single precision floating-point numbers are commonly used for numerical computations
        /// where  memory usage is a concern or where the precision of double precision is not required.</remarks>
        [Description("Single")] Single,

        /// <summary>
        /// Represents a double-precision floating-point numeric value.
        /// </summary>
        /// <remarks>A double-precision floating-point number is a 64-bit IEEE 754 value used to represent
        /// real numbers with a higher degree of precision compared to single-precision floating-point
        /// numbers.</remarks>
        [Description("Double")] Double,

        /// <summary>
        /// Represents a pair of values or entities, often used to model relationships or dual properties.
        /// </summary>
        /// <remarks>This type is commonly used in scenarios where two related values need to be grouped
        /// together. Examples include coordinate pairs, key-value pairs, or other dual-property constructs.</remarks>
        [Description("Double")] Twin
    }
}
