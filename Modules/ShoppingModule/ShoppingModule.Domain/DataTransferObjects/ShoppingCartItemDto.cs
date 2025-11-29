using ShoppingModule.Domain.Entities;
using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for an item in a shopping cart, containing details about the product,
    /// pricing, and associated metadata.
    /// </summary>
    /// <remarks>This DTO is used to transfer shopping cart item data between layers of the application. It
    /// includes information such as product identifiers,  quantities, pricing details, and other metadata relevant to
    /// the shopping cart item. The object is immutable for most properties, ensuring  consistency when used in
    /// read-only scenarios.</remarks>
    public record ShoppingCartItemDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItemDto"/> class.
        /// </summary>
        public ShoppingCartItemDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItemDto"/> class using the specified <see
        /// cref="ShoppingCartItem"/>.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="ShoppingCartItem"/>
        /// to the corresponding properties of the <see cref="ShoppingCartItemDto"/>. It ensures that all relevant data
        /// from the shopping cart item is transferred to the DTO for further use, such as in API responses or other
        /// layers of the application.</remarks>
        /// <param name="cartItem">The <see cref="ShoppingCartItem"/> instance containing the data to populate the DTO.  This parameter cannot
        /// be <see langword="null"/>.</param>
        public ShoppingCartItemDto(ShoppingCartItem cartItem) 
        {
            CartItemId = cartItem.Id;
            ParentId = cartItem.ParentId;
            Qty = cartItem.Qty;
            Adults = cartItem.Adults;
            ThumbnailUrl = cartItem.ThumbnailUrl;
            KidsGroup1 = cartItem.KidsGroup1;
            KidsGroup2 = cartItem.KidsGroup2;
            LodgingId = cartItem.LodgingId;
            PackageId = Convert.ToInt32(cartItem.PackageId);
            ProductId = cartItem.ProductId;
            UniquePartnerId = cartItem.UniquePartnerId;
            ProductName = cartItem.ProductName;
            ShortDescription = cartItem.ShortDescription;
            StartDate = cartItem.StartDate;
            EndDate = cartItem.EndDate;
            PriceExcl = cartItem.PriceExcl;
            Vat = cartItem.Vat;
            Discount = cartItem.Discount;
            Commission = cartItem.Commission;
            PriceIncl = cartItem.PriceIncl;
            PriceOld = cartItem.PriceOld;
            RateId = Convert.ToInt32(cartItem.RateId);
            ShoppingCartItemType = cartItem.ShoppingCartItemType;
            Metadata = cartItem.Metadata.Select(c => new ShoppingCartItemMetadataDto(c)).ToList();
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for a cart item.
        /// </summary>
        public string CartItemId { get; set; }

        /// <summary>
        /// Gets the identifier of the parent entity, if one exists.
        /// </summary>
        public string? ParentId { get; init; }
        
        /// <summary>
        /// Gets the quantity associated with the current instance.
        /// </summary>
        public double Qty { get; init; }

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
        /// Gets the unique identifier for the lodging.
        /// </summary>
        public string LodgingId { get; init; }

        /// <summary>
        /// Gets the unique identifier for the package.
        /// </summary>
        public int PackageId { get; init; }

        /// <summary>
        /// Gets the unique identifier for the product.
        /// </summary>
        public string ProductId { get; init; }

        /// <summary>
        /// Gets the unique identifier for a partner.
        /// </summary>
        public string UniquePartnerId { get; init; }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        public string ProductName { get; init; }

        /// <summary>
        /// Gets or sets a brief description or summary of the item.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the start date of the event or process.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event or time period.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets the URL of the thumbnail image associated with the current object.
        /// </summary>
        public string ThumbnailUrl { get; init; }

        /// <summary>
        /// Gets the serial number associated with the object.
        /// </summary>
		public string SerialNumber { get; init; }

        /// <summary>
        /// Gets the barcode associated with the item.
        /// </summary>
		public string Barcode { get; init; }

        /// <summary>
        /// Gets the stock-keeping unit (SKU) identifier for the product.
        /// </summary>
		public string Sku { get; init; }

        /// <summary>
        /// Gets or sets the unique identifier for the rate.
        /// </summary>
        public int RateId { get; set; }

        /// <summary>
        /// Gets the price of the item excluding any applicable taxes.
        /// </summary>
		public double PriceExcl { get; init; }

        /// <summary>
        /// Gets or sets the value-added tax (VAT) percentage.
        /// </summary>
        public double Vat { get; set; }

        /// <summary>
        /// Gets the discount percentage to be applied to the total price.
        /// </summary>
        public double Discount { get; init; }

        /// <summary>
        /// Gets the commission percentage associated with a transaction.
        /// </summary>
        public double Commission { get; init; }

        /// <summary>
        /// Gets the price of the item, including applicable taxes.
        /// </summary>
        public double PriceIncl { get; init; }

        /// <summary>
        /// Gets the previous price of the item, if available.
        /// </summary>
        public double? PriceOld { get; init; }

        /// <summary>
        /// Gets the metadata associated with the shopping cart items.
        /// </summary>
        public List<ShoppingCartItemMetadataDto> Metadata { get; init; } = new();

        /// <summary>
        /// Gets the type of the shopping cart item.
        /// </summary>
        public ShoppingCartItemType ShoppingCartItemType { get; init; } = ShoppingCartItemType.StandardItem;

        /// <summary>
        /// Converts the current object to a <see cref="ShoppingCartItem"/> instance.
        /// </summary>
        /// <remarks>This method maps the properties of the current object to a new <see
        /// cref="ShoppingCartItem"/> instance. Ensure that all required properties of the current object are properly
        /// set before calling this method to avoid unexpected behavior.</remarks>
        /// <returns>A <see cref="ShoppingCartItem"/> object populated with the corresponding values from the current instance.</returns>
        public ShoppingCartItem ToShoppingCartItem()
        {
            return new ShoppingCartItem()
            {
                Qty = Qty,
                ShoppingCartId = CartItemId,
                ShoppingCartItemType = ShoppingCartItemType,

                LodgingId = LodgingId,
                UniquePartnerId = UniquePartnerId,
                PackageId = Convert.ToInt32(PackageId),
                ProductId = ProductId,
                ThumbnailUrl = ThumbnailUrl,
                ProductName = ProductName,
                ShortDescription = ShortDescription,
                StartDate = StartDate,
                EndDate = EndDate,
                Adults = Adults,
                KidsGroup1 = KidsGroup1,
                KidsGroup2 = KidsGroup2,
                PriceExcl = PriceExcl,
                Discount = Discount,
                Commission = Commission,
                PriceOld = PriceIncl,
                RateId = RateId
            };
        }
    }
}
