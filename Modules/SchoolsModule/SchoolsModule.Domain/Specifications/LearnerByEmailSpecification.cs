using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification to retrieve a <see cref="Learner"/> entity based on a specific email address,
    /// including related contact numbers, email addresses, grade, and class information.
    /// </summary>
    public class LearnerByEmailSpecification : Specification<Learner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerByEmailSpecification"/> class with the specified email.
        /// </summary>
        /// <param name="email">The email address to match against the learner's email records.</param>
        public LearnerByEmailSpecification(string email)
        {
            Criteria = l => l.EmailAddresses.Any(e => e.Email == email);

            AddInclude(l => l.Include(c => c.Images));

            // Eagerly load related data to avoid multiple round-trips to the database.
            AddInclude(l => l.Include(c => c.ContactNumbers));
            AddInclude(l => l.Include(c => c.EmailAddresses));
            AddInclude(l => l.Include(c => c.SchoolGrade));
            AddInclude(l => l.Include(c => c.SchoolClass));
        }
    }

}
