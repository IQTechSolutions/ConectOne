
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Category{ActivityGroup}"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration establishes the relationships between the <see
    /// cref="Category{ActivityGroup}"/> entity and its associated subcategories and entity collection. Specifically:
    /// <list type="bullet"> <item> <description>Defines a one-to-many relationship between a category and its
    /// subcategories, with a foreign key linking subcategories to their parent category.</description> </item> <item>
    /// <description>Defines a one-to-many relationship between a category and its associated <see
    /// cref="ActivityGroup"/> entities, with cascading delete behavior.</description> </item> </list></remarks>
    public class ActivityGroupCategoryConfiguration : IEntityTypeConfiguration<Category<ActivityGroup>>
    {
        /// <summary>
        /// Configures the relationships and constraints for the <see cref="Category{ActivityGroup}"/> entity.
        /// </summary>
        /// <remarks>This method establishes the following relationships: <list type="bullet"> <item> A
        /// one-to-many relationship between the category and its subcategories, with a foreign key on the subcategories
        /// referencing the parent category. </item> <item> A one-to-many relationship between the category and its
        /// entity collection, with a foreign key on the entities referencing the category.  Deleting a category will
        /// cascade delete its associated entities. </item> </list></remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="Category{ActivityGroup}"/>
        /// entity.</param>
        public void Configure(EntityTypeBuilder<Category<ActivityGroup>> builder)
        {
            builder.HasMany(c => c.SubCategories).WithOne(c => c.ParentCategory).HasForeignKey(c => c.ParentCategoryId);
            builder.HasMany(c => c.EntityCollection).WithOne(c => c.Category).HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
