using GroupingModule.Domain.Interfaces;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Interfaces.ActivityGroups
{
    /// <summary>
    /// Defines operations for managing activity group categories.
    /// </summary>
    /// <remarks>This interface extends the generic category service to provide category management
    /// functionality specific to activity groups. Implementations typically support creating, updating, retrieving, and
    /// deleting activity group categories.</remarks>
    public interface IActivityGroupCategoryService : ICategoryService<ActivityGroup>
    {
    }
}
