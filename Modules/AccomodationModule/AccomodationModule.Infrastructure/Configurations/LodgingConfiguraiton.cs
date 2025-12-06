using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Lodging"/> and its relationships for the database model.
    /// </summary>
    /// <remarks>This configuration defines the relationships and constraints for the <see cref="Lodging"/>
    /// entity, including: <list type="bullet"> <item> A one-to-one relationship between <see cref="Lodging"/> and its
    /// <c>Settings</c>, with a principal key on <c>Id</c> and cascade delete behavior. </item> <item> One-to-many
    /// relationships between <see cref="Lodging"/> and its associated collections, such as <c>Amneties</c>,
    /// <c>Services</c>, <c>AccountTypes</c>, <c>CancellationRules</c>, <c>Vouchers</c>, and <c>Images</c>, all with
    /// foreign keys and cascade delete behavior. </item> <item> A one-to-many relationship between <see
    /// cref="Lodging"/> and <c>FeaturedImages</c>, with a foreign key on <c>LodgingId</c> and cascade delete behavior.
    /// </item> </list> This configuration is applied using the <see cref="IEntityTypeConfiguration{TEntity}"/>
    /// interface.</remarks>
    public class LodgingConfiguration : IEntityTypeConfiguration<Lodging>
    {
        /// <summary>
        /// Configures the entity type <see cref="Lodging"/> and its relationships using the provided <see
        /// cref="EntityTypeBuilder{TEntity}"/>.
        /// </summary>
        /// <remarks>This method defines the relationships and constraints for the <see cref="Lodging"/>
        /// entity, including: <list type="bullet"> <item> A one-to-one relationship with <see cref="Settings"/>, where
        /// the principal key is <see cref="Lodging.Id"/> and the delete behavior is cascade. </item> <item> One-to-many
        /// relationships with <see cref="Amneties"/>, <see cref="Services"/>, <see cref="AccountTypes"/>, <see
        /// cref="CancellationRules"/>, <see cref="Vouchers"/>, and <see cref="Images"/>,  all configured with foreign
        /// keys and cascade delete behavior. </item> <item> A one-to-many relationship with <see
        /// cref="FeaturedImages"/>, configured with a foreign key and cascade delete behavior. </item>
        /// </list></remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the <see cref="Lodging"/> entity.</param>
        public void Configure(EntityTypeBuilder<Lodging> builder)
        {
            builder.HasOne(c => c.Settings).WithOne().HasPrincipalKey<Lodging>(c => c.Id).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(c => c.LodgingType).WithMany().HasForeignKey(c => c.LodgingTypeId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Amneties).WithOne(c => c.Lodging).HasForeignKey(c => c.LodgingId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Services).WithOne(c => c.Lodging).HasForeignKey(c => c.LodgingId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Destinations).WithOne(c => c.Lodging).HasForeignKey(c => c.LodgingId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.AccountTypes).WithOne(c => c.Lodging).HasForeignKey(c => c.LodgingId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.CancellationRules).WithOne(c => c.Lodging).HasForeignKey(c => c.LodgingId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Vouchers).WithOne(c => c.Lodging).HasForeignKey(c => c.LodgingId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Images).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Videos).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Documents).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
