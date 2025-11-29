using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Represents a specification used to query participating activity groups associated with a specific event
    /// identifier, including related activity group details, categories, and participating team member information.
    /// </summary>
    /// <remarks>This specification is intended for use with repositories or data access layers that support
    /// the Specification pattern. It includes related entities such as activity groups, categories, team members, their
    /// parents, and school grades to ensure all relevant data is loaded for each participating activity
    /// group.</remarks>
    public class ParticipatingActivityGroupsByEventIdSpec : Specification<ParticipatingActivityGroup>
    {
        /// <summary>
        /// Initializes a new instance of the ParticipatingActivityGroupsByEventIdSpec class to filter participating
        /// activity groups by the specified event identifier.
        /// </summary>
        /// <remarks>This specification includes related activity group details, categories, participating
        /// team members, their parents, and school grade information. Use this constructor to retrieve all
        /// participating activity groups and their associated data for a given event.</remarks>
        /// <param name="eventId">The unique identifier of the event to filter participating activity groups for. Cannot be null or empty.</param>
        public ParticipatingActivityGroupsByEventIdSpec(string eventId)
        {
            Criteria = c => c.EventId == eventId;

            AddInclude(q => q.Include(c => c.ActivityGroup)
                .ThenInclude(c => c.Categories)
                .ThenInclude(c => c.Category));

            AddInclude(q => q.Include(c => c.ParticipatingTeamMembers)
                .ThenInclude(c => c.TeamMember)
                .ThenInclude(c => c.Parents));

            AddInclude(q => q.Include(c => c.ParticipatingTeamMembers)
                .ThenInclude(c => c.TeamMember)
                .ThenInclude(c => c.SchoolGrade));
        }
    }
}
