using BusinessModule.Domain.Entities;
using BusinessModule.Domain.Interfaces;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using GroupingModule.Infrastructure.Implementation;

namespace BusinessModule.Infrastructure.Implementation
{
    /// <summary>
    /// Provides functionality for managing and interacting with categories in a business directory.
    /// </summary>
    /// <remarks>This service extends the base <see cref="CategoryService{T}"/> to provide category management
    /// specifically for business listings. It supports operations such as retrieving, updating, and  managing
    /// categories and their associated images.</remarks>
    /// <param name="repository"></param>
    /// <param name="imageRepo"></param>
    public class BusinessDirectoryCategoryService(IRepository<Category<BusinessListing>, string> categoryRepository, IRepository<EntityCategory<BusinessListing>, string> entityCategoryRepository, IRepository<EntityImage<Category<BusinessListing>, string>, string> imageRepo)
        : CategoryService<BusinessListing>(categoryRepository, entityCategoryRepository, imageRepo), IBusinessDirectoryCategoryService { }
}
