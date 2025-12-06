using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a template for scheduling and organizing a meeting or gathering. Provides details such as the time
    /// description, contact person, location, and related information.
    /// </summary>
    /// <remarks>This class is designed to encapsulate the essential details required for planning a meeting
    /// or gathering. It includes properties for specifying the time, location, and contact person responsible for the
    /// event. The <see cref="MGPersonId"/> property serves as a foreign key linking to the <see cref="Contact"/>
    /// entity.</remarks>
    public class MeetAndGreetTemplate : EntityBase<string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description associated with the object.
        /// </summary>
        public string? Description { get; set; }


        /// <summary>
        /// Gets or sets the location of the meeting or gathering.
        /// This can include details such as the venue name, address, or specific coordinates.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Gets or sets a description of the time, which may include details such as the time of day or a specific
        /// time-related context.
        /// </summary>
        public string? TimeDescription { get; set; }
        
        #endregion

        #region One-to-Many Relationships

        /// <summary>
        /// Gets or sets the foreign key reference to the contact person responsible for the meeting or gathering.
        /// This property links to the <see cref="Contact"/> entity representing the responsible individual.
        /// </summary>
        [ForeignKey(nameof(Contact))] public string? ContactId { get; set; }

        /// <summary>
        /// Gets or sets the contact details of the person responsible for the meeting or gathering.
        /// This includes information such as the name, phone number, and email of the contact person.
        /// </summary>
        public Contact? Contact { get; set; }

        #endregion
    }
}
