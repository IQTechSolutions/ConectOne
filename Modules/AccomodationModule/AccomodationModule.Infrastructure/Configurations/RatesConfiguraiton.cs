using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Rates"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration defines the relationship between the <see cref="Rates"/> entity and the
    /// <see cref="Service"/> entity. Specifically, it establishes a one-to-many relationship where a <see
    /// cref="Service"/> can have multiple <see cref="Rates"/>, and sets up a foreign key constraint on the
    /// <c>ServiceId</c> property.</remarks>
    public class RatesConfiguraiton : IEntityTypeConfiguration<Rates>
    {
        /// <summary>
        /// Configures the entity type <see cref="Rates"/> for the database model.
        /// </summary>
        /// <remarks>This method establishes a relationship between the <see cref="Rates"/> entity and the
        /// <see cref="Service"/> entity. Specifically, it configures a foreign key association where each <see
        /// cref="Rates"/> entity is linked to a  <see cref="Service"/> entity, and a <see cref="Service"/> can have
        /// multiple associated <see cref="Rates"/>.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the <see cref="Rates"/> entity.</param>
        public void Configure(EntityTypeBuilder<Rates> builder)
        {
            builder.HasOne(c => c.Service).WithMany(c => c.Rates).HasForeignKey(c => c.ServiceId);
        }
    }
}
