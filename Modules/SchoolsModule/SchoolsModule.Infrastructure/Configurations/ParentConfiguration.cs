using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Infrastructure.Configurations
{
    /// <summary>
    /// Provides the Entity Framework Core configuration for the Parent entity type.
    /// </summary>
    /// <remarks>This configuration defines the relationships and cascade delete behaviors for related
    /// entities such as addresses, contact numbers, email addresses, emergency contacts, learners, and event consents.
    /// It is typically used by the Entity Framework Core infrastructure and is not intended to be used directly in
    /// application code.</remarks>
    public class ParentConfiguration : IEntityTypeConfiguration<Parent>
    {
        /// <summary>
        /// Configures the entity type mapping for the Parent entity and its related collections in the Entity Framework
        /// model builder.
        /// </summary>
        /// <remarks>This method sets up one-to-many relationships between the Parent entity and its
        /// related collections, including addresses, contact numbers, email addresses, emergency contacts, learners,
        /// and event consents. All relationships are configured with cascade delete behavior, so related entities are
        /// automatically deleted when a Parent is removed.</remarks>
        /// <param name="builder">The builder used to configure the Parent entity type and its relationships.</param>
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.HasMany(c => c.Addresses).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.ContactNumbers).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.EmailAddresses).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.EmergencyContacts).WithOne(c => c.Parent).HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Learners).WithOne(c => c.Parent).HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.EventConsents).WithOne(c => c.Parent).HasForeignKey(c => c.ParentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
