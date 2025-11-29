using ConectOne.Domain.Enums;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving <see cref="Learner"/> entities for notification purposes,
    /// filtered by gender and school class ID, and including their email addresses and parent contact information.
    /// </summary>
    public sealed class SchoolClassNotificationListSpecification : Specification<Learner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolClassNotificationListSpecification"/> class
        /// using the provided filter parameters.
        /// </summary>
        /// <param name="p">Filtering parameters including gender and school class ID.</param>
        public SchoolClassNotificationListSpecification(LearnerPageParameters p)
        {
            // Start with a predicate that matches all learners
            Criteria = PredicateBuilder.New<Learner>(true);

            // Apply gender filter if a specific gender is selected
            if (p.Gender != Gender.All && p.Gender != null)
            {
                Criteria = Criteria.And(c => c.Gender == p.Gender);
            }

            // Filter by school class if an ID is provided
            if (!string.IsNullOrEmpty(p.SchoolClassId))
            {
                Criteria = Criteria.And(c => c.SchoolClassId == p.SchoolClassId);
            }

            AddInclude(c => c.Include(c => c.EmailAddresses));
            AddInclude(c => c.Include(c => c.Parents).ThenInclude(c => c.Parent).ThenInclude(c => c.EmailAddresses));
        }
    }
}