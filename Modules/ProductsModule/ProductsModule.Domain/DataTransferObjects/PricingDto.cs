using ProductsModule.Domain.Entities;
using ProductsModule.Domain.Extensions;

namespace ProductsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents pricing information for a product, including cost, discounts, VAT, and profit details.
    /// </summary>
    /// <remarks>This data transfer object (DTO) is used to encapsulate pricing-related data for products.  It
    /// includes properties for calculating VAT, discounts, and profit, as well as the final price  inclusive and
    /// exclusive of VAT. The class is designed to be initialized using either a  <see cref="Price"/> object </remarks>
    [Serializable] public record PricingDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PricingDto"/> class.
        /// </summary>
        public PricingDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PricingDto"/> class using the specified price settings.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="Price"/> object to
        /// the corresponding fields in the <see cref="PricingDto"/> instance. The VAT rate is initialized to 15% by
        /// default.</remarks>
        /// <param name="priceSettings">The <see cref="Price"/> object containing the pricing configuration, including cost, discounts, VAT, and
        /// shipping details.</param>
        public PricingDto(Price priceSettings, double vatRate = 15)
        {
            Vatable = priceSettings.Vatable;
            VatRate = vatRate;
            DiscountEndDate = priceSettings.DiscountEndDate;
            DiscountPercentage = priceSettings.DiscountPercentage;
            CostExcl = priceSettings.CostExcl;
            SellingPrice = priceSettings.SellingPrice;
            ShippingAmount = priceSettings.ShippingAmount;
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether the item is subject to value-added tax (VAT).
        /// </summary>
        public bool Vatable { get; init; }

        /// <summary>
        /// Gets the VAT (Value Added Tax) rate as a percentage.
        /// </summary>
        public double VatRate { get; init; }

        /// <summary>
        /// Gets the date and time when the discount period ends.
        /// </summary>
        public DateTime? DiscountEndDate { get; init; }

        /// <summary>
        /// Gets the discount percentage to be applied to the total price.
        /// </summary>
        public double DiscountPercentage { get; init; }

        /// <summary>
        /// Gets the cost excluding any applicable taxes or additional charges.
        /// </summary>
        public double CostExcl { get; init; }

        /// <summary>
        /// Gets the price of the item excluding any applicable taxes.
        /// </summary>
        public double SellingPrice { get; init; }

        /// <summary>
        /// Gets the shipping amount for the current transaction.
        /// </summary>
        public double ShippingAmount { get; init; }

        /// <summary>
        /// Gets the discount amount applied to the cost, calculated based on the profit percentage, discount
        /// percentage, and discount end date.
        /// </summary>
        public double Discount => SellingPrice - SellingPrice.PriceWithDiscount(DiscountPercentage, DiscountEndDate);

        /// <summary>
        /// Gets the previous price of the item, including VAT if applicable.
        /// </summary>
        public double? PreviousPrice => Discount == 0 ? null : SellingPrice;

        /// <summary>
        /// Gets the calculated VAT (Value Added Tax) for the current item.
        /// </summary>
        public double Vat => SellingPrice.PriceVat(Vatable, VatRate);

        /// <summary>
        /// Gets the price including VAT (Value-Added Tax) based on the specified VAT rate and VAT applicability.
        /// </summary>
        public double PriceIncl => SellingPrice - Discount;

        /// <summary>
        /// Gets the profit calculated as the difference between the price excluding tax and the cost excluding tax.
        /// </summary>
        public double Profit => PriceIncl - CostExcl;

        /// <summary>
        /// Converts the current object to a <see cref="Price"/> instance.
        /// </summary>
        /// <returns>A <see cref="Price"/> object containing the values of the current object's properties.</returns>
        public Price ToPrice()
        {
            return new Price
            {
                Vatable = Vatable,
                DiscountEndDate = DiscountEndDate,
                DiscountPercentage = DiscountPercentage,
                CostExcl = CostExcl,
                SellingPrice = SellingPrice,
                ShippingAmount = ShippingAmount
            };
        }
    }
}

