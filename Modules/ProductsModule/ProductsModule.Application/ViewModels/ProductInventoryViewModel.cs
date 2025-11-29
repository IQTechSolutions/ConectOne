using System.ComponentModel;
using ProductsModule.Domain.DataTransferObjects;
using ProductsModule.Domain.RequestFeatures;

namespace ProductsModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for managing product inventory, including stock levels, orders, and pre-order settings.
    /// </summary>
    /// <remarks>This class provides a representation of product inventory data, including properties for
    /// tracking stock levels,  orders, and pre-orders. It can be initialized with a <see cref="ProductInventoryDto"/>
    /// object and optionally  customized with <see cref="ProductsParameters"/>. The view model is designed to
    /// facilitate data binding and  manipulation in user interfaces.</remarks>
    public class ProductInventoryViewModel
    {
        #region Constuctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductInventoryViewModel"/> class.
        /// </summary>
        public ProductInventoryViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductInventoryViewModel"/> class  with the specified product
        /// inventory data and optional product parameters.
        /// </summary>
        /// <remarks>This constructor populates the view model with inventory details such as stock
        /// levels,  pre-order status, and reorder amounts based on the provided <paramref name="product"/> data.  The
        /// optional <paramref name="parameters"/> can be used to customize the behavior or presentation  of the product
        /// inventory.</remarks>
        /// <param name="product">The product inventory data used to initialize the view model.  This parameter cannot be null and must
        /// contain valid product information.</param>
        /// <param name="parameters">Optional parameters that provide additional configuration or filtering for the product.  If not specified,
        /// default values will be used.</param>
        public ProductInventoryViewModel(ProductInventoryDto product, ProductsParameters? parameters = null) 
        {
            ProductId= product.ProductId;
            AllowPreOrders = product.AllowPreOrders;

            Sold = product.Sold;
            Ordered = product.Ordered;

            PreOrdered = product.PreOrdered;

            OnOrder = product.OnOrder;
            InStock = product.InStock;

            StockLevel = product.StockLevel;
            ReorderAmmount = product.ReorderAmount;

			Parameters=parameters;
		}

		#endregion

        /// <summary>
        /// Gets or sets the parameters used to filter or configure product-related operations.
        /// </summary>
		public ProductsParameters? Parameters { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// <remarks>This property is used to identify the product in inventory operations.</remarks>
        /// </summary>
		[DisplayName("Product Id")] public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [DisplayName("Product Name")] public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether pre-orders are allowed.
        /// </summary>
        [DisplayName("Allow Pre-Orders")] public bool AllowPreOrders { get; set; }

        /// <summary>
        /// Gets or sets the total quantity of items sold.
        /// </summary>
        [DisplayName("Sold")] public double Sold { get; set; }

        /// <summary>
        /// Gets or sets the total quantity of items ordered.
        /// </summary>
        [DisplayName("Ordered")] public double Ordered { get; set; } 

        /// <summary>
        /// Gets or sets the quantity of items that have been pre-ordered.
        /// </summary>
        [DisplayName("Pre-Ordered")] public double PreOrdered { get; set; } 

        /// <summary>
        /// Gets or sets the quantity of the item that is currently on order.
        /// </summary>
        [DisplayName("On Order")] public double OnOrder { get; set; } 

        /// <summary>
        /// Gets or sets the quantity of items currently in stock.
        /// </summary>
        [DisplayName("In Stock")] public double InStock { get; set; } 

        /// <summary>
        /// Gets or sets the current stock level of the item.
        /// </summary>
        [DisplayName("Stock Level")] public double StockLevel { get; set; } 

        /// <summary>
        /// Gets or sets the reorder amount for inventory items.
        /// </summary>
        [DisplayName("Re-Order Ammount")] public double ReorderAmmount { get; set; } 

        #region Static Methods

        /// <summary>
        /// Converts a <see cref="ProductInventoryViewModel"/> instance to a <see cref="ProductInventoryDto"/>.
        /// </summary>
        /// <param name="product">The product inventory view model to convert. Cannot be <see langword="null"/>.</param>
        /// <returns>A <see cref="ProductInventoryDto"/> containing the inventory data from the specified  <see
        /// cref="ProductInventoryViewModel"/>.</returns>
        public static ProductInventoryDto ToProductInventorySettingsDto(ProductInventoryViewModel product)
        {
            return new ProductInventoryDto()
            {
                ProductId = product.ProductId,
                Sold = product.Sold,
                Ordered= product.Ordered,
                PreOrdered = product.PreOrdered,
                OnOrder = product.Ordered,
                InStock = product.InStock,
                StockLevel = product.StockLevel
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current product inventory data to a <see cref="ProductInventoryDto"/>.
        /// </summary>
        /// <remarks>The returned <see cref="ProductInventoryDto"/> contains a snapshot of the current
        /// state of the product inventory, including stock levels, order details, and pre-order information.</remarks>
        /// <returns>A <see cref="ProductInventoryDto"/> representing the current product inventory.</returns>
        public ProductInventoryDto ToDto()
        {
            return new ProductInventoryDto()
            {
                ProductId = ProductId,
                AllowPreOrders = AllowPreOrders,
                Sold = Sold,
                Ordered = Ordered,
                PreOrdered = PreOrdered,
                OnOrder = OnOrder,
                InStock = InStock,
                StockLevel = StockLevel,
                ReorderAmount = ReorderAmmount
            };
        }

        #endregion
    }
}
