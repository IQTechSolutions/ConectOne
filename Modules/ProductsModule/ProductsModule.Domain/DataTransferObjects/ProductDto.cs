using FilingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a data transfer object (DTO) for a product, encapsulating its details, stock information,  settings, and
/// pricing for use in various application layers.
/// </summary>
/// <remarks>This class is designed to provide a comprehensive view of a product's information, including its 
/// identifiers, descriptions, images, stock levels, and pricing. It is typically used to transfer  product data between
/// application layers or to external systems.</remarks>
public record ProductDto
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductDto"/> class.
    /// </summary>
    public ProductDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductDto"/> class using the specified product and pricing
    /// information.
    /// </summary>
    /// <remarks>This constructor maps the properties of the provided <paramref name="product"/> entity to the
    /// corresponding fields in the <see cref="ProductDto"/> object. It also processes the product's images to separate
    /// cover images and other images, and initializes the pricing details using the provided <paramref
    /// name="productPricing"/>.</remarks>
    /// <param name="product">The product entity containing details such as identifiers, descriptions, images, stock levels, and other
    /// metadata.</param>
    /// <param name="productPricing">The pricing information associated with the product.</param>
    public ProductDto(Product product, Price productPricing)
    {
        ProductId = product.Id;

        Sku = product.SKU;
        Name = product.Name;
        DisplayName = product.DisplayName;

        Images = product.Images.Select(c => ImageDto.ToDto(c.Image)).ToList();
        Videos = product.Videos.Select(c => VideoDto.ToDto(c.Video)).ToList();

        ShortDescription = product.ShortDescription;
        Description = product.Description;
        ShopOwnerId = product.ShopOwnerId;
        Rating = product.Rating;
        Tags = product.Tags;
        Featured = product.Featured;
        Active = product.Active;

        Pricing = new PricingDto(product.Pricing);
        Variants = product.Variants.Select(c => new ProductDto(c, c.Pricing)).ToList();
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductDto"/> class using the specified product details.
    /// </summary>
    /// <remarks>This constructor maps the properties of the provided <see cref="ProductFullInfoDto"/> to the
    /// corresponding properties of the <see cref="ProductDto"/> instance. The <paramref name="product"/> parameter must
    /// not be null, and its properties should contain valid data to ensure proper initialization.</remarks>
    /// <param name="product">An instance of <see cref="ProductFullInfoDto"/> containing detailed information about the product.</param>
    public ProductDto(ProductFullInfoDto product)
    {
        ProductId = product.Details.ProductId;
        Sku = product.Details.Sku;
        Name = product.Details.Name;
        DisplayName = product.Details.DisplayName;
        Images = product.Images.ToList();
        Videos = product.Videos.ToList();
        ShortDescription = product.Details.ShortDescription;
        Description = product.Details.Description;
        ShopOwnerId = product.Settings.ShopOwnerId;
        Rating = product.Settings.Rating;
        Tags = product.Settings.Tags == null ? "" : string.Join(",", product.Settings.Tags);
        Featured = product.Settings.Featured;
        Active = product.Settings.Active;
        Pricing = product.Pricing;
        Variants = product.Variants.ToList();
        Images = product.Images;
        Pricing = product.Pricing;
    }

    #endregion

    /// <summary>
    /// Gets the unique identifier for the product.
    /// </summary>
    public string ProductId { get; init; }

    /// <summary>
    /// Gets or sets the identifier of the parent variant, if applicable.
    /// </summary>
    public string? VariantParentId { get; set; }

    /// <summary>
    /// Gets the stock-keeping unit (SKU) identifier for the product.
    /// </summary>
    public string Sku { get; init; }

    /// <summary>
    /// Gets the name associated with the current instance.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the display name associated with the object.
    /// </summary>
    public string? DisplayName { get; init; }

    /// <summary>
    /// Gets or sets the brand name of the product.
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// Gets a brief description or summary of the item.
    /// </summary>
    public string? ShortDescription { get; init; }

    /// <summary>
    /// Gets the description associated with the object.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the unique identifier of the shop owner associated with the current settings.
    /// </summary>
    public string? ShopOwnerId { get; init; }

    /// <summary>
    /// Gets the rating value associated with the entity.
    /// </summary>
    public double Rating { get; init; }

    /// <summary>
    /// Gets the tags associated with the current object.
    /// </summary>
    public string? Tags { get; init; }

    #region Settings

    /// <summary>
    /// Gets a value indicating whether the item is marked as featured.
    /// </summary>
    public bool Featured { get; init; }

    /// <summary>
    /// Gets a value indicating whether the entity is active.
    /// </summary>
    public bool Active { get; init; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether attributes should be displayed.
    /// </summary>
    public bool ShowAttributes { get; set; }

    #endregion

    /// <summary>
    /// Gets the pricing details associated with the current entity.
    /// </summary>
    public PricingDto Pricing { get; init; }

    /// <summary>
    /// Gets or sets the collection of product variants associated with the current product.
    /// </summary>
    public ICollection<ProductDto> Variants { get; set; } = [];

    /// <summary>
    /// Gets the collection of images associated with the current entity.
    /// </summary>
    public ICollection<ImageDto> Images { get; init; } = [];

    /// <summary>
    /// Gets or sets the collection of video files to be displayed.
    /// </summary>
    public List<VideoDto> Videos { get; set; } = [];

    /// <summary>
    /// Gets the collection of categories associated with the current entity.
    /// </summary>
    public ICollection<CategoryDto> Categories { get; init; } = [];

    /// <summary>
    /// Gets the attribute name/value pairs associated with the product.
    /// </summary>
    public ICollection<ProductAttributeDto> Attributes { get; init; } = [];
}
