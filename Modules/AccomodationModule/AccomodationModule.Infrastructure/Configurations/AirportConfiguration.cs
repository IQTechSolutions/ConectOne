using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Airport"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This class defines the relationships and foreign key constraints for the <see
    /// cref="Airport"/> entity. It is used to configure the entity within the database context.</remarks>
    public class AirportConfiguration : IEntityTypeConfiguration<Airport>
    {
        /// <summary>
        /// Configures the entity type <see cref="Booking"/> and its relationships.
        /// </summary>
        /// <remarks>This method defines the relationships between the <see cref="Airport"/> entity and other entities</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the <see cref="Airport"/> entity.</param>
        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasMany(c => c.Departures).WithOne(c => c.DepartureAirport).HasForeignKey(c => c.DepartureAirportId);
            builder.HasMany(c => c.Arrivals).WithOne(c => c.ArrivalAirport).HasForeignKey(c => c.ArrivalAirportId);
        }
    }
}
