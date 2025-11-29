using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Represents a specification for filtering and retrieving parent entities based on event permission criteria.
    /// </summary>
    /// <remarks>This specification is designed to query parent entities that match the provided email address
    /// and  eagerly load related collections, including email addresses, event consents, and learners. Use this
    /// specification to efficiently retrieve parent data for school event permissions.</remarks>
    public sealed class EventPermissionParentListSpecification : Specification<Parent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventPermissionParentListSpecification"/> class with criteria
        /// for filtering parent permissions based on the provided request arguments.
        /// </summary>
        /// <remarks>This specification is designed to filter parent permissions by matching the provided
        /// email address (case-insensitive) against the email addresses associated with parents. Additionally, it
        /// includes related collections such as email addresses, event consents, and learners to optimize query
        /// performance by reducing subsequent database queries.</remarks>
        /// <param name="args">The request arguments containing the parent email address used to filter the parent list. The <see
        /// cref="SchoolEventPermissionsRequestArgs.ParentEmail"/> property must not be null or empty.</param>
        public EventPermissionParentListSpecification(SchoolEventPermissionsRequestArgs args)
        {
            // Start with a base predicate that always evaluates to true
            Criteria = p => p.EmailAddresses.Any(e => e.Email.ToLower() == args.ParentEmail.ToLower());

            // Eagerly load related collections to reduce subsequent queries
            AddInclude(q => q.Include(p => p.EmailAddresses));
            AddInclude(q => q.Include(p => p.EmergencyContacts));
            AddInclude(q => q.Include(p => p.EventConsents));
            AddInclude(q => q.Include(p => p.Learners).ThenInclude(lp => lp.Learner));
        }
    }
}
