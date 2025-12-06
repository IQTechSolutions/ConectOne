using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a template for an itinerary entry item, including details such as the day number, content,  and
    /// associated vacation information.
    /// </summary>
    /// <remarks>This class is used to define the structure of an itinerary entry within a vacation. Each
    /// entry is  associated with a specific day and may include additional content or details.</remarks>
    public class ItineraryEntryItemTemplate : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the day number within a specific context, such as a calendar or schedule.
        /// </summary>
        public int DayNr { get; set; } 

        /// <summary>
        /// Gets or sets the content associated with this instance.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))]public string? VacationId { get; set; }

        /// <summary>
        /// Gets or sets the vacation details for the current user.
        /// </summary>
        public Vacation? Vacation { get; set; }
    }
}
