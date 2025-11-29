using AdvertisingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Affiliate"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration specifies the primary key for the <see cref="Affiliate"/> entity.
    /// Implement this class to define additional entity configurations as needed.</remarks>
    public class AffiliateConfiguration : IEntityTypeConfiguration<Affiliate>
    {
        /// <summary>
        /// Configures the entity type <see cref="Affiliate"/> for the database context.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{Affiliate}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<Affiliate> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasMany(c => c.Images).WithOne(c => c.Entity)
                .HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
