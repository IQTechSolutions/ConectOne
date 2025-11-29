using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Infrastructure.Configurations
{
    /// <summary>
    /// Provides the Entity Framework Core configuration for the Learner entity type.
    /// </summary>
    /// <remarks>This configuration defines the relationships, foreign keys, and delete behaviors for the
    /// Learner entity when used with Entity Framework Core's model builder. It should be registered as part of the
    /// model configuration process to ensure the correct mapping between the Learner entity and the database
    /// schema.</remarks>
    public class LearnerConfiguration : IEntityTypeConfiguration<Learner>
    {
        /// <summary>
        /// Configures the entity type mapping for the Learner entity using the specified builder.
        /// </summary>
        /// <remarks>This method defines the relationships between the Learner entity and related
        /// entities, including SchoolClass, ContactNumbers, EmailAddresses, Parents, and MedicalAidParent. It specifies
        /// foreign key constraints and delete behaviors for these relationships. Call this method within the Entity
        /// Framework Core model configuration process to ensure correct mapping of the Learner entity.</remarks>
        /// <param name="builder">The builder used to configure the Learner entity type and its relationships.</param>
        public void Configure(EntityTypeBuilder<Learner> builder)
        {
            builder.HasOne(c => c.SchoolClass).WithMany(c => c.Learners).HasForeignKey(c => c.SchoolClassId);

            builder.HasMany(c => c.ContactNumbers).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.EmailAddresses).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Parents).WithOne(c => c.Learner).HasForeignKey(c => c.LearnerId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.MedicalAidParent)
                .WithMany(c => c.MedicalAidLearners)
                .HasForeignKey(c => c.MedicalAidParentId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
