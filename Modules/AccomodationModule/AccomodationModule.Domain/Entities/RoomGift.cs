using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a gift associated with a room during a vacation.
    /// Includes details such as description, guest type, date, time description, and gift type.
    /// </summary>
    public class RoomGift : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomGift"/> class.
        /// </summary>
        public RoomGift() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomGift"/> class by copying the values from an existing
        /// instance.
        /// </summary>
        /// <param name="roomGift">The <see cref="RoomGift"/> instance to copy. Cannot be <see langword="null"/>.</param>
        public RoomGift(RoomGift roomGift)
        {
            DayNr = roomGift.DayNr;
            Time = roomGift.Time;
            GiftId = roomGift.GiftId;
            VacationId = roomGift.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the date associated with the room gift.
        /// </summary>
        public int DayNr { get; set; }

        /// <summary>
        /// Gets or sets the time associated with the gift.
        /// </summary>
        public TimeSpan? Time { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// Gets or sets the identifier for the associated gift.
        /// </summary>
        [ForeignKey(nameof(Gift))] public string? GiftId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Gift? Gift { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? VacationId { get; set; } 

        /// <summary>
        /// Navigation property to the associated vacation.
        /// </summary>
        public Vacation? Vacation { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a new instance of the <see cref="RoomGift"/> class that is a copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="RoomGift"/> object that is a copy of this instance.</returns>
        public RoomGift Clone()
        {
            return new RoomGift(this) { Id = Guid.NewGuid().ToString()};
        }

        #endregion
    }
}
