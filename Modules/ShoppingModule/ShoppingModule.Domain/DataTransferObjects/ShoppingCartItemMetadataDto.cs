using ShoppingModule.Domain.Entities;

namespace ShoppingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents metadata associated with a shopping cart item, including a name and an optional value.
    /// </summary>
    /// <remarks>This type is typically used to store additional information about a shopping cart item, such
    /// as custom attributes or descriptive details. The <see cref="Name"/> property is required, while the <see
    /// cref="Value"/> property is optional.</remarks>
    public record ShoppingCartItemMetadataDto
    {
        #region Constructors

        /// <summary>
        /// Represents metadata associated with a shopping cart item.
        /// </summary>
        /// <remarks>This class is used to store additional information about a shopping cart item. It is
        /// typically used in scenarios where metadata is required for processing or displaying cart items.</remarks>
        public ShoppingCartItemMetadataDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItemMetadataDto"/> class with the specified name,
        /// value, and shopping cart item ID.
        /// </summary>
        /// <param name="name">The name of the metadata item. This parameter is required and cannot be null.</param>
        /// <param name="value">The value associated with the metadata item. This parameter is optional and can be null.</param>
        /// <param name="shoppingCartItemId">The identifier of the shopping cart item associated with this metadata. This parameter is optional and can
        /// be null.</param>
        public ShoppingCartItemMetadataDto(ShoppingCartItemMetadata metaData)
        {
            Name = metaData.Name;
            Value = metaData.Value;
            ShoppingCartItemId = metaData.ShoppingCartItemId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItemMetadataDto"/> class with the specified name,
        /// value, and optional shopping cart ID.
        /// </summary>
        /// <param name="name">The name of the metadata item. This parameter is required and cannot be null.</param>
        /// <param name="value">The value of the metadata item. This parameter is optional and can be null.</param>
        /// <param name="shoppingCartId">The identifier of the shopping cart associated with this metadata item. This parameter is optional and can
        /// be null.</param>
        public ShoppingCartItemMetadataDto(string name, string? value = null, string? shoppingCartItemId = null)
        {
            Name = name;
            Value = value;
            ShoppingCartItemId = shoppingCartItemId;
        }

        #endregion

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string? Name { get; init; } = null!;

        /// <summary>
        /// Gets the value represented by this instance.
        /// </summary>
        public string? Value { get; init; }

        /// <summary>
        /// Gets the unique identifier for the shopping cart.
        /// </summary>
        public string? ShoppingCartItemId { get; init; }
    }
}
