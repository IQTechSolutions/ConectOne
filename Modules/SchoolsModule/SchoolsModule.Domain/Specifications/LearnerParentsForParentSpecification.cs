using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving all <see cref="LearnerParent"/> entities associated with a specific parent,
    /// including each linked learner's information.
    /// </summary>
    public class LearnerParentsForParentSpecification : Specification<LearnerParent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerParentsForParentSpecification"/> class.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent.</param>
        public LearnerParentsForParentSpecification(string parentId)
        {
            // Filter: Match only records linked to the specified parent ID
            Criteria = lp => lp.ParentId == parentId;

            // Eager-load the learner associated with each LearnerParent record
            AddInclude(lp => lp.Include(c => c.Learner));
        }
    }
}
