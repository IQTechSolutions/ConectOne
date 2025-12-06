using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type for <see cref="FeaturedImage"/>.
    /// </summary>
    /// <remarks>This class is used to define the configuration for the <see cref="FeaturedImage"/> entity, 
    /// including its primary key and other entity-specific settings. It is typically used in  conjunction with Entity
    /// Framework Core to customize the model's behavior.</remarks>
    public class FeaturedServiceImageConfiguration : IEntityTypeConfiguration<FeaturedImage>
    {
        /// <summary>
        /// Configures the <see cref="FeaturedImage"/> entity type.
        /// </summary>
        /// <remarks>This method sets up the primary key for the <see cref="FeaturedImage"/> entity.
        /// Additional configuration can be applied using the provided <paramref name="builder"/>.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{FeaturedImage}"/> instance used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<FeaturedImage> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
