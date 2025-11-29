using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Blazor.Components.Learners
{
    /// <summary>
    /// Provides data for the event that occurs when the selection of learners changes.
    /// </summary>
    /// <remarks>This event argument contains the updated collection of selected learners and the associated
    /// activity group, if any.</remarks>
    /// <param name="learners"></param>
    /// <param name="activityGroup"></param>
    public class LearnerSelectionChangedEventArgs(ICollection<LearnerDto> learners, ActivityGroupDto? activityGroup)
    {
        /// <summary>
        /// Gets or sets the collection of learners associated with the current context.
        /// </summary>
        public ICollection<LearnerDto> Learners { get; set; } = learners;

        /// <summary>
        /// Gets or sets the activity group associated with the current context.
        /// </summary>
        public ActivityGroupDto? ActivityGroup { get; set; } = activityGroup;
    }
}
