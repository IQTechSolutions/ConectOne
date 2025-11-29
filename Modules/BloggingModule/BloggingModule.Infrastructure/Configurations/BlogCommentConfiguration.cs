using BloggingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloggingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Comment{T}"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration defines relationships and constraints for the <see
    /// cref="Comment{BlogPost}"/> entity: <list type="bullet"> <item> <description> Establishes a one-to-many
    /// relationship between <see cref="Comment{BlogPost}.Entity"/> and its associated comments. </description> </item>
    /// <item> <description> Configures a hierarchical relationship for comments, allowing each comment to have a parent
    /// comment. </description> </item> </list></remarks>
    public class BlogCommentConfiguration : IEntityTypeConfiguration<Comment<BlogPost>>
    {
        /// <summary>
        /// Configures the entity type for <see cref="Comment{BlogPost}"/>.
        /// </summary>
        /// <remarks>This method establishes relationships between the <see cref="Comment{BlogPost}"/>
        /// entity and other entities. It configures a one-to-many relationship between <see cref="Comment{BlogPost}"/>
        /// and its parent entity,  <see cref="BlogPost"/>, as well as a self-referencing relationship for nested
        /// comments.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<Comment<BlogPost>> builder)
        {
            builder.HasOne(c => c.Entity).WithMany(c => c.Comments).HasForeignKey(c => c.EntityId);

            builder.HasMany(c => c.Comments).WithOne(c => c.ParentComment).HasForeignKey(c => c.ParentCommentId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}