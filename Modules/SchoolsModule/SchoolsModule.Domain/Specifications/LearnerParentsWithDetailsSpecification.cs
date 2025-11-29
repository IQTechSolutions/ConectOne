using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification to retrieve all <see cref="LearnerParent"/> relationships for a specific learner,
    /// including detailed information about each parent.
    /// </summary>
    public class LearnerParentsWithDetailsSpecification : Specification<LearnerParent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerParentsWithDetailsSpecification"/> class.
        /// </summary>
        /// <param name="learnerId">The ID of the learner for whom to retrieve parent associations.</param>
        public LearnerParentsWithDetailsSpecification(string learnerId)
        {
            // Filter to only include relationships for the specified learner
            Criteria = lp => lp.LearnerId == learnerId;

            // Eagerly load parent contact numbers
            AddInclude(c => c.Include(p => p.Parent).ThenInclude(c => c.Images));

            // Eagerly load parent contact numbers
            AddInclude(c => c.Include(p => p.Parent).ThenInclude(c => c.ContactNumbers));

            // Eagerly load parent email addresses
            AddInclude(c => c.Include(p => p.Parent).ThenInclude(c => c.EmailAddresses));

            // Eagerly load the learner associations of the parent (useful for reverse navigation or cross-referencing)
            AddInclude(c => c.Include(p => p.Parent));
        }
    }
}
