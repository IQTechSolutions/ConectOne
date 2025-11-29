using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using ShoppingModule.Domain.DataTransferObjects;

namespace ShoppingModule.Domain.Entities;

/// <summary>
/// Represents a key/value attribute associated with a product, such as
/// color or size.
/// </summary>
public class ShoppingCartItemMetadata : EntityBase<string>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ShoppingCartItemMetadata"/> class.
    /// </summary>
    public ShoppingCartItemMetadata() { }

    /// <summary>
    /// Represents metadata associated with a shopping cart item.
    /// </summary>
    /// <remarks>This class is typically used to store additional information about a shopping cart item, such
    /// as custom attributes or tags.</remarks>
    /// <param name="name">The name of the metadata. This value cannot be <see langword="null"/> or empty.</param>
    /// <param name="value">The value of the metadata. This parameter is optional and can be <see langword="null"/>.</param>
    /// <param name="shoppingCartItemId">The identifier of the shopping cart item to which this metadata belongs. This parameter is optional and can be
    /// <see langword="null"/>.</param>
    public ShoppingCartItemMetadata(ShoppingCartItemMetadataDto dto)
    {
        Name = dto.Name;
        Value = dto.Value;
        ShoppingCartItemId = dto.ShoppingCartItemId;
    }

    #endregion


    /// <summary>
    /// Gets or sets the name of the attribute (e.g. Color, Size).
    /// </summary>
    [Required, MaxLength(100)] public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the value of the attribute.
    /// </summary>
    [MaxLength(200)] public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated product.
    /// </summary>
    [ForeignKey(nameof(ShoppingCartItem))] public string? ShoppingCartItemId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product that owns this attribute.
    /// </summary>
    public ShoppingCartItem? ShoppingCartItem { get; set; } = null!;
}
