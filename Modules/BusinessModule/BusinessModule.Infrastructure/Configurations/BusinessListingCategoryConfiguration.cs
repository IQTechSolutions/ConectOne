using BusinessModule.Domain.Entities;
using GroupingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessModule.Infrastructure.Configurations
{
    /// <summary>
    /// Configures the entity mapping for the Category of BusinessListing in the Entity Framework model.
    /// </summary>
    /// <remarks>This configuration defines the relationships between Category<BusinessListing> entities,
    /// including subcategory and parent category associations, as well as the collection of business listings within
    /// each category. Use this class when customizing the Entity Framework model for business listing
    /// categories.</remarks>
    public class BusinessListingCategoryConfiguration : IEntityTypeConfiguration<Category<BusinessListing>>
    {
        /// <summary>
        /// Configures the entity type for the Category of BusinessListing, including its relationships and foreign key
        /// constraints.
        /// </summary>
        /// <remarks>This method sets up the one-to-many relationships between Category and its
        /// SubCategories, as well as between Category and its associated BusinessListing entities. It also configures
        /// cascade delete behavior for the EntityCollection relationship.</remarks>
        /// <param name="builder">The builder used to configure the Category entity type.</param>
        public void Configure(EntityTypeBuilder<Category<BusinessListing>> builder)
        {
            builder.HasMany(c => c.SubCategories).WithOne(c => c.ParentCategory).HasForeignKey(c => c.ParentCategoryId);
            builder.HasMany(c => c.EntityCollection).WithOne(c => c.Category).HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
