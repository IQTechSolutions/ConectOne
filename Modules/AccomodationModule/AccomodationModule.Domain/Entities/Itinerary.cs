using System.ComponentModel.DataAnnotations.Schema;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents an itinerary for a vacation, including details about the date, name, and associated itinerary items.
    /// </summary>
    public class Itinerary : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Itinerary"/> class.
        /// </summary>
        public Itinerary() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Itinerary"/> class by copying another instance and setting a new vacation ID.
        /// </summary>
        /// <param name="itinerary">The itinerary to copy.</param>
        /// <param name="newVacationIntervalId">The new vacation ID to set.</param>
        public Itinerary(Itinerary itinerary, string newVacationIntervalId)
        {
            Date = itinerary.Date;
            StartTime = itinerary.StartTime;
            EndTime = itinerary.EndTime;
            ItineraryType = itinerary.ItineraryType;
            Notes = itinerary.Notes;
            VacationIntervalId = newVacationIntervalId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the start date of the itinerary.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the start time of the itinerary.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the itinerary.
        /// </summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Gets or sets the guest type for the itinerary item.
        /// </summary>
        public ItineraryType ItineraryType { get; set; }

        /// <summary>
        /// Gets or sets the notes for the itinerary.
        /// </summary>
        public string? Notes { get; set; }

        #endregion

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the ID of the associated vacation interval.
        /// </summary>
        [ForeignKey(nameof(VacationInterval))] public string? VacationIntervalId { get; set; }
        
        /// <summary>
        /// Gets or sets the associated vacation interval.
        /// </summary>
        public VacationInterval? VacationInterval { get; set; }

        #endregion

        #region Many-To-One Relationships

        /// <summary>
        /// Gets or sets the collection of itinerary items associated with the itinerary.
        /// </summary>
        public ICollection<ItineraryItem> ItineraryDetails { get; set; } = [];

        #endregion

        #region Methods

        /// <summary>
        /// Creates a clone of the current itinerary with a new vacation ID.
        /// </summary>
        /// <param name="vacationId">The new vacation ID to set.</param>
        /// <returns>A new instance of the <see cref="Itinerary"/> class.</returns>
        public Itinerary Clone(string vacationId)
        {
            return new Itinerary(this, vacationId);
        }

        #endregion
    }
}
