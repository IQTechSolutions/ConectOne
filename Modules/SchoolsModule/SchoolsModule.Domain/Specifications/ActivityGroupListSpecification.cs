using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Represents a specification for retrieving a filtered and fully populated list of <see cref="ActivityGroup"/>
    /// entities.
    /// </summary>
    /// <remarks>This specification is designed to apply filtering and include related entities when querying
    /// for <see cref="ActivityGroup"/> objects. It supports filtering by the coach's email address and includes related
    /// entities such as categories, age groups, images, teachers, and team members.</remarks>
    public class ActivityGroupListSpecification : Specification<ActivityGroup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityGroupListSpecification"/> class with the specified
        /// filtering and inclusion criteria based on the provided parameters.
        /// </summary>
        /// <remarks>This constructor sets up the filtering criteria and includes related entities to be
        /// loaded as part of the query. The filtering criteria are based on the properties of the <paramref name="p"/>
        /// parameter. If <see cref="ActivityGroupPageParameters.CoachEmail"/> is provided, the results are filtered to
        /// include only activity groups associated with the specified coach's email address.  The following related
        /// entities are included in the query: <list type="bullet"> <item><description>Categories and their associated
        /// category details.</description></item> <item><description>Age group information.</description></item>
        /// <item><description>Images associated with the activity group.</description></item>
        /// <item><description>Teacher details, including their email addresses.</description></item>
        /// <item><description>Team members and their associated learners.</description></item> </list></remarks>
        /// <param name="p">The <see cref="ActivityGroupPageParameters"/> object containing the filtering criteria, such as the coach's
        /// email address.</param>
        public ActivityGroupListSpecification(ActivityGroupPageParameters p)
        {
            // Define the filtering criteria
            Criteria = c => true;

            if(!string.IsNullOrEmpty(p.CoachEmail))
                Criteria = Criteria.And(c => c.Teacher.EmailAddresses.Any(r => r.Email == p.CoachEmail));

            AddInclude(q => q.Include(ag => ag.Categories).ThenInclude(c => c.Category));
            AddInclude(q => q.Include(ag => ag.AgeGroup));
            AddInclude(q => q.Include(ag => ag.Images));
            AddInclude(q => q.Include(ag => ag.Teacher).ThenInclude(c => c.EmailAddresses));
            AddInclude(q => q.Include(ag => ag.TeamMembers).ThenInclude(tm => tm.Learner));
        }
    }
}
