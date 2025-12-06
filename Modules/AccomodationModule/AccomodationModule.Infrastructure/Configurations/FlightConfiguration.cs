using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Flight"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This class defines the relationships and foreign key constraints for the <see
    /// cref="Airport"/> entity. It is used to configure the entity within the database context.</remarks>
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        /// <summary>
        /// Configures the entity type <see cref="Flight"/> and its relationships.
        /// </summary>
        /// <remarks>This method defines the relationships between the <see cref="Flight"/> entity and other entities</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the <see cref="Flight"/> entity.</param>
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.DepartureAirport).WithMany(c => c.Departures).HasForeignKey(c => c.DepartureAirportId);
            builder.HasOne(c => c.ArrivalAirport).WithMany(c => c.Arrivals).HasForeignKey(c => c.ArrivalAirportId);
        }
    }
}
