using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Room"/> and its relationships for the database schema.
    /// </summary>
    /// <remarks>This configuration defines the relationships between the <see cref="Room"/> entity and other
    /// entities,  including <see cref="Package"/>, <see cref="Voucher"/>, and various collections such as featured
    /// images,  meal plans, bed types, child policy rules, and amenities. It specifies foreign key constraints and 
    /// delete behaviors for these relationships.</remarks>
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        /// <summary>
        /// Configures the entity type <see cref="Room"/> and its relationships.
        /// </summary>
        /// <remarks>This method defines the relationships and constraints for the <see cref="Room"/>
        /// entity, including foreign key associations and delete behaviors. It is typically called by the Entity
        /// Framework during model creation to set up the database schema.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{Room}"/> instance used to configure the <see cref="Room"/> entity.</param>
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasOne(c => c.Package).WithMany(c => c.Rooms).HasForeignKey(c => c.PackageId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.Voucher).WithMany(c => c.Rooms).HasForeignKey(c => c.VoucherId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.FeaturedImages).WithOne().HasForeignKey(c => c.RoomId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.MealPlans).WithOne().HasForeignKey(c => c.RoomId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.BedTypes).WithOne().HasForeignKey(c => c.RoomId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.ChildPolicyRules).WithOne().HasForeignKey(c => c.RoomId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Amneties).WithOne().HasForeignKey(c => c.RoomId).OnDelete(DeleteBehavior.Cascade);
		}
    }
}
