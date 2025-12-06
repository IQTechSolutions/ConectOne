using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for bed type information.
    /// </summary>
    /// <remarks>This class is used to encapsulate bed type details for various operations, such as
    /// transferring data  between layers or mapping to domain models. It supports initialization from multiple source
    /// types,  including view models and domain entities.</remarks>
    public record BedTypeDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BedTypeDto"/> class.
        /// </summary>
        public BedTypeDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BedTypeDto"/> class using the specified <see cref="BedType"/>
        /// object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="BedType"/> object to
        /// the corresponding properties of the <see cref="BedTypeDto"/> instance.</remarks>
        /// <param name="bedType">The <see cref="BedType"/> object containing the data to initialize the DTO. Cannot be null.</param>
        public BedTypeDto(BedType bedType)
        {
            BedTypeId = bedType.Id;
            PartnerBedTypeId = bedType.PartnerBedTypeId;
            Description = bedType.Description;
            BedCount = bedType.BedCount;
            RoomId = bedType.RoomId;
        }

        #endregion

        /// <summary>
        /// Gets or sets the identifier for the bed type.
        /// </summary>
        public string? BedTypeId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the bed type as defined by the partner system.
        /// </summary>
        public string? PartnerBedTypeId { get; set; }

        /// <summary>
        /// Gets or sets the description of the item.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of beds available.
        /// </summary>
        public int BedCount { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the room.
        /// </summary>
        public int? RoomId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this entry is the default.
        /// </summary>
        public bool DefaultEntry { get; set; }

        /// <summary>
        /// Converts the current instance to a <see cref="BedType"/> object.
        /// </summary>
        /// <remarks>This method creates a new <see cref="BedType"/> instance and populates its properties
        /// using the corresponding values from the current object.</remarks>
        /// <returns>A <see cref="BedType"/> object containing the bed count, description, room ID, and partner bed type ID from
        /// the current instance.</returns>
        public BedType ToBedType()
        {
            return new BedType()
            {
                BedCount = this.BedCount,
                Description = this.Description,
                RoomId = this.RoomId,
                PartnerBedTypeId  = this.PartnerBedTypeId,
            };
        }
    }
}
