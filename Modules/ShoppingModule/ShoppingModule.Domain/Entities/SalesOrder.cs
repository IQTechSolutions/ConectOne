using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using IdentityModule.Domain.Entities;
using ProductsModule.Domain.Enums;
using ShoppingModule.Domain.DataTransferObjects;
using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Domain.Entities
{
    /// <summary>
    /// Represents a sales order, including details about the customer, order status, delivery method,  billing and
    /// shipping addresses, and associated payments and items.
    /// </summary>
    /// <remarks>A sales order is a record of a customer's purchase, containing information such as the order
    /// date,  status, shipping details, and associated discounts. It also tracks relationships to other entities  such
    /// as the customer, quotation, invoice, and payments. The class provides calculated properties  for totals,
    /// including VAT, discounts, and the grand total.</remarks>
    public class SalesOrder : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrder"/> class.
        /// </summary>
        /// <remarks>This constructor creates a default instance of the <see cref="SalesOrder"/> class
        /// with no initial values set.</remarks>
        public SalesOrder() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrder"/> class by copying the properties of an existing
        /// <see cref="SalesOrder"/> instance.
        /// </summary>
        /// <param name="entity">The <see cref="SalesOrder"/> instance whose properties will be copied to initialize the new instance. Cannot
        /// be <see langword="null"/>.</param>
        public SalesOrder(SalesOrderDto entity)
        {
            Id = entity.SalesOrderId;

            OrderDate = entity.SalesOrderDate;
            Notes = entity.Notes;
            OrderStatus = entity.OrderStatus;
            DeliveryMethod = entity.DeliveryMethod;

            UserInfoId = entity.UserId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the order.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int OrderNr { get; set; }

        /// <summary>
        /// Gets or sets the date of the order.
        /// </summary>
        [DataType(DataType.Date)] public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [ForeignKey(nameof(UserInfo))] public string UserInfoId { get; set; }

        /// <summary>
        /// Gets or sets the user information associated with the current context.
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// Gets or sets the notes associated with the entity.
        /// </summary>
        [MaxLength(1000, ErrorMessage = "The max length of the notes is 1000 characters")] public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the current status of the order.
        /// </summary>
        /// <remarks>The <see cref="OrderStatus"/> property indicates the current state of the order, such
        /// as  whether payment is pending, the order is being processed, or it has been completed.  Use this property
        /// to track and update the order's progress through its lifecycle.</remarks>
        public OrderStatus OrderStatus { get; set; } = OrderStatus.PaymentPending;

        /// <summary>
        /// Gets or sets the delivery method for the order.
        /// </summary>
        public DeliveryMethod DeliveryMethod { get; set; } = DeliveryMethod.Free;

        /// <summary>
        /// Gets or sets the shipping date for the order.
        /// </summary>
        public DateTime? ShippingDate { get; set; }

        /// <summary>
        /// Gets or sets the estimated shipping date for the order.
        /// </summary>
        public DateTime EstimatedShippingDate { get; set; } = DateTime.Now.AddDays(5);

        /// <summary>
        /// Gets or sets the shipping reference associated with the order.
        /// </summary>
        [MaxLength(255, ErrorMessage = "The max length of the shipping reference is 255 characters")] public string? ShippingReference { get; set; }

        /// <summary>
        /// Gets or sets the tracking number associated with the shipment.
        /// </summary>
        [MaxLength(255, ErrorMessage = "The max length of the tracking nr is 255 characters")] public string? TrackingNr { get; set; }

        /// <summary>
        /// Gets or sets the total shipping cost for an order.
        /// </summary>
        public double ShippingTotal { get; set; } = (double)0;

        /// <summary>
        /// Gets or sets the discount percentage to be applied to the total price.
        /// </summary>
        /// <remarks>Ensure that the value is within the valid range to avoid unexpected
        /// behavior.</remarks>
        public double Discount { get; set; }

        /// <summary>
        /// Gets or sets the discount amount applied to the total price as a result of a coupon.
        /// </summary>
        public double CouponDiscount { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// Gets or sets the identifier of the associated invoice.
        /// </summary>
        [ForeignKey(nameof(Invoice))] public string? InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets the invoice associated with the current operation.
        /// </summary>
        public Invoice? Invoice { get; set; }

        #endregion

        #region Collections 

        /// <summary>
        /// Gets or sets the collection of addresses associated with the sales order.
        /// </summary>
        public ICollection<Address<SalesOrder>> Addresses { get; set; } = new List<Address<SalesOrder>>();

        /// <summary>
        /// Gets or sets the collection of sales order details associated with the sales order.
        /// </summary>
        public ICollection<SalesOrderDetail> Details { get; set; } = new List<SalesOrderDetail>();

        /// <summary>
        /// Gets or sets the collection of payments associated with the sales order.
        /// </summary>
        public ICollection<SalesOrderPayment> Payments { get; set; } = new List<SalesOrderPayment>();

        #endregion        

        #region Read Only

        /// <summary>
        /// Gets the total count of items by summing the quantities in the <c>Details</c> collection.
        /// </summary>
        /// <remarks>This property calculates the total dynamically based on the current state of the
        /// <c>Details</c> collection. Ensure that the <c>Details</c> collection is properly initialized and populated
        /// before accessing this property.</remarks>
        public double ItemCount => Details.Sum(c => c.Qty);

        /// <summary>
        /// Gets the total amount paid across all payments.
        /// </summary>
        public double TotalAmmountPaid => Payments.ToList().Sum(c => c.AmmountAllocated);

        /// <summary>
        /// Gets the total price excluding taxes or additional charges.
        /// </summary>
        public double TotalExcl => Details.ToList().Sum(c => c.TotalPriceExcl);

        /// <summary>
        /// Gets the total value-added tax (VAT) calculated from the details.
        /// </summary>
        public double Vat => Details.ToList().Sum(c => c.TotalVat);

        /// <summary>
        /// Gets the total price, including all applicable taxes, for all items in the collection.
        /// </summary>
        public double TotalIncl => Details.ToList().Sum(c => c.TotalPriceIncl);

        /// <summary>
        /// Gets the grand total amount, calculated as the sum of the total inclusive amount and shipping total,  minus
        /// discounts, coupon discounts, and the total amount paid, rounded to two decimal places.
        /// </summary>
        public double GrandTotal => Math.Round(TotalIncl + ShippingTotal - Discount - CouponDiscount - TotalAmmountPaid, 2);

        #endregion
    }
}
