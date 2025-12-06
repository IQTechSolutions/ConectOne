using AccomodationModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccomodationModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity type <see cref="MeetAndGreetTemplate"/> for use with Entity Framework Core.
    /// </summary>
    /// <remarks>This configuration specifies the primary key for the <see cref="MeetAndGreetTemplate"/>
    /// entity. Use this class to define additional entity configurations as needed.</remarks>
    public class MeetAndGreetTemplateConfiguration : IEntityTypeConfiguration<MeetAndGreetTemplate>
    {
        /// <summary>
        /// Configures the entity type for <see cref="MeetAndGreetTemplate"/>.
        /// </summary>
        /// <param name="builder">The <see cref="EntityTypeBuilder{MeetAndGreetTemplate}"/> used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<MeetAndGreetTemplate> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
