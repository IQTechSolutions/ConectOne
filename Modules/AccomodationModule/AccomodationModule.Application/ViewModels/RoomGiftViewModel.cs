using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for representing room gifts associated with a vacation.
    /// This class is used to bind data between the UI and the application logic.
    /// It includes details such as the gift type, description, guest type, and associated vacation ID.
    /// </summary>
    public class RoomGiftViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomGiftViewModel"/> class with default values.
        /// </summary>
        public RoomGiftViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomGiftViewModel"/> class using a <see cref="RoomGiftDto"/>.
        /// </summary>
        /// <param name="gift">The <see cref="RoomGiftDto"/> containing the data to initialize the ViewModel.</param>
        public RoomGiftViewModel(RoomGiftDto gift)
        {
            RoomGiftId = gift.RoomGiftId;
            DayNr = gift.DayNr;
            Time = gift.Time;
            Gift = gift.Gift;
            VacationId = gift.VacationId;
            VacationExtensionId = gift.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the room gift.
        /// </summary>
        public string RoomGiftId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the date associated with the room gift.
        /// </summary>
        public int DayNr { get; set; }

        /// <summary>
        /// Gets or sets the time associated with the gift.
        /// </summary>
        public TimeSpan? Time { get; set; }

        /// <summary>
        /// Gets or sets the gift details.
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
        /// Converts the current instance of the <see cref="RoomGift"/> class to a <see cref="RoomGiftDto"/>.
        /// </summary>
        /// <returns>A <see cref="RoomGiftDto"/> object that represents the current instance,  with its properties mapped from
        /// the corresponding properties of this instance.</returns>
        public RoomGiftDto ToDto()
        {
            return new RoomGiftDto
            {
                RoomGiftId = this.RoomGiftId,
                DayNr = this.DayNr,
                Time = this.Time,
                Gift = this.Gift,
                VacationId = this.VacationId,
                VacationExtensionId = this.VacationExtensionId
            };
        }

        #endregion
    }
}
