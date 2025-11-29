using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using ShoppingModule.Domain.DataTransferObjects;

namespace ShoppingModule.Domain.Entities
{
    public class SalesOrderDetailMetaData : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItemMetadata"/> class.
        /// </summary>
        public SalesOrderDetailMetaData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesOrderDetailMetaData"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <param name="dto">The data transfer object containing the metadata values for the sales order detail. Cannot be <see
        /// langword="null"/>.</param>
        public SalesOrderDetailMetaData(SalesOrderDetailMetadataDto dto)
        {
            Name = dto.Name;
            Value = dto.Value;
            SalesOrderDetailId = dto.SalesOrderDetailId;
        }

        #endregion


        /// <summary>
        /// Gets or sets the name of the attribute (e.g. Color, Size).
        /// </summary>
        [Required, MaxLength(100)] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the value of the attribute.
        /// </summary>
        [MaxLength(200)] public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the sales order detail associated with the shopping cart item.
        /// </summary>
        [ForeignKey(nameof(ShoppingCartItem))] public string? SalesOrderDetailId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the details of the associated sales order.
        /// </summary>
        public SalesOrderDetail? SalesOrderDetail { get; set; } = null!;
    }
}
