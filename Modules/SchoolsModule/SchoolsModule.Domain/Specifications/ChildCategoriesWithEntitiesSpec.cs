using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for retrieving all child <see cref="Category{T}"/> entities under a specific parent category,
    /// including their subcategories and associated entities.
    /// </summary>
    public class ChildCategoriesWithEntitiesSpec : Specification<Category<ActivityGroup>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChildCategoriesWithEntitiesSpec"/> class.
        /// Filters categories by parent ID and includes both subcategories and the entity collection.
        /// </summary>
        /// <param name="parentCategoryId">The ID of the parent category to filter by.</param>
        public ChildCategoriesWithEntitiesSpec(string parentCategoryId)
        {
            // Filter: only categories with the specified parent
            Criteria = c => c.ParentCategoryId == parentCategoryId;

            // Include direct subcategories of each matched category
            AddInclude(q => q.Include(c => c.SubCategories));

            // Include the entities (e.g., ActivityGroups) associated with the category
            AddInclude(q => q.Include(c => c.EntityCollection));
        }
    }
}
