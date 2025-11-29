using ConectOne.Domain.Enums;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving <see cref="Learner"/> entities for school grade-related notifications.
    /// Allows filtering by gender and grade ID, and includes email addresses and parent contact details.
    /// </summary>
    public sealed class SchoolGradeNotificationListSpecification : Specification<Learner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolGradeNotificationListSpecification"/> class
        /// using the provided learner filtering parameters.
        /// </summary>
        /// <param name="p">Filtering criteria including gender and grade ID.</param>
        public SchoolGradeNotificationListSpecification(LearnerPageParameters p)
        {
            // Begin with a default predicate that matches all learners
            Criteria = PredicateBuilder.New<Learner>(true);

            // Apply gender filter if specified and not 'All'
            if (p.Gender != Gender.All && p.Gender != null)
            {
                Criteria = Criteria.And(c => c.Gender == p.Gender);
            }

            // Apply grade ID filter if provided
            if (!string.IsNullOrEmpty(p.GradeId))
            {
                Criteria = Criteria.And(c => c.SchoolGradeId == p.GradeId);
            }

            AddInclude(c => c.Include(c => c.EmailAddresses));
            AddInclude(c => c.Include(c => c.Parents).ThenInclude(c => c.Parent).ThenInclude(c => c.EmailAddresses));
        }
    }
}