using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="Contact"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This class is used to define the schema and relationships for the <see cref="Contact"/>
    /// entity. It implements <see cref="IEntityTypeConfiguration{Contact}"/> to provide configuration logic for the
    /// <see cref="Contact"/> entity when building the model.</remarks>
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        /// <summary>
        /// Configures the <see cref="Contact"/> entity type for use with Entity Framework Core.
        /// </summary>
        /// <remarks>This method sets the primary key for the <see cref="Contact"/> entity to the
        /// <c>Id</c> property.</remarks>
        /// <param name="builder">An <see cref="EntityTypeBuilder{Contact}"/> instance used to configure the <see cref="Contact"/> entity.</param>
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasMany(c => c.Images).WithOne(c => c.Entity).HasForeignKey(c => c.EntityId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
