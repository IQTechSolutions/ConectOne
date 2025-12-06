using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Represents the status of a booking in the system.
    /// </summary>
    /// <remarks>This enumeration defines the various states a booking can be in, such as pending, active,
    /// completed, or cancelled. Use this to track and manage the lifecycle of a booking.</remarks>
    public enum BookingStatus
    {
        /// <summary>
        /// Represents a state where the operation is pending and has not yet completed.
        /// </summary>
        [Description("Pending")] Pending = 0,

        /// <summary>
        /// Represents the active state of an entity or operation.
        /// </summary>
        [Description("Active")] Active = 1,

        /// <summary>
        /// Represents the state of an operation that has been successfully completed.
        /// </summary>
        [Description("Completed")] Completed = 2,

        /// <summary>
        /// Represents the state of an operation that has been cancelled.
        /// </summary>
        [Description("Cancelled")] Cancelled = 3,

        /// <summary>
        /// Represents the manual confirmation status in the system.
        /// </summary>
        /// <remarks>This status indicates that a manual action is required to confirm the operation. It
        /// is typically used in workflows or processes where automated confirmation is not possible.</remarks>
        [Description("Manual Conformation")] ManualConformation = 4,
    }
}
