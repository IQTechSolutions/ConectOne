using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity framework mapping for the ParentPermission entity.
    /// </summary>
    /// <remarks>This configuration defines the relationships and foreign key constraints for the
    /// ParentPermission entity when using Entity Framework Core. It is typically used within the OnModelCreating method
    /// of a DbContext to apply custom entity configuration.</remarks>
    public class ParentPermissionConfiguration : IEntityTypeConfiguration<ParentPermission>
    {
        /// <summary>
        /// Configures the entity type mapping for the ParentPermission entity.
        /// </summary>
        /// <remarks>This method sets up the relationships between ParentPermission and related entities,
        /// including Learner, Parent, and Event, with cascade delete behavior for each foreign key
        /// relationship.</remarks>
        /// <param name="builder">The builder used to configure the ParentPermission entity type and its relationships.</param>
        public void Configure(EntityTypeBuilder<ParentPermission> builder)
        {
            builder.HasOne(c => c.Learner).WithMany(c => c.EventConsents).HasForeignKey(c => c.LearnerId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(c => c.Parent).WithMany(c => c.EventConsents).HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(c => c.Event).WithMany(c => c.EventConsents).HasForeignKey(c => c.EventId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
