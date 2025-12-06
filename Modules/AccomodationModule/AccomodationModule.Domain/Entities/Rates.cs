using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents the rates and availability information for various room types and services.
    /// </summary>
    /// <remarks>This class provides properties to define rates, availability, and associated metadata for
    /// single and double rooms. It also includes information about the service to which the rates are linked.</remarks>
    public class Rates : EntityBase<int>
    {
        /// <summary>
        /// Gets or sets a value indicating whether a single room is available.
        /// </summary>
        public bool? SingleRoom { get; set; }

        /// <summary>
        /// Gets or sets the rate for a single room.
        /// </summary>
        public double? SingleRoomRate { get; set; }

        /// <summary>
        /// Gets or sets the rate code for a single occupancy reservation.
        /// </summary>
        public string? RateCodeSingle { get; set; }

        /// <summary>
        /// Gets or sets a single value indicating whether the operation has a result.
        /// </summary>
        public string? HasValueSingle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the room is a double room.
        /// </summary>
        public bool? DoubleRoom { get; set; }

        /// <summary>
        /// Gets or sets the rate for a double room.
        /// </summary>
        public double? DoubleRoomRate { get; set; }

        /// <summary>
        /// Gets or sets the rate code as a string representation of a double value.
        /// </summary>
        public string? RateCodeDouble { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the double value is present.
        /// </summary>
        public string? HasValueDouble { get; set; }

        /// <summary>
        /// Gets or sets the date when the item becomes available.
        /// </summary>
        public DateTime? AvailableDate { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the TPN (Third-Party Network).
        /// </summary>
        public string TpnUid { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated service.
        /// </summary>
        [ForeignKey(nameof(Service))] public int? ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the service instance used to perform operations or provide functionality.
        /// </summary>
        public LodgingService? Service { get; set; }
    }
}
