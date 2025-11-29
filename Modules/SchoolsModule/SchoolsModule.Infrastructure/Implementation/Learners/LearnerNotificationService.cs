using ConectOne.Domain.Extensions;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using IdentityModule.Domain.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;
using SchoolsModule.Domain.Specifications;

namespace SchoolsModule.Infrastructure.Implementation.Learners
{
    /// <summary>
    /// Service responsible for retrieving learners, parents, and teachers for notification-related operations.
    /// </summary>
    public class LearnerNotificationService(ISchoolsModuleRepoManager schoolsModuleRepoManager) : ILearnerNotificationService
    {
        /// <summary>
        /// Retrieves a list of learners matching specified notification filters such as age, gender, and text search.
        /// </summary>
        /// <param name="learnerPageParameters">Filtering parameters such as class, grade, age range, etc.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A result object containing a list of <see cref="RecipientDto"/> or error messages.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> LearnersNotificationList(LearnerPageParameters learnerPageParameters, CancellationToken cancellationToken)
        {
            var spec = new LearnersWithNotificationDetailsSpecification(learnerPageParameters);
            var learnerResult = await schoolsModuleRepoManager.Learners.ListAsync(spec, trackChanges: false, cancellationToken);

            if (!learnerResult.Succeeded)
                return await Result<IEnumerable<RecipientDto>>.FailAsync(learnerResult.Messages);

            var learners = learnerResult.Data
                .Where(l =>
                {
                    var age = l.IdNumber.GetAge();
                    return age >= learnerPageParameters.MinAge && age <= learnerPageParameters.MaxAge;
                })
                .Where(l =>
                    string.IsNullOrEmpty(learnerPageParameters.SearchText) ||
                    l.FirstName.Contains(learnerPageParameters.SearchText, StringComparison.OrdinalIgnoreCase) ||
                    l.LastName.Contains(learnerPageParameters.SearchText, StringComparison.OrdinalIgnoreCase))
                .Select(l => new RecipientDto(
                    id: l.Id,
                    name: l.FirstName,
                    lastName: l.LastName,
                    emailAddresses: l.EmailAddresses.Select(e => e.Email).ToList(),
                    receiveNotifications: true,
                    receiveEmails: true,
                    coverImageUrl: null,
                    messageType: null
                ))
                .ToList();

            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(learners);
        }

        /// <summary>
        /// Retrieves a comprehensive list of all learners, parents, and teachers with their email addresses for global mail dispatch.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A result object containing a list of <see cref="RecipientDto"/> representing users across the system.</returns>
        public async Task<IBaseResult<IEnumerable<RecipientDto>>> GlobalMailRecipientList(CancellationToken cancellationToken)
        {
            var currentUserNotificationList = new List<RecipientDto>();

            // Collect learners
            var learnerResult = schoolsModuleRepoManager.Learners.FindAll(false);
            if (learnerResult.Succeeded)
            {
                foreach (var learner in learnerResult.Data.Include(c => c.EmailAddresses))
                {
                    if (currentUserNotificationList.All(c => c.Id != learner.Id))
                        currentUserNotificationList.Add(new RecipientDto(learner.Id, learner.FirstName, learner.LastName, learner.EmailAddresses.Select(c => c.Email).ToList(), true, true));
                }
            }

            // Collect parents
            var parentSpec = new LambdaSpec<Parent>(c => true);
            parentSpec.AddInclude(c => c.Include(g => g.EmailAddresses));
            var parentResult = await schoolsModuleRepoManager.Parents.ListAsync(parentSpec, false, cancellationToken);
            if (parentResult.Succeeded)
            {
                foreach (var parent in parentResult.Data)
                {
                    if (currentUserNotificationList.All(c => c.Id != parent.Id))
                        currentUserNotificationList.Add(new RecipientDto(parent.Id, parent.FirstName, parent.LastName, parent.EmailAddresses.Select(c => c.Email).ToList(), parent.ReceiveNotifications, parent.RecieveEmails));
                }
            }

            // Collect teachers
            var teacherResult = schoolsModuleRepoManager.Teachers.FindAll(false);
            if (teacherResult.Succeeded)
            {
                foreach (var teacher in teacherResult.Data.Include(c => c.EmailAddresses))
                {
                    if (currentUserNotificationList.All(c => c.Id != teacher.Id))
                        currentUserNotificationList.Add(new RecipientDto(teacher.Id, teacher.Name, teacher.Surname, teacher.EmailAddresses.Select(c => c.Email).ToList(), true, true));
                }
            }

            return await Result<IEnumerable<RecipientDto>>.SuccessAsync(currentUserNotificationList);
        }
    }

}
