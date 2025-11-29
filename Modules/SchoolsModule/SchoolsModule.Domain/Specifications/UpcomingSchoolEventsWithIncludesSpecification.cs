using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving upcoming school events that include detailed
    /// navigation properties like participating groups, team members, categories, and views.
    /// </summary>
    public class UpcomingSchoolEventsWithIncludesSpecification : Specification<SchoolEvent<Category<ActivityGroup>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpcomingSchoolEventsWithIncludesSpecification"/> class.
        /// Filters events whose start date is in the future and includes related data for comprehensive querying.
        /// </summary>
        public UpcomingSchoolEventsWithIncludesSpecification()
        {
            // Filter to only include events that are scheduled in the future
            Criteria = e => e.StartDate > DateTime.UtcNow;

            // Include: Activity Groups → Team Members → Learners
            AddInclude(e => e.Include(c => c.ParticipatingActivityGroups)
                .ThenInclude(c => c.ActivityGroup)
                .ThenInclude(g => g.TeamMembers)
                .ThenInclude(l => l.Learner));

            // Include: Team Members → Their Parents
            AddInclude(e => e.Include(c => c.ParticipatingActivityGroups)
                .ThenInclude(c => c.ParticipatingTeamMembers)
                .ThenInclude(g => g.TeamMember)
                .ThenInclude(l => l.Parents)
                .ThenInclude(c => c.Parent));

            // Include: Participating Categories and their linked Group Category
            AddInclude(c => c.Include(c => c.ParticipatingCategories)
                .ThenInclude(c => c.ActivityGroupCategory));

            // Include: Users who have viewed the event
            AddInclude(c => c.Include(c => c.Views));
        }
    }

}
