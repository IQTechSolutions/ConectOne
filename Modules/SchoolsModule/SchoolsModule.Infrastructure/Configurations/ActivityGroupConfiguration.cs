using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Infrastructure.Configurations
{
    /// <summary>
    /// Provides the Entity Framework Core configuration for the ActivityGroup entity type.
    /// </summary>
    /// <remarks>This configuration defines the relationships, foreign keys, and delete behaviors for the
    /// ActivityGroup entity when used with Entity Framework Core's model builder. It is typically used internally by
    /// the DbContext to configure the model during application startup.</remarks>
    public class ActivityGroupConfiguration : IEntityTypeConfiguration<ActivityGroup>
    {
        /// <summary>
        /// Configures the entity type mapping for the ActivityGroup entity.
        /// </summary>
        /// <remarks>This method defines the relationships and foreign key constraints for the
        /// ActivityGroup entity, including associations with AgeGroup, Teacher, Categories, and TeamMembers. It is
        /// typically called by the Entity Framework Core infrastructure when building the model.</remarks>
        /// <param name="builder">The builder used to configure the ActivityGroup entity type.</param>
        public void Configure(EntityTypeBuilder<ActivityGroup> builder)
        {
            builder.HasOne(c => c.AgeGroup).WithMany().HasForeignKey(c => c.AgeGroupId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c => c.Teacher).WithMany().HasForeignKey(c => c.TeacherId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Categories).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.TeamMembers).WithOne(c => c.ActivityGroup).HasForeignKey(c => c.ActivityGroupId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
