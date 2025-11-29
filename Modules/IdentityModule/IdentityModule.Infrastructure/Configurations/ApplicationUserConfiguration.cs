using IdentityModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the <see cref="ApplicationUser"/> entity.
    /// </summary>
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        /// <summary>
        /// Configures the entity type for the ApplicationUser model using the specified builder.
        /// </summary>
        /// <remarks>This method is typically called by the Entity Framework infrastructure when building
        /// the model. It sets up table mapping, property constraints, and relationships for the ApplicationUser
        /// entity.</remarks>
        /// <param name="builder">The builder used to configure the ApplicationUser entity type.</param>
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.ToTable(name: "Users", "Identity");
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.ReasonForRejection).HasMaxLength(5000);

            builder.HasOne(c => c.UserInfo).WithOne().HasPrincipalKey<ApplicationUser>(c => c.Id);
        }
    }
}
