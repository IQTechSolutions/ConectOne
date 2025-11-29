using System.ComponentModel;

namespace PayFast
{
    /// <summary>
    /// Specifies the possible statuses for a result or operation.
    /// </summary>
    /// <remarks>Use this enumeration to represent the current state of a process, task, or result. The values
    /// indicate whether the operation is active, cancelled, paused, complete, under review, failed, or system-defined.
    /// The meaning of each status may vary depending on the context in which it is used.</remarks>
    public enum ResultStatus
    {
        /// <summary>
        /// Indicates that the item is active.
        /// </summary>
        [Description("Active")] Active = 1,

        /// <summary>
        /// Indicates that the operation was cancelled before completion.
        /// </summary>
        [Description("Cancelled")] Cancelled = 2,

        /// <summary>
        /// Indicates that the operation or process is currently paused and not actively progressing.
        /// </summary>
        [Description("Paused")] Paused = 3,

        /// <summary>
        /// Indicates that the operation or process has completed successfully.
        /// </summary>
        [Description("Complete")] Complete = 4,

        /// <summary>
        /// Indicates that the item is currently under review.
        /// </summary>
        [Description("In Review")] InReview = 5,

        /// <summary>
        /// Indicates that the operation has failed.
        /// </summary>
        [Description("Failed")] Failed = 6,

        /// <summary>
        /// Represents the system log level used for logging system-related events.
        /// </summary>
        [Description("System")] System = 7
    }
}
