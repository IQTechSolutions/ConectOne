using ConectOne.Domain.Enums;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification to retrieve learners with notification-relevant details such as
    /// contact information, grade, class, and optional parent filtering.
    /// </summary>
    public class LearnersWithNotificationDetailsSpecification : Specification<Learner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LearnersWithNotificationDetailsSpecification"/> class.
        /// Applies filtering based on school class, grade, gender, and parent ID.
        /// </summary>
        /// <param name="parameters">The filter parameters including pagination, class, grade, gender, and parent info.</param>
        public LearnersWithNotificationDetailsSpecification(LearnerPageParameters parameters)
        {
            // Filtering logic based on the provided parameters
            Criteria = l =>
                (string.IsNullOrEmpty(parameters.SchoolClassId) || l.SchoolClassId == parameters.SchoolClassId) &&
                (string.IsNullOrEmpty(parameters.GradeId) || l.SchoolGradeId == parameters.GradeId) &&
                (parameters.Gender == null || parameters.Gender == Gender.All || l.Gender == parameters.Gender) &&
                (string.IsNullOrEmpty(parameters.ParentId) || l.Parents.Any(p => p.ParentId == parameters.ParentId));

            // Includes contact numbers, email addresses, grade and class for notification targeting
            AddInclude(l => l.Include(c => c.ContactNumbers));
            AddInclude(l => l.Include(c => c.EmailAddresses));
            AddInclude(l => l.Include(c => c.SchoolGrade));
            AddInclude(l => l.Include(c => c.SchoolClass));
        }
    }
}
