using System.ComponentModel.DataAnnotations;
using FilingModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a destination, including its name, description, and associated entities such as golf courses and
    /// lodgings.
    /// </summary>
    /// <remarks>This class provides properties to define the destination's details and relationships to other
    /// entities. The <see cref="Name"/> and <see cref="Description"/> properties are constrained by maximum
    /// lengths.</remarks>
    public class Destination : FileCollection<Destination, string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// </summary>
        [MaxLength(1000)] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description text, limited to a maximum length of 5000 characters.
        /// </summary>
        [MaxLength(5000)] public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the proposal.
        /// </summary>
        [MaxLength(5000)] public string? OnlineDescription { get; set; }

        /// <summary>
        /// Gets or sets the address associated with the entity.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude component of a geographic coordinate.
        /// </summary>
        public double Lng { get; set; }

        #endregion

        #region Many-To-One Relationships

        /// <summary>
        /// Gets or sets the collection of destinations associated with the golf course.
        /// </summary>
        public ICollection<GolfCourseDestination> Destinations { get; set; }

        /// <summary>
        /// Gets or sets the collection of lodging destinations associated with this entity.
        /// </summary>
        public ICollection<LodgingDestination> Lodgings { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of videos associated with the destination.
        /// </summary>
        public ICollection<EntityVideo<Destination, string>> Videos { get; set; } = [];

        #endregion
    }
}
