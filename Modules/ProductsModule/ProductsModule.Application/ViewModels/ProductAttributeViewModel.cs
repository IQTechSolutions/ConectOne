using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ProductsModule.Domain.DataTransferObjects;

namespace ProductsModule.Application.ViewModels;

/// <summary>
/// Represents a view model for a product attribute.
/// This view model holds information about a specific attribute associated with a product,
/// including its identifier, name, optional description, pricing details, and associated product reference.
/// </summary>
public class ProductAttributeViewModel
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductAttributeViewModel"/> class.
    /// This constructor creates an empty view model instance.
    /// </summary>
    public ProductAttributeViewModel() { }

    /// <summary>
        /// Initializes a new instance of the <see cref="ProductAttributeViewModel"/> class using data from a <see cref="ProductDto"/>.
        /// </summary>
        /// <param name="product">
        /// The <see cref="ProductDto"/> instance containing detailed product information such as identifier, name, SKU, description,
        /// pricing details (including cost exclusion), and price inheritance settings. This data is used to populate the view model 
        /// for display or further processing.
        /// </param>
    public ProductAttributeViewModel(ProductDto product)
    {
        Id = product.ProductId;
        Name = product.Name;
        Sku = product.Sku;
        Description = product.Description;
        Price = new PricingViewModel(product.Pricing);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the unique identifier for the product attribute.
    /// The identifier is automatically generated as a new GUID in string format.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the prefix to be used for names.
    /// </summary>
    public string NamePrefix { get; set; }

    /// <summary>
    /// Gets or sets the prefix used to identify stock-keeping units (SKUs).
    /// </summary>
    public string SkuPrefix { get; set; }

    /// <summary>
    /// Gets or sets the name of the product attribute.
    /// This property is required and should not be null.
    /// </summary>
    [Required(ErrorMessage = "Attribute Name is required")]public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the stock-keeping unit (SKU) for the product attribute.
    /// This property is required and should not be null.
    /// </summary>
    [Required(ErrorMessage = "SKU is required"), DisplayName("SKU")] public string Sku { get; set; } = null!;

    /// <summary>
    /// Gets or sets an optional description for the product attribute.
    /// This may contain additional information about the attribute.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the pricing details for the current item.
    /// </summary>
    public PricingViewModel? Price { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the associated product.
    /// This serves as a foreign key linking the attribute to its product.
    /// </summary>
    public string? ProductId { get; set; } = null!;

    #endregion

    #region Methods

    /// <summary>
    /// Converts the current product instance to a <see cref="ProductDto"/> representation.
    /// </summary>
    /// <param name="parentProductId">The identifier of the parent product, used to set the <see cref="ProductDto.VariantParentId"/> property.</param>
    /// <returns>A <see cref="ProductDto"/> object containing the mapped properties of the current product.</returns>
    public ProductDto ToDto(string parentProductId)
    {
        return new ProductDto()
        {
            ProductId = Id,
            Sku = SkuPrefix + Sku,
            Name = NamePrefix + Name,
            VariantParentId = parentProductId,
            DisplayName = NamePrefix + Name,
            Description = Description,
            Pricing = Price.ToDto(),
        };
    }

    #endregion
}
