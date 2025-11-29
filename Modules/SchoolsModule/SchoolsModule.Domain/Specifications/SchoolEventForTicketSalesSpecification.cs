using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Represents a specification for retrieving a school event and its related data,  tailored for scenarios involving
    /// ticket sales.
    /// </summary>
    /// <remarks>This specification is designed to filter and include detailed related data for a specific 
    /// school event, such as associated documents, participating categories, activity groups,  age groups, and team
    /// members. It supports querying a specific event by its ID and ensures  that all relevant hierarchical
    /// relationships are included in the query results.</remarks>
    public class SchoolEventForTicketSalesSpecification : Specification<SchoolEvent<Category<ActivityGroup>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolEventForTicketSalesSpecification"/> class with optional
        /// filtering by a specific school event ID.
        /// </summary>
        /// <remarks>This specification is designed to retrieve detailed information about school events,
        /// including associated documents, participating categories, activity groups, age groups, and team members. The
        /// following related entities are included in the query: <list type="bullet"> <item>Documents associated with
        /// the event (e.g., attachments, instructions).</item> <item>Participating categories and their group
        /// categories.</item> <item>Participating activity groups, their category hierarchy, and age group
        /// information.</item> <item>Participating team members and their associated learners.</item> </list> Use this
        /// specification to ensure all necessary data for ticket sales and event management is retrieved in a single
        /// query.</remarks>
        /// <param name="schoolEventId">The unique identifier of the school event to filter by. If <see langword="null"/>, no specific event
        /// filtering is applied.</param>
        public SchoolEventForTicketSalesSpecification(SchoolEventPageParameters pageParameters)
        {
            Criteria = e => e.TicketTypes.Any() && e.StartDate.Date >= DateTime.Now.Date;
            if (pageParameters.Featured is not null)
            {
                Criteria = Criteria.And(f => f.Featured);
            }

            AddInclude(q => q.Include(e => e.TicketTypes));
        }
    }

}
