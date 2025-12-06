using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type settings for <see cref="LodgingListingRequest"/>.
    /// </summary>
    /// <remarks>This class is used to define the configuration for the <see cref="LodgingListingRequest"/>
    /// entity, including its primary key and other entity-specific settings. Implementations of <see
    /// cref="IEntityTypeConfiguration{TEntity}"/> are typically used in the  Entity Framework Core model building
    /// process.</remarks>
    public class LodgingListingRequestConfiguraiton : IEntityTypeConfiguration<LodgingListingRequest>
    {
        /// <summary>
        /// Configures the entity type for <see cref="LodgingListingRequest"/>.
        /// </summary>
        /// <remarks>This method sets up the primary key for the <see cref="LodgingListingRequest"/>
        /// entity. Additional configuration for the entity can be applied using the provided <paramref
        /// name="builder"/>.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="LodgingListingRequest"/>
        /// entity.</param>
        public void Configure(EntityTypeBuilder<LodgingListingRequest> builder)
        {
            builder.HasKey(c => c.Id);
		}
    }
}
