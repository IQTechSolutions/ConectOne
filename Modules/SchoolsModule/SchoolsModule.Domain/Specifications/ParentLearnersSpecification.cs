using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving all learners associated with a specific parent,
    /// including their contact numbers, email addresses, and parent relationships.
    /// </summary>
    public class ParentLearnersSpecification : Specification<LearnerParent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParentLearnersSpecification"/> class.
        /// Filters <see cref="LearnerParent"/> entries based on the given parent ID,
        /// and includes detailed learner navigation properties.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent.</param>
        public ParentLearnersSpecification(string parentId)
        {
            // Filter records to only include those where the parent ID matches
            Criteria = parent => parent.ParentId == parentId;

            // Include the learner's contact numbers
            AddInclude(c => c.Include(parent => parent.Learner)
                .ThenInclude(learner => learner.ContactNumbers));

            // Include the learner's email addresses
            AddInclude(c => c.Include(parent => parent.Learner)
                .ThenInclude(learner => learner.EmailAddresses));

            // Include the learner's relationships to other parents
            AddInclude(c => c.Include(parent => parent.Learner).ThenInclude(g => g.SchoolGrade));
        }
    }

}
