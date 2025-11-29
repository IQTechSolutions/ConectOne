using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving a <see cref="Parent"/> by their email address,
    /// including email details and associated learners.
    /// </summary>
    public class ParentByEmailWithLearnersSpec : Specification<Parent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParentByEmailWithLearnersSpec"/> class.
        /// Filters by a case-insensitive email match and includes learner and email associations.
        /// </summary>
        /// <param name="email">The email address of the parent to search for.</param>
        public ParentByEmailWithLearnersSpec(string email)
        {
            // Filter: match parent by any of their email addresses (case-insensitive)
            Criteria = p => p.EmailAddresses.Any(e => e.Email.ToLower() == email.ToLower());

            // Include the parent's email addresses
            AddInclude(q => q.Include(p => p.EmailAddresses));

            // Include learner links and the learner entities themselves
            AddInclude(q => q.Include(p => p.Learners)
                .ThenInclude(lp => lp.Learner));
        }
    }

}
