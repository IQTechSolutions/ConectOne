using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for paginated retrieval of <see cref="Teacher"/> records,
    /// with optional search filtering and eager loading of related entities.
    /// </summary>
    public class PagedTeachersSpecification : Specification<Teacher>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedTeachersSpecification"/> class.
        /// Applies optional search criteria, pagination settings, and includes related entities for each teacher.
        /// </summary>
        /// <param name="p">The page parameters including page number, size, and optional search text.</param>
        public PagedTeachersSpecification(TeacherPageParameters p)
        {
            Criteria = c => true;
            // Apply search filtering if SearchText is provided
            if (!string.IsNullOrEmpty(p.SearchText))
            {
                Criteria = Criteria.And(parent => (parent.Name+parent.Surname).ToLower().Contains(p.SearchText.ToLower()));
            }
            if (!string.IsNullOrEmpty(p.ClassId))
            {
                Criteria = Criteria.And(parent => parent.SchoolClassId == p.ClassId);
            }
            if (!string.IsNullOrEmpty(p.GradeId))
            {
                Criteria = Criteria.And(parent => parent.GradeId == p.GradeId);
            }

            // Include profile image collection
            AddInclude(q => q.Include(t => t.Images));

            // Include contact numbers
            AddInclude(q => q.Include(t => t.ContactNumbers));

            // Include email addresses
            AddInclude(q => q.Include(t => t.EmailAddresses));

            // Include home or work address
            AddInclude(q => q.Include(t => t.Address));

            // Include grade assignment
            AddInclude(q => q.Include(t => t.Grade));

            // Include assigned school class
            AddInclude(q => q.Include(t => t.SchoolClass));
        }
    }
}
