namespace SchoolsModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the result of an event update operation, including the updated event details and any changes to
    /// associated team members or activity groups.
    /// </summary>
    /// <remarks>This record provides information about the updated state of a school event after an update
    /// operation. It includes the updated event details, as well as lists of team members and activity groups that were
    /// added or removed as part of the update.</remarks>
    public record EventUpdateResponse
    {
        /// <summary>
        /// Gets the school event associated with this instance.
        /// </summary>
        public SchoolEventDto SchoolEvent { get; init; } = null!;

        /// <summary>
        /// Gets the list of team members that were added.
        /// </summary>
        public List<LearnerDto> TeamMembersAdded { get; init; } = [];

        /// <summary>
        /// Gets the list of team members who have been removed.
        /// </summary>
        public List<LearnerDto> TeamMembersRemoved { get; init; } = [];

        /// <summary>
        /// Gets the list of activity groups that have been added.
        /// </summary>
        public List<ActivityGroupDto> ActivityGroupsAdded { get; init; } = [];

        /// <summary>
        /// Gets the list of activity groups that have been removed.
        /// </summary>
        public List<ActivityGroupDto> ActivityGroupsRemoved { get; init; } = [];
    }
}
