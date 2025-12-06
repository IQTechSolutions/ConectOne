using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents the association between a vacation and a contact.
    /// </summary>
    /// <remarks>This class establishes a relationship between a vacation and a contact,  allowing for the
    /// management of contacts associated with specific vacations.</remarks>
    public class VacationContact : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the unique identifier for the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the vacation details for the current user or entity.
        /// </summary>
        public Vacation Vacation { get; set; }
        
        /// <summary>
        /// Gets or sets the unique identifier for the associated contact.
        /// </summary>
        [ForeignKey(nameof(Contact))] public string ContactId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the contact information associated with this entity.
        /// </summary>
        public Contact Contact { get; set; } = null!;
    }
}
