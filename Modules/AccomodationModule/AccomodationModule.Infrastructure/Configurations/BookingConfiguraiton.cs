using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Provides configuration for the <see cref="Booking"/> entity and its relationships in the database model.
    /// </summary>
    /// <remarks>This class implements <see cref="IEntityTypeConfiguration{TEntity}"/> to configure the <see
    /// cref="Booking"/> entity. It defines relationships between the <see cref="Booking"/> entity and related entities,
    /// including <see cref="Room"/>, <see cref="Package"/>, <see cref="Lodging"/>, and <see cref="User"/>. The
    /// configuration includes foreign key constraints and navigation properties to ensure proper mapping in the
    /// database schema.</remarks>
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        /// <summary>
        /// Configures the entity type <see cref="Booking"/> and its relationships with other entities.
        /// </summary>
        /// <remarks>This method establishes the relationships between the <see cref="Booking"/> entity
        /// and the  <see cref="Room"/>, <see cref="Package"/>, <see cref="Lodging"/>, and <see cref="User"/> entities.
        /// It defines foreign key constraints and navigation properties for these relationships.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="Booking"/> entity.</param>
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasOne(c => c.Room).WithMany().HasForeignKey(c => c.RoomId);
            builder.HasOne(c => c.Package).WithMany(c => c.Bookings).HasForeignKey(c => c.PackageId);
            builder.HasOne(c => c.Lodging).WithMany().HasForeignKey(c => c.LodgingId);
            builder.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId);
        }
    }
}
