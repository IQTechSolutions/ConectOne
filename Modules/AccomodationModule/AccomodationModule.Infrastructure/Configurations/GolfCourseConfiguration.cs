using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="GolfCourse"/> and its relationships for the database model.
    /// </summary>
    /// <remarks>This configuration defines the relationship between the <see cref="GolfCourse"/> entity and
    /// its associated images. It specifies that a <see cref="GolfCourse"/> can have multiple images, and that deleting
    /// a <see cref="GolfCourse"/>  will cascade the deletion to its related images.</remarks>
    public class GolfCourseConfiguration : IEntityTypeConfiguration<GolfCourse>
    {
        /// <summary>
        /// Configures the entity type <see cref="GolfCourse"/> and its relationships.
        /// </summary>
        /// <remarks>This method establishes a one-to-many relationship between <see cref="GolfCourse"/>
        /// and its associated images. The relationship is configured with a foreign key on the images pointing to the
        /// <see cref="GolfCourse"/> entity. Deleting a <see cref="GolfCourse"/> will cascade the delete operation to
        /// its associated images.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="GolfCourse"/> entity.</param>
        public void Configure(EntityTypeBuilder<GolfCourse> builder)
        {
            builder.HasMany(c => c.Destinations).WithOne(c => c.GolfCourse).HasForeignKey(c => c.GolfCourseId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Images).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
