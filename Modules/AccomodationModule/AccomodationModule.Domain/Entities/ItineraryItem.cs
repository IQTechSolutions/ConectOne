using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents an item in an itinerary, including details about the date, time, guest type, and notes.
    /// </summary>
    public class ItineraryItem : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryItem"/> class.
        /// </summary>
        public ItineraryItem() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItineraryItem"/> class by copying another instance and setting a new itinerary ID.
        /// </summary>
        /// <param name="itineraryItem">The itinerary item to copy.</param>
        /// <param name="itineraryId">The new itinerary ID to set.</param>
        public ItineraryItem(ItineraryItem itineraryItem, string itineraryId)
        {
            StartTime = itineraryItem.StartTime;
            EndTime = itineraryItem.EndTime;
            Description = itineraryItem.Description;
            ItineraryId = itineraryId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the start time of the itinerary item.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the itinerary item.
        /// </summary>
        public DateTime EndTime { get; set; }
        
        /// <summary>
        /// Gets or sets the notes for the itinerary item.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        #endregion

        #region One-To-Many Relationships

        /// <summary>
        /// Gets or sets the ID of the associated itinerary.
        /// </summary>
        [ForeignKey(nameof(Itinerary))] public string? ItineraryId { get; set; }

        /// <summary>
        /// Navigation property to the associated itinerary.
        /// </summary>
        public Itinerary? Itinerary { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the itinerary item.
        /// </summary>
        /// <returns>A string that represents the itinerary item.</returns>
        public override string ToString()
        {
            return $"Itinerary Item";
        }

        /// <summary>
        /// Creates a clone of the current itinerary item with a new itinerary ID.
        /// </summary>
        /// <param name="itineraryId">The new itinerary ID to set.</param>
        /// <returns>A new instance of the <see cref="ItineraryItem"/> class.</returns>
        public ItineraryItem Clone(string itineraryId)
        {
            return new ItineraryItem(this, itineraryId);
        }

        #endregion
    }
}
