using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Domain.Entities
{
    /// <summary>
    /// Represents an item in a shopping cart, including details about the product, pricing, and associated metadata.
    /// </summary>
    /// <remarks>This class is used to model individual items within a shopping cart, including their
    /// quantity, pricing,  and associated product or package details. It supports hierarchical relationships, allowing
    /// items to have  child items (e.g., for bundled products). The class also provides calculated properties for VAT
    /// and total  price, as well as optional metadata such as serial numbers and barcodes.</remarks>
    public class ShoppingCartItem : EntityBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItem"/> class.
        /// </summary>
        public ShoppingCartItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItem"/> class, representing an item in a shopping
        /// cart.
        /// </summary>
        /// <param name="shoppingCartId">The unique identifier for the shopping cart to which this item belongs. Cannot be null or empty.</param>
        /// <param name="lodgingId">The unique identifier for the lodging associated with this item. Cannot be null or empty.</param>
        /// <param name="uniquePartnerId">The unique identifier for the partner associated with this item. Cannot be null or empty.</param>
        /// <param name="packageId">The identifier for the package associated with this item. Must be a non-negative integer.</param>
        /// <param name="startDate">The start date of the booking or reservation. Must be a valid <see cref="DateTime"/> value.</param>
        /// <param name="endDate">The end date of the booking or reservation. Must be a valid <see cref="DateTime"/> value and later than
        /// <paramref name="startDate"/>.</param>
        /// <param name="roomTypeId">The unique identifier for the room type associated with this item. Cannot be null or empty.</param>
        /// <param name="roomName">The name of the room associated with this item. Cannot be null or empty.</param>
        /// <param name="teaserText">A short description or teaser text for the item. Can be null or empty.</param>
        /// <param name="qty">The quantity of this item. Defaults to 1. Must be a positive value.</param>
        /// <param name="adults">The number of adults associated with this item. Defaults to 0. Must be a non-negative integer.</param>
        /// <param name="kidsGroup1">The number of children in the first age group associated with this item. Defaults to 0. Must be a
        /// non-negative integer.</param>
        /// <param name="kidsGroup2">The number of children in the second age group associated with this item. Defaults to 0. Must be a
        /// non-negative integer.</param>
        /// <param name="rate">The rate or price of this item, excluding taxes. Defaults to 0. Must be a non-negative value.</param>
        /// <param name="shoppingCartItemType">The type of the shopping cart item. Defaults to <see cref="Domain.Enums.ShoppingCartItemType.StandardItem"/>.</param>
        public ShoppingCartItem(string shoppingCartId, string lodgingId, string uniquePartnerId, int packageId, DateTime startDate, DateTime endDate, string roomTypeId, string roomName, string teaserText, double qty = 1, int adults= 0, int kidsGroup1 = 0, int kidsGroup2 = 0, double rate = 0, ShoppingCartItemType shoppingCartItemType = ShoppingCartItemType.StandardItem)
        {
            Qty= qty;

            ShoppingCartId= shoppingCartId;
            ShoppingCartItemType= shoppingCartItemType;

            LodgingId= lodgingId;
            UniquePartnerId= uniquePartnerId;
            PackageId= packageId;
            ProductId = roomTypeId;
            ProductName= roomName;
            ShortDescription= teaserText;
            StartDate = startDate;
            EndDate = endDate;
            Adults = adults;
            KidsGroup1 = kidsGroup1;
            KidsGroup2 = kidsGroup2;
            PriceExcl = rate;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the shopping cart.
        /// </summary>
        [MaxLength(255)] public string ShoppingCartId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of the shopping cart item.
        /// </summary>
        public ShoppingCartItemType ShoppingCartItemType { get; set; } = ShoppingCartItemType.StandardItem;

        /// <summary>
        /// Gets or sets the unique identifier for the lodging.
        /// </summary>
        public string? LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the package.
        /// </summary>
        public int? PackageId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public string ProductId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for a partner.
        /// </summary>
        public string? UniquePartnerId { get; set; }

        /// <summary>
        /// Gets or sets the quantity associated with the operation.
        /// </summary>
        public double Qty { get; set; } = 1;

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// Gets or sets a brief description or summary of the item.
        /// </summary>
        public string? ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the start date of the event or process.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the event or time period.
        /// </summary>
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
        /// Gets the URL of the thumbnail image associated with the object.
        /// </summary>
        public string? ThumbnailUrl { get; init; }

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
        /// Gets the price of the item excluding any applicable taxes.
        /// </summary>
        public double PriceExcl { get; init; }

        /// <summary>
        /// Gets the value-added tax (VAT) calculated as 15% of the price excluding tax.
        /// </summary>
        public double Vat { get; init; }

        /// <summary>
        /// Gets the discount percentage to be applied to the total price.
        /// </summary>
        public double Discount { get; init; }

        /// <summary>
        /// Gets the commission percentage associated with a transaction.
        /// </summary>
        public double Commission { get; init; }

        /// <summary>
        /// Gets the total price including VAT.
        /// </summary>
        public double PriceIncl { get; set; }
   
        /// <summary>
        /// Gets the previous price of the item, if available.
        /// </summary>
        public double? PriceOld { get; init; }

        /// <summary>
        /// Gets or sets the identifier for the rate associated with the entity.
        /// </summary>
        public int? RateId { get; set; }

        #region Relationships

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        [ForeignKey(nameof(Parent))] public string? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent shopping cart item, if this item is part of a hierarchical structure.
        /// </summary>
        public ShoppingCartItem? Parent { get; set; }

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the collection of metadata associated with shopping cart items.
        /// </summary>
        public ICollection<ShoppingCartItemMetadata> Metadata { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of child items in the shopping cart.
        /// </summary>
        /// <remarks>This property holds the items that are associated with the current shopping cart. 
        /// The collection is initialized to an empty list by default.</remarks>
        [InverseProperty(nameof(Parent))] public ICollection<ShoppingCartItem> Children { get; set; } = [];

        #endregion

        #region ReadOnly

        /// <summary>
        /// Gets a value indicating whether this instance has any child elements.
        /// </summary>
        public bool HasChildren => Children.Any();

        #endregion

        /// <summary>
        /// Returns a string that represents the current shopping cart item.
        /// </summary>
        /// <returns>A string that represents the shopping cart item.</returns>
        public override string ToString()
        {
            return $"Shopping Cart Item";
        }
    }
}
