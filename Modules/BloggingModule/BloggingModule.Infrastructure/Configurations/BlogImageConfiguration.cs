using BloggingModule.Domain.Entities;
using FilingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloggingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="EntityImage{TEntity, TKey}"/> for use with the <see cref="BlogPost"/>
    /// entity.
    /// </summary>
    /// <remarks>This configuration establishes the relationships between the <see cref="EntityImage{TEntity,
    /// TKey}"/> and the  <see cref="BlogPost"/> entity, as well as the associated image entity. Specifically: <list
    /// type="bullet"> <item> <description>Defines a one-to-many relationship between <see cref="BlogPost"/> and its
    /// images, using the <c>EntityId</c> foreign key.</description> </item> <item> <description>Defines a foreign key
    /// relationship between the image entity and the <c>ImageId</c> property.</description> </item> </list></remarks>
    public class BlogImageConfiguration : IEntityTypeConfiguration<EntityImage<BlogPost, string>>
    {
        /// <summary>
        /// Configures the entity type for <see cref="EntityImage{TEntity, TKey}"/> with specific relationships and
        /// keys.
        /// </summary>
        /// <remarks>This method establishes the relationships between the <see cref="EntityImage{TEntity,
        /// TKey}"/> entity  and its associated entities. Specifically, it configures: <list type="bullet"> <item> A
        /// one-to-many relationship between the entity and its images, with a foreign key on <c>EntityId</c>. </item>
        /// <item> A foreign key relationship for the image, using <c>ImageId</c>. </item> </list></remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<EntityImage<BlogPost, string>> builder)
        {
            builder.HasOne(c => c.Entity).WithMany(c => c.Images).HasForeignKey(c => c.EntityId);
            builder.HasOne(c => c.Image).WithMany().HasForeignKey(c => c.ImageId); }
    }
}
