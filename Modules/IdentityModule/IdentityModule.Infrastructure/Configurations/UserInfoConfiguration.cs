using IdentityModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type settings for the <see cref="UserInfo"/> entity.
    /// </summary>
    /// <remarks>This configuration defines the properties and relationships for the <see cref="UserInfo"/>
    /// entity: <list type="bullet"> <item><description>Sets maximum lengths for the <see cref="UserInfo.IdentityNr"/>,
    /// <see cref="UserInfo.MoodStatus"/>, and <see cref="UserInfo.Bio"/> properties.</description></item>
    /// <item><description>Configures a one-to-one relationship between <see cref="UserInfo"/> and <see
    /// cref="UserAppSettings"/> with <see cref="UserInfo.Id"/> as the principal key.</description></item>
    /// <item><description>Configures one-to-many relationships between <see cref="UserInfo"/> and its associated
    /// collections of <see cref="ContactNumbers"/>, <see cref="EmailAddresses"/>, and <see cref="Addresses"/> entities,
    /// using <c>EntityId</c> as the foreign key.</description></item> </list></remarks>
    public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        /// <summary>
        /// Configures the entity type for the <see cref="UserInfo"/> class.
        /// </summary>
        /// <remarks>This method defines the properties and relationships for the <see cref="UserInfo"/>
        /// entity: <list type="bullet"> <item><description>Sets maximum lengths for the <see
        /// cref="UserInfo.IdentityNr"/>, <see cref="UserInfo.MoodStatus"/>, and <see cref="UserInfo.Bio"/>
        /// properties.</description></item> <item><description>Configures a one-to-one relationship between <see
        /// cref="UserInfo"/> and <see cref="UserAppSettings"/>.</description></item> <item><description>Configures
        /// one-to-many relationships between <see cref="UserInfo"/> and its associated <see cref="ContactNumbers"/>,
        /// <see cref="EmailAddresses"/>, and <see cref="Addresses"/> entities.</description></item> </list></remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the <see cref="UserInfo"/> entity.</param>
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.Property(c => c.IdentityNr).HasMaxLength(25);
            builder.Property(c => c.MoodStatus).HasMaxLength(255);
            builder.Property(c => c.Bio).HasMaxLength(5000);

            builder.HasOne(c => c.UserAppSettings).WithOne().HasPrincipalKey<UserInfo>(c => c.Id);

            builder.HasMany(c => c.ContactNumbers).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId);
            builder.HasMany(c => c.EmailAddresses).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId);
            builder.HasMany(c => c.Addresses).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId);
        }
    }
}
