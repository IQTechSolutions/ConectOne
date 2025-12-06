using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="ChildAgeParams"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration defines the relationship between <see cref="ChildAgeParams"/> and its
    /// associated <see cref="Service"/> entity, specifying a one-to-many relationship and a foreign key
    /// constraint.</remarks>
    public class ChildAgeParamsConfiguration : IEntityTypeConfiguration<ChildAgeParams>
    {
        /// <summary>
        /// Configures the entity type <see cref="ChildAgeParams"/> for the database model.
        /// </summary>
        /// <remarks>This method establishes a relationship between the <see cref="ChildAgeParams"/>
        /// entity and the  <see cref="Service"/> entity, specifying that <see cref="ChildAgeParams"/> has a foreign key
        /// referencing <see cref="Service.ServiceId"/>.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{ChildAgeParams}"/> instance used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<ChildAgeParams> builder)
        {
            builder.HasOne(c => c.Service).WithMany(c => c.ChildAgeParams).HasForeignKey(c => c.ServiceId);
        }
    }
}
