using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using IdentityModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices.Learners
{
    /// <summary>
    /// Provides methods for retrieving notification-related data for learners, including lists of recipients for
    /// learner-specific and global notifications.
    /// </summary>
    /// <remarks>This service interacts with a REST API to fetch notification recipient data. It is designed
    /// to be used in scenarios where learner-specific or global notification recipient lists are required.</remarks>
    /// <param name="provider"></param>
    public class LearnerNotificationRestService(IBaseHttpProvider provider) : ILearnerNotificationService
    {
        /// <summary>
        /// Retrieves a list of learners' notifications based on the specified pagination parameters.
        /// </summary>
        /// <remarks>This method communicates with an external provider to retrieve the notifications.
        /// Ensure that the <paramref name="learnerPageParameters"/> object is properly configured before calling this
        /// method.</remarks>
        /// <param name="learnerPageParameters">The pagination parameters used to filter and retrieve the learners' notifications.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// containing an enumerable collection of <see cref="RecipientDto"/> objects representing the learners'
        /// notifications.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> LearnersNotificationList(LearnerPageParameters learnerPageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>("learners/notificationList");
            return result;
        }

        /// <summary>
        /// Retrieves the global list of mail recipients.
        /// </summary>
        /// <remarks>This method fetches the global notification list of recipients from the underlying
        /// provider. The returned result may be empty if no recipients are available.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// containing an enumerable collection of <see cref="RecipientDto"/> objects representing the global mail
        /// recipients.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> GlobalMailRecipientList(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<RecipientDto>>("learners/notificationList/global");
            return result;
        }
    }
}
