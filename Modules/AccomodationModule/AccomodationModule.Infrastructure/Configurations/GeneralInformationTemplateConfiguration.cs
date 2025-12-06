using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="GeneralInformationTemplate"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration defines the primary key for the <see cref="GeneralInformationTemplate"/>
    /// entity. Use this class to apply additional configurations, such as relationships, constraints, or
    /// indexes.</remarks>
    public class GeneralInformationTemplateConfiguration : IEntityTypeConfiguration<GeneralInformationTemplate>
    {
        /// <summary>
        /// Configures the entity type <see cref="GeneralInformationTemplate"/>.
        /// </summary>
        /// <remarks>This method sets up the primary key for the <see cref="GeneralInformationTemplate"/>
        /// entity. Additional configuration for the entity can be added using the provided <paramref
        /// name="builder"/>.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{GeneralInformationTemplate}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<GeneralInformationTemplate> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
