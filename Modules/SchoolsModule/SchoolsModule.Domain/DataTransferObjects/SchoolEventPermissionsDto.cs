using MessagingModule.Domain.Enums;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the permissions and consents associated with a learner's participation in a school event.
    /// </summary>
    /// <remarks>This data transfer object encapsulates information about a learner's attendance and transport
    /// consents, as well as their association with a specific activity group. It provides properties to determine
    /// whether consents are required and to retrieve consent statuses.</remarks>
    public record SchoolEventPermissionsDto
    {
        /// <summary>
        /// Gets or sets the learner associated with the current context.
        /// </summary>
        public LearnerDto Learner { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets the identifier of the participating activity group.
        /// </summary>
        public string ParticipatingActivityGroupId { get; set; }

        /// <summary>
        /// Gets or sets the activity group associated with the current context.
        /// </summary>
        public ActivityGroupDto ActivityGroup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether consent for attendance has been given.
        /// </summary>
        public bool? AttendanceConsentGiven { get; set; }

        /// <summary>
        /// Gets a value indicating whether attendance consent is required.
        /// </summary>
        public bool RequireAttendanceConsent => !AttendanceConsentGiven.HasValue;

        /// <summary>
        /// Gets a value indicating whether attendance consent has been given.
        /// </summary>
        public bool AttendanceConsent => AttendanceConsentGiven.HasValue && AttendanceConsentGiven!.Value;

        /// <summary>
        /// Gets or sets a value indicating whether consent for transport has been provided.
        /// </summary>
        public bool? TransportConsentGiven { get; set; }

        /// <summary>
        /// Gets or sets the direction of consent for the current operation.
        /// </summary>
        public ConsentDirection? ConsentDirection { get; set; }

        /// <summary>
        /// Gets a description of the consent direction based on its current value.
        /// </summary>
        public string ConsentDirectionDescription
        {
            get
            {
                return ConsentDirection switch
                {
                    MessagingModule.Domain.Enums.ConsentDirection.ToAndFrom => "Consent Granted",
                    MessagingModule.Domain.Enums.ConsentDirection.To => "Consent Refused",
                    MessagingModule.Domain.Enums.ConsentDirection.From => "Consent Retracted",
                    _ => string.Empty
                };
            }
        }

        /// <summary>
        /// Gets a value indicating whether transport consent is required.
        /// </summary>
        public bool RequireTransportConsent => !TransportConsentGiven.HasValue;

        /// <summary>
        /// Gets a value indicating whether transport consent has been given.
        /// </summary>
        public bool TransportConsent => TransportConsentGiven.HasValue && TransportConsentGiven.Value;
    }
}
