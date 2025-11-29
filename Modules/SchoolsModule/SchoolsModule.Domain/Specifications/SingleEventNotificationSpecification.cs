using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification to retrieve a single <see cref="SchoolEvent{TEntity}"/> with all necessary related entities
    /// for notification purposes, including team members, their parents, and assigned teachers.
    /// </summary>
    public class SingleEventNotificationSpecification : Specification<SchoolEvent<Category<ActivityGroup>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleEventNotificationSpecification"/> class.
        /// </summary>
        /// <param name="schoolEventId">The ID of the event to retrieve.</param>
        public SingleEventNotificationSpecification(string? schoolEventId = null)
        {
            // Filter to the event with the specified ID
            Criteria = e => e.Id == schoolEventId;

            // Include teacher email addresses
            AddInclude(q => q
                .Include(c => c.ParticipatingActivityGroups)
                .ThenInclude(c => c.ActivityGroup)
                .ThenInclude(c => c.Teacher)
                .ThenInclude(c => c.EmailAddresses));

            // Include learner (team member) email addresses
            AddInclude(q => q
                .Include(c => c.ParticipatingActivityGroups)
                .ThenInclude(c => c.ParticipatingTeamMembers)
                .ThenInclude(c => c.TeamMember)
                .ThenInclude(c => c.EmailAddresses));

            // Include parent email addresses for each learner
            AddInclude(q => q
                .Include(c => c.ParticipatingActivityGroups)
                .ThenInclude(c => c.ParticipatingTeamMembers)
                .ThenInclude(c => c.TeamMember)
                .ThenInclude(c => c.Parents)
                .ThenInclude(c => c.Parent)
                .ThenInclude(c => c.EmailAddresses));

            // Include learner consents for the event
            AddInclude(q => q
                .Include(c => c.ParticipatingActivityGroups)
                .ThenInclude(c => c.ParticipatingTeamMembers)
                .ThenInclude(c => c.TeamMember)
                .ThenInclude(c => c.EventConsents));
        }
    }
}
