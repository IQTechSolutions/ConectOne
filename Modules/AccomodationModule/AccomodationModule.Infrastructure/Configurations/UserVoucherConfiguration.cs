using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type settings for the <see cref="UserVoucher"/> model.
    /// </summary>
    /// <remarks>This configuration defines relationships and constraints for the <see cref="UserVoucher"/>
    /// entity. Specifically, it establishes foreign key relationships with the <see cref="Voucher"/> and <see
    /// cref="Room"/> entities.</remarks>
    public class UserVoucherConfiguration : IEntityTypeConfiguration<UserVoucher>
    {
        /// <summary>
        /// Configures the entity type <see cref="UserVoucher"/> and its relationships.
        /// </summary>
        /// <remarks>This method establishes the following relationships: <list type="bullet"> <item> A
        /// one-to-many relationship between <see cref="UserVoucher"/> and <see cref="Voucher"/>,  with <see
        /// cref="UserVoucher.VoucherId"/> as the foreign key. </item> <item> A one-to-many relationship between <see
        /// cref="UserVoucher"/> and <see cref="Room"/>,  with <see cref="UserVoucher.RoomId"/> as the foreign key. The
        /// delete behavior for this relationship is set to <see cref="DeleteBehavior.NoAction"/>. </item>
        /// </list></remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{UserVoucher}"/> instance used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<UserVoucher> builder)
        {
            builder.HasOne(c => c.Voucher).WithMany(c => c.UserVouchers).HasForeignKey(c => c.VoucherId);
            builder.HasOne(c => c.Room).WithMany().HasForeignKey(c => c.RoomId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
