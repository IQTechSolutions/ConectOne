using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification to retrieve a single school event along with all its related data.
    /// </summary>
    /// <remarks>
    /// This specification is typically used when a detailed view of a single school event is needed,
    /// including associated documents, categories, activity groups, team members, and related metadata.
    /// </remarks>
    public class PagedSchoolEventsSpecification : Specification<SchoolEvent<Category<ActivityGroup>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleEventSpecification"/> class.
        /// </summary>
        /// <param name="schoolEventId">The ID of the school event to retrieve.</param>
        public PagedSchoolEventsSpecification(SchoolEventPageParameters pageParameters)
        {
            // Filter to retrieve a specific event by its ID
            Criteria = e => e.Published == true;

            if (!string.IsNullOrEmpty(pageParameters.CategoryId))
                Criteria = Criteria.And(e => e.ParticipatingCategories.Any(c => c.ActivityGroupCategoryId == pageParameters.CategoryId));
            
            if(pageParameters.Archived)
                Criteria = Criteria.And(e => e.StartDate.Date < DateTime.Today.Date);
            if (pageParameters.Active)
                Criteria = Criteria.And(e => e.StartDate.Date == DateTime.Today.Date);
            if (!pageParameters.Archived && !pageParameters.Active)
                Criteria = Criteria.And(e => e.StartDate.Date >= DateTime.Today.Date);

            AddInclude(c => c.Include(g => g.ParticipatingCategories));
        }
    }
}
