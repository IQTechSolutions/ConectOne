using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Infrastructure.Configurations
{
    /// <summary>
    /// Provides the Entity Framework Core configuration for the ParticipatingActivityGroup entity type.
    /// </summary>
    /// <remarks>This configuration defines the relationships and foreign key constraints for the
    /// ParticipatingActivityGroup entity when used with Entity Framework Core's model builder. It is typically used
    /// during the model creation process to ensure correct mapping between the entity and the database
    /// schema.</remarks>
    public class ParticipatingActivityGroupConfiguration : IEntityTypeConfiguration<ParticipatingActivityGroup>
    {
        /// <summary>
        /// Configures the entity type for ParticipatingActivityGroup using the specified builder.
        /// </summary>
        /// <remarks>This method sets up the relationships between ParticipatingActivityGroup and related
        /// entities, including foreign key constraints and delete behaviors. It is typically called by the Entity
        /// Framework infrastructure when building the model.</remarks>
        /// <param name="builder">The builder used to configure the ParticipatingActivityGroup entity type.</param>
        public void Configure(EntityTypeBuilder<ParticipatingActivityGroup> builder)
        {
            builder.HasOne(c => c.ActivityGroup).WithMany(c => c.ParticipatingActivityGroups).HasForeignKey(c => c.ActivityGroupId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.Event).WithMany(c => c.ParticipatingActivityGroups).HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
