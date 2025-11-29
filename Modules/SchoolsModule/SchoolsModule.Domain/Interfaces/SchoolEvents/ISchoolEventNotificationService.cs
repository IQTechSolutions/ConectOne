using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Enums;

namespace SchoolsModule.Domain.Interfaces.SchoolEvents
{
    /// <summary>
    ///     Contract for services that build recipient lists for notifications
    ///     related to <c>SchoolEvent</c> entities.
    ///     <para>
    ///         Implementations must be <b>query‑only</b>; they return lists but do not
    ///         mutate the underlying data store.  That division of responsibility keeps
    ///         read logic independent from write logic and simplifies unit testing.
    ///     </para>
    /// </summary>
    public interface ISchoolEventNotificationService
    {
        /// <summary>
        ///     Calculates every unique parent and teacher that should receive a
        ///     generic “upcoming event” notification.
        /// </summary>
        /// <param name="schoolEventId">
        ///     Primary‑key of the event whose audience is being requested.
        /// </param>
        /// <param name="cancellationToken">
        ///     Token for cooperative cancellation—important when the query could
        ///     traverse large object graphs (many teams → many learners → many parents).
        /// </param>
        /// <returns>
        ///     A <see cref="SuccessResult{T}"/> containing a de‑duplicated collection of
        ///     <see cref="RecipientDto"/> objects on success, or a <see cref="FailResult{T}"/>
        ///     with error messages on failure.
        /// </returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> EventNotificationList(string schoolEventId, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Retrieves parents (and teachers for informational purposes) who still need
        ///     to supply a specific consent—optionally scoped to a single team or learner.
        /// </summary>
        /// <param name="consentType">
        ///     The type of consent in question (e.g. <see cref="ConsentTypes.Attendance"/>).
        /// </param>
        /// <param name="schoolEventId">Primary‑key of the target event.</param>
        /// <param name="activityGroupId">
        ///     Optional — when provided, the query is limited to the specified activity group.
        /// </param>
        /// <param name="learnerId">
        ///     Optional — when provided, the query is further limited to a single learner.
        ///     Useful when prompting an individual parent directly from a UI workflow.
        /// </param>
        /// <param name="cancellationToken">Token for cooperative cancellation.</param>
        /// <returns>
        ///     A <see cref="SuccessResult{T}"/> containing the list of recipients to notify,
        ///     or a <see cref="FailResult{T}"/> if the event cannot be found or another
        ///     error occurs.
        /// </returns>
        Task<IBaseResult<IEnumerable<RecipientDto>>> EventPermissionNotificationList(ConsentTypes consentType, string schoolEventId, string? activityGroupId = null, string? learnerId = null, CancellationToken cancellationToken = default);
    }
}
