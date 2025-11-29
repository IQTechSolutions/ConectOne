using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving published school events with detailed participant and group information.
    /// </summary>
    public class PublishedEventsWithParticipantsSpec : Specification<SchoolEvent<Category<ActivityGroup>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublishedEventsWithParticipantsSpec"/> class.
        /// Filters for events marked as published and includes categories, teams, team members, and teachers.
        /// </summary>
        public PublishedEventsWithParticipantsSpec()
        {
            // Filter: only include events that are marked as published
            Criteria = e => e.Published;

            // Include all participating categories linked to the event
            AddInclude(q => q.Include(e => e.ParticipatingCategories));

            // Include activity groups and their associated team members
            AddInclude(q => q.Include(e => e.ParticipatingActivityGroups)
                .ThenInclude(pag => pag.ParticipatingTeamMembers)
                .ThenInclude(pag => pag.TeamMember));

            // Include each activity group and its assigned teacher
            AddInclude(q => q.Include(e => e.ParticipatingActivityGroups)
                .ThenInclude(pag => pag.ActivityGroup)
                .ThenInclude(ag => ag.Teacher));
        }
    }

}
