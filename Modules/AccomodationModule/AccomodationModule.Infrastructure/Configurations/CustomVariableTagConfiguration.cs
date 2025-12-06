using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="CustomVariableTag"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This class defines the relationships and foreign key constraints for the <see
    /// cref="Airport"/> entity. It is used to configure the entity within the database context.</remarks>
    public class CustomVariableTagConfiguration : IEntityTypeConfiguration<CustomVariableTag>
    {
        /// <summary>
        /// Configures the entity type <see cref="Booking"/> and its relationships.
        /// </summary>
        /// <remarks>This method defines the relationships between the <see cref="CustomVariableTag"/> entity and other entities</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the <see cref="CustomVariableTag"/> entity.</param>
        public void Configure(EntityTypeBuilder<CustomVariableTag> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsUnicode();
        }
    }
}
