using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using GroupingModule.Domain.Entities;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;

namespace SchoolsModule.Infrastructure.Implementation.ActivityGroups
{
    /// <summary>
    /// Service for handling notification-related operations for activity groups, categories, and participating groups.
    /// </summary>
    public class ActivityGroupNotificationService(ISchoolsModuleRepoManager schoolsModuleRepoManager, IRepository<Category<ActivityGroup>, string> categoryRepo) : IActivityGroupNotificationService
    {
        /// <summary>
        /// Retrieves a list of recipients (learners, parents, and teachers) associated with the specified activity group.
        /// </summary>
        /// <param name="activityGroupId">The ID of the activity group.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A list of recipients for notification.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> ActivityGroupNotificationList(string activityGroupId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(activityGroupId))
                return await Result<IEnumerable<RecipientDto>>.FailAsync("ActivityGroupId must be provided.");

            var spec = new LambdaSpec<ActivityGroup>(ag => ag.Id == activityGroupId);
            spec.AddInclude(q => q.Include(a => a.TeamMembers)
                                  .ThenInclude(tm => tm.Learner).ThenInclude(l => l.EmailAddresses));
            spec.AddInclude(q => q.Include(a => a.TeamMembers)
                                  .ThenInclude(tm => tm.Learner).ThenInclude(l => l.Parents).ThenInclude(p => p.Parent).ThenInclude(pp => pp.EmailAddresses));

            var result = await schoolsModuleRepoManager.ActivityGroups.ListAsync(spec, false, cancellationToken);
            var activityGroup = result.Data.FirstOrDefault();
            if (activityGroup == null) return await Result<IEnumerable<RecipientDto>>.FailAsync("Activity group not found.");

            var recipients = new List<RecipientDto>();

            foreach (var tm in activityGroup.TeamMembers)
            {
                var learner = tm.Learner;
                if (recipients.All(r => r.Id != learner.Id))
                {
                    recipients.Add(new RecipientDto(learner.Id, learner.FirstName, learner.LastName, learner.EmailAddresses.Select(e => e.Email).ToList(), true, true));
                }

                foreach (var parentRel in learner.Parents)
                {
                    var parent = parentRel.Parent;
                    if (recipients.All(r => r.Id != parent.Id))
                    {
                        recipients.Add(new RecipientDto(parent.Id, parent.FirstName, parent.LastName, parent.EmailAddresses.Select(e => e.Email).ToList(), parent.ReceiveNotifications, parent.RecieveEmails));
                    }
                }
            }

            var teacherSpec = new LambdaSpec<Teacher>(t => t.Id == activityGroup.TeacherId);
            teacherSpec.AddInclude(q => q.Include(t => t.EmailAddresses));
            var teacherResult = await schoolsModuleRepoManager.Teachers.ListAsync(teacherSpec, false, cancellationToken);

            foreach (var teacher in teacherResult.Data)
            {
                if (recipients.All(r => r.Id != teacher.Id))
                {
                    recipients.Add(new RecipientDto(teacher.Id, teacher.Name, teacher.Surname, teacher.EmailAddresses.Select(e => e.Email).ToList(), true, false));
                }
            }

            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(recipients);
        }

        /// <summary>
        /// Retrieves a list of recipients associated with the specified activity group category,
        /// including learners, their selected parents, and teachers of the groups under that category.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A list of recipients for notification.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> ActivityGroupCategoryNotificationList(string categoryId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                return await Result<IEnumerable<RecipientDto>>.FailAsync("CategoryId must be provided.");

            var groupResult = await GetCategorySubCategoriesEntities(categoryId, new List<ActivityGroupDto>());
            if (!groupResult.Succeeded) return await Result<IEnumerable<RecipientDto>>.FailAsync(groupResult.Messages);

            var recipients = new List<RecipientDto>();

