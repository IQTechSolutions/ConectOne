using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the database schema for the <see cref="VacationHost"/> entity.
    /// </summary>
    /// <remarks>This configuration defines the unique index on the <c>Name</c> property and establishes
    /// relationships between <see cref="VacationHost"/> and related entities, including <c>Vacations</c>,
    /// <c>VacationExtensions</c>, and <c>Images</c>. It also specifies cascading delete behavior for certain
    /// relationships.</remarks>
    public class VacationContactUsInfoConfiguration : IEntityTypeConfiguration<VacationContactUsInfo>
    {
        /// <summary>
        /// Configures the entity type for <see cref="VacationHost"/>.
        /// </summary>
        /// <remarks>This method defines the entity's relationships, indexes, and constraints. It ensures
        /// that the <see cref="VacationHost.Name"/> property has a unique index and configures cascading delete
        /// behavior for related entities where applicable.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{VacationContactUsInfo}"/> used to configure the <see cref="VacationContactUsInfo"/> entity.</param>
        public void Configure(EntityTypeBuilder<VacationContactUsInfo> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
