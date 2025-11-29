using BloggingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BloggingModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="BlogPost"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration defines property constraints and relationships for the <see
    /// cref="BlogPost"/> entity. It sets the maximum length and required status for the <c>Title</c> property, the
    /// maximum length for the  <c>Description</c> property, and establishes a one-to-many relationship between
    /// <c>BlogPost</c> and  <c>Categories</c>.</remarks>
    public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        /// <summary>
        /// Configures the <see cref="BlogPost"/> entity type for use with Entity Framework Core.
        /// </summary>
        /// <remarks>This method sets up property constraints and relationships for the <see
        /// cref="BlogPost"/> entity: <list type="bullet"> <item> <description>Configures the <c>Title</c> property to
        /// have a maximum length of 250 characters and to be required.</description> </item> <item>
        /// <description>Configures the <c>Description</c> property to have a maximum length of 5000
        /// characters.</description> </item> <item> <description>Establishes a one-to-many relationship between
        /// <c>BlogPost</c> and <c>Categories</c>, with a foreign key on <c>EntityId</c>.</description> </item>
        /// </list></remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the <see cref="BlogPost"/> entity.</param>
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.Property(c => c.Title).HasMaxLength(250).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(5000);

            builder.HasMany(c => c.Categories).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId);
        }
    }
}