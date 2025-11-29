using AdvertisingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type settings for the <see cref="AdvertisementTier"/> class.
    /// </summary>
    /// <remarks>This configuration defines the primary key for the <see cref="AdvertisementTier"/> entity and
    /// establishes a one-to-many relationship with the <see cref="Advertisement"/> entity. The relationship specifies
    /// that when a tier is deleted, the foreign key in related advertisements will be set to null.</remarks>
    public class AdvertisementTierConfiguration : IEntityTypeConfiguration<AdvertisementTier>
    {
        /// <summary>
        /// Configures the entity type <see cref="AdvertisementTier"/> and its relationships.
        /// </summary>
        /// <remarks>This configuration sets the primary key for the <see cref="AdvertisementTier"/>
        /// entity and defines a one-to-many  relationship with the <see cref="Advertisement"/> entity. The relationship
        /// specifies that when a tier is deleted,  the foreign key in related advertisements is set to null.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="AdvertisementTier"/> entity.</param>
        public void Configure(EntityTypeBuilder<AdvertisementTier> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasMany(c => c.Advertisements)
                   .WithOne(a => a.AdvertisementTier)
                   .HasForeignKey(a => a.AdvertisementTierId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
