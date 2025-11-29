using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving a <see cref="SchoolEvent{TEntity}"/> with its related 
    /// participating activity groups and team member learners by event ID.
    /// </summary>
    public class EventWithActivityGroupLearnersSpecification : Specification<SchoolEvent<Category<ActivityGroup>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventWithActivityGroupLearnersSpecification"/> class
        /// with a given school event ID to include participating activity groups and their team members.
        /// </summary>
        /// <param name="eventId">The ID of the school event to retrieve.</param>
        public EventWithActivityGroupLearnersSpecification(string eventId)
        {
            // Define the filtering criteria
            Criteria = e => e.Id == eventId;

            // Define the required related data to eagerly load
            AddInclude(e => e
                .Include(c => c.ParticipatingActivityGroups)
                .ThenInclude(c => c.ParticipatingTeamMembers)
                .ThenInclude(c => c.TeamMember));
        }
    }
}
