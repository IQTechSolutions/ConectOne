using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a type of bed, including its description, count, and associated room information.
    /// </summary>
    /// <remarks>This class is used to define the characteristics of a bed type, such as its description, the
    /// number of beds,  and its association with a specific room. It can be initialized directly or constructed from an
    /// external  bed type object.</remarks>
    public class BedType : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the identifier for the bed type as defined by the partner system.
        /// </summary>
        public string? PartnerBedTypeId { get; set; }

        /// <summary>
        /// Gets or sets the description associated with the current object.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of beds available.
        /// </summary>
        public int BedCount { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated room.
        /// </summary>
        [ForeignKey(nameof(Room))] public int? RoomId {  get; set; }

        /// <summary>
        /// Gets or sets the room associated with the current context.
        /// </summary>
        public Room? Room { get; set; }

        /// <summary>
        /// Returns a string representation of the bed type.
        /// </summary>
        /// <returns>A string that represents the bed type.</returns>
        public override string ToString()
        {
            return $"Bed Type";
        }
    }
}
