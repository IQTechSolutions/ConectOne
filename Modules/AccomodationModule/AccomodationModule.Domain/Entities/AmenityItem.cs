using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents an amenity item associated with a lodging, room, or other entity.
    /// </summary>
    /// <remarks>This class provides a way to associate amenities with specific entities, such as lodgings or
    /// rooms. It supports multiple constructors for initializing the association based on the entity type.</remarks>
    /// <typeparam name="TEntity">The type of the entity associated with the amenity item.</typeparam>
    /// <typeparam name="TId">The type of the identifier for the entity.</typeparam>
    public class AmenityItem<TEntity, TId> : EntityBase<TId>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenityItem"/> class.
        /// </summary>
        public AmenityItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenityItem"/> class with the specified lodging ID and amenity
        /// ID.
        /// </summary>
        /// <param name="lodgingId">The unique identifier for the lodging associated with the amenity. Cannot be null or empty.</param>
        /// <param name="amenityId">The unique identifier for the amenity. Must be a positive integer.</param>
        public AmenityItem(string lodgingId, int amenityId) 
        {
			LodgingId = lodgingId;
            AmenityId = amenityId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmenityItem"/> class with the specified room and amenity
        /// identifiers.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room associated with the amenity.</param>
        /// <param name="amenityId">The unique identifier of the amenity associated with the room.</param>
		public AmenityItem(int roomId, int amenityId)
		{
			RoomId = roomId;
			AmenityId = amenityId;
		}

		#endregion

        /// <summary>
        /// Gets or sets the unique identifier for the associated amenity.
        /// </summary>
		[ForeignKey(nameof(Amenity))] public int AmenityId { get; set; }

        /// <summary>
        /// Gets or sets the amenity associated with the current entity.
        /// </summary>
        public Amenity Amenity { get; set;}


        /// <summary>
        /// Gets or sets the unique identifier for the associated lodging entity.
        /// </summary>
        [ForeignKey(nameof(Lodging))] public string? LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the lodging details associated with the current entity.
        /// </summary>
        public Lodging? Lodging { get; set; }

        
        /// <summary>
        /// Gets or sets the identifier of the associated room.
        /// </summary>
		[ForeignKey(nameof(Room))] public int? RoomId { get; set; }

        /// <summary>
        /// Gets or sets the room associated with the current context.
        /// </summary>
		public Room? Room { get; set; }

        /// <summary>
        /// Returns a string representation of the amenity item, including the name of the associated entity type.
        /// </summary>
        /// <returns>A string that identifies the amenity item and its associated entity type.</returns>
        public override string ToString()
        {
            return $"Amenity Item for {typeof(TEntity).Name}";
        }
    }
}
