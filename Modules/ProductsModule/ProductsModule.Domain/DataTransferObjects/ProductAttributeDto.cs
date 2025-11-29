namespace ProductsModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a view model for a product attribute.
/// This view model holds information about a specific attribute associated with a product,
/// including its identifier, name, optional description, pricing details, and associated product reference.
/// </summary>
public record ProductAttributeDto
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductAttributeDto"/> class.
    /// </summary>
    public ProductAttributeDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductAttributeDto"/> class using the specified product details.
    /// </summary>
    /// <remarks>This constructor maps the properties of the provided <see cref="ProductDto"/> to the
    /// corresponding fields of the <see cref="ProductAttributeDto"/>. Ensure that the <paramref name="product"/>
    /// parameter contains valid data, as no additional validation is performed.</remarks>
    /// <param name="product">The product data used to populate the attributes of the DTO. Cannot be <see langword="null"/>.</param>
    public ProductAttributeDto(ProductDto product)
    {
        Id = product.ProductId;
        Name = product.Name;
        Sku = product.Sku;
        Description = product.Description;
        Price = product.Pricing.CostExcl;
    }

    #endregion

    /// <summary>
    /// Gets or sets the unique identifier for the product attribute.
    /// The identifier is automatically generated as a new GUID in string format.
    /// </summary>
    public string Id { get; init; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the product attribute.
    /// This property is required and should not be null.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Gets or sets the stock-keeping unit (SKU) for the product attribute.
    /// This property is required and should not be null.
    /// </summary>
    public string Sku { get; init; } = null!;

    /// <summary>
    /// Gets or sets an optional description for the product attribute.
    /// This may contain additional information about the attribute.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the price of the item. The value is optional and may be null if the price is not set.
    /// </summary>
    public double? Price { get; init; }

    /// <summary>
    /// Gets or sets the identifier for the associated product.
    /// This serves as a foreign key linking the attribute to its product.
    /// </summary>
    public string? ProductId { get; init; } = null!;
}
