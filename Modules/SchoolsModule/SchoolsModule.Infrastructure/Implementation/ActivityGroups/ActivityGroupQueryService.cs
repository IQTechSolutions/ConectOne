using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation.ActivityGroups
{
    /// <summary>
    /// Provides querying capabilities for retrieving and filtering activity group data.
    /// </summary>
    public class ActivityGroupQueryService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IRepository<Category<ActivityGroup>, string> categoryRepository) : IActivityGroupQueryService
    {
        /// <summary>
        /// Retrieves all activity groups, with optional filtering by learner or category.
        /// </summary>
        /// <param name="pageParameters">The parameters for filtering the activity groups.</param>
        /// <returns>A result containing a collection of <see cref="ActivityGroupDto"/> instances.</returns>
        public async Task<IBaseResult<IEnumerable<ActivityGroupDto>>> AllActivityGroupsAsync(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ActivityGroup>(ag => !ag.IsDeleted);
            spec.AddInclude(q => q.Include(ag => ag.Categories).ThenInclude(c => c.Category));
            spec.AddInclude(q => q.Include(ag => ag.AgeGroup));
            spec.AddInclude(q => q.Include(ag => ag.Images));
            spec.AddInclude(q => q.Include(ag => ag.Teacher));
            spec.AddInclude(q => q.Include(ag => ag.TeamMembers).ThenInclude(tm => tm.Learner).ThenInclude(l => l.Parents));

            var groupResult = await schoolsModuleRepoManager.ActivityGroups.ListAsync(spec, false, cancellationToken);
            if (!groupResult.Succeeded)
                return await Result<IEnumerable<ActivityGroupDto>>.FailAsync(groupResult.Messages);

            var groups = groupResult.Data;

            // Optional Learner filter
            if (!string.IsNullOrWhiteSpace(pageParameters.LearnerId))
            {
                groups = groups
                    .Where(g => g.TeamMembers.Any(tm => tm.LearnerId == pageParameters.LearnerId))
                    .ToList();
            }

            // Optional Category filtering
            if (!string.IsNullOrWhiteSpace(pageParameters.CategoryIds))
            {
                var filteredGroups = new List<ActivityGroup>();
                var seen = new HashSet<string>();

                foreach (var categoryId in pageParameters.CategoryIds.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    var categoryGroups = await ProcessCategoriesForPagedActivityGroupsForEventAsync(categoryId, []);
                    foreach (var group in categoryGroups)
                    {
                        if (seen.Add(group.Id))
                            filteredGroups.Add(group);
                    }
                }

                filteredGroups = filteredGroups.OrderBy(g => g.AgeGroup?.MinAge).ThenBy(g => g.Name).ToList();
                return await Result<IEnumerable<ActivityGroupDto>>.SuccessAsync(filteredGroups.Select(g => new ActivityGroupDto(g)));
            }

            return await Result<IEnumerable<ActivityGroupDto>>.SuccessAsync(groups.Select(g => new ActivityGroupDto(g)));
        }

        /// <summary>
        /// Retrieves a paginated list of activity groups with optional filtering by learner or categories.
        /// </summary>
        /// <param name="pageParameters">The pagination and filter parameters.</param>
        /// <returns>A paginated result of <see cref="ActivityGroupDto"/> objects.</returns>
        public async Task<PaginatedResult<ActivityGroupDto>> PagedActivityGroupsAsync(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            // Build base specification with includes
            var spec = new ActivityGroupListSpecification(pageParameters);

            var result = await schoolsModuleRepoManager.ActivityGroups.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return PaginatedResult<ActivityGroupDto>.Failure(result.Messages);

            var groups = result.Data;

            // Filter by LearnerId if provided
            if (!string.IsNullOrWhiteSpace(pageParameters.LearnerId))
            {
                groups = groups
                    .Where(ag => ag.TeamMembers.Any(tm => tm.LearnerId == pageParameters.LearnerId))
                    .ToList();
            }

            // Filter by CategoryIds if provided
            if (!string.IsNullOrWhiteSpace(pageParameters.CategoryIds))
            {
                var categoryGroups = new List<ActivityGroup>();
                var seen = new HashSet<string>();

                foreach (var catId in pageParameters.CategoryIds.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    var filtered = await ProcessCategoriesForPagedActivityGroupsForEventAsync(catId, []);
                    foreach (var g in filtered)
                    {
                        if (seen.Add(g.Id)) categoryGroups.Add(g);
                    }
                }

                categoryGroups = categoryGroups.OrderBy(g => g.AgeGroup?.MinAge).ThenBy(g => g.Name).ToList();

                var pagedFiltered = categoryGroups
                    .Select(g => new ActivityGroupDto(g))
                    .ToList();

                return PaginatedResult<ActivityGroupDto>.Success(
                    pagedFiltered, categoryGroups.Count, pageParameters.PageNr, pageParameters.PageSize);
            }

            // Default paging
            var dtoList = groups?.Select(g => new ActivityGroupDto(g))?.ToList();

            return PaginatedResult<ActivityGroupDto>.Success(dtoList == null ? new List<ActivityGroupDto>() : dtoList.ToList(), dtoList == null ? 0 : dtoList.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a paginated list of activity groups associated with a specific event or categories.
        /// </summary>
        /// <param name="pageParameters">Parameters containing event ID and/or category IDs.</param>
        /// <returns>A paginated result of <see cref="ActivityGroupDto"/> objects.</returns>
        public async Task<PaginatedResult<ActivityGroupDto>> PagedActivityGroupsForEventAsync(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(pageParameters.EventId) && string.IsNullOrWhiteSpace(pageParameters.CategoryIds))
                return PaginatedResult<ActivityGroupDto>.Success([], 0, pageParameters.PageNr, pageParameters.PageSize);

            if (!string.IsNullOrWhiteSpace(pageParameters.EventId))
            {
                var spec = new LambdaSpec<SchoolEvent<Category<ActivityGroup>>>(e => e.Id == pageParameters.EventId);
                spec.AddInclude(q => q.Include(e => e.ParticipatingActivityGroups)
                                      .ThenInclude(p => p.ActivityGroup).ThenInclude(ag => ag.AgeGroup));
                spec.AddInclude(q => q.Include(e => e.ParticipatingActivityGroups)
                                      .ThenInclude(p => p.ParticipatingTeamMembers)
                                      .ThenInclude(t => t.TeamMember).ThenInclude(l => l.Parents));
                var result = await schoolsModuleRepoManager.SchoolEvents.ListAsync(spec, true, cancellationToken);
                var evt = result.Data.FirstOrDefault();

                if (evt != null)
                {
                    var dtos = evt.ParticipatingActivityGroups.Select(p => new ActivityGroupDto(p));
                    var ordered = dtos.OrderBy(d => d.AgeGroup?.MinAge).ThenBy(d => d.Name).ToList();

                    return PaginatedResult<ActivityGroupDto>.Success(
                        ordered,
                        ordered.Count,
                        pageParameters.PageNr,
                        pageParameters.PageSize
                    );
                }
            }

            // fallback to category-based filtering
            var allGroups = new List<ActivityGroup>();
            var categoryIds = pageParameters.CategoryIds!.Split(",");
            var seen = new HashSet<string>();

            foreach (var catId in categoryIds)
            {
                var groupList = await ProcessCategoriesForPagedActivityGroupsForEventAsync(catId, []);
                foreach (var g in groupList)
                {
                    if (seen.Add(g.Id)) allGroups.Add(g);
                }
            }

            var dtosFromCategory = allGroups.Select(a => new ActivityGroupDto(a)).ToList();

            return PaginatedResult<ActivityGroupDto>.Success(
                dtosFromCategory,
                dtosFromCategory.Count,
                pageParameters.PageNr,
                pageParameters.PageSize
            );
        }

        /// <summary>
        /// Retrieves a paginated list of activity groups based on categories associated with an event.
        /// </summary>
        /// <param name="pageParameters">The pagination and event identification parameters.</param>
        /// <returns>A paginated list of activity groups participating in event categories.</returns>
        public async Task<PaginatedResult<ActivityGroupDto>> PagedActivityGroupsForParticipatingEventCategories(ActivityGroupPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(pageParameters.EventId))
                return PaginatedResult<ActivityGroupDto>.Failure(new List<string>() { "EventId must be provided." });

            var spec = new LambdaSpec<ParticipatingActivityGroup>(pag => pag.EventId == pageParameters.EventId);
            spec.AddInclude(q => q.Include(pag => pag.ActivityGroup).ThenInclude(ag => ag.Categories));

            var result = await schoolsModuleRepoManager.ParticipatingActivityGroups.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return PaginatedResult<ActivityGroupDto>.Failure(result.Messages);

            var categoryIds = result.Data
                .SelectMany(p => p.ActivityGroup?.Categories ?? [])
                .Select(c => c.CategoryId)
                .Distinct()
                .ToList();

            if (!categoryIds.Any())
                return PaginatedResult<ActivityGroupDto>.Success(new List<ActivityGroupDto>(), 0, pageParameters.PageNr, pageParameters.PageSize);

            var activityGroups = new List<ActivityGroup>();
            var seen = new HashSet<string>();

            foreach (var categoryId in categoryIds)
            {
                var groupSpec = new LambdaSpec<ActivityGroup>(ag => ag.Categories.Any(ec => ec.CategoryId == categoryId));
                var groupResult = await schoolsModuleRepoManager.ActivityGroups.ListAsync(groupSpec, false, cancellationToken);

                foreach (var ag in groupResult.Data)
                {
                    if (seen.Add(ag.Id)) activityGroups.Add(ag);
                }
            }

            var ordered = activityGroups.OrderBy(a => a.AgeGroup).ThenBy(a => a.Name).ToList();
            var dtos = ordered.Select(a => new ActivityGroupDto(a)).ToList();

            return PaginatedResult<ActivityGroupDto>.Success(dtos, dtos.Count, pageParameters.PageNr, pageParameters.PageSize);
        }
        
        /// <summary>
        /// Retrieves a paginated list of learners associated with a specific activity group.
        /// </summary>
        /// <param name="pageParameters">Parameters that include the activity group ID and paging info.</param>
        /// <param name="trackChanges">Indicates whether change tracking should be enabled.</param>
        /// <param name="cancellationToken">Cancellation token for async operations.</param>
        /// <returns>
        /// A <see cref="PaginatedResult{LearnerDto}"/> containing the list of learners,
        /// or a failure result if the operation could not be completed.
        /// </returns>
        public async Task<PaginatedResult<LearnerDto>> PagedActivityGroupTeamMembersAsync(LearnerPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(pageParameters.ActivityGroupId))
                return PaginatedResult<LearnerDto>.Failure(["ActivityGroupId must be provided."]);

            var spec = new LambdaSpec<ActivityGroupTeamMember>(tm => tm.ActivityGroupId == pageParameters.ActivityGroupId);
            spec.AddInclude(q => q.Include(tm => tm.Learner));

            var result = await schoolsModuleRepoManager.ActivityGroupTeamMembers.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return PaginatedResult<LearnerDto>.Failure(result.Messages);

            var dtos = result.Data.Select(tm => new LearnerDto(tm.Learner, true)).ToList();
            return PaginatedResult<LearnerDto>.Success(dtos, dtos.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves a paginated list of learners participating in a specific activity group
        /// within a given school event.
        /// </summary>
        /// <param name="pageParameters">
        /// Contains the event ID, activity group ID, and paging parameters such as page number and size.
        /// </param>
        /// <param name="trackChanges">
        /// Indicates whether Entity Framework should track changes to the retrieved entities.
        /// </param>
        /// <returns>
        /// A <see cref="PaginatedResult{LearnerDto}"/> containing the list of learners,
        /// or a failure result if the event is not found or data retrieval fails.
        /// </returns>
        public async Task<PaginatedResult<LearnerDto>> PagedEventActivityGroupTeamMembersAsync(LearnerPageParameters pageParameters, CancellationToken cancellationToken = default)
        {
            var eventResult = await schoolsModuleRepoManager.SchoolEvents
                .FirstOrDefaultAsync(new EventWithActivityGroupLearnersSpecification(pageParameters.EventId), false, cancellationToken);

            if (!eventResult.Succeeded)
                return PaginatedResult<LearnerDto>.Failure(eventResult.Messages);

            var eventItem = eventResult.Data;
            if (eventItem is null)
                return PaginatedResult<LearnerDto>.Success(new List<LearnerDto>(), 0, pageParameters.PageNr, pageParameters.PageSize);

            var learners = eventItem.ParticipatingActivityGroups
                .Where(g => g.ActivityGroupId == pageParameters.ActivityGroupId)
                .SelectMany(g => g.ParticipatingTeamMembers)
                .Where(tm => tm.TeamMember is not null)
                .Select(tm => new LearnerDto(tm.TeamMember, true))
                .ToList();

            return PaginatedResult<LearnerDto>.Success(
                learners,
                learners.Count,
                pageParameters.PageNr,
                pageParameters.PageSize
            );
        }

        /// <summary>
        /// Retrieves the learners assigned to an activity group.
        /// </summary>
        /// <param name="args">Parameters including the activity group ID.</param>
        /// <returns>A result containing a list of learners in the specified activity group.</returns>
        public async Task<IBaseResult<IEnumerable<LearnerDto>>> ActivityGroupTeamMembersAsync(LearnerPageParameters args, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(args.ActivityGroupId))
                return await Result<IEnumerable<LearnerDto>>.FailAsync("ActivityGroupId must be provided.");

            var spec = new LambdaSpec<ActivityGroupTeamMember>(tm => tm.ActivityGroupId == args.ActivityGroupId);
            spec.AddInclude(q => q.Include(tm => tm.Learner));

            var result = await schoolsModuleRepoManager.ActivityGroupTeamMembers.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded)
                return await Result<IEnumerable<LearnerDto>>.FailAsync(result.Messages);

            var dtos = result.Data.Select(tm => new LearnerDto(tm.Learner, true)).ToList();
            return await Result<IEnumerable<LearnerDto>>.SuccessAsync(dtos);
        }

        /// <summary>
        /// Retrieves a single activity group by ID.
        /// </summary>
        /// <param name="activityGroupId">The unique identifier of the activity group.</param>
        /// <param name="trackChanges">Whether to track changes to the retrieved entity.</param>
        /// <returns>A result containing the activity group details or an error message.</returns>
        public async Task<IBaseResult<ActivityGroupDto>> ActivityGroupAsync(string activityGroupId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(activityGroupId))
                return await Result<ActivityGroupDto>.FailAsync("ActivityGroupId must be provided.");

            var spec = new LambdaSpec<ActivityGroup>(ag => ag.Id == activityGroupId);
            spec.AddInclude(q => q.Include(ag => ag.AgeGroup));
            spec.AddInclude(q => q.Include(ag => ag.Categories).ThenInclude(c => c.Category));
            spec.AddInclude(q => q.Include(ag => ag.Images));
            spec.AddInclude(q => q.Include(ag => ag.Teacher));
            spec.AddInclude(q => q.Include(ag => ag.TeamMembers).ThenInclude(tm => tm.Learner).ThenInclude(l => l.Parents));

            var result = await schoolsModuleRepoManager.ActivityGroups.ListAsync(spec, false, cancellationToken);
            var entity = result.Data.FirstOrDefault();

            if (entity == null)
                return await Result<ActivityGroupDto>.FailAsync("No activity group found with the provided ID.");

            return await Result<ActivityGroupDto>.SuccessAsync(new ActivityGroupDto(entity));
        }

        #region Helpers

        /// <summary>
        /// Recursively collects all <see cref="ActivityGroup"/>s under the specified category and its subcategories.
        /// </summary>
        /// <param name="categoryId">The root category ID to begin traversal.</param>
        /// <param name="accumulatedGroups">A running list of accumulated activity groups.</param>
        /// <returns>A list of aggregated <see cref="ActivityGroup"/>s.</returns>
        private async Task<List<ActivityGroup>> ProcessCategoriesForPagedActivityGroupsForEventAsync(string categoryId, List<ActivityGroup> accumulatedGroups, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                return accumulatedGroups;

            var spec = new LambdaSpec<Category<ActivityGroup>>(c => c.Id == categoryId);
            spec.AddInclude(q => q.Include(c => c.SubCategories));
            spec.AddInclude(q => q.Include(c => c.EntityCollection.Where(ec => !ec.Entity.IsDeleted))
                .ThenInclude(ec => ec.Entity).ThenInclude(e => e.AgeGroup));
            spec.AddInclude(q => q.Include(c => c.EntityCollection.Where(ec => !ec.Entity.IsDeleted))
                .ThenInclude(ec => ec.Entity).ThenInclude(e => e.TeamMembers).ThenInclude(tm => tm.Learner));

            var result = await categoryRepository.ListAsync(spec, false, cancellationToken);
            var category = result.Data.FirstOrDefault();

            if (category == null)
                return accumulatedGroups;

            if (category.HasSubCategories)
            {
                foreach (var sub in category.SubCategories)
                {
                    await ProcessCategoriesForPagedActivityGroupsForEventAsync(sub.Id, accumulatedGroups, cancellationToken);
                }
            }
            else
            {
                var groups = category.EntityCollection
                    .Where(ec => !ec.Entity.IsDeleted)
                    .Select(ec => ec.Entity)
                    .ToList();

                accumulatedGroups.AddRange(groups);
            }

            return accumulatedGroups;
        }

        #endregion
    }
}
