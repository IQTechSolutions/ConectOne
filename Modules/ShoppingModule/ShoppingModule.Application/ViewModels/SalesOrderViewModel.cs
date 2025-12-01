using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IdentityModule.Domain.DataTransferObjects;

namespace ShoppingModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a sales order, providing details such as order ID, customer information,  totals,
    /// and associated order details.
    /// </summary>
    /// <remarks>This class is designed to encapsulate the data required to display or process a sales order
    /// in a user interface  or application layer. It includes properties for the sales order's metadata, financial
    /// totals, and associated  details. The <see cref="Details"/> property contains a collection of line items or order
    /// details.</remarks>
    public class SalesOrderViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderViewModel"/> class.
        /// </summary>
        public SalesOrderViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderViewModel"/> class using the specified sales order
        /// data.
        /// </summary>
        /// <remarks>This constructor populates the view model properties with the corresponding values
        /// from the provided <paramref name="salesOrder"/>. It ensures that the view model is initialized with all
        /// relevant sales order information, such as identifiers, totals, and user details.</remarks>
        /// <param name="salesOrder">The sales order data transfer object (DTO) containing the details of the sales order.</param>
        public SalesOrderViewModel(SalesOrderDto salesOrder)
        {
            QuotationId = salesOrder.QuotationId;
            SalesOrderId = salesOrder.SalesOrderId;
            SalesOrderNr = salesOrder.SalesOrderNr;
            SalesOrderDate = salesOrder.SalesOrderDate;
            Notes = salesOrder.Notes;
            UserInfo = salesOrder.User;
            ItemCount = salesOrder.ItemCount;
            TotalExcl = salesOrder.SubTotal;
            TotalVat = salesOrder.Vat;
            TotalIncl = salesOrder.TotalIncl;
            ItemCount = salesOrder?.ItemCount ?? 0;
            GrandTotal = salesOrder.Total;
            Paid = salesOrder.TotalAmmountPaid;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the sales order.
        /// </summary>
        public string SalesOrderId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the quotation.
        /// </summary>
        public string? QuotationId { get; set; }

        /// <summary>
        /// Gets or sets the sales order number associated with the transaction.
        /// </summary>
        public string SalesOrderNr { get; set; }

        /// <summary>
        /// Gets or sets the user information associated with the current context.
        /// </summary>
        public UserInfoDto UserInfo { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the sales order was created.
        /// </summary>
        public DateTime SalesOrderDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets the current status of the order.
        /// </summary>
        public OrderStatus OrderStatus { get; init; } = OrderStatus.PaymentPending;

        /// <summary>
        /// Gets or sets additional information or comments.
        /// </summary>
        [DataType(DataType.MultilineText), DisplayName(@"Notes")] public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the amount that has been paid.
        /// </summary>
        public double Paid { get; set; }

        /// <summary>
        /// Gets or sets the total amount excluding any applicable taxes or fees.
        /// </summary>
        public double TotalExcl { get; set; }

        /// <summary>
        /// Gets or sets the total value-added tax (VAT) amount.
        /// </summary>
        public double TotalVat { get; set; }

        /// <summary>
        /// Gets or sets the total amount, including all applicable taxes and fees.
        /// </summary>
        public double TotalIncl { get; set; }

        /// <summary>
        /// Gets or sets the grand total amount.
        /// </summary>
        public double GrandTotal { get; set; }

        /// <summary>
        /// Gets or sets the number of items in the collection.
        /// </summary>
        public double ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the collection of sales order details associated with the sales order.
        /// </summary>
        public ICollection<SalesOrderDetailViewModel> Details { get; set;} = new List<SalesOrderDetailViewModel>();
    }
}