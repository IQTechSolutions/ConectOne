using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for an item in a shopping cart, including details about the product, pricing, and
    /// quantities.
    /// </summary>
    /// <remarks>This class is used to encapsulate the data for a shopping cart item, including product
    /// information, pricing details,  and quantities for adults and children. It provides calculated properties for
    /// total prices and other derived values.</remarks>
    public class CartItemViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemViewModel"/> class.
        /// </summary>
        public CartItemViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemViewModel"/> class using the specified shopping cart
        /// item data transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see
        /// cref="ShoppingCartItemAddUpdateDto"/> to the corresponding properties of the <see
        /// cref="CartItemViewModel"/>. It is used to create a view model representation of a shopping cart item for
        /// display or processing purposes.</remarks>
        /// <param name="cartItemDto">A <see cref="ShoppingCartItemAddUpdateDto"/> object containing the details of the shopping cart item to
        /// initialize the view model.</param>
        public CartItemViewModel(ShoppingCartItemAddUpdateDto cartItemDto)
        {
            ParentId = cartItemDto.ParentId;
            Qty = cartItemDto.Qty;
            Adults = cartItemDto.Adults;
            KidsGroup1 = cartItemDto.KidsGroup1;
            KidsGroup2 = cartItemDto.KidsGroup2;
            StartDate = cartItemDto.StartDate;
            EndDate = cartItemDto.EndDate;
            LodgingId = cartItemDto.LodgingId;
            ProductId = cartItemDto.ProductId;
            PackageId = cartItemDto.PackageId;
            UniquePartnerId = cartItemDto.UniquePartnerId;
            ProductName = cartItemDto.ProductName;
            ShortDescription = cartItemDto.ShortDescription;
            ThumbnailUrl = cartItemDto.ThumbnailUrl;
            Sku = cartItemDto.Sku;
            PriceExcl = cartItemDto.Pricing.CostExcl;
            Vat = cartItemDto.Pricing.Vat;
            Discount = cartItemDto.Pricing.Discount;
            Commission = 0;
            PriceIncl = cartItemDto.Pricing.PriceIncl;
            PriceOld = cartItemDto.Pricing.PreviousPrice;
            RateId = cartItemDto.RateId;
            ShoppingCartItemType = cartItemDto.ShoppingCartItemType;
            Metadata = cartItemDto.Metadata;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemViewModel"/> class using the specified shopping cart
        /// item data.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see
        /// cref="ShoppingCartItemDto"/> to the corresponding properties of the <see cref="CartItemViewModel"/>. It is
        /// used to create a view model representation of a shopping cart item for display or further
        /// processing.</remarks>
        /// <param name="cartItemDto">An instance of <see cref="ShoppingCartItemDto"/> containing the data to initialize the cart item view model.</param>
        public CartItemViewModel(ShoppingCartItemDto cartItemDto)
        {
            ParentId = cartItemDto.ParentId;
            Qty = cartItemDto.Qty;
            Adults = cartItemDto.Adults;
            KidsGroup1 = cartItemDto.KidsGroup1;
            KidsGroup2 = cartItemDto.KidsGroup2;
            StartDate = cartItemDto.StartDate;
            EndDate = cartItemDto.EndDate;
            LodgingId = cartItemDto.LodgingId;
            ProductId = cartItemDto.ProductId;
            UniquePartnerId = cartItemDto.UniquePartnerId;
            PackageId = cartItemDto.PackageId;
            ProductName = cartItemDto.ProductName;
            ShortDescription = cartItemDto.ShortDescription;
            ThumbnailUrl = cartItemDto.ThumbnailUrl;
            Sku = cartItemDto.Sku;
            RateId = cartItemDto.RateId;
            PriceExcl = cartItemDto.PriceExcl;
            Vat = cartItemDto.Vat;
            Discount = cartItemDto.Discount;
            Commission = cartItemDto.Commission;
            PriceIncl = cartItemDto.PriceIncl;
            PriceOld = cartItemDto.PriceOld;
            RateId = cartItemDto.RateId;
            ShoppingCartItemType = cartItemDto.ShoppingCartItemType;
            Metadata = cartItemDto.Metadata;
        }

        #endregion

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the quantity associated with the current operation or entity.
        /// </summary>
        public double Qty { get; set; }

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
        /// Gets or sets the unique identifier for the lodging.
        /// </summary>
        public string LodgingId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier for the package.
        /// </summary>
        public int PackageId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// Gets the unique identifier for a partner.
        /// </summary>
        public string? UniquePartnerId { get; init; }

        /// <summary>
        /// Gets or sets the start date of the event or process.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event or time period.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets the total number of nights between the start and end dates, inclusive.
        /// </summary>
        public int Nights => (EndDate - StartDate).Days + 1;

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a brief description or summary of the item.
        /// </summary>
        public string ShortDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the thumbnail image associated with the item.
        /// </summary>
        public string ThumbnailUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets the stock-keeping unit (SKU) identifier for the product.
        /// </summary>
        public string Sku { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier for the rate.
        /// </summary>
        public int RateId { get; set; }

        /// <summary>
        /// Gets or sets the price of the item excluding any applicable taxes.
        /// </summary>
        public double PriceExcl { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage to be applied to the total price.
        /// </summary>
        public double Discount { get; set; }

        /// <summary>
        /// Gets or sets the commission percentage applied to a transaction.
        /// </summary>
        public double Commission { get; set; }

        /// <summary>
        /// Gets or sets the value-added tax (VAT) percentage.
        /// </summary>
        public double Vat { get; set; }

        /// <summary>
        /// Gets or sets the price of the item, including applicable taxes.
        /// </summary>
        public double PriceIncl { get; set; }

        /// <summary>
        /// Gets or sets the previous price of the item, if available.
        /// </summary>
        public double? PriceOld { get; set; }

        /// <summary>
        /// Gets or sets the type of the shopping cart item.
        /// </summary>
        public ShoppingCartItemType ShoppingCartItemType { get; set; } = ShoppingCartItemType.StandardItem;

        #region Readonly

        /// <summary>
        /// Gets the total price excluding tax.
        /// </summary>
        public double TotalPriceExcl => Qty * PriceExcl;

        /// <summary>
        /// Gets the total VAT (Value Added Tax) amount for the current item.
        /// </summary>
        public double TotalVat => Qty * Vat;

        /// <summary>
        /// Gets the total discount amount calculated as the product of the quantity and the discount per unit.
        /// </summary>
        public double TotalDiscount => Qty * Discount;

        /// <summary>
        /// Gets the total commission calculated as the product of quantity and commission rate.
        /// </summary>
        public double TotalCommission => Qty * Commission;

        /// <summary>
        /// Gets the total price, including any applicable taxes or fees, for the specified quantity.
        /// </summary>
        public double TotalPriceIncl => Qty * PriceIncl;

        /// <summary>
        /// Gets the metadata associated with the shopping cart items.
        /// </summary>
        public List<ShoppingCartItemMetadataDto> Metadata { get; init; } = new();

        #endregion
    }
}
