using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.Configurations;

/// <summary>
/// The configuration for a Product Category entity in the data store schema
/// </summary>
public class ProductCategoryConfiguration : IEntityTypeConfiguration<EntityCategory<Product>>
{
    /// <summary>
    /// Configures the relationships and keys for the <see cref="EntityCategory{TEntity}"/> entity type.
    /// </summary>
    /// <remarks>This method establishes the foreign key relationships between the <see
    /// cref="EntityCategory{TEntity}"/> entity and its associated <see cref="Entity"/> and <see cref="Category"/>
    /// entities.</remarks>
    /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="EntityCategory{TEntity}"/> entity.</param>
    public void Configure(EntityTypeBuilder<EntityCategory<Product>> builder)
    {
        builder.HasOne(c => c.Entity).WithMany(c => c.Categories).HasForeignKey(c => c.EntityId);
        builder.HasOne(c => c.Category).WithMany(c => c.EntityCollection).HasForeignKey(c => c.CategoryId);
    }
}
