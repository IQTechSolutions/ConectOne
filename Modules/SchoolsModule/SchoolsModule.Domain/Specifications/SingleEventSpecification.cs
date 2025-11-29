using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification to retrieve a single school event along with all its related data.
    /// </summary>
    /// <remarks>
    /// This specification is typically used when a detailed view of a single school event is needed,
    /// including associated documents, categories, activity groups, team members, and related metadata.
    /// </remarks>
    public class SingleEventSpecification : Specification<SchoolEvent<Category<ActivityGroup>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleEventSpecification"/> class.
        /// </summary>
        /// <param name="schoolEventId">The ID of the school event to retrieve.</param>
        public SingleEventSpecification(string? schoolEventId = null)
        {
            // Filter to retrieve a specific event by its ID
            Criteria = e => e.Id == schoolEventId;

            // Include associated documents (e.g., attachments, instructions)
            AddInclude(q => q.Include(e => e.Documents));

            // Include participating categories and their group categories
            AddInclude(q => q.Include(e => e.ParticipatingCategories)
                             .ThenInclude(pc => pc.ActivityGroupCategory));

            // Include participating activity groups and their category hierarchy
            AddInclude(q => q.Include(e => e.ParticipatingActivityGroups)
                             .ThenInclude(p => p.ActivityGroup)
                             .ThenInclude(ag => ag.Categories)
                             .ThenInclude(ec => ec.Category));

            // Include age group information for activity groups
            AddInclude(q => q.Include(e => e.ParticipatingActivityGroups)
                             .ThenInclude(p => p.ActivityGroup)
                             .ThenInclude(ag => ag.AgeGroup));

            // Include participating team members and their associated learners
            AddInclude(q => q.Include(e => e.ParticipatingActivityGroups)
                             .ThenInclude(p => p.ParticipatingTeamMembers)
                             .ThenInclude(pt => pt.TeamMember));
        }
    }

}
