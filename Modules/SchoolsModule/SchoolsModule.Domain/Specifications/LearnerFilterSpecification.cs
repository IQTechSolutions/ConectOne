using ConectOne.Domain.Enums;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Filters <see cref="Learner"/> entities based on optional learner ID and gender criteria,
    /// including their associated parent relationships.
    /// </summary>
    public class LearnerFilterSpecification : Specification<Learner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LearnerFilterSpecification"/> class.
        /// </summary>
        /// <param name="learnerId">Optional learner ID to filter by. If null or empty, this filter is ignored.</param>
        /// <param name="gender">The gender filter to apply. If <see cref="Gender.All"/>, this filter is ignored.</param>
        public LearnerFilterSpecification(string? learnerId, Gender gender)
        {
            Criteria = learner =>
                (string.IsNullOrEmpty(learnerId) || learner.Id == learnerId) &&
                (gender == Gender.All || learner.Gender == gender);

            // Eagerly load parent relationships for each learner
            AddInclude(l => l.Include(c => c.Parents));
            AddInclude(l => l.Include(c => c.Images));
        }
    }

}
