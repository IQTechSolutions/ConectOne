using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="MealAdditionTemplate"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration specifies the primary key for the <see cref="MealAdditionTemplate"/>
    /// entity.</remarks>
    public class MealAdditionTemplateConfiguration : IEntityTypeConfiguration<MealAdditionTemplate>
    {
        /// <summary>
        /// Configures the entity type <see cref="MealAdditionTemplate"/> for the database context.
        /// </summary>
        /// <param name="builder">An <see cref="EntityTypeBuilder{MealAdditionTemplate}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<MealAdditionTemplate> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
