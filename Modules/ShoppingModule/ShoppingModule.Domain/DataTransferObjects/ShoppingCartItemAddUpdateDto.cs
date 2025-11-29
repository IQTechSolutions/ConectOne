using ProductsModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Entities;
using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data transfer object for adding or updating items in the shopping cart.
    /// </summary>
    public record ShoppingCartItemAddUpdateDto
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ShoppingCartItemAddUpdateDto() { }

        /// <summary>
        /// Constructor for adding a standard item to the shopping cart.
        /// </summary>
        /// <param name="shoppingCartId">The shopping cart ID.</param>
        /// <param name="lodgingId">The lodging ID.</param>
        /// <param name="uniquePartnerId">The unique partner ID.</param>
        /// <param name="packageId">The package ID.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="roomTypeId">The room type ID.</param>
        /// <param name="roomName">The room name.</param>
        /// <param name="pricing">The pricing details.</param>
        /// <param name="qty">The quantity.</param>
        /// <param name="adults">The number of adults.</param>
        /// <param name="kidsGroup1">The number of kids in group 1.</param>
        /// <param name="kidsGroup2">The number of kids in group 2.</param>
        /// <param name="shoppingCartItemType">The type of the shopping cart item.</param>
        public ShoppingCartItemAddUpdateDto(string shoppingCartId, string lodgingId, string uniquePartnerId, int packageId, DateTime startDate, DateTime endDate, string roomTypeId, string roomName, PricingDto pricing, double qty = 1, int adults = 0, int kidsGroup1 = 0, int kidsGroup2 = 0, ShoppingCartItemType shoppingCartItemType = ShoppingCartItemType.StandardItem)
        {
            Qty = qty;

            ShoppingCartId = shoppingCartId;
            ShoppingCartItemType = shoppingCartItemType;

            LodgingId = lodgingId;
            PackageId = packageId;
            ProductId = roomTypeId;
            UniquePartnerId = uniquePartnerId;

            ProductName = roomName;
            StartDate = startDate;
            EndDate = endDate;
            Adults = adults;
            KidsGroup1 = kidsGroup1;
            KidsGroup2 = kidsGroup2;

            Pricing = pricing;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItemAddUpdateDto"/> class with the specified
        /// shopping cart details, product information, pricing, quantity, and item type.
        /// </summary>
        /// <param name="shoppingCartId">The unique identifier of the shopping cart to which the item belongs. Cannot be <see langword="null"/> or
        /// empty.</param>
        /// <param name="productId">The unique identifier of the product being added or updated. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="productName">The name of the product being added or updated. Cannot be <see langword="null"/> or empty.</param>
        /// <param name="pricing">The pricing details for the product. Cannot be <see langword="null"/>.</param>
        /// <param name="qty">The quantity of the product to add or update. Must be greater than 0. The default value is 1.</param>
        /// <param name="shoppingCartItemType">The type of the shopping cart item. The default value is <see cref="ShoppingCartItemType.StandardItem"/>.</param>
        public ShoppingCartItemAddUpdateDto(string shoppingCartId, string productId, string thumbnailUrl, string productName, string shortDescription, string sku, PricingDto pricing, double qty = 1, ShoppingCartItemType shoppingCartItemType = ShoppingCartItemType.StandardItem)
        {
            Qty = qty;
            ThumbnailUrl = thumbnailUrl;
            ShoppingCartId = shoppingCartId;
            ShoppingCartItemType = shoppingCartItemType;
            ProductId = productId;
            ProductName = productName;
            ShortDescription = shortDescription;
            Sku = sku;
            Barcode = "";
            SerialNumber = "";
            Pricing = pricing;
            Qty = qty;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ShoppingCartItemId { get; init; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the unique identifier for the shopping cart.
        /// </summary>
        public string ShoppingCartId { get; init; } = null!;

        /// <summary>
        /// Gets the unique identifier for the lodging.
        /// </summary>
        public string? LodgingId { get; init; }

        /// <summary>
        /// Gets the unique identifier for the package.
        /// </summary>
        public int PackageId { get; init; }

        /// <summary>
        /// Gets the unique identifier for the product.
        /// </summary>
        public string ProductId { get; init; } = null!;

        /// <summary>
        /// Gets the unique identifier for a partner.
        /// </summary>
        public string? UniquePartnerId { get; init; }

        /// <summary>
        /// Gets the identifier of the parent entity, if one exists.
        /// </summary>
        public string? ParentId { get; init; }

        /// <summary>
        /// Gets the URL of the thumbnail image associated with the object.
        /// </summary>
        public string? ThumbnailUrl { get; init; }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        public string ProductName { get; init; } = null!;

        /// <summary>
        /// Gets a brief description or summary of the item.
        /// </summary>
        public string ShortDescription { get; init; } = null!;

        /// <summary>
        /// Gets the quantity associated with the current instance.
        /// </summary>
        public double Qty { get; init; }

        /// <summary>
        /// Gets or sets the start date of the event or process.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event or operation.
        /// </summary>
        /// <remarks>Ensure that the <see cref="EndDate"/> is set to a value later than the start date, if
        /// applicable,  to maintain logical consistency.</remarks>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the number of adults associated with the current context.
        /// </summary>
        public int Adults { get; set; }

        /// <summary>
        /// Gets or sets the number of children in the first group.
        /// </summary>
        public int KidsGroup1 { get; set; }

        /// <summary>
        /// Gets or sets the number of children in the second group.
        /// </summary>
        public int KidsGroup2 { get; set; }

        /// <summary>
        /// Gets the serial number associated with the object.
        /// </summary>
        public string? SerialNumber { get; init; }

        /// <summary>
        /// Gets the barcode associated with the item.
        /// </summary>
        public string? Barcode { get; init; }

        /// <summary>
        /// Gets the stock-keeping unit (SKU) identifier for the product.
        /// </summary>
        public string? Sku { get; init; }

        /// <summary>
        /// Gets the pricing details associated with the current entity.
        /// </summary>
        public PricingDto Pricing { get; init; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the rate.
        /// </summary>
        public int RateId { get; set; }

        /// <summary>
        /// Gets the type of the shopping cart item.
        /// </summary>
        public ShoppingCartItemType ShoppingCartItemType { get; init; } = ShoppingCartItemType.StandardItem;

        #region Collection

        /// <summary>
        /// Gets the metadata associated with the shopping cart items.
        /// </summary>
        public List<ShoppingCartItemMetadataDto> Metadata { get; init; } = new();

        #endregion

        /// <summary>
        /// Converts the current object to a <see cref="ShoppingCartItem"/> instance.
        /// </summary>
        /// <remarks>This method maps the properties of the current object to a new <see
        /// cref="ShoppingCartItem"/> object. It ensures that all relevant fields are transferred, including pricing
        /// details, product information,  and associated identifiers. If <c>ThumbnailUrl</c> is null or empty, it
        /// defaults to an empty string.</remarks>
        /// <returns>A <see cref="ShoppingCartItem"/> instance populated with the corresponding values from the current object.</returns>
        public ShoppingCartItem ToShoppingCartItem()
        {
            return new ShoppingCartItem()
            {
                Id = ShoppingCartItemId,
                Qty = Qty,
                ShoppingCartId = ShoppingCartId,
                ShoppingCartItemType = ShoppingCartItemType,
                LodgingId = LodgingId,
                UniquePartnerId = UniquePartnerId,
                PackageId = Convert.ToInt32(PackageId),
                ProductId = ProductId,
                ThumbnailUrl = string.IsNullOrEmpty(ThumbnailUrl) ? "" : ThumbnailUrl,
                ProductName = ProductName,
                ShortDescription = ShortDescription,
                StartDate = StartDate,
                EndDate = EndDate,
                Adults = Adults,
                KidsGroup1 = KidsGroup1,
                KidsGroup2 = KidsGroup2,
                PriceExcl = Pricing.CostExcl,
                PriceIncl = Pricing.PriceIncl,
                Vat = Pricing.Vat,
                Discount = 0,
                Commission = 0,
                PriceOld = Pricing.CostExcl,
                RateId = RateId,
                Metadata = Metadata.Select(c=> new ShoppingCartItemMetadata(c)).ToList()
            };
        }
    }
}