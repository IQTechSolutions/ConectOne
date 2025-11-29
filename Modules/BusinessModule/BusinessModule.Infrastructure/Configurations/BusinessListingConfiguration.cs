using BusinessModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessModule.Infrastructure.Configurations;

/// <summary>
/// Configures the entity type settings for the <see cref="BusinessListing"/> model.
/// </summary>
/// <remarks>This configuration defines the primary key for the <see cref="BusinessListing"/> entity. Additional
/// configuration for the entity can be added within this method.</remarks>
public class BusinessListingConfiguration : IEntityTypeConfiguration<BusinessListing>
{
    /// <summary>
    /// Configures the entity type for <see cref="BusinessListing"/>.
    /// </summary>
    /// <remarks>This method sets the primary key for the <see cref="BusinessListing"/> entity.</remarks>
    /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="BusinessListing"/> entity.</param>
    public void Configure(EntityTypeBuilder<BusinessListing> builder)
    {
        builder.HasKey(c => c.Id);
    }
}