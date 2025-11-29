using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving a filtered and optionally paginated list of <see cref="SchoolClass"/> entities.
    /// </summary>
    /// <remarks>
    /// This specification allows filtering by Grade ID and includes the related <see cref="SchoolGrade"/> entity.
    /// Use this when displaying or exporting school class data with associated grade information.
    /// </remarks>
    public sealed class PagedSchoolClassSpecification : Specification<SchoolClass>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedSchoolClassSpecification"/> class with optional filtering parameters.
        /// </summary>
        /// <param name="p">Parameters for filtering and paging school class data.</param>
        public PagedSchoolClassSpecification(SchoolClassPageParameters p)
        {
            // Start with a predicate that always returns true (no filter)
            Criteria = PredicateBuilder.New<SchoolClass>(true);
            AddInclude(b => b.Include(g => g.PersonnelCollection).ThenInclude(r => r.EmailAddresses));

            if (p.GradeId != null)
            {
                Criteria = Criteria.And(c => c.GradeId == p.GradeId);
            }

            if (p.TeacherId != null)
            {
                Criteria = Criteria.And(c => c.PersonnelCollection.Any(g => g.Id == p.TeacherId));
            }

            if (p.TeacherEmail != null)
            {
                Criteria = Criteria.And(c => c.PersonnelCollection.Any(g => g.EmailAddresses.Any(r => r.Email == p.TeacherEmail)));
            }

            // Include the related Grade entity for eager loading
            AddInclude(c => c.Include(c => c.Grade));
        }
    }
}