using BusinessModule.Domain.Entities;
using ConectOne.Domain.Enums;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace BusinessModule.Infrastructure.Specifications
{
    /// <summary>
    /// Represents a specification for retrieving detailed business listings, including related entities such as images,
    /// categories, products, and services.
    /// </summary>
    /// <remarks>This specification is designed to query business listings that meet specific criteria, such
    /// as their approval status,  and to include related entities necessary for a detailed view. The related entities
    /// include images, categories, products,  and services, along with their associated images. This ensures that all
    /// relevant data is efficiently loaded for a comprehensive  business listing.</remarks>
    public sealed class DetailedBusinessListingSpecification : Specification<BusinessListing>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetailedBusinessListingSpecification"/> class, specifying the
        /// criteria and related entities to include for a detailed business listing query.
        /// </summary>
        /// <remarks>This constructor sets up the specification to filter business listings based on their
        /// approval status and includes related entities such as images, categories, products, and services, along with
        /// their associated images. This ensures that all necessary data for a detailed business listing is loaded
        /// efficiently.</remarks>
        public DetailedBusinessListingSpecification()
        {
            Criteria = PredicateBuilder.New<BusinessListing>(c => c.Status == ReviewStatus.Approved);

            AddInclude(s => s.Include(v => v.ListingTier));
            AddInclude(s => s.Include(v => v.Images).ThenInclude(i => i.Image));
            AddInclude(i => i.Include(c => c.Categories).ThenInclude(c => c.Category));
            AddInclude(l => l.Include(c => c.Products).ThenInclude(c => c.Images).ThenInclude(c => c.Image));
            AddInclude(l => l.Include(c => c.Services).ThenInclude(c => c.Images).ThenInclude(c => c.Image));
        }
    }
}
