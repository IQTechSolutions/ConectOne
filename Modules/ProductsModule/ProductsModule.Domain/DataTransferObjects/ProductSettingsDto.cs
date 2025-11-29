using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.DataTransferObjects
{
	/// <summary>
	/// The data transfer object used to transfer information about a specific product's settings
	/// </summary>
    public record ProductSettingsDto
    {
		#region Constructors

		/// <summary>
		/// Parameterless Construction
		/// </summary>
		public ProductSettingsDto() { }

		/// <summary>
		/// Entity Constructor
		/// </summary>
		/// <param name="product">The entity <see cref="Product"/> used to create this data transfer object</param>
        public ProductSettingsDto(Product product) 
		{
            ProductId = product.Id;
            ShopOwnerId = product.ShopOwnerId;
            Featured = product.Featured;
            Active = product.Active;
            Vatable = product.Pricing.Vatable;
            Tags = product.Tags;
        }

		#endregion

		/// <summary>
		/// The identity of the product that owns this settings
		/// </summary>
		public string ProductId { get; init; }

        /// <summary>
        /// Gets or sets the unique identifier of the shop owner associated with the current settings.
        /// </summary>
        public string? ShopOwnerId { get; set; }

        /// <summary>
        /// Flag to indicate if the product is vatable
        /// </summary>
        public bool Vatable { get; init; }

		/// <summary>
		/// Flag to indicate if the product is a featured item
		/// </summary>
		public bool Featured { get; init; }

        /// <summary>
        /// Flag to indicate if the product is an active item
        /// </summary>
        public bool Active { get; init; } = true;

		/// <summary>
		/// Tags that identity with this product, usually used for marketing purposes
		/// </summary>
		public string Tags { get; set; }

        /// <summary>
        /// The rating of the product calculated by the reviews
        /// </summary>
        public double Rating { get; init; }
    }
}
