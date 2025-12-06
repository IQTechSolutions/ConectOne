using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type for <see cref="VacationExtensionAddition"/>.
    /// </summary>
    /// <remarks>This configuration establishes relationships between the <see
    /// cref="VacationExtensionAddition"/> entity and its related entities. It defines foreign key constraints and
    /// specifies the behavior on delete operations.</remarks>
    public class VacationExtensionConfiguration : IEntityTypeConfiguration<VacationExtensionAddition>
    {
        /// <summary>
        /// Configures the entity relationships for the <see cref="VacationExtensionAddition"/> type.
        /// </summary>
        /// <remarks>This method establishes the foreign key relationships between the <see
        /// cref="VacationExtensionAddition"/> entity and its related entities, ensuring that delete actions do not
        /// cascade.</remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<VacationExtensionAddition> builder)
        {
            builder.HasOne(c => c.ParentVacation).WithMany(c => c.Extensions).HasForeignKey(c => c.ExtensioId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.Extension).WithMany(c => c.ParentVacations).HasForeignKey(c => c.ParentVacationId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
