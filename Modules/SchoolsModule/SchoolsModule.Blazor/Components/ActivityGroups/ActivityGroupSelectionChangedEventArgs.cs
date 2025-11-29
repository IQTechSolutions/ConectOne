using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Blazor.Components.ActivityGroups
{
    /// <summary>
    /// Event arguments for when the selection of activity groups changes.
    /// </summary>
    public class ActivityGroupSelectionChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityGroupSelectionChangedEventArgs"/> class.
        /// </summary>
        /// <param name="activityGroup">The activity group that was selected or deselected.</param>
        /// <param name="action">The action that was performed on the activity group.</param>
        public ActivityGroupSelectionChangedEventArgs(ActivityGroupDto? activityGroup, ActivityGroupSelectionAction action, LearnerDto learner)
        {
            ActivityGroup = activityGroup;
            Action = action;
            Learner = learner;
        }

        /// <summary>
        /// Gets or sets the activity group that was selected or deselected.
        /// </summary>
        public ActivityGroupDto? ActivityGroup { get; set; }

        /// <summary>
        /// Gets or sets the action that was performed on the activity group.
        /// </summary>
        public ActivityGroupSelectionAction Action { get; set; }

        /// <summary>
        /// Gets or sets the learner associated with this instance.
        /// </summary>
        public LearnerDto Learner { get; set; }
    }

    /// <summary>
    /// Enum representing the possible actions that can be performed on an activity group.
    /// </summary>
    public enum ActivityGroupSelectionAction
    {
        /// <summary>
        /// Indicates that an activity group was added.
        /// </summary>
        Add,

        /// <summary>
        /// Indicates that an activity group was removed.
        /// </summary>
        Remove,

        /// <summary>
        /// Indicates that a team member was added to the activity group.
        /// </summary>
        TeamMemberAdded,

        /// <summary>
        /// Indicates that a team member was removed from the activity group.
        /// </summary>
        TeamMemberRemoved,

        /// <summary>
        /// Indicates that the team members of the activity group were updated.
        /// </summary>
        TeamMembersUpdated
    }
}