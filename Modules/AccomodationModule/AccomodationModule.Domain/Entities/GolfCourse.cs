using System.ComponentModel.DataAnnotations;
using FilingModule.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a golf course, including its name, description, amenities, and associated destination.
    /// </summary>
    /// <remarks>The <see cref="GolfCourse"/> class provides properties to describe the golf course, such as
    /// its name,  description, and available amenities (e.g., carts, caddies, golf clubs, and halfway facilities).  It
    /// also supports a relationship to a <see cref="Destination"/> object, allowing the golf course to be  associated
    /// with a specific destination.</remarks>
    public class GolfCourse : FileCollection<GolfCourse, string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// </summary>
        [MaxLength(1000)] public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description text associated with the entity.
        /// </summary>
        [MaxLength(5000)] public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the proposal.
        /// </summary>
        [MaxLength(5000)] public string? OnlineDescription { get; set; }

        /// <summary>
        /// Gets or sets the address associated with the entity.
        /// </summary>
        public string? Address { get; set; } = null!;

        /// <summary>
        /// Gets or sets the latitude coordinate of a geographic location.
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude component of a geographic coordinate.
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// Gets or sets the type of the course.
        /// </summary>
        public string? CourseType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether carts are enabled.
        /// </summary>
        public bool Carts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether caddies are enabled.
        /// </summary>
        public bool Caddies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether golf clubs are available.
        /// </summary>
        public bool GolfClubs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the golf course has halfway facilities.
        /// </summary>
        public bool Halfway { get; set; }

        /// <summary>
        /// Gets or sets the name of the designer associated with the object.
        /// </summary>
        public string? DesignedBy { get; set; }

        /// <summary>
        /// Gets or sets the ranking value associated with the entity.
        /// </summary>
        public int Ranking { get; set; }

        #endregion

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the destination associated with the current operation.
        /// </summary>
        public ICollection<GolfCourseDestination> Destinations { get; set; } = [];

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a string representation of the golf course.
        /// </summary>
        /// <returns>A string that represents the golf course.</returns>
        public override string ToString()
        {
            return $"Golf Course";
        }

        #endregion
    }
}