using System.Linq.Expressions;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving a single <see cref="Teacher"/> by either their ID or email address,
    /// along with related profile and organizational data.
    /// </summary>
    public class SingleTeacherSpecification : Specification<Teacher>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleTeacherSpecification"/> class.
        /// Filters by teacher ID or email, and includes all relevant navigation properties.
        /// </summary>
        /// <param name="parentId">The teacher's unique identifier (optional).</param>
        /// <param name="emailAddress">The teacher's email address (optional, case-insensitive).</param>
        public SingleTeacherSpecification(string? parentId = null, string? emailAddress = null)
        {
            // Apply filter by teacher ID if provided
            if (!string.IsNullOrEmpty(parentId))
                Criteria = parent => parent.Id == parentId;

            // Override or set filter by email address if provided
            if (!string.IsNullOrEmpty(emailAddress))
            {
                var emailCriteria = (Expression<Func<Teacher, bool>>)(t =>
                    t.EmailAddresses.Any(e => e.Email.ToLower() == emailAddress.ToLower()));
                Criteria = emailCriteria;
            }

            // Include teacher's image files
            AddInclude(q => q.Include(t => t.Images));

            // Include contact numbers
            AddInclude(q => q.Include(t => t.ContactNumbers));

            // Include email addresses
            AddInclude(q => q.Include(t => t.EmailAddresses));

            // Include address information
            AddInclude(q => q.Include(t => t.Address));

            // Include assigned grade
            AddInclude(q => q.Include(t => t.Grade));

            // Include assigned school class
            AddInclude(q => q.Include(t => t.SchoolClass));
        }
    }

}
