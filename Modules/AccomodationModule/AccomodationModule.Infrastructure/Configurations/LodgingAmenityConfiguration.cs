using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type for <see cref="AmenityItem{Lodging, string}"/> within the application's data model.
    /// </summary>
    /// <remarks>This configuration establishes relationships between <see cref="AmenityItem{Lodging,
    /// string}"/> and its associated <see cref="Lodging"/> and <see cref="Amenity"/> entities. Specifically: <list
    /// type="bullet"> <item> <description>Defines a one-to-many relationship between <see cref="Lodging"/> and its
    /// amenities.</description> </item> <item> <description>Defines a foreign key relationship for the <c>LodgingId</c>
    /// and <c>AmenityId</c> properties.</description> </item> </list> This configuration should be applied during the
    /// model-building process in Entity Framework Core.</remarks>
    public class LodgingAmenityConfiguration : IEntityTypeConfiguration<AmenityItem<Lodging, string>>
    {
        /// <summary>
        /// Configures the entity type for <see cref="AmenityItem{Lodging, string}"/>.
        /// </summary>
        /// <remarks>This method sets up relationships and foreign key constraints for the <see
        /// cref="AmenityItem{Lodging, string}"/> entity. It establishes a one-to-many relationship between <see
        /// cref="AmenityItem{Lodging, string}.Lodging"/> and its amenities, and a foreign key relationship for <see
        /// cref="AmenityItem{Lodging, string}.Amenity"/>.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the <see cref="AmenityItem{Lodging,
        /// string}"/> entity.</param>
        public void Configure(EntityTypeBuilder<AmenityItem<Lodging, string>> builder)
        {
            builder.HasOne(c => c.Lodging).WithMany(c => c.Amneties).HasForeignKey(c => c.LodgingId);
			builder.HasOne(c => c.Amenity).WithMany().HasForeignKey(c => c.AmenityId);
		}
    }
}
