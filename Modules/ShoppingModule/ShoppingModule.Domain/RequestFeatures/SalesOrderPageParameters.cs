using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Entities;
using ConectOne.Domain.RequestFeatures;
using ProductsModule.Domain.Enums;
using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the parameters used to filter and paginate sales order data in a request.
    /// </summary>
    /// <remarks>This class provides various filtering options, such as date ranges, amounts, and specific
    /// criteria  like order status, payment method, and delivery method. It also supports text-based search and 
    /// filtering by products or customers. By default, the results are ordered by "OrderNr asc".</remarks>
    public class SalesOrderPageParameters : RequestParameters
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderPageParameters"/> class with default values for its
        /// properties.
        /// </summary>
        /// <remarks>The <see cref="OrderBy"/> property is initialized to "OrderNr asc" by default. This
        /// ensures that sales orders are sorted in ascending order by their order number unless explicitly
        /// overridden.</remarks>
        public SalesOrderPageParameters()
        {
            OrderBy = "OrderNr asc";
        }

        #endregion

        /// <summary>
        /// Gets or sets the text used for search operations.
        /// </summary>
        public string? SearchText { get; set; }

        /// <summary>
        /// Gets or sets the start date used to filter results.
        /// </summary>
        [DataType(DataType.Date)] public DateTime? StartDateFilter { get; set; }

        /// <summary>
        /// Gets or sets the end date used to filter results.
        /// </summary>
        [DataType(DataType.Date)] public DateTime? EndDateFilter { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowable amount.
        /// </summary>
        public double MinAmmount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum allowable amount.
        /// </summary>
        public double MaxAmmount { get; set; } = double.MaxValue;

        /// <summary>
        /// Gets or sets the status of the order.
        /// </summary>
        public OrderStatus? OrderStatus { get; set; }

        /// <summary>
        /// Gets or sets the payment method associated with the transaction.
        /// </summary>
        public PaymentMethod? PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the delivery method for the operation.
        /// </summary>
        public DeliveryMethod? DeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the filter criteria used to narrow down the list of products.
        /// </summary>
        public string? ProductFilter { get; set; }

        /// <summary>
        /// Gets or sets the list of products available for selection.
        /// </summary>
        public List<DropDownListItem>? Products { get; set; } = [];

        /// <summary>
        /// Gets or sets the filter criteria used to narrow down the list of customers.
        /// </summary>
        public string? CustomerFilter { get; set; }

        /// <summary>
        /// Gets or sets the list of customers represented as dropdown list items.
        /// </summary>
        public List<DropDownListItem>? Customers { get; set; } = [];
    }
}
