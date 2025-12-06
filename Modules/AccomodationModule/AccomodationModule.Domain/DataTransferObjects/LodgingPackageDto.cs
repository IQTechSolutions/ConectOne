using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data transfer object for a package, used to transfer package-related data between layers of the application.
    /// </summary>
    public record LodgingPackageDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageDto"/> class.
        /// </summary>
        public LodgingPackageDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageDto"/> class with specified parameters.
        /// </summary>
        /// <param name="packageId">The package ID.</param>
        /// <param name="shortDescription">The short description of the package.</param>
        /// <param name="longDescription">The long description of the package.</param>
        /// <param name="lodgingId">The lodging ID associated with the package.</param>
        /// <param name="availablePartnerUid">The available partner UID.</param>
        /// <param name="specialRateId">The special rate ID.</param>
        /// <param name="rooms">The collection of rooms associated with the package.</param>
        /// <param name="deleted">Indicates whether the package is deleted.</param>
        public LodgingPackageDto(int packageId, string? shortDescription, string longDescription, string lodgingId, string availablePartnerUid, string specialRateId, ICollection<Room> rooms, bool deleted = false)
        {
            PackageId = packageId;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            LodgingId = lodgingId;
            AvailablePartnerUid = availablePartnerUid;
            SpecialRateId = specialRateId;
            Deleted = deleted;

            Rooms = rooms?.Select(g => new RoomDto(g))?.ToList();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PackageDto"/> class from a <see cref="Package"/> entity.
        /// </summary>
        /// <param name="model">The package entity.</param>
        public LodgingPackageDto(LodgingPackage model)
        {
            PackageId = model.Id;
            ShortDescription = model.ShortDescription;
            LongDescription = model.LongDescription;
            LodgingId = model.LodgingId;
            AvailablePartnerUid = model.AvailablePartnerUid;
            Deleted = model.Deleted;
            SpecialRateId = model.SpecialRateId;

            Rooms = model.Rooms.Select(g => new RoomDto(g)).ToList();
        }

        #endregion

        /// <summary>
        /// Gets or sets the package ID.
        /// </summary>
        public int PackageId { get; set; }

        /// <summary>
        /// Gets or sets the short description of the package.
        /// </summary>
        public string? ShortDescription { get; set; } 

        /// <summary>
        /// Gets or sets the long description of the package.
        /// </summary>
        public string LongDescription { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the package is deleted.
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the lodging ID associated with the package.
        /// </summary>
        public string LodgingId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the available partner UID.
        /// </summary>
        public string? AvailablePartnerUid { get; set; }

        /// <summary>
        /// Gets or sets the special rate ID.
        /// </summary>
        public string SpecialRateId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of rooms associated with the package.
        /// </summary>
        public List<RoomDto> Rooms { get; set; } = [];

        /// <summary>
        /// Converts the current instance to a <see cref="Package"/> entity.
        /// </summary>
        /// <returns>A new <see cref="Package"/> entity.</returns>
        public LodgingPackage ToPackage()
        {
            var package = new LodgingPackage
            {
                ShortDescription = this.ShortDescription,
                LongDescription = this.LongDescription,
                AvailablePartnerUid = this.AvailablePartnerUid,
                LodgingId = this.LodgingId
            };

            return package;
        }
    }
}