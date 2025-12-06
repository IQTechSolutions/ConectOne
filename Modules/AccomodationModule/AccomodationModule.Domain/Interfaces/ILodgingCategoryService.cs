using AccomodationModule.Domain.Entities;
using GroupingModule.Domain.Interfaces;

namespace AccomodationModule.Domain.Interfaces
{
    /// <summary>
    /// Defines a service for managing lodging categories, providing operations specific to the <see cref="Lodging"/>
    /// entity.
    /// </summary>
    /// <remarks>This interface extends <see cref="ICategoryService{TEntity}"/> with <see cref="Lodging"/> as the
    /// entity type,  inheriting all category-related operations for lodging management.</remarks>
    public interface ILodgingCategoryService : ICategoryService<Lodging> { }
}
