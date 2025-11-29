using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity of type <see cref="Teacher"/>.
    /// </summary>
    /// <remarks>This configuration sets up the relationships between the <see cref="Teacher"/> entity and its
    /// related entities, such as <see cref="Address"/>, <see cref="ContactNumbers"/>, and <see cref="EmailAddresses"/>.
    /// It specifies cascading delete behavior for these relationships.</remarks>
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        /// <summary>
        /// Configures the entity of type <see cref="Teacher"/>.
        /// </summary>
        /// <remarks>This method establishes relationships between the <see cref="Teacher"/> entity and
        /// its related entities. It configures a one-to-one relationship with the <see cref="Address"/> entity and
        /// one-to-many relationships with the <see cref="ContactNumbers"/> and <see cref="EmailAddresses"/>
        /// collections. All related entities are configured to be deleted when the <see cref="Teacher"/> entity is
        /// deleted.</remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{T}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasOne(c => c.Address).WithOne(c => c.Entity).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.ContactNumbers).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.EmailAddresses).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
