using System.ComponentModel;

namespace ShoppingModule.Domain.Enums
{
    /// <summary>
    /// Represents the type of an item in a shopping cart.
    /// </summary>
    /// <remarks>This enumeration defines the various categories of items that can be added to a shopping
    /// cart.  Each value corresponds to a specific type of item, such as standard products, bundled items, or
    /// lodging-related items.</remarks>
    public enum ShoppingCartItemType
    {
        /// <summary>
        /// Represents a standard item in the system.
        /// </summary>
        /// <remarks>This enumeration value is typically used to identify items that conform to standard
        /// or default configurations.</remarks>
        [Description("Standard Item")] StandardItem,

        /// <summary>
        /// Represents the parent item in a hierarchical structure.
        /// </summary>
        /// <remarks>This enumeration value is typically used to identify or refer to the parent node or
        /// item in a collection, tree, or other hierarchical data structure.</remarks>
        [Description("Parent Item")] ParentItem,

        /// <summary>
        /// Represents a product that is a combination of multiple items.
        /// </summary>
        /// <remarks>This enumeration value is typically used to identify products that consist of bundled
        /// items sold together as a single unit.</remarks>
        [Description("Combo Product")] ComboProduct,

        /// <summary>
        /// Represents a product that is included as part of a larger offering or package.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate that the product is bundled or
        /// provided  as part of another product or service.</remarks>
        [Description("Included Product")] IncludedProduct,

        /// <summary>
        /// Represents a lodging item, such as a hotel, motel, or other accommodation.
        /// </summary>
        /// <remarks>This enumeration value is used to categorize items related to lodging.</remarks>
        [Description("Lodging Item")] LodgingItem,

        /// <summary>
        /// Represents a lodging voucher, typically used to document accommodations or stays.
        /// </summary>
        /// <remarks>This enumeration value is often used in contexts where lodging-related transactions
        /// or records need to be identified.</remarks>
        [Description("Lodging Voucher")] LodgingVoucher,

        /// <summary>
        /// Represents a service category or type.
        /// </summary>
        /// <remarks>This enumeration value is used to identify a specific service within the
        /// application.</remarks>
        [Description("Service")] Service,

        /// <summary>
        /// Represents the registration status or process in the application.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate or categorize actions, states,
        /// or data  related to user registration within the system.</remarks>
        [Description("Registration")] Registration,

        /// <summary>
        /// Represents the advertising category.
        /// </summary>
        [Description("Advertising")] Advertising,
    }
}
