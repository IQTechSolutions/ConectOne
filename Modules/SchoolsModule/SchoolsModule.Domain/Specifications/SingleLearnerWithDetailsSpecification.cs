using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// A specification to retrieve a single <see cref="Learner"/> entity by ID, 
    /// including detailed related navigation properties such as contact numbers, 
    /// email addresses, parents, school grade, and school class.
    /// </summary>
    public class SingleLearnerWithDetailsSpecification : Specification<Learner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleLearnerWithDetailsSpecification"/> class
        /// with criteria to fetch a learner and associated detailed entities.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner.</param>
        public SingleLearnerWithDetailsSpecification(string learnerId)
        {
            Criteria = l => l.Id == learnerId;

            // Include Images
            AddInclude(l => l.Include(c => c.Images));

            // Include contact numbers
            AddInclude(l => l.Include(c => c.ContactNumbers));

            // Include email addresses
            AddInclude(l => l.Include(c => c.EmailAddresses));

            // Include parent associations
            AddInclude(l => l.Include(c => c.Parents));

            // Include related school grade
            AddInclude(l => l.Include(c => c.SchoolGrade));

            // Include related school class
            AddInclude(l => l.Include(c => c.SchoolClass));
        }
    }
}
