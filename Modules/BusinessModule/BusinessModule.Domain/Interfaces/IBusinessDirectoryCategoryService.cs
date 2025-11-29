using BusinessModule.Domain.Entities;
using GroupingModule.Domain.Interfaces;

namespace BusinessModule.Domain.Interfaces
{
    /// <summary>
    /// Provides functionality for managing and retrieving categories specific to business directory listings.
    /// </summary>
    /// <remarks>This interface extends <see cref="ICategoryService{TEntity}"/> with the generic type parameter <see
    /// cref="BusinessListing"/>,  enabling operations tailored to categories associated with business
    /// listings.</remarks>
    public interface IBusinessDirectoryCategoryService : ICategoryService<BusinessListing> { }
}
