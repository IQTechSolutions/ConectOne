using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving a single <see cref="Parent"/> by ID, including their associated learners.
    /// </summary>
    public class ParentWithLearnersByIdSpec : Specification<Parent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParentWithLearnersByIdSpec"/> class.
        /// Filters by the specified parent ID and includes all learner relationships.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent.</param>
        public ParentWithLearnersByIdSpec(string parentId)
        {
            // Filter: match only the specified parent
            Criteria = p => p.Id == parentId;

            // Include the learners linked to this parent (via join entity)
            AddInclude(q => q.Include(p => p.Learners)
                .ThenInclude(lp => lp.Learner));
        }
    }

}
