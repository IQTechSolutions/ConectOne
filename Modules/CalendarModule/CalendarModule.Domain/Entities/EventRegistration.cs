using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Enums;

namespace CalendarModule.Domain.Entities
{
    /// <summary>
    /// Represents a registration record for an event.
    /// Stores basic attendee information, including name, contact details, number of attendees, and additional notes.
    /// </summary>
    public class EventRegistration : EntityBase<string>
    {
        /// <summary>
        /// The title of the attendee, such as Mr., Mrs., Dr., or a custom "Me" title (default).
        /// </summary>
        [DisplayName("Title")] public Title Title { get; set; } = Title.Me;

        /// <summary>
        /// The attendee's first name. Required for identification and personalized communication.
        /// </summary>
        [DisplayName("First Name"), Required] public string FirstName { get; set; } = null!;

        /// <summary>
        /// The attendee's last name. Required for identification and forms of address.
        /// </summary>
        [DisplayName("Last Name"), Required] public string LastName { get; set; } = null!;

        /// <summary>
        /// The attendee's primary contact number, required for updates, reminders, or last-minute changes.
        /// </summary>
        [DisplayName("Contact Nr"), Required] public string ContactNr { get; set; } = null!;

        /// <summary>
        /// The total number of attendees represented by this registration, including the primary attendee.
        /// Allows group bookings under one registration.
        /// </summary>
        [DisplayName("Amount")] public int Attendees { get; set; }

        /// <summary>
        /// The attendee's email address. Required for sending confirmation emails, event details, and digital tickets.
        /// </summary>
        [EmailAddress, DisplayName("Email Address"), Required] public string EmailAddress { get; set; } = null!;

        /// <summary>
        /// Optional notes or special requests the attendee might have.
        /// Could include dietary restrictions, accessibility requirements, or other instructions.
        /// </summary>
        [DataType(DataType.MultilineText)] public string? Notes { get; set; }
    }
}
