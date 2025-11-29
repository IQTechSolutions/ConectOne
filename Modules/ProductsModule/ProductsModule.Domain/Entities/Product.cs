using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using ConectOne.Domain.Extensions;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using ProductsModule.Domain.DataTransferObjects;

namespace ProductsModule.Domain.Entities
{
    /// <summary>
    /// Represents a product with various attributes, settings, quantities, pricing, and relational properties.
    /// </summary>
    /// <remarks>The <see cref="Product"/> class provides a comprehensive representation of a product, including its
    /// name, descriptions, identifiers (e.g., SKU, barcode), dimensions, stock levels, pricing, and associated 
    /// categories, brands, and suppliers. It also includes settings for catalog visibility, featured status,  and whether
    /// the product is active or used. Additionally, the class supports relationships with bundled  products, services, and
    /// packaging.</remarks>
    public class Product : FileCollection<Product, string>
    {
        private string? _displayName;
        private string? _shortDescription;
        private string? _slug;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class using the specified product data transfer
        /// object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="ProductDto"/> to the
        /// corresponding properties of the <see cref="Product"/> instance. Ensure that the <paramref name="product"/>
        /// parameter is not null and contains valid data to avoid unexpected behavior.</remarks>
        /// <param name="product">The data transfer object containing the product details.  The properties of the <see cref="Product"/>
        /// instance will be initialized based on the values in this object.</param>
        public Product(ProductDto product)
        {
            Id = product.ProductId;
            Name = product.Name;
            DisplayName = product.DisplayName;
            SKU = product.Sku;
            ShortDescription = ShortDescription;
            Description = product.Description;
            ShopOwnerId = product.ShopOwnerId;
            Rating = product.Rating;
            Featured = product.Featured;
            Active = product.Active;
            Tags = product.Tags;
            VariantParentId = product.VariantParentId;
        }

        #endregion
        
        #region Public Properties

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the display name of the entity.
        /// </summary>
        public string? DisplayName
        {
            get => string.IsNullOrEmpty(_displayName) ? Name : _displayName;
            set => _displayName = value;
        }

        /// <summary>
        /// Gets or sets a slug that can be used in URLs. If not specified, it is generated from <see cref="Name"/>.
        /// </summary>
        public string? Slug
        {
            get => string.IsNullOrWhiteSpace(_slug) ? GenerateSlug(Name) : _slug!;
            set => _slug = value;
        }

        /// <summary>
        /// Gets or sets the stock-keeping unit (SKU) identifier for the product.
        /// </summary>
        public string? SKU { get; set; }

        /// <summary>
        /// Gets or sets a brief description of the content.
        /// </summary>
        public string? ShortDescription
        {
            get => string.IsNullOrEmpty(_shortDescription)
                ? Description.TruncateLongString(50).HtmlToPlainText()
                : _shortDescription;
            set => _shortDescription = value;
        }

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; set; }

        #region Settings

        /// <summary>
        /// Gets or sets the unique identifier of the shop owner associated with the current settings.
        /// </summary>
        public string? ShopOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the rating value.
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is marked as featured.
        /// </summary>
        public bool Featured { get; set; } 

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets a comma-separated list of tags associated with the entity.
        /// </summary>
        public string? Tags { get; set; }

        #endregion

        #endregion

        #region One-To-One Properties

        /// <summary>
        /// Gets or sets the pricing details for the current context.
        /// </summary>
        public Price Pricing { get; set; } 

        #endregion

        #region Attributes

        /// <summary>
        /// Gets or sets the identifier of the parent product when this product represents
        /// an attribute or variant of another product.
        /// </summary>
        [ForeignKey(nameof(VariantParent))] public string? VariantParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent product associated with this attribute product.
        /// </summary>
        public Product? VariantParent { get; set; }

        /// <summary>
        /// Sellable attribute products (variants) linked to this product.
        /// </summary>
        /// <remarks>
        /// Variants are child products representing options such as size, color or brand and can be priced independently.
        /// </remarks>
        [InverseProperty(nameof(VariantParent))]
        public virtual ICollection<Product> Variants { get; set; } = new List<Product>();

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the collection of categories associated with the product.
        /// </summary>
        /// <remarks>Use this property to manage the categories associated with the product.  The default value is an
        /// empty collection.</remarks>
        public virtual ICollection<EntityCategory<Product>> Categories { get; set; } =
            new List<EntityCategory<Product>>();

        /// <summary>
        /// Gets or sets the collection of custom attributes associated with the product.
        /// </summary>
        public virtual ICollection<ProductMetadata> MetaDataCollection { get; set; } = new List<ProductMetadata>();

        #endregion

        #region ReadOnly

        /// <summary>
        /// Gets a value indicating whether the item was created within the last 30 days.
        /// </summary>
        public bool RecentItem
        {
            get
            {
                if (CreatedOn != null && CreatedOn >= CreatedOn.Value.AddDays(-30))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the truncated version of the name, limited to 35 characters.
        /// </summary>
        public string ShortName => Name.TruncateLongString(35);

        /// <summary>
        /// Generates a URL-friendly slug from the specified string value.
        /// </summary>
        /// <remarks>The method removes non-alphanumeric characters (except spaces and hyphens), replaces
        /// multiple spaces with a single space, trims leading and trailing spaces,  and truncates the result to a
        /// maximum of 45 characters. Spaces are replaced with hyphens to form the final slug.</remarks>
        /// <param name="value">The input string to convert into a slug. Cannot be null or whitespace.</param>
        /// <returns>A lowercase, hyphen-separated string suitable for use in URLs. Returns an empty string if the input is null,
        /// empty, or consists only of whitespace.</returns>
        private static string GenerateSlug(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.ToLowerInvariant();
            value = Regex.Replace(value, @"[^a-z0-9\s-]", string.Empty);
            value = Regex.Replace(value, @"\s+", " ").Trim();
            if (value.Length > 45)
                value = value.Substring(0, 45).Trim();
            return value.Replace(" ", "-");
        }

        #endregion
    }
}
