using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="DayTourActivityTemplate"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration specifies the primary key for the <see cref="DayTourActivityTemplate"/>
    /// entity.</remarks>
    public class DayTourActivityTemplateConfiguration : IEntityTypeConfiguration<DayTourActivityTemplate>
    {
        /// <summary>
        /// Configures the entity type for <see cref="DayTourActivityTemplate"/>.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{DayTourActivityTemplate}"/> used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<DayTourActivityTemplate> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
