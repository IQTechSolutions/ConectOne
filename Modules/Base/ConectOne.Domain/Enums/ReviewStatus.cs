using System.ComponentModel;

namespace ConectOne.Domain.Enums
{
    /// <summary>
    /// Represents the status of a review process.
    /// </summary>
    /// <remarks>This enumeration is used to indicate the current state of a review.  The possible values are:
    /// <list type="bullet"> <item><term><see cref="Pending"/></term><description>The review has not started
    /// yet.</description></item> <item><term><see cref="Approved"/></term><description>The review has been completed
    /// and approved.</description></item> <item><term><see cref="Rejected"/></term><description>The review has been
    /// completed and rejected.</description></item> <item><term><see cref="InReview"/></term><description>The review is
    /// currently in progress.</description></item> </list></remarks>
    public enum ReviewStatus
    {
        /// <summary>
        /// Indicates that the operation or request is pending and has not yet been completed or processed.
        /// </summary>
        [Description("Pending")] Pending = 0,

        /// <summary>
        /// Indicates that the item has been approved.
        /// </summary>
        [Description("Approved")] Approved = 1,

        /// <summary>
        /// Indicates that the request or operation was rejected.
        /// </summary>
        [Description("Rejected")] Rejected = 2,

        /// <summary>
        /// Indicates that the item is currently under review.
        /// </summary>
        [Description("In Review")] InReview = 3
    }
}
