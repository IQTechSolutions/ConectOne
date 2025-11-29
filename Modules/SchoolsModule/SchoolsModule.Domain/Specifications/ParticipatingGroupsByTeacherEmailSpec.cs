using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving all participating activity groups where the assigned teacher
    /// has a specific email address. Includes related event and teacher details.
    /// </summary>
    public class ParticipatingGroupsByTeacherEmailSpec : Specification<ParticipatingActivityGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticipatingGroupsByTeacherEmailSpec"/> class.
        /// Filters participating groups by teacher email and includes relevant event and teacher data.
        /// </summary>
        /// <param name="email">The email address of the teacher (case-insensitive).</param>
        public ParticipatingGroupsByTeacherEmailSpec(string email)
        {
            // Filter: only include groups where the teacher's email matches
            Criteria = g => g.ActivityGroup.Teacher.EmailAddresses
                .Any(e => e.Email.ToLower() == email.ToLower());

            // Include event details and its participating categories
            AddInclude(q => q.Include(g => g.Event)
                .ThenInclude(e => e.ParticipatingCategories));

            // Include the activity group, its teacher, and the teacher's email addresses
            AddInclude(q => q.Include(g => g.ActivityGroup)
                .ThenInclude(a => a.Teacher)
                .ThenInclude(t => t.EmailAddresses));
        }
    }
}
