using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingModule.Domain.Entities;

namespace ShoppingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Coupon"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration specifies the primary key for the <see cref="Coupon"/> entity. Additional
    /// configurations for the entity can be added within this method as needed.</remarks>
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        /// <summary>
        /// Configures the entity type for the <see cref="Coupon"/> model.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="Coupon"/> entity.</param>
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
