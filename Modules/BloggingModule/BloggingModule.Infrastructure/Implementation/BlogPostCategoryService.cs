using BloggingModule.Domain.Entities;
using BloggingModule.Domain.Interfaces;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using GroupingModule.Infrastructure.Implementation;

namespace BloggingModule.Infrastructure.Implementation
{
    /// <summary>
    /// Provides category management functionality for blog posts, including support for category images and grouping
    /// operations.
    /// </summary>
    /// <param name="repository">The repository manager used to access and group blog post entities by category. Cannot be null.</param>
    /// <param name="imageRepo">The repository used to manage images associated with blog post categories. Cannot be null.</param>
    public class BlogPostCategoryService(IRepository<Category<BlogPost>, string> repository, IRepository<EntityCategory<BlogPost>, string> entityCategoryService,
        IRepository<EntityImage<Category<BlogPost>, string>, string> imageRepo) : CategoryService<BlogPost>(repository, entityCategoryService, imageRepo), IBlogPostCategoryService { }
}
