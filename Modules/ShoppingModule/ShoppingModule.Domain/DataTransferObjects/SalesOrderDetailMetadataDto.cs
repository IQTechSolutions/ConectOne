namespace ShoppingModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents metadata associated with a sales order detail, including a name, value, and optional identifier.
    /// </summary>
    /// <remarks>This data transfer object (DTO) is used to encapsulate additional information about a sales
    /// order detail. It is immutable and can be initialized using the provided constructors.</remarks>
    public record SalesOrderDetailMetadataDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderDetailMetadataDto"/> class.
        /// </summary>
        public SalesOrderDetailMetadataDto() { }

        /// <summary>
        /// Represents metadata associated with a sales order detail.
        /// </summary>
        /// <param name="name">The name of the metadata. This parameter is required and cannot be null.</param>
        /// <param name="value">The value of the metadata. This parameter is optional and can be null.</param>
        /// <param name="salesOrderDetailId">The identifier of the associated sales order detail. This parameter is optional and can be null.</param>
        public SalesOrderDetailMetadataDto(string name, string? value = null, string? salesOrderDetailId = null)
        {
            Name = name;
            Value = value;
            SalesOrderDetailId = salesOrderDetailId;
        }

        #endregion

        /// <summary>
        /// Gets the name associated with the object.
        /// </summary>
        public string? Name { get; init; } = null!;

        /// <summary>
        /// Gets the value associated with this instance.
        /// </summary>
        public string? Value { get; init; }

        /// <summary>
        /// Gets the unique identifier for the sales order detail.
        /// </summary>
        public string? SalesOrderDetailId { get; init; }
    }
}
