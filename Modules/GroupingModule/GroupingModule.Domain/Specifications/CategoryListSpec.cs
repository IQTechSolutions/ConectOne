using ConectOne.Domain.Interfaces;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace GroupingModule.Domain.Specifications
{
    /// <summary>
    /// Represents a specification for retrieving a list of categories, optionally filtered by a parent category ID.
    /// </summary>
    /// <remarks>This specification is designed to retrieve categories that are not marked as deleted. If a
    /// parent category ID is provided,  the specification further filters the categories to include only those that
    /// belong to the specified parent category. The specification also includes related data, such as the entity
    /// collection, images, and subcategories with their respective images and entity collections.</remarks>
    /// <typeparam name="T">The type of the entity associated with the category, which must implement <see cref="IAuditableEntity"/>
    /// with a string key.</typeparam>
    public sealed class CategoryListSpec<T> : Specification<Category<T>> where T : IAuditableEntity<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryListSpec"/> class with optional filtering by parent
        /// category.
        /// </summary>
        /// <remarks>This specification is used to query categories that are not marked as deleted. It
        /// also includes related entities such as the category's entity collection, images, and subcategories, along
        /// with their respective images and entity collections.</remarks>
        /// <param name="parentId">The identifier of the parent category to filter by. If specified, only categories with the given parent ID
        /// will be included. If <see langword="null"/> or empty, no parent category filter is applied.</param>
        public CategoryListSpec(string? parentId = null)
        {
            Criteria = category => !category.IsDeleted;
            
            if (!string.IsNullOrEmpty(parentId))
            {
                Criteria = Criteria.And(f => f.ParentCategoryId == parentId);
            }

            AddInclude(c => c.Include(category => category.EntityCollection));
            AddInclude(c => c.Include(category => category.Images).ThenInclude(category => category.Image));
            AddInclude(c => c.Include(category => category.SubCategories).ThenInclude(category => category.Images).ThenInclude(category => category.Image));
            AddInclude(c => c.Include(category => category.SubCategories).ThenInclude(category => category.EntityCollection));
        }
    }
}
