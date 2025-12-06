using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="CancellationRule"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration defines the relationship between the <see cref="CancellationRule"/> entity
    /// and the <see cref="Lodging"/> entity. Specifically, it establishes a one-to-many relationship where a <see
    /// cref="Lodging"/> can have multiple <see cref="CancellationRule"/> entries.</remarks>
    public class CancellationRuleConfiguration : IEntityTypeConfiguration<CancellationRule>
    {
        /// <summary>
        /// Configures the <see cref="CancellationRule"/> entity type.
        /// </summary>
        /// <remarks>This method establishes a relationship between the <see cref="CancellationRule"/>
        /// entity and the  <see cref="Lodging"/> entity. It specifies that a <see cref="CancellationRule"/> is
        /// associated with  one <see cref="Lodging"/>, and a <see cref="Lodging"/> can have many <see
        /// cref="CancellationRule"/>  instances. The foreign key for this relationship is <c>LodgingId</c>.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{CancellationRule}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<CancellationRule> builder)
        {
            builder.HasOne(c => c.Lodging).WithMany(c => c.CancellationRules).HasForeignKey(c => c.LodgingId);
        }
    }
}
