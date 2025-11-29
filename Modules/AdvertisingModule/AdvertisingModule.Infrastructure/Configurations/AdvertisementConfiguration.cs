using AdvertisingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdvertisingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Advertisement"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration specifies the primary key for the <see cref="Advertisement"/> entity. Use
    /// this class to define additional entity-specific configurations, such as relationships, constraints, or
    /// indexes.</remarks>
    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        /// <summary>
        /// Configures the entity type for <see cref="Advertisement"/>.
        /// </summary>
        /// <remarks>This method sets up the primary key for the <see cref="Advertisement"/> entity.
        /// Additional configuration for the entity can be applied using the provided <paramref
        /// name="builder"/>.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the <see cref="Advertisement"/>
        /// entity.</param>
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
