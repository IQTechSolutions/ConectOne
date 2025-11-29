using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using GroupingModule.Infrastructure.Implementation;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces.ActivityGroups;

namespace SchoolsModule.Infrastructure.Implementation.ActivityGroups
{
    /// <summary>
    /// Provides services for managing categories associated with activity groups, including retrieval, creation, and
    /// association of categories and related images.
    /// </summary>
    /// <param name="categoryRepository">The repository used to access and manage category data for activity groups.</param>
    /// <param name="entityCategoryRepository">The repository used to manage associations between activity groups and their categories.</param>
    /// <param name="imageRepo">The repository used to manage images associated with activity group categories.</param>
    public class ActivityGroupCategoryService(IRepository<Category<ActivityGroup>, string> categoryRepository, IRepository<EntityCategory<ActivityGroup>, string> entityCategoryRepository, IRepository<EntityImage<Category<ActivityGroup>, string>, string> imageRepo) 
        : CategoryService<ActivityGroup>(categoryRepository, entityCategoryRepository, imageRepo), IActivityGroupCategoryService
    {
    }
}
