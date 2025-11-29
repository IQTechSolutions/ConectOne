using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for capturing consent information related to an event.
    /// </summary>
    /// <remarks>This class is used to encapsulate details about a learner's consent for a specific event, 
    /// including their identification, grade, and team association. It is typically used for  transferring data between
    /// application layers or services.</remarks>
    public class EventConsentDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventConsentDto"/> class.
        /// </summary>
        public EventConsentDto(){}

        /// <summary>
        /// Initializes a new instance of the <see cref="EventConsentDto"/> class with the specified learner, consent,
        /// and team information.
        /// </summary>
        /// <param name="learner">The learner whose information is used to populate the DTO. Cannot be <see langword="null"/>.</param>
        /// <param name="consent">The consent status of the learner for the event. Can be <see langword="null"/> if no consent is provided.</param>
        /// <param name="team">The name of the team associated with the learner. Cannot be <see langword="null"/> or empty.</param>
        public EventConsentDto(Learner learner, string? consent, string team)
        {
            Id = learner.Id;
            Learner = $"{learner.FirstName} {learner.LastName}";
            Grade = learner.SchoolGrade?.Name;
            IdNumber = learner.IdNumber;
            Consent = consent;
            Team = team;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the learner.
        /// </summary>
        public string Learner { get; set; }

        /// <summary>
        /// Gets or sets the grade associated with the entity.
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the identification number associated with the entity.
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// Gets or sets the consent status as a string value.
        /// </summary>
        public string? Consent { get; set; }

        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        public string Team { get; set; }
    }
}
