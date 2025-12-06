using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents the contact form data.
    /// </summary>
    public class VacationContactUsInfo : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// </summary>
        public string? Surname { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// </summary>
        public string Cell { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the subject of the message.
        /// </summary>
        public string Subject { get; set; } = null!;

        /// <summary>
        /// Gets or sets the website URL associated with the entity.
        /// </summary>
        public string? Website { get; set; } = null!;

        /// <summary>
        /// Gets or sets the message content.
        /// </summary>
        public string Message { get; set; } = null!;
    }
}
