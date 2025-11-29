using BusinessModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="ListingTier"/> and its relationships for the database context.
    /// </summary>
    /// <remarks>This configuration defines the primary key for the <see cref="ListingTier"/> entity and sets
    /// up a one-to-many  relationship with the <see cref="BusinessListing"/> entity. The foreign key constraint is
    /// configured to allow  null values when a <see cref="ListingTier"/> is deleted.</remarks>
    public class ListingTierConfiguration : IEntityTypeConfiguration<ListingTier>
    {
        /// <summary>
        /// Configures the entity type for <see cref="ListingTier"/>.
        /// </summary>
        /// <remarks>This configuration sets the primary key for the <see cref="ListingTier"/> entity and
        /// defines a one-to-many  relationship with the <see cref="BusinessListing"/> entity. The foreign key
        /// constraint is configured to  set the foreign key to <see langword="null"/> when a related <see
        /// cref="ListingTier"/> is deleted.</remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="ListingTier"/> entity.</param>
        public void Configure(EntityTypeBuilder<ListingTier> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasMany(c => c.BusinessListings)
                   .WithOne(a => a.ListingTier)
                   .HasForeignKey(a => a.ListingTierId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
