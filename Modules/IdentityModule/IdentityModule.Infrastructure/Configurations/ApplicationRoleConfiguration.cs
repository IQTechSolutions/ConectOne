using IdentityModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityModule.Infrastructure.Configurations
{
    /// <summary>
    /// Provides the Entity Framework Core configuration for the ApplicationRole entity.
    /// </summary>
    /// <remarks>This configuration specifies how the ApplicationRole entity is mapped to the database,
    /// including table name and key generation. It is typically used by the Entity Framework Core infrastructure and is
    /// not intended to be used directly in application code.</remarks>
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        /// <summary>
        /// Configures the entity type for the ApplicationRole model.
        /// </summary>
        /// <remarks>This method is typically called by the Entity Framework infrastructure when building
        /// the model. It sets up the table mapping and key generation for the ApplicationRole entity.</remarks>
        /// <param name="builder">The builder used to configure the ApplicationRole entity type.</param>
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.ToTable("UserRoles", "Identity");
        }
    }
}