            foreach (var group in groupResult.Data)
            {
                foreach (var learner in group.TeamMembers)
                {
                    if (recipients.All(r => r.Id != learner.LearnerId))
                    {
                        recipients.Add(new RecipientDto(learner.LearnerId, learner.FirstName, learner.LastName,
                            new List<string> { learner.EmailAddress }, true, true));
                    }

                    foreach (var parent in learner.SelectedParents)
                    {
                        if (recipients.All(r => r.Id != parent.ParentId) && parent.RecieveEmails)
                        {
                            recipients.Add(new RecipientDto(parent.ParentId, parent.FirstName, parent.LastName,
                                parent.EmailAddresses.Select(e => e.EmailAddress).ToList(), parent.ReceiveNotifications, parent.RecieveEmails));
                        }
                    }
                }

                var teacherSpec = new LambdaSpec<Teacher>(t => t.Id == group.Teacher.TeacherId);
                teacherSpec.AddInclude(q => q.Include(t => t.EmailAddresses));
                var teacherResult = await schoolsModuleRepoManager.Teachers.ListAsync(teacherSpec, false, cancellationToken);

                if (teacherResult.Data != null && teacherResult.Data.Any())
                {
                    foreach (var teacher in teacherResult.Data)
                    {
                        if (recipients.All(r => r.Id != teacher.Id))
                        {
                            recipients.Add(new RecipientDto(teacher.Id, teacher.Name, teacher.Surname,
                                teacher.EmailAddresses.Select(e => e.Email).ToList(), true, false));
                        }
                    }
                }
            }

            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(recipients);
        }

        /// <summary>
        /// Retrieves a list of recipients associated with a participating activity group.
        /// </summary>
        /// <param name="participatingActivityGroupId">The ID of the participating activity group.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A list of recipients for notification.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> ParticipatingActivityGroupNotificationList(string participatingActivityGroupId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(participatingActivityGroupId))
                return await Result<IEnumerable<RecipientDto>>.FailAsync("ParticipatingActivityGroupId must be provided.");

            var spec = new LambdaSpec<ParticipatingActivityGroup>(p => p.Id == participatingActivityGroupId);
            spec.AddInclude(q => q.Include(p => p.ActivityGroup));
            spec.AddInclude(q => q.Include(p => p.ParticipatingTeamMembers)
                                  .ThenInclude(tm => tm.TeamMember).ThenInclude(l => l.Parents).ThenInclude(p => p.Parent).ThenInclude(pp => pp.EmailAddresses));

            var result = await schoolsModuleRepoManager.ParticipatingActivityGroups.ListAsync(spec, false, cancellationToken);
            var pag = result.Data.FirstOrDefault();
            if (pag == null) return await Result<IEnumerable<RecipientDto>>.FailAsync("Participating activity group not found.");

            var recipients = new List<RecipientDto>();

            foreach (var tm in pag.ParticipatingTeamMembers)
            {
                var learner = tm.TeamMember;
                if (recipients.All(r => r.Id != learner.Id))
                {
                    recipients.Add(new RecipientDto(learner.Id, learner.FirstName, learner.LastName, learner.EmailAddresses.Select(e => e.Email).ToList(), true, true));
                }

                foreach (var parent in learner.Parents)
                {
                    var p = parent.Parent;
                    if (recipients.All(r => r.Id != p.Id))
                    {
                        recipients.Add(new RecipientDto(p.Id, p.FirstName, p.LastName, p.EmailAddresses.Select(e => e.Email).ToList(), p.ReceiveNotifications, p.RecieveEmails));
                    }
                }
            }

            var teacherSpec = new LambdaSpec<Teacher>(t => t.Id == pag.ActivityGroup.TeacherId);
            teacherSpec.AddInclude(q => q.Include(t => t.EmailAddresses));
            var teacherResult = await schoolsModuleRepoManager.Teachers.ListAsync(teacherSpec, false, cancellationToken);

            foreach (var teacher in teacherResult.Data)
            {
                if (recipients.All(r => r.Id != teacher.Id))
                {
                    recipients.Add(new RecipientDto(teacher.Id, teacher.Name, teacher.Surname, teacher.EmailAddresses.Select(e => e.Email).ToList(), true, false));
                }
            }

            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(recipients);
        }

        #region Helpers

        /// <summary>
        /// Recursively retrieves activity groups from a category and all its subcategories.
        /// </summary>
        /// <param name="activityGroupCategoryId">The ID of the category.</param>
        /// <param name="activityGroupList">The list to populate with activity group DTOs.</param>
        /// <returns>A list of activity group DTOs under the specified category and subcategories.</returns>
        private async Task<IBaseResult<List<ActivityGroupDto>>> GetCategorySubCategoriesEntities(string activityGroupCategoryId, List<ActivityGroupDto> activityGroupList)
        {
            var spec = new LambdaSpec<Category<ActivityGroup>>(c => c.Id == activityGroupCategoryId);
            spec.AddInclude(q => q.Include(c => c.SubCategories));
            spec.AddInclude(q => q.Include(c => c.EntityCollection).ThenInclude(ec => ec.Entity).ThenInclude(e => e.TeamMembers).ThenInclude(tm => tm.Learner).ThenInclude(c => c.EmailAddresses));
            spec.AddInclude(q => q.Include(c => c.EntityCollection).ThenInclude(ec => ec.Entity).ThenInclude(e => e.TeamMembers).ThenInclude(tm => tm.Learner).ThenInclude(l => l.Parents).ThenInclude(p => p.Parent).ThenInclude(c => c.EmailAddresses));
            spec.AddInclude(q => q.Include(c => c.EntityCollection).ThenInclude(ec => ec.Entity).ThenInclude(c => c.Teacher).ThenInclude(c => c.EmailAddresses));

            var result = await categoryRepo.ListAsync(spec, false);
            var category = result.Data.FirstOrDefault();

            if (category == null)
                return await Result<List<ActivityGroupDto>>.FailAsync("Category not found");

            if (category.HasSubCategories)
            {
                foreach (var sub in category.SubCategories)
                {
                    var subResult = await GetCategorySubCategoriesEntities(sub.Id, activityGroupList);
                    if (!subResult.Succeeded)
                        return await Result<List<ActivityGroupDto>>.FailAsync(subResult.Messages);

                    activityGroupList = subResult.Data;
                }
            }

            var groups = category.EntityCollection.Select(ec => new ActivityGroupDto(ec.Entity)).ToList();
            activityGroupList.AddRange(groups);

            return await Result<List<ActivityGroupDto>>.SuccessAsync(activityGroupList);
        }

        #endregion
    }
}
