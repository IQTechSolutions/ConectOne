using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Infrastructure.Configurations
{
    /// <summary>
    /// Provides the Entity Framework Core configuration for the ActivityGroupTeamMember entity.
    /// </summary>
    /// <remarks>This configuration defines the relationships and foreign keys between
    /// ActivityGroupTeamMember, Learner, and ActivityGroup entities. It is typically used within the OnModelCreating
    /// method of a DbContext to apply entity-specific configuration.</remarks>
    public class ActivityGroupTeamMemberConfiguraiton : IEntityTypeConfiguration<ActivityGroupTeamMember>
    {
        /// <summary>
        /// Configures the entity type mapping for the ActivityGroupTeamMember entity.
        /// </summary>
        /// <remarks>This method sets up the relationships between ActivityGroupTeamMember, Learner, and
        /// ActivityGroup entities. It is typically called by the Entity Framework infrastructure when building the
        /// model.</remarks>
        /// <param name="builder">The builder used to configure the ActivityGroupTeamMember entity type.</param>
        public void Configure(EntityTypeBuilder<ActivityGroupTeamMember> builder)
        {
            builder.HasOne(c => c.Learner).WithMany(c => c.ActivityGroups).HasForeignKey(c => c.LearnerId);
            builder.HasOne(c => c.ActivityGroup).WithMany(c => c.TeamMembers).HasForeignKey(c => c.ActivityGroupId);
        }
    }
}
