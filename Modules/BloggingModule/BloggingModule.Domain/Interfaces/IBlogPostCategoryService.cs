using BloggingModule.Domain.Entities;
using GroupingModule.Domain.Interfaces;

namespace BloggingModule.Domain.Interfaces
{
    /// <summary>
    /// Defines operations for managing categories associated with blog posts.
    /// </summary>
    /// <remarks>This interface extends the generic category service to provide category management
    /// functionality specifically for blog posts. Implementations typically support creating, updating, deleting, and
    /// retrieving categories linked to blog post entities.</remarks>
    public interface IBlogPostCategoryService : ICategoryService<BlogPost> { }
}
