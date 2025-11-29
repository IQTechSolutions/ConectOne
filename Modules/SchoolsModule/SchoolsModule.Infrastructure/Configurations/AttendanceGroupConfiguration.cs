using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="AttendanceGroup"/>.
    /// </summary>
    /// <remarks>This configuration sets up the relationship between <see cref="AttendanceGroup"/> and its
    /// associated attendance records, ensuring that when an <see cref="AttendanceGroup"/> is deleted, its related
    /// attendance records are also removed from the database.</remarks>
    public class AttendanceGroupConfiguration : IEntityTypeConfiguration<AttendanceGroup>
    {
        /// <summary>
        /// Configures the entity type <see cref="AttendanceGroup"/> and its relationships.
        /// </summary>
        /// <remarks>This method sets up a one-to-many relationship between <see cref="AttendanceGroup"/>
        /// and  <see cref="AttendanceRecord"/> entities, with a cascading delete behavior on the foreign key.</remarks>
        /// <param name="builder">The builder used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<AttendanceGroup> builder)
        {
            builder.HasMany(c => c.AttendanceRecords).WithOne(c => c.Group).HasForeignKey(c => c.GroupId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
