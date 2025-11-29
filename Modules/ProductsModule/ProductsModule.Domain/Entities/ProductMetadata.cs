using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace ProductsModule.Domain.Entities;

/// <summary>
/// Represents a key/value attribute associated with a product, such as
/// color or size.
/// </summary>
public class ProductMetadata : EntityBase<string>
{
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
    [ForeignKey(nameof(Product))] public string? ProductId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the product that owns this attribute.
    /// </summary>
    public Product? Product { get; set; } = null!;
}
