using System.ComponentModel.DataAnnotations.Schema;
using AccomodationModule.Domain.DataTransferObjects;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a package that includes lodging, rooms, and bookings, along with associated metadata.
    /// </summary>
    /// <remarks>A package is a collection of related entities, such as lodging, rooms, and bookings, that can
    /// be used  to define and manage accommodations. It includes descriptions, availability, and special rate
    /// identifiers.</remarks>
    public class LodgingPackage : EntityBase<int>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the LodgingPackage class.
        /// </summary>
        public LodgingPackage() { } 

        /// <summary>
        /// Initializes a new instance of the LodgingPackage class using the specified data transfer object.
        /// </summary>
        /// <param name="dto">An object containing the data used to populate the properties of the LodgingPackage. Cannot be null.</param>
        public LodgingPackage(LodgingPackageDto dto)
        {
            Id = dto.PackageId;
            ShortDescription = dto.ShortDescription;
            LongDescription = dto.LongDescription;
            Deleted = dto.Deleted;
            AvailablePartnerUid = dto.AvailablePartnerUid;
            SpecialRateId = dto.SpecialRateId;
            LodgingId = dto.LodgingId;
        }

        #endregion

        /// <summary>
        /// Gets or sets a brief description of the item or entity.
        /// </summary>
        public string? ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the detailed description of the item or entity.
        /// </summary>
        public string LongDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the entity is marked as deleted.
        /// </summary>
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// Gets or sets the unique identifier of the available partner.
        /// </summary>
        public string? AvailablePartnerUid { get; set; }

        /// <summary>
        /// Gets or sets the identifier for a special rate.
        /// </summary>
        public string SpecialRateId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier for the associated lodging entity.
        /// </summary>
        [ForeignKey(nameof(Lodging))] public string? LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the lodging details associated with the current entity.
        /// </summary>
        public Lodging? Lodging { get; set; }

        /// <summary>
        /// Gets or sets the collection of rooms associated with the entity.
        /// </summary>
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

        /// <summary>
        /// Gets or sets the collection of bookings associated with this entity.
        /// </summary>
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
