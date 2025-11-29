using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.RequestFeatures;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductsModule.Application.ViewModels
{
    /// <summary>
    /// Represents the settings and metadata for a product, including dimensions, tags, and various flags indicating the
    /// product's state and features.
    /// </summary>
    /// <remarks>This view model is used to encapsulate product-related settings and parameters for display or
    /// manipulation in a user interface. It supports initialization from multiple product data transfer objects (DTOs)
    /// and includes properties for product dimensions, tags, and state flags such as whether the product is active,
    /// featured, or used.</remarks>
    public class ProductSettingsViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductSettingsViewModel"/> class.
        /// </summary>
        public ProductSettingsViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductSettingsViewModel"/> class with the specified product
        /// details and optional parameters.
        /// </summary>
        /// <param name="product">The product details and settings used to initialize the view model. Cannot be null.</param>
        /// <param name="parameters">Optional parameters for additional product configuration. Can be null.</param>
        public ProductSettingsViewModel(ProductFullInfoDto product, ProductsParameters? parameters = null)
        {
            ProductId = product.Details.ProductId;
            ShopOwnerId = product.Settings.ShopOwnerId;
            ListingItem = false;
            Featureddd = product.Settings.Featured;
            Active= product.Settings.Active;
            Tags = product.Settings.Tags;

            Parameters = parameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductSettingsViewModel"/> class with the specified product
        /// details and optional parameters.
        /// </summary>
        /// <remarks>This constructor initializes the view model with the provided product details,
        /// including its ID, name, dimensions, tags, and other attributes. If <paramref name="parameters"/> is
        /// provided, it will be used to further configure the view model.</remarks>
        /// <param name="product">The product data transfer object containing details about the product. Cannot be null.</param>
        /// <param name="parameters">Optional parameters for configuring the product settings. Can be null.</param>
        public ProductSettingsViewModel(ProductDto product, ProductsParameters? parameters = null)
        {
            ProductId = product.ProductId;
            ShopOwnerId = product.ShopOwnerId;
            ProductName = product.Name;
            ListingItem = false;
            Featureddd = product.Featured;
            Active= product.Active;
            Tags = product.Tags;

			Parameters = parameters;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parameters used to configure product-related operations.
        /// </summary>
        public ProductsParameters? Parameters { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
		public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the shop owner associated with the current settings.
        /// </summary>
        public string? ShopOwnerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [DisplayName("Product Name")] public string? ProductName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is listed.
        /// </summary>
        [DisplayName("Listing Item")] public bool ListingItem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
        [DisplayName("Featured")] public bool Featureddd { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
		public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the item is subject to value-added tax (VAT).
        /// </summary>
        public bool Vatable { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the product is used.
        /// </summary>
        [DisplayName("Used Product")] public bool UsedProduct { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pre-orders are allowed.
        /// </summary>
        public bool AllowPreOrders { get; set; } = true;

        /// <summary>
        /// Gets or sets the weight of the object.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the width of the object.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the object.
        /// </summary>
		public double Height { get; set; }

        /// <summary>
        /// Gets or sets the length of the object.
        /// </summary>
		public double Length { get; set; }

        /// <summary>
        /// Gets or sets the tags associated with the entity.
        /// </summary>
        [Required(ErrorMessage = "Tags is required!")] public string Tags { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of <see cref="ProductSettingsViewModel"/> to a <see cref="ProductSettingsDto"/>.
        /// </summary>
        /// <returns>A <see cref="ProductSettingsDto"/> object that represents the current product settings.</returns>
        public ProductSettingsDto ToDto()
        {
            return new ProductSettingsDto
            {
                ProductId = ProductId,
                ShopOwnerId = ShopOwnerId,
                Featured = Featureddd,
                Active = Active,
                Vatable = Vatable,
                Tags = Tags
            };
        }

        #endregion
    }
}
