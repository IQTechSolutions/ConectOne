using MessagingModule.Domain.Enums;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for the ParentPermission entity.
    /// ParentPermission records capture whether a parent has granted or denied consent for a given event or activity
    /// involving a learner. It helps in ensuring that parental permissions are respected for events that require it.
    ///
    /// Key properties:
    /// - Id: Unique identifier of this permission record.
    /// - ConsentType: The type of consent required (e.g., Transport, Attendance), represented as a string.
    /// - Granted: Indicates whether the consent has been granted (true) or not (false).
    /// - ParentId: The ID of the parent who may grant or withhold consent.
    /// - LearnerId: The ID of the learner to whom this consent pertains.
    /// - EventId: The ID of the event or activity requiring the parent's consent.
    ///
    /// Use this DTO when displaying or managing parental consent records for specific learners and events.
    /// </summary>
    public record ParentPermissionDto
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentPermissionDto"/> class.
        /// </summary>
        public ParentPermissionDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentPermissionDto"/> class using the specified <see
        /// cref="ParentPermission"/> object.
        /// </summary>
        /// <param name="parentPermission">The <see cref="ParentPermission"/> object containing the data to initialize the DTO.  This parameter must
        /// not be <see langword="null"/>.</param>
        public ParentPermissionDto(ParentPermission parentPermission)
        {
            Id = parentPermission.Id;
            ConsentType = parentPermission.ConsentType.ToString();
            Granted = parentPermission.Granted;
            ParentId = parentPermission.ParentId;
            LearnerId = parentPermission.LearnerId;
            EventId = parentPermission.EventId;
        }

        #endregion

        /// <summary>
        /// The unique identifier of the parent permission record.
        /// </summary>
        public string Id { get; init; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The type of consent required, such as "Transport" or "Attendance".
        /// Defaults to "Unknown" if not specified.
        /// </summary>
        public string ConsentType { get; init; } = ConsentTypes.Attendance.ToString();

        /// <summary>
        /// Indicates if the consent is granted (true) or not (false).
        /// </summary>
        public bool Granted { get; init; }

        /// <summary>
        /// The ID of the parent associated with this permission.
        /// </summary>
        public string? ParentId { get; init; }

        /// <summary>
        /// The ID of the learner for whom the consent applies.
        /// </summary>
        public string? LearnerId { get; init; }

        /// <summary>
        /// The ID of the event that requires the parent's consent.
        /// </summary>
        public string? EventId { get; init; }
    }
}
