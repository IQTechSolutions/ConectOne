using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a simple key/value attribute for a product variant.
/// </summary>
public record ProductMetadataDto
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductMetadataDto"/> class.
    /// </summary>
    public ProductMetadataDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductMetadataDto"/> class using the specified product metadata.
    /// </summary>
    /// <param name="attribute">The product metadata used to populate the properties of the DTO. Cannot be null.</param>
    public ProductMetadataDto(ProductMetadata attribute)
    {
        Id = attribute.Id;
        Name = attribute.Name;
        Value = attribute.Value;
        ProductId = attribute.ProductId;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The unique identifier for the product attribute.
    /// </summary>
    public string Id { get; init; } 

    /// <summary>
    /// Gets or sets the name of the attribute (e.g. Color, Size).
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the attribute.
    /// </summary>
    public string? Value { get; init; }

    /// <summary>
    /// The identifier of the associated product.
    /// </summary>
    public string ProductId { get; set; }

    #endregion
}
