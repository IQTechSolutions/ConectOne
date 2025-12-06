using AccomodationModule.Domain.DataTransferObjects;
using Newtonsoft.Json;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for bed type information, including details such as bed type ID, description, bed count,
    /// and associated room data.
    /// </summary>
    /// <remarks>This class is designed to encapsulate bed type details for use in client-side applications or
    /// APIs. It provides properties for serialization and deserialization, making it suitable for JSON-based
    /// communication.</remarks>
    public class BedTypeViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BedTypeViewModel"/> class.
        /// </summary>
        public BedTypeViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BedTypeViewModel"/> class using the specified <see
        /// cref="BedTypeDto"/>.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="BedTypeDto"/> to the
        /// corresponding properties of the <see cref="BedTypeViewModel"/>. Ensure that the <paramref name="bedType"/>
        /// parameter is properly initialized before passing it to this constructor.</remarks>
        /// <param name="bedType">The data transfer object containing information about the bed type.  Must not be <see langword="null"/> and
        /// must have a valid <see cref="BedTypeDto.RoomId"/> value.</param>
        public BedTypeViewModel(BedTypeDto bedType)
        {
            BedTypeId = bedType.BedTypeId;
            Description = bedType.Description;
            BedCount = bedType.BedCount;
            RoomId = bedType.RoomId.Value;
        }

        #endregion

        /// <summary>
        /// Gets or sets the identifier for the bed type.
        /// </summary>
        [JsonProperty("bedtypecode")] public string BedTypeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        [JsonProperty("description")] public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of beds available.
        /// </summary>
        [JsonProperty("bedcount")] public int BedCount { get; set; }

        /// <summary>
        /// Gets or sets the room number associated with the entity.
        /// </summary>
        [JsonProperty("roomNumber")] public int RoomNumber { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a room.
        /// </summary>
        public int RoomId { get; set; }

        #region Methods

        /// <summary>
        /// Converts the current instance of the bed type to a <see cref="BedTypeDto"/>.
        /// </summary>
        /// <returns>A <see cref="BedTypeDto"/> object containing the bed type's identifier, description, bed count, and
        /// associated room identifier.</returns>
        public BedTypeDto ToDto()
        {
            return new BedTypeDto
            {
                BedTypeId = BedTypeId,
                Description = Description,
                BedCount = BedCount,
                RoomId = RoomId
            };
        }

        #endregion
    }
}
