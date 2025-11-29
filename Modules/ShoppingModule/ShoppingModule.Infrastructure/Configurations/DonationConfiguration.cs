using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingModule.Domain.Entities;

namespace ShoppingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Provides configuration settings for the <see cref="Donation"/> entity type.
    /// </summary>
    /// <remarks>This class is used to configure the <see cref="Donation"/> entity within the Entity Framework
    /// Core model. It defines the entity's key and other mappings as required.</remarks>
    public class DonationConfiguration : IEntityTypeConfiguration<Donation>
    {
        /// <summary>
        /// Configures the entity type for <see cref="Donation"/>.
        /// </summary>
        /// <remarks>This method sets the primary key for the <see cref="Donation"/> entity.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the <see cref="Donation"/> entity.</param>
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
