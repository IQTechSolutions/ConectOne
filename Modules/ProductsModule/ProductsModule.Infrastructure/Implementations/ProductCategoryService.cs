using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using GroupingModule.Infrastructure.Implementation;
using ProductsModule.Domain.Entities;
using ProductsModule.Domain.Interfaces;

namespace ProductsModule.Infrastructure.Implementations
{
    /// <summary>
    /// Provides functionality for managing product categories, including operations for retrieving, updating, and
    /// organizing product-related category data.
    /// </summary>
    /// <remarks>This service extends the base <see cref="CategoryService{T}"/> to provide category management
    /// specifically for products.  It integrates with repositories for product grouping and category image
    /// management.</remarks>
    /// <param name="repository"></param>
    /// <param name="imageRepo"></param>
    public class ProductCategoryService(IRepository<Category<Product>, string> categoryRepository, IRepository<EntityCategory<Product>, string> entityCategoryRepository, 
        IRepository<EntityImage<Category<Product>, string>, string> imageRepo) : CategoryService<Product>(categoryRepository, entityCategoryRepository, imageRepo), IProductCategoryService { }
}
