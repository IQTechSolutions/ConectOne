using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Interfaces;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using GroupingModule.Infrastructure.Implementation;

namespace AccomodationModule.Infrastructure.Implimentation
{
    /// <summary>
    /// Provides services for managing lodging categories, including retrieval, creation, and association of categories
    /// with lodging entities.
    /// </summary>
    /// <param name="categoryRepository">The repository used to access and manage lodging category data.</param>
    /// <param name="entityCategoryRepository">The repository used to manage associations between lodging entities and their categories.</param>
    /// <param name="imageRepo">The repository used to manage images associated with lodging categories.</param>
    public class LodgingCategoryService(IRepository<Category<Lodging>, string> categoryRepository, IRepository<EntityCategory<Lodging>, string> entityCategoryRepository, IRepository<EntityImage<Category<Lodging>, string>, string> imageRepo)
        : CategoryService<Lodging>(categoryRepository, entityCategoryRepository, imageRepo), ILodgingCategoryService { }
}
