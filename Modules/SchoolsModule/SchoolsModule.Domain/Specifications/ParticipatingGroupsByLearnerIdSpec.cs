using CalendarModule.Domain.Entities;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving all <see cref="ParticipatingActivityGroup"/> records associated with a specific learner,
    /// based on the learner's ID as a team member in any group.
    /// </summary>
    /// <remarks>
    /// This specification filters groups where the learner is listed as a team member and includes the following related data:
    /// <list type="bullet">
    ///     <item><see cref="ParticipatingActivityGroup.Event"/>, including its <see cref="Event{TEntity}.ParticipatingCategories"/> and each category's <see cref="ActivityGroupCategory"/>.</item>
    ///     <item><see cref="ParticipatingActivityGroup.ParticipatingTeamMembers"/> collection.</item>
    /// </list>
    /// It is used to ensure that all relevant context for the learner's participation is eagerly loaded in a single query.
    /// </remarks>
    public class ParticipatingGroupsByLearnerIdSpec : Specification<ParticipatingActivityGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticipatingGroupsByLearnerIdSpec"/> class to filter by learner ID.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner whose groups should be retrieved.</param>
        public ParticipatingGroupsByLearnerIdSpec(string learnerId)
        {
            // Filters to groups where the learner is a participating team member
            Criteria = g => g.ParticipatingTeamMembers.Any(m => m.TeamMemberId == learnerId);

            // Eagerly loads the event and its categories and category types
            AddInclude(q => q.Include(g => g.Event)
                .ThenInclude(e => e.ParticipatingCategories)
                .ThenInclude(c => c.ActivityGroupCategory));

            // Eagerly loads the team members for the group
            AddInclude(q => q.Include(g => g.ParticipatingTeamMembers));
        }
    }

}
