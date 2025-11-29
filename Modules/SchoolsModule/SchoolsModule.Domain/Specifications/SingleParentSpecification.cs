using System.Linq.Expressions;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving a single parent entity by either parent ID or email address,
    /// including full contact and learner relationship data.
    /// </summary>
    public class SingleParentSpecification : Specification<Parent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleParentSpecification"/> class.
        /// Allows querying by parent ID or email address and includes associated contact details and learners.
        /// </summary>
        /// <param name="parentId">The unique identifier of the parent (optional).</param>
        /// <param name="emailAddress">The email address to match (optional, case-insensitive).</param>
        public SingleParentSpecification(string? parentId = null, string? emailAddress = null)
        {
            // Apply filter by parent ID if provided
            if (!string.IsNullOrEmpty(parentId))
                Criteria = parent => parent.Id == parentId;

            // Override or set filter by email address if provided
            if (!string.IsNullOrEmpty(emailAddress))
            {
                var emailCriteria = (Expression<Func<Parent, bool>>)(p =>
                    p.EmailAddresses.Any(e => e.Email.ToLower() == emailAddress.ToLower()));

                Criteria = emailCriteria;
            }

            // Include associated images
            AddInclude(c => c.Include(parent => parent.Images));

            // Include associated contact numbers
            AddInclude(c => c.Include(parent => parent.ContactNumbers));

            // Include associated email addresses
            AddInclude(c => c.Include(parent => parent.EmailAddresses));

            // Include emergency contacts
            AddInclude(c => c.Include(parent => parent.EmergencyContacts));

            // Include physical addresses
            AddInclude(c => c.Include(parent => parent.Addresses));

            // Include learner relationships (links and learners)
            AddInclude(c => c.Include(parent => parent.Learners).ThenInclude(link => link.Learner));
        }
    }

}
