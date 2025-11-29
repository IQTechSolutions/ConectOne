using ConectOne.Domain.Enums;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// A specification used to filter, include related entities, and paginate learner records
    /// based on criteria such as class, grade, gender, and parent association.
    /// </summary>
    public class PagedLearnersSpecification : Specification<Learner>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedLearnersSpecification"/> class.
        /// Applies dynamic filtering and eager loading based on provided <see cref="LearnerPageParameters"/>.
        /// </summary>
        /// <param name="parameters">Parameters used to filter learners, such as grade, class, gender, and parent ID.</param>
        public PagedLearnersSpecification(LearnerPageParameters parameters)
        {
            Criteria =  PredicateBuilder.New<Learner>(true);

            if (parameters.Gender != null && parameters.Gender != Gender.All)
                Criteria = Criteria.And(l => l.Gender == parameters.Gender);

            if (!string.IsNullOrEmpty(parameters.ParentId))
                Criteria = Criteria.And(l => l.Parents.Any(g => g.ParentId == parameters.ParentId));

            if (!string.IsNullOrEmpty(parameters.SchoolClassId))
                Criteria = Criteria.And(l => l.SchoolClassId == parameters.SchoolClassId);

            if (!string.IsNullOrEmpty(parameters.GradeId))
                Criteria = Criteria.And(l => l.SchoolGradeId == parameters.GradeId);


            if (!string.IsNullOrEmpty(parameters.SearchText))
                Criteria = Criteria.And(c => c.FirstName.ToLower().Contains(parameters.SearchText.ToLower()) || c.LastName.ToLower().Contains(parameters.SearchText.ToLower()));

            AddInclude(l => l.Include(c => c.Images));

            // Eager-load related entities to avoid lazy loading and improve query performance
            AddInclude(l => l.Include(c => c.ContactNumbers));     // Include learner's contact numbers
            AddInclude(l => l.Include(c => c.EmailAddresses));     // Include learner's email addresses
            AddInclude(l => l.Include(c => c.Parents));            // Include associations with parents
            AddInclude(l => l.Include(c => c.SchoolGrade));        // Include learner's school grade
            AddInclude(l => l.Include(c => c.SchoolClass));        // Include learner's school class
        }
    }
}

