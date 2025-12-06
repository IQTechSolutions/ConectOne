using AccomodationModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the database schema for the <see cref="EntityCategory{T}"/> entity type where <typeparamref
    /// name="TEntity"/> is <see cref="Lodging"/>.
    /// </summary>
    /// <remarks>This configuration establishes relationships between the <see
    /// cref="EntityCategory{TEntity}"/> entity and its associated <see cref="Lodging"/> and category entities.
    /// Specifically: <list type="bullet"> <item> Configures a one-to-many relationship between <see
    /// cref="EntityCategory{TEntity}.Entity"/> and <see cref="EntityCategory{TEntity}.Categories"/>, with a foreign key
    /// on <see cref="EntityCategory{TEntity}.EntityId"/>. </item> <item> Configures a one-to-many relationship between
    /// <see cref="EntityCategory{TEntity}.Category"/> and <see cref="EntityCategory{TEntity}.EntityCollection"/>, with
    /// a foreign key on <see cref="EntityCategory{TEntity}.CategoryId"/>. </item> </list></remarks>
    public class LodgingCategoryConfiguration : IEntityTypeConfiguration<EntityCategory<Lodging>>
    {
        /// <summary>
        /// Configures the relationships and constraints for the <see cref="EntityCategory{TEntity}"/> entity type.
        /// </summary>
        /// <remarks>This method establishes foreign key relationships between the <see
        /// cref="EntityCategory{TEntity}"/> entity and  its associated <see cref="Entity"/> and <see cref="Category"/>
        /// entities. It ensures that: <list type="bullet"> <item> <description> The <see
        /// cref="EntityCategory{TEntity}.Entity"/> navigation property is configured with a one-to-many relationship.
        /// </description> </item> <item> <description> The <see cref="EntityCategory{TEntity}.Category"/> navigation
        /// property is configured with a one-to-many relationship. </description> </item> </list></remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="EntityCategory{TEntity}"/>
        /// entity.</param>
        public void Configure(EntityTypeBuilder<EntityCategory<Lodging>> builder)
        {
            builder.HasOne(c => c.Entity).WithMany(c => c.Categories).HasForeignKey(c => c.EntityId);
            builder.HasOne(c => c.Category).WithMany(c => c.EntityCollection).HasForeignKey(c => c.CategoryId);
        }
    }
}
