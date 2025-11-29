using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingModule.Domain.Entities;

namespace ShoppingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type settings for the <see cref="ShoppingCartCoupon"/> entity.
    /// </summary>
    /// <remarks>This configuration establishes the relationship between the <see cref="ShoppingCartCoupon"/>
    /// entity and its associated <see cref="Coupon"/> entity. Specifically, it defines a foreign key constraint on the
    /// <c>CouponId</c> property and specifies that deletions of related <see cref="Coupon"/> entities will not cascade
    /// to <see cref="ShoppingCartCoupon"/> entities.</remarks>
    public class ShoppingCartCouponConfiguration : IEntityTypeConfiguration<ShoppingCartCoupon>
    {
        /// <summary>
        /// Configures the entity type for <see cref="ShoppingCartCoupon"/>.
        /// </summary>
        /// <remarks>This method establishes a relationship between the <see cref="ShoppingCartCoupon"/>
        /// entity and the  <see cref="Coupon"/> entity, specifying a foreign key on <c>CouponId</c> and configuring the
        /// delete behavior  to <see cref="DeleteBehavior.NoAction"/>.</remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="ShoppingCartCoupon"/> entity.</param>
        public void Configure(EntityTypeBuilder<ShoppingCartCoupon> builder)
        {
            builder.HasOne(c => c.Coupon).WithMany().HasForeignKey(c => c.CouponId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
