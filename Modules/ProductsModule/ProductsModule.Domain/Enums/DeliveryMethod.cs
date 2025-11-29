using System.ComponentModel;

namespace ProductsModule.Domain.Enums
{
    /// <summary>
    /// Specifies the available delivery methods for shipping items.
    /// </summary>
    /// <remarks>This enumeration defines the different ways shipping costs can be calculated: <list
    /// type="bullet"> <item> <term><see cref="Free"/></term> <description>Indicates that shipping is free of
    /// charge.</description> </item> <item> <term><see cref="Flat"/></term> <description>Indicates that a flat rate is
    /// applied to the shipping cost.</description> </item> <item> <term><see cref="PerItem"/></term>
    /// <description>Indicates that shipping costs are calculated per item in the order.</description> </item> </list>
    /// Use this enumeration to specify the delivery method when configuring shipping options.</remarks>
    public enum DeliveryMethod
    {
        /// <summary>
        /// Represents a free tier or category.
        /// </summary>
        /// <remarks>This value is typically used to indicate that no cost is associated with the item or
        /// service.</remarks>
        [Description("Free")] Free,

        /// <summary>
        /// Represents a flat or level surface.
        /// </summary>
        [Description("Flat")] Flat,

        /// <summary>
        /// Represents a pricing model where charges are calculated per individual item.
        /// </summary>
        [Description("Per Item")] PerItem
    }
}
