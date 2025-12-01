using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Application.RestServices.Learners
{
    /// <summary>
    /// Provides a REST-based implementation of the <see cref="ILearnerQueryService"/> interface for querying
    /// learner-related data.
    /// </summary>
    /// <remarks>This service interacts with a RESTful API to retrieve learner information, including
    /// paginated lists of learners, individual learner details, learner counts, and associated parent data. It supports
    /// asynchronous operations and allows for optional tracking of changes to retrieved data.</remarks>
    /// <param name="provider"></param>
    public class LearnerQueryRestService(IBaseHttpProvider provider) : ILearnerQueryService
    {
        /// <summary>
        /// Retrieves a paginated list of learners based on the specified page parameters.
        /// </summary>
        /// <remarks>The method fetches learners from the "learners/pagedlearners" endpoint using the
        /// provided pagination parameters. The result includes metadata about the pagination state, such as the total
        /// number of items and pages.</remarks>
        /// <param name="pageParameters">The parameters that define the pagination and filtering options for the learners.</param>
        /// <param name="trackChanges">A value indicating whether to track changes to the retrieved learners. Defaults to <see langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> containing the paginated list of learners.</returns>
        public async Task<PaginatedResult<LearnerDto>> PagedLearnersAsync(LearnerPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetPagedAsync<LearnerDto, LearnerPageParameters>("learners/pagedlearners", pageParameters);
            return result;
        }

        /// <summary>
        /// Retrieves a paginated list of learners based on the specified parameters.
        /// </summary>
        /// <remarks>This method sends a request to the underlying data provider to retrieve learners. The
        /// result includes pagination and filtering based on the provided <paramref name="pageParameters"/>.</remarks>
        /// <param name="pageParameters">The pagination and filtering parameters to apply when retrieving the learners.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of <see cref="IEnumerable{T}"/> containing <see cref="LearnerDto"/> objects that match the specified
        /// parameters.</returns>
        public async Task<IBaseResult<IEnumerable<LearnerDto>>> AllLearnersAsync(LearnerPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<IEnumerable<LearnerDto>>($"learners?{pageParameters.GetQueryString()}");
            return result;
        }

        /// <summary>
        /// Retrieves the total number of learners.
        /// </summary>
        /// <remarks>This method sends an asynchronous request to fetch the count of learners. The result
        /// is returned as an  <see cref="IBaseResult{T}"/> containing the count as an integer.</remarks>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total number of learners.</returns>
        public async Task<IBaseResult<int>> LearnerCount(CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<int>($"learners/count");
            return result;
        }

        /// <summary>
        /// Retrieves learner information based on the specified learner ID.
        /// </summary>
        /// <remarks>This method asynchronously retrieves learner data from the underlying provider. The
        /// <paramref name="trackChanges"/> parameter determines whether the retrieved data will be tracked for
        /// changes.</remarks>
        /// <param name="learnerId">The unique identifier of the learner to retrieve. This value cannot be null or empty.</param>
        /// <param name="trackChanges">A boolean value indicating whether to track changes to the retrieved learner data. Defaults to <see
        /// langword="false"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// of type <see cref="LearnerDto"/> with the learner's information.</returns>
        public async Task<IBaseResult<LearnerDto>> LearnerAsync(string learnerId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<LearnerDto>($"learners/{learnerId}");
            return result;
        }

        /// <summary>
        /// Retrieves a learner's information based on their email address.
        /// </summary>
        /// <remarks>This method performs an asynchronous HTTP GET request to retrieve the learner's
        /// details. Ensure the provided email address is valid and corresponds to an existing learner in the
        /// system.</remarks>
        /// <param name="emailAddress">The email address of the learner to retrieve. This value cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with the learner's information as a <see cref="LearnerDto"/>. If no learner is found, the result may
        /// indicate an empty or null value.</returns>
        public async Task<IBaseResult<LearnerDto>> LearnerByEmailAsync(string emailAddress, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<LearnerDto>($"learners/byemail/{emailAddress}");
            return result;
        }

        /// <summary>
        /// Checks if a learner exists based on the provided email address.
        /// </summary>
        /// <remarks>The method sends a request to the underlying provider to determine if a learner
        /// exists for the given email address. The result may vary depending on the implementation of the
        /// provider.</remarks>
        /// <param name="emailAddress">The email address of the learner to check. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Optional.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult{T} where the
        /// value is a string indicating the existence of the learner.</returns>
        public async Task<IBaseResult<string>> LearnerExist(string emailAddress, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<string>($"learners/exist/{emailAddress}");
            return result;
        }

        /// <summary>
        /// Retrieves a list of parents associated with the specified learner.
        /// </summary>
        /// <remarks>This method sends an asynchronous request to retrieve the parents of a learner
        /// identified by the given <paramref name="learnerId"/>. The result includes a list of parent details
        /// encapsulated in <see cref="ParentDto"/> objects.</remarks>
        /// <param name="learnerId">The unique identifier of the learner whose parents are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// object with a list of <see cref="ParentDto"/> representing the parents of the specified learner.</returns>
        public async Task<IBaseResult<List<ParentDto>>> LearnerParentsAsync(string learnerId, CancellationToken cancellationToken = default)
        {
            var result = await provider.GetAsync<List<ParentDto>>($"learners/learnerparents/{learnerId}");
            return result;
        }
    }
}
