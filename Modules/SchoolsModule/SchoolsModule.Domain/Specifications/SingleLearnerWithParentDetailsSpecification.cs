using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// A specification for querying a single <see cref="Learner"/> by ID, including their associated parent relationships.
    /// </summary>
    public class SingleLearnerWithParentDetailsSpecification : Specification<Learner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="learnerId"/> class.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner to query.</param>
        public SingleLearnerWithParentDetailsSpecification(string learnerId)
        {
            Criteria = l => l.Id == learnerId;

            // Includes parent associations
            AddInclude(l => l.Include(c => c.Parents).ThenInclude(c => c.Parent).ThenInclude(c => c.EmailAddresses));
        }
    }
}