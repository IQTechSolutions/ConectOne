using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsModule.Domain.Entities;

namespace ProductsModule.Domain.Configurations;

/// <summary>
/// Configures the entity type <see cref="ServiceTier"/> for use with Entity Framework Core.
/// </summary>
/// <remarks>This configuration specifies that the <see cref="ServiceTier.Name"/> property is required and has a
/// maximum length of 30 characters. Use this class to apply consistent configuration for the <see cref="ServiceTier"/>
/// entity across the data model.</remarks>
public class ServiceTierConfiguration : IEntityTypeConfiguration<ServiceTier>
{
    /// <summary>
    /// Configures the entity type for <see cref="ServiceTier"/>.
    /// </summary>
    /// <remarks>This method sets the <c>Name</c> property of the <see cref="ServiceTier"/> entity as required
    /// and limits its maximum length to 30 characters.</remarks>
    /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="ServiceTier"/> entity.</param>
    public void Configure(EntityTypeBuilder<ServiceTier> builder)
    {
        builder.Property(c => c.Name).IsRequired().HasMaxLength(30);
    }
}