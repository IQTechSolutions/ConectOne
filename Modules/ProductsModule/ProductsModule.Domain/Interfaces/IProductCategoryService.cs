using GroupingModule.Domain.Interfaces;
using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.Interfaces
{
    /// <summary>
    /// Provides operations for managing product categories within the system.
    /// </summary>
    /// <remarks>This interface extends the generic category service for product-specific scenarios.
    /// Implementations typically support creating, updating, retrieving, and deleting product categories. Thread safety
    /// and specific behaviors depend on the concrete implementation.</remarks>
    public interface IProductCategoryService : ICategoryService<Product> { }
}
