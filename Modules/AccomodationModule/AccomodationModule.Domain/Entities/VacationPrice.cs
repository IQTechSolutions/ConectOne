using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a price associated with a vacation or vacation extension.
    /// Inherits from <see cref="EntityBase{TId}"/>.
    /// </summary>
    public class VacationPrice : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationPrice"/> class.
        /// </summary>
        public VacationPrice() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationPrice"/> class by copying another instance.
        /// </summary>
        /// <param name="price">The vacation price to copy.</param>
        public VacationPrice(VacationPrice price)
        {
            Name = price.Name;
            Description = price.Description;
            Price = price.Price;
            Selector = price.Selector;
            Order = price.Order;
            VacationId = price.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the vacation price.
        /// </summary>
        [MaxLength(1000)] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the vacation price.
        /// </summary>
        [MaxLength(5000)] public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the price value.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the selector for the vacation price.
        /// </summary>
        [MaxLength(1000)] public string? Selector { get; set; }
        
        /// <summary>
        /// Gets or sets the order of the vacation price.
        /// </summary>
        public int Order { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// Gets or sets the ID of the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? VacationId { get; set; }

        /// <summary>
        /// Navigation property to the associated vacation.
        /// </summary>
        public Vacation? Vacation { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a copy of the current vacation price.
        /// </summary>
        /// <returns>A new instance of <see cref="VacationPrice"/> that is a copy of the current instance.</returns>
        public VacationPrice Clone()
        {
            return new VacationPrice(this) { Id = Guid.NewGuid().ToString() };
        }

        /// <summary>
        /// Returns a string representation of the vacation price.
        /// </summary>
        /// <returns>A string that represents the vacation price.</returns>
        public override string ToString()
        {
            return $"Vacation Price";
        }
        
        #endregion
    }
}

