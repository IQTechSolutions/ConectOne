using FilingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.DataTransferObjects;
using ProductsModule.Domain.DataTransferObjects;

namespace ProductsModule.Application.ViewModels
{
	/// <summary>
	/// Represents a view model for a product, encapsulating its details, pricing, settings, inventory, and other related
	/// information.
	/// </summary>
	/// <remarks>This class is designed to provide a comprehensive representation of a product for use in UI or API
	/// layers.  It includes properties for product metadata, pricing, inventory, and associated entities such as brands,
	/// categories, and suppliers. The class supports initialization with default values or from various data transfer
	/// objects (DTOs) to facilitate mapping from backend data sources.</remarks>
    public class ProductViewModel
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ProductViewModel"/> class with default values.
		/// </summary>
		/// <remarks>This constructor generates a new unique identifier for the <see cref="ProductId"/> property and
		/// initializes the <see cref="Details"/>, <see cref="Images"/>, <see cref="Description"/>,  and <see cref="Pricing"/>
		/// properties with default values. The <see cref="Call.Details.CoverImageUrl"/>  is set to a placeholder image
		/// path.</remarks>
		public ProductViewModel() 
		{
			ProductId = Guid.NewGuid().ToString();

			Details = new ProductDetailsViewModel()
			{
				SerialNr = string.Empty,
				Barcode = string.Empty,
				Sku = string.Empty,

				DisplayName = string.Empty,
				Name = string.Empty,

				CoverImageUrl = "/_content/Products.Mvc/images/NoImage.jpg"
			};
			Images = [];

			Description = string.Empty;

			Pricing = new PricingViewModel();
            Settings = new ProductSettingsViewModel();
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="product"></param>
		/// <param name="vatRate"></param>
		public ProductViewModel(ProductFullInfoDto product, double vatRate = 15) 
		{
			ProductId = product.Details.ProductId;

			Details = new ProductDetailsViewModel()
			{
				Sku = product.Details.Sku,
				DisplayName = product.Details.DisplayName,
				Name = product.Details.Name,
				ShortDescription= product.Details.ShortDescription,		
				CoverImageUrl = product.Details.CoverImageUrl
			};
			Images = product.Images;
            Videos = product.Videos;
			Active= product.Settings.Active;
			Featured = product.Settings.Featured;
			Description= product.Details.Description;
			Rating= product.Settings.Rating;  
			Created = DateTime.Now;
			Tags = product.Settings.Tags?.Split(',');
			Pricing = new PricingViewModel(product.Pricing);
            Settings = new ProductSettingsViewModel() { ShopOwnerId = product.Settings.ShopOwnerId };
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ProductViewModel"/> class using the specified product data.
		/// </summary>
		/// <remarks>This constructor maps the properties of the provided <see cref="ProductDto"/> to the
		/// corresponding properties of the <see cref="ProductViewModel"/>. It initializes nested view models for product
		/// details, pricing, and settings.</remarks>
		/// <param name="product">The product data transfer object (<see cref="ProductDto"/>) containing the information used to populate the view
		/// model.</param>
		public ProductViewModel(ProductDto product)
		{
			ProductId = product.ProductId;

			Details = new ProductDetailsViewModel()
			{
				Sku = product.Sku,
				DisplayName = product.DisplayName,
				Name = product.Name,
                ShortDescription= product.ShortDescription
			};
			Images = product.Images;
            Videos = product.Videos;

			Active= product.Active;
			Featured = product.Featured;

			Description= product.Description;

			Rating= product.Rating;
			Created = DateTime.Now;

			Tags = product.Tags?.Split(',');

            Pricing = new PricingViewModel(product.Pricing);
			Settings = new ProductSettingsViewModel(product);
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public string ProductId { get; set; } = Guid.NewGuid().ToString();

		/// <summary>
		/// Gets or sets the details of the product.
		/// </summary>
		public ProductDetailsViewModel Details { get; set; }

		/// <summary>
		/// Gets or sets the pricing details for the product.
		/// </summary>
		public PricingViewModel Pricing { get; set; }

		/// <summary>
		/// Gets or sets the settings for the product configuration.
		/// </summary>
		public ProductSettingsViewModel Settings { get; set; }

		/// <summary>
		/// Gets or sets the inventory details for the product.
		/// </summary>
		public ProductInventoryViewModel Inventory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; } = true;

		/// <summary>
		/// Gets or sets a value indicating whether the item is marked as featured.
		/// </summary>
		public bool Featured { get; set; }

		/// <summary>
		/// Gets or sets the description associated with the object.
		/// </summary>
		public string Description { get; set; }

        /// <summary>
        /// Gets or sets the rating value.
        /// </summary>
        public double Rating { get; set; }

		/// <summary>
		/// Gets or sets the date and time when the entity was created.
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets the collection of categories to be displayed or processed.
		/// </summary>
		public IEnumerable<CategoryDto> Categories { get; set; }

		/// <summary>
		/// Gets or sets the collection of tags associated with the current object.
		/// </summary>
		public IEnumerable<string> Tags { get; set; } = new List<string>();

		/// <summary>
		/// Gets or sets the collection of image file paths.
		/// </summary>
		public ICollection<ImageDto> Images { get; set; }

        /// <summary>
        /// Gets or sets the collection of video files to be displayed.
        /// </summary>
        public List<VideoDto> Videos { get; set; } = [];

        #endregion

        #region Methods

		/// <summary>
		/// Converts the current product instance to a <see cref="ProductDto"/> representation.
		/// </summary>
		/// <remarks>This method maps the properties of the product, including details, images, videos, and pricing, 
		/// to a new <see cref="ProductDto"/> object. Collections such as images and videos are converted to lists,  and tags
		/// are joined into a comma-separated string.</remarks>
		/// <returns>A <see cref="ProductDto"/> object containing the mapped data from the current product instance.</returns>
        public ProductDto ToDto()
        {
            return new ProductDto()
            {
                ProductId = ProductId,
                Sku = Details.Sku,
                Name = Details.Name,
                DisplayName = Details.DisplayName,
                Images = Images.ToList(),
                Videos = Videos.ToList(),
                ShortDescription = Details.ShortDescription,
                Description = Description,
				ShopOwnerId = Settings.ShopOwnerId,
                Rating = Rating,
                Tags = Tags == null ? "" : string.Join(",", Tags),
                Featured = Featured,
                Active = Active,
                Pricing = Pricing.ToDto()
            };
        }

        #endregion
    }
}
