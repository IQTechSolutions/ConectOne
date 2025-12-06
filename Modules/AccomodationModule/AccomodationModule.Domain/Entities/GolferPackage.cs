using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a golfer package associated with a vacation.
    /// Includes details such as date, start time, notes, and associated relationships.
    /// </summary>
    public class GolferPackage : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolferPackage"/> class with default values.
        /// </summary>
        public GolferPackage() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GolferPackage"/> class by copying another instance
        /// and setting a new vacation ID.
        /// </summary>
        /// <param name="itinerary">The golfer package to copy.</param>
        /// <param name="vacationId">The new vacation ID to set.</param>
        public GolferPackage(GolferPackage golferPackage)
        {
            DayNr = golferPackage.DayNr;
            StartTime = golferPackage.StartTime;
            Notes = golferPackage.Notes;
            Carts = golferPackage.Carts;
            Caddies = golferPackage.Caddies;
            Halfway = golferPackage.Halfway;
            GolfCourseId = golferPackage.GolfCourseId;
            VacationId = golferPackage.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the day number represented by this instance.
        /// </summary>
        public int DayNr { get; set; }

        /// <summary>
        /// Gets or sets the start time of the golfer package.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// Gets or sets additional notes for the golfer package.
        /// </summary>
        [MaxLength(5000)] public string? Notes { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether carts are included in the golfer package.
        /// </summary>
        public bool Carts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether caddies are included in the golfer package.
        /// </summary>
        public bool Caddies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether halfway refreshments are included in the golfer package.
        /// </summary>
        public bool Halfway { get; set; }

        #endregion

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the foreign key reference to the associated golf course.
        /// </summary>
        [ForeignKey(nameof(GolfCourse))] public string? GolfCourseId { get; set; }

        /// <summary>
        /// Navigation property to the associated golf course.
        /// </summary>
        public GolfCourse? GolfCourse { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? VacationId { get; set; }

        /// <summary>
        /// Navigation property to the associated vacation.
        /// </summary>
        public Vacation? Vacation { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a clone of the current golfer package with a new vacation ID.
        /// </summary>
        /// <param name="vacationId">The new vacation ID to set.</param>
        /// <returns>A new instance of the <see cref="GolferPackage"/> class.</returns>
        public GolferPackage Clone()
        {
            return new GolferPackage(this){ Id = Guid.NewGuid().ToString()};
        }

        #endregion
    }
}
