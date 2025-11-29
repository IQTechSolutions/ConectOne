using FilingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a comprehensive data transfer object (DTO) containing detailed information about a product,  including
/// its details, inventory, pricing, settings, and associated metadata.
/// </summary>
/// <remarks>This DTO is designed to aggregate all relevant product information into a single object for use in 
/// scenarios such as API responses or inter-service communication. It includes details about the product's  brand,
/// category, inventory, pricing, and settings, as well as counts of related entities such as combo  categories, bundled
/// products, and bundled services.</remarks>
public record ProductFullInfoDto
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductFullInfoDto"/> class with default values.
    /// </summary>
    /// <remarks>This constructor initializes the <see cref="Details"/>, <see cref="Inventory"/>, <see
    /// cref="Pricing"/>,  and <see cref="Settings"/> properties with their respective default instances.</remarks>
    public ProductFullInfoDto()
    {
        Details = new ProductDetailsDto();
        Inventory = new ProductInventoryDto();
        Pricing = new PricingDto();
        Settings = new ProductSettingsDto();
        Variants = new List<ProductDto>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductFullInfoDto"/> class, providing detailed information about a
    /// product, including its inventory, settings, pricing, brand, category, and associated counts for combos, bundled
    /// products, and services.
    /// </summary>
    /// <remarks>This constructor aggregates data from the provided <paramref name="product"/> and <paramref
    /// name="productPricing"/> to populate the properties of the <see cref="ProductFullInfoDto"/>. It extracts the
    /// first available brand and category names, as well as counts of combos, bundled products, and bundled services
    /// associated with the product.</remarks>
    /// <param name="product">The <see cref="Product"/> instance containing the product's details, inventory, settings, brand, category, and
    /// associated items.</param>
    /// <param name="productPricing">The <see cref="Price"/> instance containing the pricing information for the product.</param>
    public ProductFullInfoDto(Product product, Price productPricing)
    {
        Details = new ProductDetailsDto(product);
        Inventory = new ProductInventoryDto(product);
        Settings = new ProductSettingsDto(product);
        Pricing = new PricingDto(productPricing);

        Images = product.Images.Select(c => ImageDto.ToDto(c)).ToList();
        Variants = product.Variants.Select(v => new ProductDto(v, v.Pricing)).ToList();
        MetaDataCollection = product.MetaDataCollection.Select(v => new ProductMetadataDto(v)).ToList();
        Categories = product.Categories.Select(c => CategoryDto.ToCategoryDto(c.Category)).ToList();
    }

    #endregion

    /// <summary>
    /// Gets or sets the details of the product.
    /// </summary>
    public ProductDetailsDto Details { get; set; }

    /// <summary>
    /// Gets or sets the inventory details for the product.
    /// </summary>
    public ProductInventoryDto Inventory { get; set; }

    /// <summary>
    /// Gets the pricing details associated with the current entity.
    /// </summary>
    public PricingDto Pricing { get; init; }

    /// <summary>
    /// Gets or sets the settings associated with the product.
    /// </summary>
    public ProductSettingsDto Settings { get; set; }  

    /// <summary>
    /// Gets or sets the brand name of the product.
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// Gets the collection of variant products associated with this product.
    /// </summary>
    public List<ProductDto> Variants { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of product metadata associated with this instance.
    /// </summary>
    public List<ProductMetadataDto> MetaDataCollection { get; set; } = [];

    /// <summary>
    /// Gets the collection of categories associated with this product.
    /// </summary>
    public List<CategoryDto> Categories { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of images associated with the entity.
    /// </summary>
    public List<ImageDto> Images { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of video files to be displayed.
    /// </summary>
    public List<VideoDto> Videos { get; set; } = [];
}
