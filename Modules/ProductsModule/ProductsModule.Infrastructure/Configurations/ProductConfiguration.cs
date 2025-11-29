using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.Configurations;

/// <summary>
/// Configures the entity type <see cref="Product"/> and its relationships for the database schema.
/// </summary>
/// <remarks>This configuration defines the properties, constraints, and relationships for the <see
/// cref="Product"/> entity. It specifies required fields, maximum lengths for string properties, and the relationships
/// between <see cref="Product"/>  and related entities such as categories, brands, combos, bundled products, suppliers,
/// and pricing.</remarks>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the entity type for the <see cref="Product"/> class.
    /// </summary>
    /// <remarks>This method defines the schema for the <see cref="Product"/> entity, including property
    /// constraints  such as required fields, maximum lengths, and relationships with other entities.  It ensures proper
    /// database mapping and enforces constraints like foreign keys and delete behaviors.</remarks>
    /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="Product"/> entity.</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.DisplayName).IsRequired().HasMaxLength(200);
        builder.Property(c => c.SKU).HasMaxLength(30);
        builder.Property(c => c.ShortDescription).HasMaxLength(1000);
        builder.Property(c => c.Description).HasMaxLength(5000);

        builder.HasOne(c => c.Pricing).WithOne().HasPrincipalKey<Product>(c => c.Id);

        builder.HasMany(c => c.MetaDataCollection).WithOne(c => c.Product).HasForeignKey(c => c.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.Categories).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
    }
}