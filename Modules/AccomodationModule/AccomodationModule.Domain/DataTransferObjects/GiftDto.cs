using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing gifts associated with a vacation or extension.
    /// </summary>
    public record GiftDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GiftDto"/> class.
        /// </summary>
        public GiftDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GiftDto"/> class using a <see cref="Gift"/> entity.
        /// </summary>
        /// <param name="gift">The gift entity used to initialize the DTO.</param>
        public GiftDto(Gift gift)
        {
            GiftId = gift?.Id;
            Name = gift?.Name;
            Description = gift?.Description;
            GuestType = gift?.GuestType == null ? GuestType.All : gift.GuestType;
            TimeDescription = gift?.TimeDescription;
            GiftType = gift?.GiftType == null ? GiftType.Room : gift.GiftType; ;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the gift.
        /// </summary>
        public string? GiftId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier for the gift.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the gift.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the type of guest for whom the gift is intended.
        /// </summary>
        public GuestType GuestType { get; set; } = GuestType.All;

        /// <summary>
        /// Gets or sets the description of the time associated with the gift.
        /// </summary>
        public string? TimeDescription { get; set; }

        /// <summary>
        /// Gets or sets the type of gift (e.g., room, person, couple).
        /// </summary>
        public GiftType GiftType { get; set; } = GiftType.Room;
        #endregion

        #region Methods
        /// <summary>
        /// Converts the current DTO into a <see cref="Gift"/> entity.
        /// </summary>
        public Gift ToGift()
        {
            return new Gift
            {
                Id = GiftId,
                Name = Name,
                Description = Description,
                GuestType = GuestType,
                TimeDescription = TimeDescription,
                GiftType = GiftType
            };
        }

        /// <summary>
        /// Updates an existing <see cref="Gift"/> entity with the values of this DTO.
        /// </summary>
        /// <param name="gift">The gift entity to update.</param>
        public void UpdateGiftValues(in Gift gift)
        {
            gift.Id = GiftId;
            gift.Name = Name;
            gift.Description = Description;
            gift.GuestType = GuestType;
            gift.TimeDescription = TimeDescription;
            gift.GiftType = GiftType;
        }

        #endregion
    }
}
