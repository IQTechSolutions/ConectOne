using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation.SchoolEvents
{
    /// <summary>
    /// Service responsible for handling school event categories, including participating groups and team members.
    /// </summary>
    /// <param name="schoolsModuleRepoManager">Repository manager for school-related entities.</param>
    /// <param name="groupingRepositoryManager">Repository manager for category grouping entities.</param>
    public class SchoolEventCategoryService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IRepository<Category<ActivityGroup>, string> categoryRepository) : ISchoolEventCategoryService
    {
        #region Categories

        /// <summary>
        /// Retrieves paginated school event categories relevant to a parent or teacher.
        /// </summary>
        public async Task<PaginatedResult<CategoryDto>> PagedSchoolEventActivityGroupCategoriesForAppAsync(SchoolEventPageParameters pageParameters, bool trackChanges = false, CancellationToken cancellationToken = default)
        {
            // 1. Get published events
            var eventResult = await schoolsModuleRepoManager.SchoolEvents.ListAsync(
                new PublishedEventsWithParticipantsSpec(), trackChanges, cancellationToken);

            if (!eventResult.Succeeded || !eventResult.Data.Any())
                return PaginatedResult<CategoryDto>.Success([], 0, pageParameters.PageNr, pageParameters.PageSize);

            var publishedEvents = eventResult.Data.ToList();

            // 2. Load learners for the given parent
            var learners = new List<Learner>();
            if (!string.IsNullOrEmpty(pageParameters.ParentId))
            {
                var parentResult = await schoolsModuleRepoManager.Parents.FirstOrDefaultAsync(
                    new ParentWithLearnersByIdSpec(pageParameters.ParentId), false, cancellationToken);

                if (parentResult?.Data?.Learners != null)
                    learners = parentResult.Data.Learners.Select(l => l.Learner).ToList();
            }

            // 3. If no learners, fallback to teachers
            var teachers = new List<Teacher>();
            if (!learners.Any())
            {
                var teacherResult = await schoolsModuleRepoManager.Teachers.ListAsync(false, cancellationToken);
                if (teacherResult.Succeeded)
                    teachers = teacherResult.Data.ToList();
            }

            // 4. Filter events based on learner or teacher participation
            var userRelevantEvents = new List<SchoolEvent<Category<ActivityGroup>>>();

            foreach (var ev in publishedEvents)
            {
                foreach (var group in ev.ParticipatingActivityGroups)
                {
                    bool matchesLearner = learners.Any(l => group.ParticipatingTeamMembers.Any(v => v.TeamMemberId == l.Id));
                    bool matchesTeacher = teachers.Any(t => t.Id == group.ActivityGroup?.TeacherId);

                    if (matchesLearner || matchesTeacher)
                    {
                        userRelevantEvents.Add(ev);
                        break;
                    }
                }
            }

            // 5. Get child categories under selected parent category
            var categoryResult = await categoryRepository.ListAsync(
                new ChildCategoriesWithEntitiesSpec(pageParameters.CategoryId), false, cancellationToken);

            if (!categoryResult.Succeeded)
                return PaginatedResult<CategoryDto>.Failure(categoryResult.Messages);

            var matchedCategories = new List<Category<ActivityGroup>>();
            var childCategories = categoryResult.Data.ToList();

            foreach (var ev in userRelevantEvents.DistinctBy(e => e.Id))
            {
                foreach (var category in childCategories)
                {
                    if (ev.ParticipatingCategories.Any(pc => pc.ActivityGroupCategoryId == category.Id))
                        matchedCategories.Add(category);
                }
            }

            var resultDtos = matchedCategories.DistinctBy(c => c.Id)
                                              .OrderBy(c => c.Name)
                                              .Select(CategoryDto.ToCategoryDto)
                                              .ToList();

            return PaginatedResult<CategoryDto>.Success(resultDtos, resultDtos.Count, pageParameters.PageNr, pageParameters.PageSize);
        }

        /// <summary>
        /// Retrieves all participating categories for a specific event.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<CategoryDto>>> ParticipatingActivityGroupCategories(string eventId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ParticipatingActivityGroupCategory>(c => c.EventId == eventId);
            spec.AddInclude(c => c.Include(c => c.ActivityGroupCategory)
                                  .ThenInclude(c => c.EntityCollection));

            var categoryResult = await schoolsModuleRepoManager.ParticipatingActivityGroupCategories.ListAsync(spec, false, cancellationToken);
            if (!categoryResult.Succeeded) return await Result<IEnumerable<CategoryDto>>.FailAsync(categoryResult.Messages);

            return await Result<IEnumerable<CategoryDto>>.SuccessAsync(
                categoryResult.Data.Select(c => CategoryDto.ToCategoryDto(c.ActivityGroupCategory)));
        }

        /// <summary>
        /// Retrieves a specific participating category by its event ID and category ID.
        /// </summary>
        public async Task<IBaseResult<CategoryDto>> ParticipatingActivityGroupCategory(string eventId, string participatingActivityCategoryId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ParticipatingActivityGroupCategory>(c =>
                c.EventId == eventId && c.ActivityGroupCategoryId == participatingActivityCategoryId);
            spec.AddInclude(c => c.Include(c => c.ActivityGroupCategory));

            var categoryResult = await schoolsModuleRepoManager.ParticipatingActivityGroupCategories.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!categoryResult.Succeeded || categoryResult.Data?.ActivityGroupCategory == null)
                return await Result<CategoryDto>.FailAsync("No participating category found");

            return await Result<CategoryDto>.SuccessAsync(CategoryDto.ToCategoryDto(categoryResult.Data.ActivityGroupCategory));
        }

        #endregion

        #region Activity Groups

        /// <summary>
        /// Retrieves all participating activity groups for a given school event.
        /// </summary>
        public async Task<IBaseResult<IEnumerable<ActivityGroupDto>>> ParticipatingActivityGroups(string eventId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<ParticipatingActivityGroup>(c => c.EventId == eventId);
            spec.AddInclude(c => c.Include(c => c.ActivityGroup));
            spec.AddInclude(c => c.Include(c => c.ParticipatingTeamMembers)
                                  .ThenInclude(c => c.TeamMember));

            var result = await schoolsModuleRepoManager.ParticipatingActivityGroups.ListAsync(spec, false, cancellationToken);
            if (!result.Succeeded) return await Result<IEnumerable<ActivityGroupDto>>.FailAsync(result.Messages);

            return await Result<IEnumerable<ActivityGroupDto>>.SuccessAsync(
                result.Data.Select(c => new ActivityGroupDto(c)));
        }

        #endregion

        #region Team Members

        /// <summary>
        /// Retrieves all team members (learners) for a specific activity group in a school event.
        /// </summary>
        public async Task<IBaseResult<List<LearnerDto>>> ParticipatingActivityGroupTeamMembers(string eventId, string activityGroupId, CancellationToken cancellationToken = default)
        {
            var spec = new LambdaSpec<SchoolEvent<Category<ActivityGroup>>>(c => c.Id == eventId);
            spec.AddInclude(c => c.Include(c => c.ParticipatingActivityGroups)
                                  .ThenInclude(c => c.ParticipatingTeamMembers)
                                  .ThenInclude(c => c.TeamMember)
                                  .ThenInclude(c => c.Parents));

            var result = await schoolsModuleRepoManager.SchoolEvents.FirstOrDefaultAsync(spec, false, cancellationToken);
            if (!result.Succeeded || result.Data == null) return await Result<List<LearnerDto>>.SuccessAsync([]);

            var activityGroup = result.Data.ParticipatingActivityGroups
                                           .FirstOrDefault(c => c.ActivityGroupId == activityGroupId);

            if (activityGroup == null) return await Result<List<LearnerDto>>.SuccessAsync([]);

            return await Result<List<LearnerDto>>.SuccessAsync(
                activityGroup.ParticipatingTeamMembers.Select(c => new LearnerDto(c)).ToList());
        }

        #endregion
    }

}
