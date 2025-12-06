using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Vacation"/> and its relationships for the database schema.
    /// </summary>
    /// <remarks>This configuration defines various relationships and constraints for the <see
    /// cref="Vacation"/> entity,  including indexes, foreign keys, and cascading behaviors. It ensures proper mapping
    /// of the entity's  properties and navigation properties to the database.</remarks>
    public class VacationConfiguration : IEntityTypeConfiguration<Vacation>
    {
        /// <summary>
        /// Configures the entity type <see cref="Vacation"/> and its relationships using the provided <see
        /// cref="EntityTypeBuilder{TEntity}"/>.
        /// </summary>
        /// <remarks>This method defines the following configurations for the <see cref="Vacation"/>
        /// entity: <list type="bullet"> <item> <description>Indexes: Configures a unique index on the <see
        /// cref="Vacation.Name"/> property.</description> </item> <item> <description>Relationships: Configures
        /// multiple one-to-many and many-to-one relationships, including foreign key constraints and delete
        /// behaviors.</description> </item> <item> <description>Collections: Configures cascading delete behavior for
        /// related collections such as <see cref="Vacation.Prices"/>, <see cref="Vacation.Images"/>, and
        /// others.</description> </item> </list></remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the <see cref="Vacation"/> entity.</param>
        public void Configure(EntityTypeBuilder<Vacation> builder)
        {
            builder.HasIndex(c => c.Slug).IsUnique();

            builder.HasOne(c => c.VacationHost).WithMany(c => c.Vacations).HasForeignKey(c => c.VacationHostId).OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Prices).WithOne(c => c.Vacation).HasForeignKey(c => c.VacationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Images).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Intervals).WithOne(c => c.Vacation).HasForeignKey(c => c.VacationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Flights).WithOne(c => c.Vacation).HasForeignKey(c => c.VacationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.GolferPackages).WithOne(c => c.Vacation).HasForeignKey(c => c.VacationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.DayTourActivities).WithOne(c => c.Vacation).HasForeignKey(c => c.VacationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.MealAdditions).WithOne(c => c.Vacation).HasForeignKey(c => c.VacationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.PaymentRules).WithOne(c => c.Vacation).HasForeignKey(c => c.VacationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.VacationInclusionDisplayTypeInfos).WithOne(c => c.Vacation).HasForeignKey(c => c.VacationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Gifts).WithOne(c => c.Vacation).HasForeignKey(c => c.VacationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.VacationPriceGroups).WithOne(c => c.Vacation).HasForeignKey(c => c.VacationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.ItineraryEntryItemTemplates).WithOne(c => c.Vacation).HasForeignKey(c => c.VacationId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
