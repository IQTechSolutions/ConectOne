using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents the relationship between a golf course and a destination.
    /// </summary>
    /// <remarks>This class is used to associate a specific golf course with a destination, enabling scenarios
    /// such as mapping golf courses to travel destinations or organizing related entities in a database. Each instance
    /// of <see cref="GolfCourseDestination"/> links one golf course to one destination.</remarks>
    public class GolfCourseDestination : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the identifier of the associated golf course.
        /// </summary>
        [ForeignKey(nameof(GolfCourse))] public string? GolfCourseId { get; set; }

        /// <summary>
        /// Gets or sets the golf course associated with the current context.
        /// </summary>
        public GolfCourse? GolfCourse { get; set; } 

        /// <summary>
        /// Gets or sets the unique identifier of the associated destination.
        /// </summary>
        [ForeignKey(nameof(Destination))] public string? DestinationId { get; set; }

        /// <summary>
        /// Gets or sets the destination associated with the current operation.
        /// </summary>
        public Destination? Destination { get; set; }
    }
}
