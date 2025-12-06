using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a voucher that can be used for lodging and related services.
    /// </summary>
    /// <remarks>A voucher contains details such as descriptions, pricing information, features, and terms. It
    /// is associated with a specific lodging and may include a collection of rooms and user vouchers.</remarks>
    public class Voucher : EntityBase<int>
    {
        /// <summary>
        /// Gets or sets the title of the item.
        /// </summary>
        public string? Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets a brief description of the item.
        /// </summary>
        public string ShortDescription { get; set; } = null!;

        /// <summary>
        /// Gets or sets the detailed description of an item or entity.
        /// </summary>
        public string LongDescription { get; set; } = null!;

        /// <summary>
        /// Gets or sets the rate value.
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// Gets or sets the markup percentage applied to the base price.
        /// </summary>
        public double MarkupPercentage { get; set; }

        /// <summary>
        /// Gets or sets the commission percentage applied to a transaction.
        /// </summary>
        public double Commission { get; set; }

        /// <summary>
        /// Gets or sets the features associated with the current object.
        /// </summary>
        public string Features { get; set; } = null!;

        /// <summary>
        /// Gets or sets the terms and conditions associated with the current context.
        /// </summary>
        public string Terms { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is featured.
        /// </summary>
		public bool Featured { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated lodging entity.
        /// </summary>
		[ForeignKey(nameof(Lodging))]
        public string? LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the lodging details associated with the current entity.
        /// </summary>
        public Lodging? Lodging { get; set; }

        /// <summary>
        /// Gets or sets the collection of rooms associated with the entity.
        /// </summary>
        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        /// <summary>
        /// Gets or sets the collection of vouchers associated with the user.
        /// </summary>
        public ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();
    }
}