using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using ProductsModule.Domain.Entities;
using ProductsModule.Domain.Extensions;
using ShoppingModule.Domain.DataTransferObjects;

namespace ShoppingModule.Domain.Entities
{
    /// <summary>
    /// Represents the details of a sales order, including product information, pricing, discounts, and calculated
    /// totals.
    /// </summary>
    /// <remarks>This class provides properties to manage and calculate various aspects of a sales order
    /// detail, such as the quantity, price,  discounts, VAT, and total amounts. It also includes relationships to the
    /// associated product and sales order.</remarks>
    public class SalesOrderDetail : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderDetail"/> class.
        /// </summary>
        public SalesOrderDetail() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderDetail"/> class by copying the values from an
        /// existing instance.
        /// </summary>
        /// <param name="detail">The <see cref="SalesOrderDetail"/> instance whose values are used to initialize the new instance. Cannot be
        /// <see langword="null"/>.</param>
        public SalesOrderDetail(SalesOrderDetailDto detail)
        {
            Id = detail.SalesOrderDetailId;
            Qty = detail.Qty;
            ProductId = detail.ProductId;
            VatRate = detail.PriceVat;
            Price = detail.PriceIncl;
            ProfitPercentage = 0;
            ProductDiscount = 0;
            CustomerDiscountPercentage = 0;
            Processed = detail.Processed;
            SalesOrderId = detail.SalesOrderId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the processing operation has been completed.
        /// </summary>
        public bool Processed { get; set; } = false;

        /// <summary>
        /// Gets or sets the quantity associated with the operation.
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
        /// Gets or sets the discount percentage applied to resellers.
        /// </summary>
        public double ResellerDiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the reseller's profit percentage.
        /// </summary>
        public double ResellerProfitPercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage applied to the product.
        /// </summary>
        public double ProductDiscount { get; set; }

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
        /// Gets or sets the unique identifier for the associated sales order.
        /// </summary>
        [ForeignKey(nameof(SalesOrder))]
        public string SalesOrderId { get; set; }

        /// <summary>
        /// Gets or sets the sales order associated with the current operation.
        /// </summary>
        public SalesOrder SalesOrder { get; set; }

        #endregion

        #region Collections

        /// <summary>
        /// Gets or sets the collection of metadata associated with sales order details.
        /// </summary>
        public ICollection<SalesOrderDetailMetaData> MetaData { get; set; }

        #endregion

        #region Read Only

        /// <summary>
        /// Gets the discount applied to the customer as a percentage of the price after the product discount.
        /// </summary>
        public double CustomerDiscount => PricingExtensions.PercentageValue(Price - ProductDiscount, CustomerDiscountPercentage);

        /// <summary>
        /// Gets the discount percentage applied to resellers based on the adjusted price.
        /// </summary>
        public double ResellerDiscount => PricingExtensions.PercentageValue(Price - ProductDiscount - CustomerDiscount, ResellerDiscountPercentage);

        /// <summary>
        /// Gets the profit percentage earned by the reseller based on the adjusted price after applying all discounts.
        /// </summary>
        public double ResellerProfit => PricingExtensions.PercentageValue(Price - ProductDiscount - CustomerDiscount - ResellerDiscount, ResellerProfitPercentage);

        /// <summary>
        /// Gets the price excluding taxes, calculated as the sum of the base price and the reseller's profit.
        /// </summary>
        public double PriceExcl => Price + ResellerProfit;

        /// <summary>
        /// Gets the discounted price of the item, excluding taxes.
        /// </summary>
        public double DiscountedPriceExcl => this.PriceExcl - Discount;

        /// <summary>
        /// Gets the total discount applied, which is the sum of product, customer, and reseller discounts.
        /// </summary>
        public double Discount => ProductDiscount + CustomerDiscount + ResellerDiscount;

        /// <summary>
        /// Gets the price including VAT, rounded to two decimal places.
        /// </summary>
        public double PriceVat => Math.Round(DiscountedPriceExcl * (this.VatRate/100), 2);

        /// <summary>
        /// Gets the total price, including VAT, after applying any discounts.
        /// </summary>
        public double PriceIncl => Math.Round(DiscountedPriceExcl + PriceVat);

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
