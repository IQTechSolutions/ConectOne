using ShoppingModule.Application.ViewModels;
using ShoppingModule.Domain.DataTransferObjects;

namespace ShoppingModule.Blazor.Models
{
    /// <summary>
    /// Represents a shopping cart that manages items, discounts, and totals for an e-commerce transaction.
    /// </summary>
    /// <remarks>The <see cref="ShoppingCart"/> class provides functionality to manage a collection of items,
    /// apply discounts,  and calculate totals such as subtotal, VAT, and the total amount due. It supports adding and
    /// removing items,  applying coupons, and tracking the last accessed time. The shopping cart is designed to handle
    /// scenarios  where items may have quantities, prices, and VAT applied.  This class is intended for use in
    /// e-commerce applications where shopping cart functionality is required.</remarks>
    public class ShoppingCart
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCart"/> class.
        /// </summary>
        /// <remarks>This constructor creates an empty shopping cart instance. Items can be added to the
        /// cart using the appropriate methods.</remarks>
        public ShoppingCart() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCart"/> class with the specified shopping cart
        /// identifier.
        /// </summary>
        /// <remarks>The <paramref name="shoppingCartId"/> is used to uniquely identify the shopping cart
        /// instance. Ensure that the provided identifier is unique within the context of your application.</remarks>
        /// <param name="shoppingCartId">The unique identifier for the shopping cart. This value cannot be null or empty.</param>
        public ShoppingCart(string shoppingCartId)
        {
            ShoppingCartId = shoppingCartId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCart"/> class using the specified shopping cart data
        /// transfer object.
        /// </summary>
        /// <remarks>The <see cref="ShoppingCartId"/> is set based on the provided DTO, and the cart items
        /// are initialized by mapping each item in the DTO to a <see cref="CartItemViewModel"/>.  The <see
        /// cref="LastAccessed"/> property is set to the current date and time at the moment of
        /// initialization.</remarks>
        /// <param name="shoppingCart">The data transfer object containing the shopping cart's initial state, including its ID and items.</param>
        public ShoppingCart(ShoppingCartDto shoppingCart)
        {
            ShoppingCartId = shoppingCart.ShoppingCartId;
            //    Coupon = new CouponViewModel(shoppingCart.Coupon);
            CartItems = shoppingCart.Items.Select(x => new CartItemViewModel(x)).ToList();
            LastAccessed = DateTime.Now;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the shopping cart.
        /// </summary>
        public string ShoppingCartId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the coupon associated with the current transaction.
        /// </summary>
        public CouponViewModel? Coupon { get; set; }

        /// <summary>
        /// Gets or sets the collection of items in the shopping cart.
        /// </summary>
        public List<CartItemViewModel>? CartItems { get; set; } = new();

        /// <summary>
        /// Gets or sets the date and time when the resource was last accessed.
        /// </summary>
        public DateTime LastAccessed { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the number of days before an item is considered expired.
        /// </summary>
        public int TimeBeforeExpiryInDays { get; set; } = 14;

        /// <summary>
        /// Adds a product to the shopping cart or updates the quantity if the product already exists.
        /// </summary>
        /// <remarks>If the product already exists in the cart, its quantity is updated to the value
        /// specified in <paramref name="product"/>. Otherwise, the product is added as a new item in the
        /// cart.</remarks>
        /// <param name="product">The product to add, including its quantity and product identifier.</param>
        public void Add(ShoppingCartItemDto product)
        {
            if(CartItems.Any(c => c.ProductId == product.ProductId))
            {
                var item = CartItems.FirstOrDefault(c => c.ProductId == product.ProductId);
                item.Qty = product.Qty;
            }
            else
            {
              CartItems!.Add(new CartItemViewModel(product));
            }
        }

        /// <summary>
        /// Removes a product from the cart based on the specified product ID.
        /// </summary>
        /// <remarks>If the product exists in the cart with a quantity greater than one, the quantity is
        /// decremented by one.  If the product exists with a quantity of one, it is removed from the cart entirely.  If
        /// the product does not exist in the cart, no action is taken.</remarks>
        /// <param name="productId">The unique identifier of the product to remove from the cart.</param>
        public void Remove(string productId)
        {
            if (CartItems!.Any(c => c.ProductId == productId))
            {
                var item = CartItems!.FirstOrDefault(c => c.ProductId == productId);
                if(item!.Qty > 1)
                    item.Qty--;
                else
                    CartItems!.Remove(item);
            }
        }

        /// <summary>
        /// Gets the total number of items in the cart, rounded to the nearest whole number.
        /// </summary>
        public double ItemCount
        {
            get
            {
                return Math.Round(CartItems!.Sum(c => c.Qty), 0);
            }
            
        }

        /// <summary>
        /// Gets the total amount due for all items in the cart, including applicable prices.
        /// </summary>
        public double TotalDue => CartItems!.Sum(c => c.TotalPriceIncl);

        /// <summary>
        /// Gets the subtotal of the cart, excluding taxes and after applying any coupon discounts.
        /// </summary>
        public double SubTotalExcl => CartItems!.Sum(c => c.TotalPriceExcl) - CouponDiscount;

        /// <summary>
        /// Gets the total VAT discount applied to the cart based on the active coupon.
        /// </summary>
        private double VatDiscount
        {
            get
            {
                if (Coupon is not null && Coupon.IsActive)
                {
                    return CartItems!.Sum(c => c.TotalVat) * (Coupon.DiscountPercentage/100);
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the total value-added tax (VAT) for all items in the cart, adjusted for any VAT discounts.
        /// </summary>
        public double TotalVat => CartItems!.Sum(c => c.TotalVat) - VatDiscount;

        /// <summary>
        /// Gets the total discount amount applied to the cart based on the active coupon.
        /// </summary>
        public double CouponDiscount
        {
            get
            {
                if (Coupon is not null && Coupon.IsActive)
                {
                    return CartItems!.Sum(c => c.TotalPriceExcl) * (Coupon.DiscountPercentage/100);
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the shipping cost associated with the order.
        /// </summary>
        public double Shipping { get; set; } = 0;

        /// <summary>
        /// Gets the total amount due, including the sum of all cart items' prices (excluding tax) and the shipping
        /// cost.
        /// </summary>
        public double AmmountDue => CartItems!.Sum(c => c.TotalPriceExcl) + Shipping;  
    }
}
