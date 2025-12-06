using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Destination"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration defines the relationship between the <see cref="Destination"/> entity and
    /// its associated images. It specifies that a <see cref="Destination"/> can have many images, and each image is
    /// associated with a single <see cref="Destination"/>. The foreign key linking the images to the <see
    /// cref="Destination"/> is <c>EntityId</c>, and the delete behavior is set to cascade.</remarks>
    public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
    {
        /// <summary>
        /// Configures the entity type <see cref="Destination"/> and its relationships.
        /// </summary>
        /// <remarks>This method establishes a one-to-many relationship between <see cref="Destination"/>
        /// and its associated images. The foreign key is defined on the <c>EntityId</c> property of the related
        /// <c>Image</c> entity, and the delete behavior is set to cascade.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{Destination}"/> used to configure the <see cref="Destination"/> entity.</param>
        public void Configure(EntityTypeBuilder<Destination> builder)
        {
            builder.HasMany(c => c.Images).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Lodgings).WithOne(c => c.Destination).HasForeignKey(c => c.DestinationId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
