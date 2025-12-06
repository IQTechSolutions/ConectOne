using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the <see cref="Package"/> entity and its relationships with other entities.
    /// </summary>
    /// <remarks>This method defines the relationships between the <see cref="Package"/> entity and related
    /// entities, including <see cref="Lodging"/>, <see cref="Room"/>, and <see cref="Booking"/>. It specifies foreign
    /// key constraints and cascade delete behavior for dependent entities.</remarks>
    public class PackageConfiguration : IEntityTypeConfiguration<LodgingPackage>
    {
        /// <summary>
        /// Configures the entity type <see cref="Package"/> and its relationships.
        /// </summary>
        /// <remarks>This method defines the relationships between the <see cref="Package"/> entity and
        /// other entities,  including <see cref="Lodging"/>, <see cref="Room"/>, and <see cref="Booking"/>. It
        /// specifies foreign key constraints  and cascade delete behavior for related entities.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{Package}"/> used to configure the <see cref="Package"/> entity.</param>
        public void Configure(EntityTypeBuilder<LodgingPackage> builder)
        {
            builder.HasOne(c => c.Lodging).WithMany(c => c.AccountTypes).HasForeignKey(c => c.LodgingId);

            builder.HasMany(c => c.Rooms).WithOne(c => c.Package).HasForeignKey(c => c.PackageId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Bookings).WithOne(c => c.Package).HasForeignKey(c => c.PackageId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
