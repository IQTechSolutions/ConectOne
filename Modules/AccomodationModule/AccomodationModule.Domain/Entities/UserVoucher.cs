using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a user voucher in the accommodation system.
    /// Associates a user with a specific voucher, room, and order.
    /// </summary>
    public class UserVoucher : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the ID of the user associated with this voucher.
        /// </summary>
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the voucher.
        /// </summary>
        [ForeignKey(nameof(Voucher)), Required(ErrorMessage = "Voucher ID is required.")]
        public int VoucherId { get; set; }

        /// <summary>
        /// Gets or sets the voucher associated with this user voucher.
        /// </summary>
        public Voucher Voucher { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the room associated with this voucher.
        /// </summary>
        [ForeignKey(nameof(Room)), Required(ErrorMessage = "Room ID is required.")]
        public int RoomId { get; set; }

        /// <summary>
        /// Gets or sets the room associated with this user voucher.
        /// </summary>
        public Room Room { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the order associated with this voucher.
        /// </summary>
        [ForeignKey(nameof(Order))]
        public string? OrderId { get; set; }

        /// <summary>
        /// Gets or sets the order associated with this user voucher.
        /// </summary>
        public Order? Order { get; set; } = null!;
    }
}
