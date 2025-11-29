using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification used to retrieve a paginated list of <see cref="Parent"/> entities with optional filtering and eager loading of related data.
    /// </summary>
    /// <remarks>
    /// This specification supports the following filters:
    /// <list type="bullet">
    ///     <item>Filtering by a specific <c>LearnerId</c>, optionally restricted when <c>LinkParents</c> is false.</item>
    ///     <item>Text search by matching concatenated <c>FirstName</c> and <c>LastName</c>.</item>
    /// </list>
    /// Additionally, it sets pagination parameters using <c>Skip</c> and <c>Take</c> and includes related collections such as:
    /// <see cref="Parent.ContactNumbers"/>, <see cref="Parent.EmailAddresses"/>, <see cref="Parent.Addresses"/>, and nested learners.
    /// </remarks>
    public sealed class PagedParentsSpecification : Specification<Parent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedParentsSpecification"/> class using the provided paging and filtering parameters.
        /// </summary>
        /// <param name="p">Paging and filter parameters supplied by the client.</param>
        public PagedParentsSpecification(ParentPageParameters p)
        {
            // Start with a base predicate that always evaluates to true
            Criteria = PredicateBuilder.New<Parent>(true);

            // Filter by learner ID only if provided and not linking multiple parents
            if (p.LearnerId != null && !p.LinkParents)
            {
                Criteria = Criteria.And(f => f.Learners.Any(l => l.Id == p.LearnerId));
            }

            // Apply text-based search on full name (concatenated first and last names, case-insensitive)
            if (!string.IsNullOrEmpty(p.SearchText))
            {
                Criteria = Criteria.And(parent => parent.FirstName.ToLower().Contains(p.SearchText.ToLower()) || 
                                                  parent.LastName.ToLower().Contains(p.SearchText.ToLower()));
            }

            AddInclude(c => c.Include(parent => parent.Images));
            AddInclude(c => c.Include(parent => parent.ContactNumbers));
            AddInclude(c => c.Include(parent => parent.EmailAddresses));
            AddInclude(c => c.Include(parent => parent.EmergencyContacts));
            AddInclude(c => c.Include(parent => parent.Addresses));
            AddInclude(c => c.Include(parent => parent.Learners).ThenInclude(c => c.Learner));
        }
    }
}
