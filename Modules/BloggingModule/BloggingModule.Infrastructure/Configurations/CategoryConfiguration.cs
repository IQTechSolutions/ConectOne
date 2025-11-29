using BloggingModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloggingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Category{T}"/> for the database context.
    /// </summary>
    /// <remarks>This configuration defines the relationships between the <see cref="Category{TEntity}"/>
    /// entity and its associated collections,  such as <c>EntityCollection</c>, <c>Images</c>, <c>Videos</c>, and
    /// <c>Documents</c>.  It specifies the foreign key constraints and navigation properties for these
    /// relationships.</remarks>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category<BlogPost>>
    {
        /// <summary>
        /// Configures the entity type for <see cref="Category{TEntity}"/> with specific relationships and constraints.
        /// </summary>
        /// <remarks>This method establishes the relationships between the <see cref="Category{TEntity}"/>
        /// entity and its associated collections,  including <c>EntityCollection</c>, <c>Images</c>, <c>Videos</c>, and
        /// <c>Documents</c>.  Each relationship is configured with the appropriate foreign key constraints.</remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<Category<BlogPost>> builder)
        {
            builder.HasMany(c => c.EntityCollection).WithOne(c => c.Category).HasForeignKey(c => c.CategoryId);

            builder.HasMany(c => c.Images).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId);
            builder.HasMany(c => c.Videos).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId);
            builder.HasMany(c => c.Documents).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId);
        }
    }
}
