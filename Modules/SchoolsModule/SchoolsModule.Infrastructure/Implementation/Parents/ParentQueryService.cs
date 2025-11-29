using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Parents;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation.Parents
{
    /// <summary>
    ///     Read‑only service that exposes <see cref="Parent"/> query operations for the SchoolsEnterprise domain.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     <b>Responsibilities</b>:
    ///     <list type="bullet">
    ///         <item>Delegates data access to the <see cref="ISchoolsModuleRepoManager"/> abstraction.</item>
    ///         <item>Projects entities to DTOs (<see cref="ParentDto"/>, <see cref="LearnerDto"/>) where required.</item>
    ///         <item>Normalises repository results to <see cref="IBaseResult"/> and <see cref="PaginatedResult{T}"/> for consistent API consumption.</item>
    ///     </list>
    /// </para>
    /// <para>
    ///     The class is deliberately <c>query‑only</c>; any state‑mutating behaviour belongs in a dedicated command service to preserve the
    ///     <em>Command Query Responsibility Segregation</em> (CQRS) boundary adopted by the application architecture.
    /// </para>
    /// </remarks>
    public class ParentQueryService(ISchoolsModuleRepoManager schoolsModuleRepoManager) : IParentQueryService
    {
        /// <summary>
        ///     Retrieves every <see cref="Parent"/> in the system or—when <paramref name="parentId"/> is supplied—only the parent
        ///     with that identifier.
        /// </summary>
        /// <param name="parentId">Optional unique identifier of the parent to query. When <c>null</c> or empty all parents are returned.</param>
        /// <param name="cancellationToken">Token used to propagate cancellation from the caller.</param>
        /// <returns>
        ///     A success result containing an <see cref="IEnumerable{Parent}"/> sequence when at least one parent was found, or a
        ///     failure result enumerating the encountered errors.
        /// </returns>
        /// <remarks>
        ///     The call falls back to <see cref="ISchoolsModuleRepoManager.Parents.ListAsync(bool, CancellationToken)"/> when no
        ///     specific <paramref name="parentId"/> is provided, thereby avoiding the overhead of an unnecessary specification.
        /// </remarks>
        public async Task<IBaseResult<IEnumerable<ParentDto>>> AllParentsAsync(string? parentId = null, CancellationToken cancellationToken = default)
        {
            var parentSpec = new LambdaSpec<Parent>(c => c.Id == parentId);
            parentSpec.AddInclude(c => c.Include(i => i.Images));
            parentSpec.AddInclude(c => c.Include(i => i.EmergencyContacts));

            var parentResult = !string.IsNullOrEmpty(parentId) ? await schoolsModuleRepoManager.Parents.ListAsync(parentSpec, false, cancellationToken)
                : await schoolsModuleRepoManager.Parents.ListAsync(false, cancellationToken);

            var sql = schoolsModuleRepoManager.Parents.GetQueryString();
            Console.WriteLine(sql);

            if (!parentResult.Succeeded)
                return await Result<IEnumerable<ParentDto>>.FailAsync(parentResult.Messages);

            return await Result<IEnumerable<ParentDto>>.SuccessAsync(parentResult.Data.Select(c => new ParentDto(c)));
        }

        /// <summary>
        ///     Retrieves a single paged slice of parents, projecting each entity into a <see cref="ParentDto"/>.
        /// </summary>
        /// <param name="pageParameters">Paging information as supplied by the client.</param>
        /// <param name="cancellationToken">Token used to propagate cancellation from the caller.</param>
        /// <returns>
        ///     A <see cref="PaginatedResult{T}"/> on success, otherwise a failure result containing error messages.
        /// </returns>
        /// <example>
        /// <code>
        /// var page = await _parentQueryService.PagedParentsAsync(new ParentPageParameters { PageNr = 2, PageSize = 25 });
        /// </code>
        /// </example>
        public async Task<PaginatedResult<ParentDto>> PagedParentsAsync(ParentPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var parentResult = await schoolsModuleRepoManager.Parents.ListAsync(new PagedParentsSpecification(pageParameters), false, cancellationToken);

            if (!parentResult.Succeeded)
                return PaginatedResult<ParentDto>.Failure(parentResult.Messages);

            var parentCount = await schoolsModuleRepoManager.Parents.CountAsync(null, cancellationToken);
            if (!parentCount.Succeeded)
                return PaginatedResult<ParentDto>.Failure(parentCount.Messages);

            var response = parentResult.Data.Select(c => new ParentDto(c, true));
            return PaginatedResult<ParentDto>.Success(response.ToList(), parentCount.Data, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        ///     Counts all <see cref="Parent"/> records in the data store.
        /// </summary>
        /// <param name="cancellationToken">Token used to propagate cancellation from the caller.</param>
        /// <returns>A success result containing the count or a failure result with error information.</returns>
        public async Task<IBaseResult<int>> ParentCount(CancellationToken cancellationToken)
        {
            var parents = await schoolsModuleRepoManager.Parents.CountAsync(null, cancellationToken);
            if (!parents.Succeeded)
                return await Result<int>.FailAsync(parents.Messages);

            return await Result<int>.SuccessAsync(parents.Data);
        }

        /// <summary>
        ///     Retrieves a single <see cref="Parent"/> by its identifier and projects it to a <see cref="ParentDto"/>.
        /// </summary>
        /// <param name="parentId">Unique identifier of the parent.</param>
        /// <param name="trackChanges">When <c>true</c>, the returned entity will be tracked by the underlying <c>DbContext</c>.</param>
        /// <param name="cancellationToken">Token used to propagate cancellation from the caller.</param>
        /// <returns>
        ///     A success result with a populated <see cref="ParentDto"/> if the parent exists; otherwise a failure result with a
        ///     descriptive error message such as <c>"No parent found"</c>.
        /// </returns>
        public async Task<IBaseResult<ParentDto>> ParentAsync(string parentId, bool trackChanges, CancellationToken cancellationToken = default)
        {
            var parentResult = await schoolsModuleRepoManager.Parents.FirstOrDefaultAsync(
                new SingleParentSpecification(parentId),
                trackChanges,
                cancellationToken);

            if (!parentResult.Succeeded)
                return await Result<ParentDto>.FailAsync(parentResult.Messages);

            if (parentResult.Data == null)
                return await Result<ParentDto>.FailAsync("No parent found");

            return await Result<ParentDto>.SuccessAsync(new ParentDto(parentResult.Data, true));
        }

        /// <summary>
        ///     Retrieves a single <see cref="Parent"/> using their primary email address.
        /// </summary>
        /// <param name="emailAddress">The email address associated with the parent account.</param>
        /// <param name="cancellationToken">Token used to propagate cancellation from the caller.</param>
        /// <returns>
        ///     A success result with a <see cref="ParentDto"/> on match; otherwise a failure result indicating why the lookup failed.
        /// </returns>
        public async Task<IBaseResult<ParentDto>> ParentByEmailAsync(string emailAddress, CancellationToken cancellationToken = default)
        {
            var parentResult = await schoolsModuleRepoManager.Parents.FirstOrDefaultAsync(
                new SingleParentSpecification(emailAddress: emailAddress),
                /*trackChanges*/ false,
                cancellationToken);

            if (!parentResult.Succeeded)
                return await Result<ParentDto>.FailAsync(parentResult.Messages);

            if (parentResult.Data == null)
                return await Result<ParentDto>.FailAsync("No parent found");

            return await Result<ParentDto>.SuccessAsync(new ParentDto(parentResult.Data, true));
        }

        /// <summary>
        ///     Retrieves every <see cref="Learner"/> associated with the specified parent identifier.
        /// </summary>
        /// <param name="parentId">Unique identifier of the parent.</param>
        /// <param name="cancellationToken">Token used to propagate cancellation from the caller.</param>
        /// <returns>A success result containing a list of <see cref="LearnerDto"/> instances; otherwise a failure result.</returns>
        public async Task<IBaseResult<List<LearnerDto>>> ParentLearnersAsync(string parentId, CancellationToken cancellationToken = default)
        {
            var list = await schoolsModuleRepoManager.LearnerParents.ListAsync(new ParentLearnersSpecification(parentId), false, cancellationToken);

            if (!list.Succeeded)
                return await Result<List<LearnerDto>>.FailAsync(list.Messages);

            var learnersResult = list.Data!.Select(c => new LearnerDto(c.Learner, true)).ToList();
            return await Result<List<LearnerDto>>.SuccessAsync(learnersResult);
        }

        /// <summary>
        /// Retrieves a list of parents for notifications asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of UserInfoDto entities.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> ParentsNotificationList(ParentPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = pageParameters.ParentId != null ? new LambdaSpec<Parent>(p => p.Id == pageParameters.ParentId) : new LambdaSpec<Parent>(p => true);
            spec.AddInclude(q => q.Include(p => p.EmailAddresses));

            var result = await schoolsModuleRepoManager.Parents.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<RecipientDto>>.FailAsync(result.Messages);

            var recipients = new List<RecipientDto>();

            foreach (var parent in result.Data)
            {
                if (recipients.All(c => c.Id != parent.Id))
                    recipients.Add(new RecipientDto(parent.Id, parent.FirstName, parent.LastName, parent.EmailAddresses.Select(c => c.Email).ToList(), parent.ReceiveNotifications, parent.RecieveEmails));
            }
            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(recipients);
        }
    }
}
