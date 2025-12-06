using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="VacationTitleTemplate"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration specifies the primary key for the <see cref="VacationTitleTemplate"/>
    /// entity. Use this class to define additional entity configurations as needed.</remarks>
    public class VacationTitleTemplateConfiguration : IEntityTypeConfiguration<VacationTitleTemplate>
    {
        /// <summary>
        /// Configures the entity type for <see cref="VacationTitleTemplate"/>.
        /// </summary>
        /// <remarks>This method sets up the primary key for the <see cref="VacationTitleTemplate"/>
        /// entity.</remarks>
        /// <param name="builder">The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the <see cref="VacationTitleTemplate"/>
        /// entity.</param>
        public void Configure(EntityTypeBuilder<VacationTitleTemplate> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
