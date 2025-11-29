using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// The data transfer object used to display information about a product inventory settings and values
    /// </summary>
    public record ProductInventoryDto
    {
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ProductInventoryDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductInventoryDto"/> class using the specified product.
        /// </summary>
        /// <param name="product">The product from which to initialize the inventory data. Cannot be <see langword="null"/>.</param>
        public ProductInventoryDto(Product product)
        {
            ProductId = product.Id;
        }

        #endregion

        /// <summary>
        /// The identity of the featured Product
        /// </summary>
        public string ProductId { get; init; }

        /// <summary>
        /// The flag to indicate if the featured product allows pre orders
        /// </summary>
        public bool AllowPreOrders { get; set; } = false;

        /// <summary>
        /// The quatity that has been sold 
        /// </summary>
        public double Sold { get; set; } = 0;

        /// <summary>
        /// The quantity that is currently on order
        /// </summary>
        public double Ordered { get; set; } = 0;

        /// <summary>
        /// The quantity that is currently on preorder
        /// Can only populate when preorders on the product is allowed <see cref="AllowPreOrders"/>
        /// </summary>
        public double PreOrdered { get; set; } = 0;

        /// <summary>
        /// The quantity currently on order by a customer, that has not been shipped yet
        /// </summary>
        public double OnOrder { get; set; } = 0;

        /// <summary>
        /// The quantity currently in stock
        /// </summary>
        public double InStock { get; set; } = 0;

        /// <summary>
        /// The level of stock that should be maintianed by this product
        /// </summary>
        public double StockLevel { get; set; } = 0;

        /// <summary>
        /// The ammount that should automatically be reordered if stock ammount <see cref="InStock"/> is below the stock level <see cref="StockLevel"/>
        /// </summary>
        public double ReorderAmount { get; set; } = 0;
    }
}
