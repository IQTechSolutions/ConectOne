using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Provides configuration for the <see cref="Itinerary"/> entity type and its relationships.
    /// </summary>
    /// <remarks>This configuration establishes a one-to-many relationship between <see cref="Itinerary"/> and
    /// <see cref="ItineraryDetails"/>, with a foreign key on <see cref="ItineraryDetails.ItineraryId"/>.  The
    /// relationship is configured to cascade deletes, ensuring that deleting an <see cref="Itinerary"/>  will also
    /// delete its associated <see cref="ItineraryDetails"/>.</remarks>
    public class ItiniraryConfiguration : IEntityTypeConfiguration<Itinerary>
    {
        /// <summary>
        /// Configures the entity type for <see cref="Itinerary"/> and its relationships.
        /// </summary>
        /// <remarks>This configuration establishes a one-to-many relationship between <see
        /// cref="Itinerary"/> and  <see cref="ItineraryDetails"/>, with a cascading delete behavior when an <see
        /// cref="Itinerary"/> is removed.</remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{Itinerary}"/> used to configure the <see cref="Itinerary"/> entity.</param>
        public void Configure(EntityTypeBuilder<Itinerary> builder)
        {
            builder.HasMany(c => c.ItineraryDetails).WithOne(c => c.Itinerary).HasForeignKey(c => c.ItineraryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
