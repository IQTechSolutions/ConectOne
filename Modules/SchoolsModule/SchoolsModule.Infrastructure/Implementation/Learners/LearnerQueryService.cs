using ConectOne.Domain.Enums;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation.Learners
{
    /// <summary>
    /// Provides querying capabilities for learner-related operations.
    /// </summary>
    /// <param name="schoolsModuleRepoManager">Central repository manager for school entities.</param>
    public class LearnerQueryService(ISchoolsModuleRepoManager schoolsModuleRepoManager) : ILearnerQueryService
    {
        /// <summary>
        /// Retrieves all learners matching optional filters such as learner ID, gender, and age range.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<LearnerDto>>> AllLearnersAsync(string? learnerId = null, Gender gender = Gender.All, int minAge = 0, int maxAge = 100, CancellationToken cancellationToken = default)
        {
            var spec = new LearnerFilterSpecification(learnerId, gender);
            var result = await schoolsModuleRepoManager.Learners.ListAsync(spec, false, cancellationToken);

            if (!result.Succeeded) return await Result<IEnumerable<LearnerDto>>.FailAsync(result.Messages);

            var filtered = result.Data
                .Where(l => {
                    var age = l.IdNumber.GetAge();
                    return age >= minAge && age <= maxAge;
                })
                .ToList();

            return await Result<IEnumerable<LearnerDto>>.SuccessAsync(filtered.Select(c => new LearnerDto(c)));
        }

        /// <summary>
        /// Returns a paginated list of learners based on filtering, searching, and paging criteria.
        /// </summary>
        public async Task<PaginatedResult<LearnerDto>> PagedLearnersAsync(LearnerPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var bb = new PagedLearnersSpecification(pageParameters);

            var result = await schoolsModuleRepoManager.Learners.ListAsync(bb, trackChanges, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<LearnerDto>.Failure(result.Messages);

            var paginated = result.Data.Select(l => new LearnerDto(l, true)).ToList();
            return PaginatedResult<LearnerDto>.Success(paginated, result.Data.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves the total count of learners in the database.
        /// </summary>
        public async Task<IBaseResult<int>> LearnerCount(CancellationToken cancellationToken = default)
        {
            var result = await schoolsModuleRepoManager.Learners.CountAsync(null, cancellationToken);
            return result.Succeeded
                ? await Result<int>.SuccessAsync(result.Data)
                : await Result<int>.FailAsync(result.Messages);
        }

        /// <summary>
        /// Retrieves a specific learner with full details by ID.
        /// </summary>
        public async Task<IBaseResult<LearnerDto>> LearnerAsync(string learnerId, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            var spec = new SingleLearnerWithDetailsSpecification(learnerId);
            var learnerResult = await schoolsModuleRepoManager.Learners.FirstOrDefaultAsync(spec, trackChanges, cancellationToken);
            if (!learnerResult.Succeeded || learnerResult.Data == null)
                return await Result<LearnerDto>.FailAsync(learnerResult.Messages.Any() ? learnerResult.Messages : [$"No learner matching id '{learnerId}' found."]);

            return await Result<LearnerDto>.SuccessAsync(new LearnerDto(learnerResult.Data, true));
        }

        /// <summary>
        /// Retrieves a learner by their email address.
        /// </summary>
        public async Task<IBaseResult<LearnerDto>> LearnerByEmailAsync(string emailAddress, CancellationToken cancellationToken = default)
        {
            var spec = new LearnerByEmailSpecification(emailAddress);
            var learnerResult = await schoolsModuleRepoManager.Learners.FirstOrDefaultAsync(spec, false, cancellationToken);
            return learnerResult.Succeeded && learnerResult.Data != null
                ? Result<LearnerDto>.Success(new LearnerDto(learnerResult.Data, true))
                : await Result<LearnerDto>.FailAsync("No learner found.");
        }

        /// <summary>
        /// Checks if a learner exists by email address and returns their ID if found.
        /// </summary>
        public async Task<IBaseResult<string>> LearnerExist(string emailAddress, CancellationToken cancellationToken = default)
        {
            var spec = new LearnerByEmailSpecification(emailAddress);
            var learnerResult = await schoolsModuleRepoManager.Learners.FirstOrDefaultAsync(spec, false, cancellationToken);
            return learnerResult.Succeeded
                ? Result<string>.Success(data: learnerResult.Data?.Id)
                : await Result<string>.FailAsync(learnerResult.Messages);
        }

        /// <summary>
        /// Retrieves parent details linked to a specific learner.
        /// </summary>
        public async Task<IBaseResult<List<ParentDto>>> LearnerParentsAsync(string learnerId, CancellationToken cancellationToken = default)
        {
            var spec = new LearnerParentsWithDetailsSpecification(learnerId);
            var listResult = await schoolsModuleRepoManager.LearnerParents.ListAsync(spec, false, cancellationToken);
            if (!listResult.Succeeded)
                return await Result<List<ParentDto>>.FailAsync(listResult.Messages);

            var parentDtos = listResult.Data
                .Where(lp => lp.Parent != null)
                .Select(lp => new ParentDto(lp.Parent!, true))
                .ToList();

            return await Result<List<ParentDto>>.SuccessAsync(parentDtos);
        }

        /// <summary>
        /// Retrieves a paginated list of learners based on the specified parameters.
        /// </summary>
        /// <remarks>This method queries the data source for learners using the provided pagination
        /// parameters. The result indicates whether the operation was successful and provides the corresponding data or
        /// error messages.</remarks>
        /// <param name="pageParameters">The pagination parameters, including page number and page size, used to filter the learners.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult{T}"/>
        /// where <c>T</c> is an <see cref="IEnumerable{T}"/> of <see cref="LearnerDto"/> objects. If the operation
        /// succeeds, the result contains the paginated list of learners; otherwise, it contains error messages.</returns>
        public async Task<IBaseResult<IEnumerable<LearnerDto>>> AllLearnersAsync(LearnerPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var bb = new PagedLearnersSpecification(pageParameters);

            var result = await schoolsModuleRepoManager.Learners.ListAsync(bb, false, cancellationToken);
            return result.Succeeded
                ? await Result<IEnumerable<LearnerDto>>.SuccessAsync(result.Data.Select(c => new LearnerDto(c)))
                : await Result<IEnumerable<LearnerDto>>.FailAsync(result.Messages);
        }
    }
}
