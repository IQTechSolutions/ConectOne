using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="PaymentExclusionTemplate"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This class is used to define the database schema and relationships for the <see
    /// cref="PaymentExclusionTemplate"/> entity. It implements <see cref="IEntityTypeConfiguration{TEntity}"/> to provide
    /// configuration logic for Entity Framework Core during model creation.</remarks>
    public class PaymentExclusionTemplateConfiguration : IEntityTypeConfiguration<PaymentExclusionTemplate>
    {
        /// <summary>
        /// Configures the entity type for <see cref="PaymentExclusionTemplate"/>.
        /// </summary>
        /// <remarks>This method is typically called by the Entity Framework during model configuration to
        /// define the entity's key and other properties.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{PaymentTermsTemplate}"/> instance used to configure the <see cref="PaymentTermsTemplate"/>
        /// entity.</param>
        public void Configure(EntityTypeBuilder<PaymentExclusionTemplate> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
