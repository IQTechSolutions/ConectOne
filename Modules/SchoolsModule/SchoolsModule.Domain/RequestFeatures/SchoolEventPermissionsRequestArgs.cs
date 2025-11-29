namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents the arguments required to request permissions for a school event.
    /// </summary>
    /// <remarks>This class encapsulates the necessary identifiers and optional details needed to determine
    /// permissions for a specific school event. At a minimum, the <see cref="EventId"/> must be provided. Additional
    /// properties, such as <see cref="ParentId"/>, <see cref="ParentEmail"/>, <see cref="LearnerId"/>, or <see
    /// cref="ActivityGroupId"/>, can be supplied to refine the request context.</remarks>
    public class SchoolEventPermissionsRequestArgs
    {
        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        public string EventId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the parent entity.
        /// </summary>
        public string? ParentId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email address of the parent.
        /// </summary>
        public string? ParentEmail { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the learner.
        /// </summary>
        public string? LearnerId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier for the activity group associated with this entity.
        /// </summary>
        public string? ActivityGroupId { get; set; } = null!;
    }
}
