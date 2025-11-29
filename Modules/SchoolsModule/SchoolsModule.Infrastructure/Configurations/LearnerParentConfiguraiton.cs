using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Infrastructure.Configurations
{
    /// <summary>
    /// Provides the Entity Framework Core configuration for the LearnerParent entity.
    /// </summary>
    /// <remarks>This configuration defines the relationships between the LearnerParent entity and the related
    /// Learner and Parent entities. It is typically used within the DbContext's OnModelCreating method to apply custom
    /// mapping and relationship rules.</remarks>
    public class LearnerParentConfiguration : IEntityTypeConfiguration<LearnerParent>
    {
        /// <summary>
        /// Configures the entity relationships for the LearnerParent entity type.
        /// </summary>
        /// <remarks>This method sets up the foreign key relationships between LearnerParent, Learner, and
        /// Parent entities. It is typically called by the Entity Framework Core infrastructure when building the
        /// model.</remarks>
        /// <param name="builder">The builder used to configure the LearnerParent entity type.</param>
        public void Configure(EntityTypeBuilder<LearnerParent> builder)
        {
            builder.HasOne(c => c.Learner).WithMany(c => c.Parents).HasForeignKey(c => c.LearnerId);
            builder.HasOne(c => c.Parent).WithMany(c => c.Learners).HasForeignKey(c => c.ParentId);
        }
    }
}
