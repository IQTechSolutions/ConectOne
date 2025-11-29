using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using ProductsModule.Domain.Entities;
using ProductsModule.Domain.Extensions;

namespace ShoppingModule.Domain.Entities
{
    /// <summary>
    /// Represents the details of an invoice line item, including pricing, discounts, VAT, and relationships to
    /// associated entities.
    /// </summary>
    /// <remarks>This class provides properties for managing the quantity, price, discounts, and VAT rate of a
    /// product in an invoice.  It also includes calculated properties for deriving values such as total discounts,
    /// prices (exclusive and inclusive of VAT),  and relationships to the associated product and invoice.</remarks>
    public class InvoiceDetail : EntityBase<string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the quantity value.
        /// </summary>
        public double Qty { get; set; } = 1;

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the VAT (Value Added Tax) rate as a percentage.
        /// </summary>
        public double VatRate { get; set; }

        /// <summary>
        /// Gets or sets the profit percentage as a double value.
        /// </summary>
        public double ProfitPercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage applied to the customer's purchase.
        /// </summary>
        public double CustomerDiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage applied to the product.
        /// </summary>
        public double ProductDiscountPercentage { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// Gets or sets the identifier of the associated product.
        /// </summary>
        [ForeignKey(nameof(Product))] public string? ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product associated with the current entity.
        /// </summary>
        public Product? Product { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated invoice.
        /// </summary>
        [ForeignKey(nameof(Invoice))] public string InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the invoice associated with the current operation.
        /// </summary>
        public Invoice Invoice { get; set; }

        #endregion

        #region Read Only

        /// <summary>
        /// Gets the discount percentage applied to the customer based on the product price and discount.
        /// </summary>
        public double CustomerDiscount => PricingExtensions.PercentageValue(Price - ProductDiscount, CustomerDiscountPercentage);

        /// <summary>
        /// Gets the discount amount for the product based on its price and discount percentage.
        /// </summary>
        public double ProductDiscount => PricingExtensions.PercentageValue(Price, ProductDiscountPercentage);

        /// <summary>
        /// Gets the total discount applied, which is the sum of the product-specific discount and the customer-specific
        /// discount.
        /// </summary>
        public double TotalDiscount => ProductDiscount + CustomerDiscount;

        /// <summary>
        /// Gets the price of the item excluding any applied discounts.
        /// </summary>
        public double PriceExcl => this.Price - TotalDiscount;

        /// <summary>
        /// Gets the price including VAT, rounded to two decimal places.
        /// </summary>
        public double PriceVat => Math.Round(PriceExcl * (this.VatRate/100), 2);

        /// <summary>
        /// Gets the total price, including VAT, rounded to the nearest double-precision value.
        /// </summary>
        public double PriceIncl => Math.Round(PriceExcl + PriceVat);

        /// <summary>
        /// Gets the total price excluding tax, calculated as the product of quantity and the price per unit excluding
        /// tax.
        /// </summary>
        public double TotalPriceExcl => Qty * PriceExcl;

        /// <summary>
        /// Gets the total VAT (Value Added Tax) for the item, calculated as the product of the quantity and the
        /// VAT-inclusive price per unit.
        /// </summary>
        public double TotalVat => Qty * PriceVat;

        /// <summary>
        /// Gets the total price, including any applicable taxes or fees, for the specified quantity.
        /// </summary>
        public double TotalPriceIncl => Qty * PriceIncl;

        #endregion
    }
}
