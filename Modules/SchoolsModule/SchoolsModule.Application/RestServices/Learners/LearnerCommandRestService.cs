using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Learners;

namespace SchoolsModule.Application.RestServices.Learners
{
    /// <summary>
    /// Provides REST-based operations for managing learners, including creating, updating, and removing learner
    /// records.
    /// </summary>
    /// <remarks>This service interacts with a REST API to perform operations on learner data. It uses an <see
    /// cref="IBaseHttpProvider"/>  to send HTTP requests and handle responses. The service supports asynchronous
    /// operations for creating, updating,  and deleting learners, as well as updating learner-parent
    /// relationships.</remarks>
    /// <param name="provider"></param>
    public class LearnerCommandRestService(IBaseHttpProvider provider) : ILearnerCommandService
    {
        /// <summary>
        /// Creates a new learner asynchronously.
        /// </summary>
        /// <param name="learnerDto">The data transfer object containing the details of the learner to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// object with the created learner's details.</returns>
        public async Task<IBaseResult<LearnerDto>> CreateAsync(LearnerDto learnerDto, CancellationToken cancellationToken = default)
        {
            var result = await provider.PutAsync<LearnerDto, LearnerDto>("learners", learnerDto);
            return result;
        }

        /// <summary>
        /// Updates the learner information asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated learner information to the underlying provider. Ensure
        /// that the  <paramref name="learner"/> object contains valid data before calling this method.</remarks>
        /// <param name="learner">The <see cref="LearnerDto"/> object containing the updated learner information.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateAsync(LearnerDto learner, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync("learners", learner);
            return result;
        }

        /// <summary>
        /// Removes a learner entity identified by the specified parent ID.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to delete a learner entity. Ensure
        /// that the specified <paramref name="parentId"/> corresponds to a valid entity. The operation may fail if the
        /// entity does not exist.</remarks>
        /// <param name="parentId">The unique identifier of the parent entity whose associated learner entity is to be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
        /// indicating the outcome of the removal operation.</returns>
        public async Task<IBaseResult> RemoveAsync(string parentId, CancellationToken cancellationToken = default)
        {
            var result = await provider.DeleteAsync("learners", parentId);
            return result;
        }

        /// <summary>
        /// Updates the parent information for a specified learner asynchronously.
        /// </summary>
        /// <remarks>This method sends the updated parent information to the server for the specified
        /// learner.  Ensure that the <paramref name="learnerId"/> is valid and that the <paramref
        /// name="learnerParents"/>  list contains the correct data before calling this method.</remarks>
        /// <param name="learnerId">The unique identifier of the learner whose parent information is being updated.</param>
        /// <param name="learnerParents">A list of <see cref="ParentDto"/> objects representing the updated parent information for the learner.</param>
        /// <param name="cancellationToken">An optional token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> 
        /// indicating the outcome of the update operation.</returns>
        public async Task<IBaseResult> UpdateLearnerParentsAsync(string learnerId, List<ParentDto> learnerParents, CancellationToken cancellationToken = default)
        {
            var result = await provider.PostAsync($"learners/learnerparents/{learnerId}");
            return result;
        }
    }
}
