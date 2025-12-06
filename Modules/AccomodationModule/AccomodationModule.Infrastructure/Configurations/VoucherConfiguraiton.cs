using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Voucher"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration defines relationships and constraints for the <see cref="Voucher"/>
    /// entity: <list type="bullet"> <item> <description> Configures a one-to-many relationship between <see
    /// cref="Voucher"/> and <see cref="Lodging"/>,  with a foreign key on <see cref="Voucher.LodgingId"/>. The delete
    /// behavior is set to <see cref="DeleteBehavior.NoAction"/>. </description> </item> <item> <description> Configures
    /// a one-to-many relationship between <see cref="Voucher"/> and <see cref="Room"/>,  with a foreign key on <see
    /// cref="Room.VoucherId"/>. The delete behavior is set to <see cref="DeleteBehavior.Cascade"/>. </description>
    /// </item> </list></remarks>
    public class VoucherConfiguraiton : IEntityTypeConfiguration<Voucher>
    {
        /// <summary>
        /// Configures the entity type <see cref="Voucher"/> and its relationships.
        /// </summary>
        /// <remarks>This method defines the relationships between the <see cref="Voucher"/> entity and
        /// other entities: <list type="bullet"> <item> Configures a one-to-many relationship between <see
        /// cref="Voucher"/> and <see cref="Lodging"/>,  with a foreign key on <see cref="Voucher.LodgingId"/> and no
        /// action on delete. </item> <item> Configures a one-to-many relationship between <see cref="Voucher"/> and
        /// <see cref="Room"/>,  with a foreign key on <see cref="Room.VoucherId"/> and cascade delete behavior. </item>
        /// </list></remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{Voucher}"/> instance used to configure the <see cref="Voucher"/> entity.</param>
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasOne(c => c.Lodging).WithMany(c => c.Vouchers).HasForeignKey(c => c.LodgingId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(c => c.Rooms).WithOne(c => c.Voucher).HasForeignKey(c => c.VoucherId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
