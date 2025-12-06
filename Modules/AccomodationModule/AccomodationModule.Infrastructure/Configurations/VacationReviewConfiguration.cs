using AccomodationModule.Domain.Entities;
using FeedbackModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type for <see cref="Review{TEntity}"/> in the database model.
    /// </summary>
    /// <remarks>This configuration defines relationships and constraints for the <see
    /// cref="Review{Vacation}"/> entity. It establishes a one-to-many relationship between <see cref="Vacation"/> and
    /// its reviews, as well as a one-to-many relationship between <see cref="Review{Vacation}"/> and its associated
    /// images.</remarks>
    public class VacationReviewConfiguration : IEntityTypeConfiguration<Review<Vacation>>
    {
        /// <summary>
        /// Configures the entity type for <see cref="Review{Vacation}"/>.
        /// </summary>
        /// <remarks>This method establishes relationships between the <see cref="Review{Vacation}"/>
        /// entity and other entities. It configures a one-to-many relationship between <see cref="Review{Vacation}"/>
        /// and <see cref="Vacation"/>,  and a one-to-many relationship between <see cref="Review{Vacation}"/> and its
        /// associated images. The cascade delete behavior is applied to the images.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<Review<Vacation>> builder)
        {
             builder.HasMany(c => c.Images).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
