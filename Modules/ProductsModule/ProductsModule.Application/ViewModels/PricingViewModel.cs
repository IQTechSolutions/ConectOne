using ProductsModule.Domain.DataTransferObjects;
using System.ComponentModel.DataAnnotations;
using ProductsModule.Domain.Extensions;

namespace ProductsModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for pricing calculations, including properties for VAT, discounts, profit, and shipping.
    /// </summary>
    /// <remarks>This class provides a comprehensive set of properties and calculated values to manage and
    /// display pricing information. It includes support for VAT calculations, discount application, and profit margin
    /// analysis.</remarks>
    public class PricingViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PricingViewModel"/> class.
        /// </summary>
        public PricingViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PricingViewModel"/> class with the specified pricing details
        /// and VAT rate.
        /// </summary>
        /// <remarks>The <paramref name="pricing"/> parameter provides the core pricing data, such as
        /// cost, discount percentage, and profit percentage. The <paramref name="vatRate"/> parameter allows
        /// customization of the VAT rate, with a default value of 15% if not specified.</remarks>
        /// <param name="pricing">The pricing details used to initialize the view model, including cost, discounts, and profit information.</param>
        /// <param name="vatRate">The VAT rate to apply, expressed as a percentage. The default value is 15.</param>
        public PricingViewModel(PricingDto pricing, double vatRate = 15) 
        {
            Vatable = pricing.Vatable;
            VatRate = vatRate;
            DiscountPercentage = pricing.DiscountPercentage;
            DiscountEndDate = pricing.DiscountEndDate;  
            ProfitPercentage = pricing.Profit;
            CostExcl = pricing.CostExcl;
            SellingPrice = pricing.SellingPrice;
            ShippingAmount = pricing.ShippingAmount;
        }

        #endregion
        
        /// <summary>
        /// Gets or sets a value indicating whether the item is subject to value-added tax (VAT).
        /// </summary>
        public bool Vatable { get; set; }

        /// <summary>
        /// Gets or sets the VAT (Value-Added Tax) rate as a percentage.
        /// </summary>
        public double VatRate { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the discount period ends.
        /// </summary>
        public DateTime? DiscountEndDate { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage to be applied to the total price.
        /// </summary>
        public double DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the profit percentage for a transaction or operation.
        /// </summary>
        [Range(typeof(double), "1", "79228162514264337593543950335")]
        public double ProfitPercentage { get; set; }

        /// <summary>
        /// Gets or sets the cost excluding taxes or additional charges.
        /// </summary>
        [Range(typeof(double), "1", "79228162514264337593543950335")]
        public double CostExcl { get; set; }

        /// <summary>
        /// Gets or sets the price of the item excluding tax.
        /// </summary>
        [Range(typeof(double), "1", "79228162514264337593543950335")]
        public double SellingPrice { get; set; }

        /// <summary>
        /// Gets or sets the shipping amount for an order.
        /// </summary>
        public double ShippingAmount { get; set; }

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

        #region Methods

        /// <summary>
        /// Converts the current pricing information to a <see cref="PricingDto"/> object.
        /// </summary>
        /// <returns>A <see cref="PricingDto"/> object containing the pricing details, including VAT status, cost,  selling
        /// price, VAT rate, discount details, and shipping amount.</returns>
        public PricingDto ToDto()
        {
            return new PricingDto()
            {
                Vatable = Vatable,
                CostExcl = CostExcl,
                SellingPrice = SellingPrice,

                VatRate = VatRate,

                DiscountEndDate = DiscountEndDate,
                DiscountPercentage = DiscountPercentage,
                ShippingAmount = ShippingAmount
            };
        }

        #endregion
    }
}
