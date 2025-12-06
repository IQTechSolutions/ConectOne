using System.ComponentModel;

namespace AccomodationModule.Domain.Enums
{
    /// <summary>
    /// Represents the type of guest in a system, such as a golfer or non-golfer.
    /// </summary>
    /// <remarks>This enumeration is typically used to categorize guests based on their activities or roles.
    /// For example, it can distinguish between guests who participate in golfing activities and those who do
    /// not.</remarks>
    public enum GuestType
    {
        /// <summary>
        /// Represents a value indicating all possible options or states.
        /// </summary>
        /// <remarks>This value is typically used to specify that all items, options, or states should be
        /// included or considered.</remarks>
        [Description("All")] All,

        /// <summary>
        /// Represents a golfer in the system.
        /// </summary>
        /// <remarks>This class is used to store and manage information about a golfer, such as their
        /// name, score, or other relevant details.</remarks>
        [Description("Golfer")] Golfer,

        /// <summary>
        /// Represents a non-golfer designation.
        /// </summary>
        /// <remarks>This value is typically used to identify individuals who do not participate in
        /// golfing activities.</remarks>
        [Description("Non-Golfer")] NonGolfer
    }
}
