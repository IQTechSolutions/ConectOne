using FilingModule.Domain.Enums;
using ShoppingModule.Domain.Entities;

namespace ShoppingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the details of a sales order line item, including product information, pricing, and processing
    /// status.
    /// </summary>
    /// <remarks>This data transfer object (DTO) is used to encapsulate the details of a single sales order
    /// line item for use in application layers such as presentation or API responses. It includes product metadata,
    /// pricing details, and processing status.</remarks>
	public record SalesOrderDetailDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderDetailDto"/> class.
        /// </summary>
        public SalesOrderDetailDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderDetailDto"/> class using the specified sales order
        /// detail.
        /// </summary>
        /// <remarks>This constructor extracts relevant information from the provided <see
        /// cref="SalesOrderDetail"/> object, including product details, pricing, and processing status, and initializes
        /// the corresponding properties of the DTO.</remarks>
        /// <param name="detail">The <see cref="SalesOrderDetail"/> instance containing the sales order detail information to populate the
        /// DTO.</param>
        public SalesOrderDetailDto(SalesOrderDetail detail)
        {
            SalesOrderId = detail.SalesOrderId;
            SalesOrderDetailId= detail.Id;

            Qty = detail.Qty;
            ProductId = detail.ProductId;
            ProductName = detail.Product.Name;
            Description = detail.Product.Description;
            ThumbnailUrl = detail.Product.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover) == null ? "/_content/Products.Mvc/images/NoImage.jpg" : detail.Product.Images.FirstOrDefault(c => c.Image.ImageType == UploadType.Cover).Image.RelativePath;

            PriceExcl = detail.PriceExcl;
            PriceVat = detail.PriceVat;
            PriceIncl = detail.PriceIncl;

            Processed = detail.Processed;

            MetaData = detail.MetaData?.Select(meta => new ShoppingCartItemMetadataDto(meta.Name, meta.Value, meta.SalesOrderDetailId)).ToList()
                ?? new List<ShoppingCartItemMetadataDto>();
        }

        #endregion  

        /// <summary>
        /// Gets the unique identifier for the sales order.
        /// </summary>
        public string SalesOrderId { get; init; }

        /// <summary>
        /// Gets the unique identifier for the sales order detail.
        /// </summary>
        public string SalesOrderDetailId { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the processing operation has been completed.
        /// </summary>
        public bool Processed { get; set; } = false;

        /// <summary>
        /// Gets the quantity associated with the operation. The default value is 1.
        /// </summary>
        public double Qty { get; init; } = 1;

        /// <summary>
        /// Gets the unique identifier for the product.
        /// </summary>
        public string ProductId { get; init; }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        public string ProductName { get; init; }

        /// <summary>
        /// Gets the description associated with the object.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets the URL of the thumbnail image associated with the object.
        /// </summary>
        public string ThumbnailUrl { get; init; } 

        /// <summary>
        /// Gets the price of the item excluding any applicable taxes.
        /// </summary>
        public double PriceExcl { get; init; }

        /// <summary>
        /// Gets the price including VAT (Value Added Tax).
        /// </summary>
        public double PriceVat { get; init; }

        /// <summary>
        /// Gets the price of the item, including applicable taxes.
        /// </summary>
        public double PriceIncl { get; init; }

        /// <summary>
        /// Gets or sets the collection of metadata associated with the shopping cart items.
        /// </summary>
        public ICollection<ShoppingCartItemMetadataDto> MetaData { get; set; } = new List<ShoppingCartItemMetadataDto>();
    }
}
