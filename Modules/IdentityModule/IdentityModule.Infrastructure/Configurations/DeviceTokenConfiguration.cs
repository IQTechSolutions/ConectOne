using IdentityModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type settings for the <see cref="DeviceToken"/> class.
    /// </summary>
    /// <remarks>This configuration specifies the primary key for the <see cref="DeviceToken"/> entity.
    /// Implement this class to define additional entity configurations as needed.</remarks>
    public class DeviceTokenConfiguration : IEntityTypeConfiguration<DeviceToken>
    {
        /// <summary>
        /// Configures the entity type for <see cref="DeviceToken"/>.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="DeviceToken"/> entity.</param>
        public void Configure(EntityTypeBuilder<DeviceToken> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
