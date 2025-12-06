using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing room gifts associated with a vacation.
    /// This class is used to transfer data between layers, such as from the database to the UI or API.
    /// It includes details such as the gift type, description, guest type, and associated vacation ID.
    /// </summary>
    public record RoomGiftDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomGiftDto"/> class with default values.
        /// </summary>
        public RoomGiftDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomGiftDto"/> class using a <see cref="RoomGift"/> entity.
        /// </summary>
        /// <param name="gift">The <see cref="RoomGift"/> entity containing the data to initialize the DTO.</param>
        public RoomGiftDto(RoomGift gift)
        {
            RoomGiftId = gift.Id;
            DayNr = gift.DayNr;
            Time = gift.Time;
            Gift = gift.Gift == null ? new GiftDto() : new GiftDto(gift.Gift);
            VacationId = gift.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the room gift.
        /// </summary>
        public string RoomGiftId { get; set; }

        /// <summary>
        /// Gets or sets the date associated with the room gift.
        /// </summary>
        public int DayNr { get; set; }

        /// <summary>
        /// Gets or sets the time associated with the gift.
        /// </summary>
        public TimeSpan? Time { get; set; }

        /// <summary>
        /// Gets or sets the description of the room gift.
        /// </summary>
        public GiftDto Gift { get; set; } 

        /// <summary>
        /// Gets or sets the unique identifier for the associated vacation.
        /// </summary>
        public string? VacationId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated vacation extension.
        /// </summary>
        public string? VacationExtensionId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current DTO into a <see cref="RoomGift"/> entity for persistence in the database.
        /// </summary>
        /// <returns>A <see cref="RoomGift"/> entity with the DTO's data.</returns>
        public RoomGift ToVacationRoomGift()
        {
            return new RoomGift()
            {
                Id = RoomGiftId,
                DayNr = DayNr,
                Time = Time,
                GiftId = Gift.GiftId,
                VacationId = VacationId
            };
        }

        /// <summary>
        /// Updates the values of an existing <see cref="RoomGift"/> entity with the properties of this DTO.
        /// </summary>
        /// <param name="gift">The vacation interval entity to update.</param>
        public void UpdateVacationRoomGiftValues(in RoomGift gift)
        {
            gift.Id = RoomGiftId;
            gift.DayNr = DayNr;
            gift.Time = Time;
            gift.GiftId = Gift.GiftId;
        }

        #endregion
    }
}
