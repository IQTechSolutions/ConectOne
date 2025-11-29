using System.ComponentModel;

namespace ConectOne.Domain.Enums
{
    /// <summary>
    /// Specifies the type of address associated with an entity.
    /// </summary>
    /// <remarks>This enumeration is used to differentiate between various address types, such as physical,
    /// shipping, and billing. It is commonly used in scenarios where multiple address types need to be managed or
    /// processed.</remarks>
    public enum AddressType
    {
        /// <summary>
        /// Represents the physical type of an entity or object.
        /// </summary>
        [Description("Physical")]
        Physical = 0,

        /// <summary>
        /// Represents the shipping status of an order.
        /// </summary>
        [Description("Shipping")]
        Shipping = 1,

        /// <summary>
        /// Represents the billing category in the system.
        /// </summary>
        /// <remarks>This enumeration value is used to identify operations or entities related to
        /// billing.</remarks>
        [Description("Billing")]
        Billing = 2
    }
}
