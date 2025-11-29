using IdentityModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="ApplicationRoleClaim"/> for the database schema.
    /// </summary>
    /// <remarks>This configuration sets up the properties and relationships for the <see
    /// cref="ApplicationRoleClaim"/> entity: <list type="bullet"> <item><description>The <c>Id</c> property is
    /// configured to have values generated on add.</description></item> <item><description>A relationship is
    /// established between <see cref="ApplicationRoleClaim"/> and its associated role,  with a foreign key on
    /// <c>RoleId</c> and a delete behavior of <c>NoAction</c>.</description></item> <item><description>The entity is
    /// mapped to the <c>RoleClaims</c> table in the <c>Identity</c> schema.</description></item> </list></remarks>
    public class ApplicationRoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        /// <summary>
        /// Configures the entity type for <see cref="ApplicationRoleClaim"/>.
        /// </summary>
        /// <remarks>This configuration sets the <c>Id</c> property to have values generated on add,
        /// establishes a relationship between <see cref="ApplicationRoleClaim"/> and its associated role, and maps the
        /// entity to the "RoleClaims" table in the "Identity" schema.</remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="ApplicationRoleClaim"/>
        /// entity.</param>
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasOne(d => d.Role).WithMany(p => p.RoleClaims).HasForeignKey(d => d.RoleId).OnDelete(DeleteBehavior.NoAction);
            builder.ToTable(name: "RoleClaims", "Identity");
        }
    }
}
