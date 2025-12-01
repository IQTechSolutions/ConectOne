using ShoppingModule.Domain.DataTransferObjects;

namespace ShoppingModule.Application.ViewModels
{
    /// <summary>
    /// Represents a detailed view model for a sales order item, including product information, pricing, and quantities.
    /// </summary>
    /// <remarks>This view model is designed to encapsulate the details of a single sales order item,
    /// including its associated product, pricing (exclusive of VAT, VAT amount, and inclusive of VAT), and quantity. It
    /// also provides calculated properties for the total price exclusive of VAT, total VAT, and total price inclusive
    /// of VAT based on the quantity.</remarks>
	public class SalesOrderDetailViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderDetailViewModel"/> class.
        /// </summary>
        public SalesOrderDetailViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderDetailViewModel"/> class using the specified sales
        /// order detail data transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see
        /// cref="SalesOrderDetailDto"/> to the corresponding properties of the <see cref="SalesOrderDetailViewModel"/>
        /// instance.</remarks>
        /// <param name="salesOrderDetail">A <see cref="SalesOrderDetailDto"/> containing the sales order detail information to populate the view
        /// model. This parameter must not be <c>null</c>.</param>
        public SalesOrderDetailViewModel(SalesOrderDetailDto salesOrderDetail) 
        {
            SalesOrderId = salesOrderDetail.SalesOrderId;
            Processed = salesOrderDetail.Processed;
            Qty = salesOrderDetail.Qty;
            ProductId = salesOrderDetail.ProductId;
            ProductName = salesOrderDetail.ProductName;
            ProductDescription = salesOrderDetail.Description;
            PriceExcl = salesOrderDetail.PriceExcl;
            PriceVat = salesOrderDetail.PriceVat;
            PriceIncl = salesOrderDetail.PriceIncl;
        }

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier for the sales order.
        /// </summary>
        public string SalesOrderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the processing operation has been completed.
        /// </summary>
        public bool Processed { get; set; }

        /// <summary>
        /// Gets or sets the quantity associated with the operation.
        /// </summary>
        public double Qty { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Gets or sets the price of the item excluding any applicable taxes.
        /// </summary>
        public double PriceExcl { get; set; }

        /// <summary>
        /// Gets or sets the price including VAT (Value Added Tax).
        /// </summary>
        public double PriceVat { get; set; }

        /// <summary>
        /// Gets or sets the price of the item, including applicable taxes.
        /// </summary>
        public double PriceIncl { get; set; }

        /// <summary>
        /// Gets the total price excluding tax, calculated as the product of the quantity and the price per unit
        /// excluding tax.
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
    }
}
