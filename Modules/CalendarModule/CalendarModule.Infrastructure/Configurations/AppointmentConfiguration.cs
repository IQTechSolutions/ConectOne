using CalendarModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type settings for the <see cref="Appointment"/> class.
    /// </summary>
    /// <remarks>This configuration defines the primary key for the <see cref="Appointment"/> entity. It is
    /// typically used in the context of Entity Framework Core to configure the database schema.</remarks>
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        /// <summary>
        /// Configures the entity type for <see cref="Appointment"/>.
        /// </summary>
        /// <remarks>This method sets the primary key for the <see cref="Appointment"/> entity.</remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="Appointment"/> entity.</param>
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.AudienceType).HasConversion<int>();

            builder.HasMany(c => c.UserInvites)
                .WithOne(c => c.Appointment)
                .HasForeignKey(c => c.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.RoleInvites)
                .WithOne(c => c.Appointment)
                .HasForeignKey(c => c.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
