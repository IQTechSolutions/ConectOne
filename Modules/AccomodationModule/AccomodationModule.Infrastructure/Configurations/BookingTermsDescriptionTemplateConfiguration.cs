using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="BookingTerm"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This class is used to define the database schema and relationships for the <see
    /// cref="BookingTerm"/> entity. It implements <see cref="IEntityTypeConfiguration{TEntity}"/> to provide
    /// configuration logic for Entity Framework Core during model creation.</remarks>
    public class BookingTermsDescriptionTemplateConfiguration : IEntityTypeConfiguration<BookingTermsDescriptionTemplate>
    {
        /// <summary>
        /// Configures the entity type for <see cref="BookingTermsDescriptionTemplate"/>.
        /// </summary>
        /// <remarks>This method is typically called by the Entity Framework during model configuration to
        /// define the entity's key and other properties.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{BookingTerm}"/> instance used to configure the <see cref="BookingTermsDescriptionTemplate"/>
        /// entity.</param>
        public void Configure(EntityTypeBuilder<BookingTermsDescriptionTemplate> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
