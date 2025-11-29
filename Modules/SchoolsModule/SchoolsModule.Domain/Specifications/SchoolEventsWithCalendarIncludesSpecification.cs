using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification used to fetch school events along with related data for calendar-based views.
    /// Includes filters based on start date and necessary navigation properties.
    /// </summary>
    public class SchoolEventsWithCalendarIncludesSpecification : Specification<SchoolEvent<Category<ActivityGroup>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolEventsWithCalendarIncludesSpecification"/> class.
        /// Filters events either by a specific start date or for all upcoming events.
        /// </summary>
        /// <param name="startDate">
        /// Optional start date for filtering events. If provided, only events starting on this date are included.
        /// If null, all events starting today or later are included.
        /// </param>
        public SchoolEventsWithCalendarIncludesSpecification(DateTime? startDate = null)
        {
            // Filter events either by exact start date or from today forward
            Criteria = startDate.HasValue
                ? (e => e.StartDate.Date == startDate.Value.Date)
                : (e => e.StartDate.Date >= DateTime.Today);

            // Include event category details
            AddInclude(e => e.Include(p => p.ParticipatingCategories)
                .ThenInclude(c => c.ActivityGroupCategory));

            // Include team members participating in events
            AddInclude(c => c.Include(p => p.ParticipatingActivityGroups)
                .ThenInclude(g => g.ParticipatingTeamMembers)
                .ThenInclude(c => c.TeamMember));

            // Include associated activity groups
            AddInclude(c => c.Include(p => p.ParticipatingActivityGroups)
                .ThenInclude(g => g.ActivityGroup));

            // Include event views
            AddInclude(c => c.Include(p => p.Views));
        }
    }

}
