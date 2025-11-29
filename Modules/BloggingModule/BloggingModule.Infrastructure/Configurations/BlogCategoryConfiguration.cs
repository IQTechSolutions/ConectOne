using BloggingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloggingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type for <see cref="EntityCategory{BlogPost}"/>.
    /// </summary>
    /// <remarks>This configuration defines the composite key for the <see cref="EntityCategory{BlogPost}"/>
    /// entity and establishes relationships between the <see cref="EntityCategory{BlogPost}"/> entity,  <see
    /// cref="BlogPost"/> entities, and their associated categories.</remarks>
    public class BlogCategoryConfiguration : IEntityTypeConfiguration<EntityCategory<BlogPost>>
    {
        /// <summary>
        /// Configures the entity type for <see cref="EntityCategory{TEntity}"/> with specific key and relationship
        /// mappings.
        /// </summary>
        /// <remarks>This method defines a composite primary key for the <see
        /// cref="EntityCategory{TEntity}"/> entity using  the <c>EntityId</c> and <c>CategoryId</c> properties. It also
        /// establishes relationships: <list type="bullet"> <item> <description> Configures a one-to-many relationship
        /// between the <c>Entity</c> and its associated <c>Categories</c>,  with <c>EntityId</c> as the foreign key.
        /// </description> </item> <item> <description> Configures a one-to-many relationship between the
        /// <c>Category</c> and its associated <c>EntityCollection</c>,  with <c>CategoryId</c> as the foreign key.
        /// </description> </item> </list></remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<EntityCategory<BlogPost>> builder)
        {
            builder.HasKey(sc => new { sc.EntityId, sc.CategoryId });
            builder.HasOne(c => c.Entity).WithMany(c => c.Categories).HasForeignKey(c => c.EntityId);
            builder.HasOne(c => c.Category).WithMany(c => c.EntityCollection).HasForeignKey(c => c.CategoryId); }
    }
}
