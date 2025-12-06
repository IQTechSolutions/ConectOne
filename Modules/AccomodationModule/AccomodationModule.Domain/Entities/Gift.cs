using System.ComponentModel.DataAnnotations;
using AccomodationModule.Domain.Enums;
using FilingModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a gift associated with a room during a vacation.
    /// Includes details such as description, guest type, date, time description, and gift type.
    /// </summary>
    public class Gift : FileCollection<Gift, string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the gift.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the room gift.
        /// </summary>
        [MaxLength(5000)] public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the type of guest for whom the room gift is intended.
        /// Default value: <see cref="Domain.Enums.GuestType.All"/>.
        /// </summary>
        public GuestType GuestType { get; set; } = GuestType.All;

        /// <summary>
        /// Gets or sets the time description for the room gift.
        /// Default value: "Standard".
        /// </summary>
        [MaxLength(1000)] public string? TimeDescription { get; set; } = "Standard";

        /// <summary>
        /// Gets or sets the type of gift.
        /// Default value: <see cref="Room"/>.
        /// </summary>
        public GiftType GiftType { get; set; } = GiftType.Room;

        #endregion
    }
}
