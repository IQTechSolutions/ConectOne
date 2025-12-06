using AccomodationModule.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the view model for adding an amenity to a lodging or room in the owner portal.
    /// </summary>
    /// <remarks>This view model is used to populate the modal dialog for selecting and adding amenities. It
    /// includes information about the lodging or room, the type of amenity being added, and a list of available
    /// amenities.</remarks>
    public class AddOwnerAmenityModalViewModel 
    {
        /// <summary>
        /// Gets or sets the unique identifier for the lodging entity.
        /// </summary>
        public string? LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the room.
        /// </summary>
		public string? RoomId { get; set; }

        /// <summary>
        /// Gets or sets the type of amenity associated with the entity.
        /// </summary>
		public AmenityType AmenityType { get; set; } = AmenityType.Lodging;

        /// <summary>
        /// Gets or sets the unique identifier for an amenity.
        /// </summary>
        public int AmenityId { get; set; }

        /// <summary>
        /// Gets or sets the collection of available amenities for selection.
        /// </summary>
        public ICollection<SelectListItem> AvailableAmenities { get; set; } = [];
    }
    
}
