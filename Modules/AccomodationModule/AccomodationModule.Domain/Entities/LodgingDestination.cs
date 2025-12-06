using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents the relationship between a lodging entity and a destination,  providing details about their
    /// association.
    /// </summary>
    /// <remarks>This class is used to model the linkage between a lodging and a destination,  including their
    /// respective identifiers and associated details. It is typically  used in scenarios where a lodging is tied to a
    /// specific destination, such as  in travel or tourism applications.</remarks>
    public class LodgingDestination : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the unique identifier for the associated lodging entity.
        /// </summary>
        [ForeignKey(nameof(Lodging))] public string? LodgingId { get; set; } 

        /// <summary>
        /// Gets or sets the lodging details associated with the current entity.
        /// </summary>
        public Lodging Lodging { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated destination.
        /// </summary>
        [ForeignKey(nameof(Destination))] public string? DestinationId { get; set; } 

        /// <summary>
        /// Gets or sets the destination associated with the current operation.
        /// </summary>
        public Destination Destination { get; set; }
    }
}
