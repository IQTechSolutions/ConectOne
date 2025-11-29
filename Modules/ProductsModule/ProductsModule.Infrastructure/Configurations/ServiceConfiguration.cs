using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.Configurations;

/// <summary>
/// Configures the entity type <see cref="Service"/> and its relationships for the database schema.
/// </summary>
/// <remarks>This configuration defines the properties, constraints, and relationships for the <see
/// cref="Service"/> entity. It specifies required fields, maximum lengths for string properties, and the relationships
/// between <see cref="Service"/>  and related entities such as categories, brands, combos, bundled products, suppliers,
/// and pricing.</remarks>
public class ServiceConfiguration : IEntityTypeConfiguration<OfferedService>
{
    /// <summary>
    /// Configures the entity type for the <see cref="Service"/> class.
    /// </summary>
    /// <remarks>This method defines the schema for the <see cref="Product"/> entity, including property
    /// constraints  such as required fields, maximum lengths, and relationships with other entities.  It ensures proper
    /// database mapping and enforces constraints like foreign keys and delete behaviors.</remarks>
    /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="Service"/> entity.</param>
    public void Configure(EntityTypeBuilder<OfferedService> builder)
    {
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.DisplayName).IsRequired().HasMaxLength(200);
        builder.Property(c => c.ShortDescription).HasMaxLength(1000);
        builder.Property(c => c.Description).HasMaxLength(5000);

        builder.HasMany(c => c.Categories).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
    }
}