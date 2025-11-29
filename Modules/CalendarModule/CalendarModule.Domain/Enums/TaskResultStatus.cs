using System.ComponentModel;

namespace CalendarModule.Domain.Enums
{
    /// <summary>
    /// Represents the status of a task in a workflow or process.
    /// </summary>
    /// <remarks>This enumeration is used to indicate the current state of a task.  The possible values are:
    /// <list type="bullet"> <item> <description><see cref="ToDo"/>: The task is pending and has not yet
    /// started.</description> </item> <item> <description><see cref="Completed"/>: The task has been successfully
    /// finished.</description> </item> <item> <description><see cref="Busy"/>: The task is currently in
    /// progress.</description> </item> <item> <description><see cref="Cancelled"/>: The task has been terminated before
    /// completion.</description> </item> </list></remarks>
    public enum TaskResultStatus
    {
        /// <summary>
        /// Represents a task or item that needs to be completed.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate a placeholder or an item that
        /// requires further implementation or action.</remarks>
        [Description("To Do")] ToDo,

        /// <summary>
        /// Represents a completed state.
        /// </summary>
        [Description("Completed")] Completed,

        /// <summary>
        /// Represents a state indicating that the system or process is currently busy.
        /// </summary>
        [Description("Busy")] Busy,

        /// <summary>
        /// Represents a state where the operation has been cancelled.
        /// </summary>
        /// <remarks>This enumeration value is typically used to indicate that a process or task was
        /// intentionally stopped  before completion. Use this value to handle scenarios where cancellation is a valid
        /// outcome.</remarks>
        [Description("Cancelled")] Cancelled,
    }
}
