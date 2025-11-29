using ConectOne.Domain.DataTransferObjects;
using ConectOne.Domain.Enums;
using ConectOne.Domain.Extensions;
using IdentityModule.Domain.DataTransferObjects;
using ProductsModule.Domain.Enums;
using ShoppingModule.Domain.Entities;
using ShoppingModule.Domain.Enums;

namespace ShoppingModule.Domain.DataTransferObjects;

/// <summary>
/// Represents a data transfer object (DTO) for a sales order, encapsulating key details about the order, including its
/// metadata, associated user, addresses, financial totals, and status.
/// </summary>
/// <remarks>This DTO is designed to provide a simplified, serializable representation of a sales order for use in
/// data transfer scenarios, such as API responses or inter-service communication. It includes information about the
/// sales order's creation date, unique identifiers, user details, delivery and billing addresses, and financial
/// breakdowns such as totals, discounts, and taxes.</remarks>
public record SalesOrderDto
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesOrderDto"/> class.
    /// </summary>
    public SalesOrderDto() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesOrderDto"/> class using the specified sales order.
    /// </summary>
    /// <remarks>This constructor maps the properties of the provided <see cref="SalesOrder"/> object to the
    /// corresponding  properties of the <see cref="SalesOrderDto"/>. It initializes nested objects such as <see
    /// cref="UserInfoDto"/>  and <see cref="AddressDto"/> for shipping and billing addresses based on the sales order's
    /// data.</remarks>
    /// <param name="salesOrder">The sales order from which to populate the data transfer object.  This parameter cannot be <see
    /// langword="null"/>.</param>
    public SalesOrderDto(SalesOrder salesOrder)
    {
        SalesOrderDate = salesOrder.OrderDate;
        SalesOrderId = salesOrder.Id;
        SalesOrderNr = salesOrder.OrderNr.ToString();
        User = salesOrder.UserInfo is null ? new UserInfoDto() : new UserInfoDto(salesOrder.UserInfo);
        Notes = salesOrder.Notes;
        DeliveryMethod = salesOrder.DeliveryMethod;
        OrderStatus = salesOrder.OrderStatus;
        ShippingAddress = new AddressDto(salesOrder.Addresses?.Where(c => c.AddressType.Equals(AddressType.Shipping))?.ToList()?.GetDefaultEntry());
        BillingAddress = new AddressDto(salesOrder.Addresses?.Where(c => c.AddressType.Equals(AddressType.Billing))?.ToList()?.GetDefaultEntry());

        ItemCount = salesOrder.ItemCount;
        TotalAmmountPaid = salesOrder.TotalAmmountPaid;
        ShippingTotal = salesOrder.ShippingTotal;
        Discount = salesOrder.Discount;
        CouponDiscount = salesOrder.CouponDiscount;
        SubTotal = salesOrder.TotalExcl;
        Vat = salesOrder.Vat;
        TotalIncl = salesOrder.TotalIncl;
        Total = salesOrder.GrandTotal;
    }

    #endregion

    /// <summary>
    /// Gets or sets the date and time when the sales order was created.
    /// </summary>
    public DateTime SalesOrderDate { get; set; }

    /// <summary>
    /// Gets the unique identifier for the sales order.
    /// </summary>
    public string SalesOrderId { get; init; }

    /// <summary>
    /// Gets the sales order number associated with the current transaction.
    /// </summary>
    public string? SalesOrderNr { get; init; }

    /// <summary>
    /// Gets the unique identifier for the user.
    /// </summary>
    public string UserId { get; init; }

    /// <summary>
    /// Gets the user information associated with the current context.
    /// </summary>
    public UserInfoDto? User { get; init; }

    /// <summary>
    /// Gets the unique identifier for the quotation.
    /// </summary>
    public string? QuotationId { get; init; }

    /// <summary>
    /// Gets the notes or additional information associated with the object.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Gets the delivery method used for sending messages.
    /// </summary>
    public DeliveryMethod DeliveryMethod { get; init; }

    /// <summary>
    /// Gets the current status of the order.
    /// </summary>
    public OrderStatus OrderStatus { get; init; } = OrderStatus.PaymentPending;

    /// <summary>
    /// Gets the shipping address associated with the order.
    /// </summary>
    public AddressDto? ShippingAddress { get; init; }

    /// <summary>
    /// Gets the billing address associated with the customer.
    /// </summary>
    public AddressDto? BillingAddress { get; init; }

    /// <summary>
    /// Gets the total number of items.
    /// </summary>
    public double ItemCount { get; init; }

    /// <summary>
    /// Gets the total amount paid for a transaction or series of transactions.
    /// </summary>
    public double TotalAmmountPaid { get; init; }

    /// <summary>
    /// Gets the total shipping cost for the order.
    /// </summary>
    public double ShippingTotal { get; init; }

    /// <summary>
    /// Gets the discount percentage to be applied to the total price.
    /// </summary>
    public double Discount { get; init; }

    /// <summary>
    /// Gets the discount amount applied to the total price as a result of a coupon.
    /// </summary>
    public double CouponDiscount { get; init; }

    /// <summary>
    /// Gets the subtotal amount for the current transaction.
    /// </summary>
    public double SubTotal { get; init; }

    /// <summary>
    /// Gets the Value-Added Tax (VAT) percentage as a double.
    /// </summary>
    public double Vat { get; init; }

    /// <summary>
    /// Gets the total amount, including all applicable taxes and fees.
    /// </summary>
    public double TotalIncl { get; init; }

    /// <summary>
    /// Gets the total amount calculated for the operation.
    /// </summary>
    public double Total { get; init; }
}
