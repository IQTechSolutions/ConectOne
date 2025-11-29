using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving school events, either past (historical) or upcoming (current/future),
    /// with necessary related entities included for evaluation or display.
    /// </summary>
    public class RelevantSchoolEventsSpecification : Specification<SchoolEvent<Category<ActivityGroup>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelevantSchoolEventsSpecification"/> class.
        /// Determines whether to load historical or upcoming events based on the provided flag.
        /// </summary>
        /// <param name="history">If true, retrieves events in the past; otherwise, retrieves current or future events.</param>
        public RelevantSchoolEventsSpecification(bool history)
        {
            // Criteria selection based on history flag
            Criteria = history
                ? (e => e.StartDate < DateTime.UtcNow)    // Past events
                : (e => e.StartDate >= DateTime.UtcNow);  // Current or future events

            // Include users who have viewed the event
            AddInclude(c => c.Include(g => g.Views));

            // Include participating activity groups and their team members and learners
            AddInclude(e => e.Include(c => c.ParticipatingActivityGroups)
                .ThenInclude(c => c.ActivityGroup)
                .ThenInclude(c => c.TeamMembers)
                .ThenInclude(g => g.Learner));

            // Include event categories and their group category details
            AddInclude(e => e.Include(c => c.ParticipatingCategories)
                .ThenInclude(c => c.ActivityGroupCategory));
        }
    }
}
