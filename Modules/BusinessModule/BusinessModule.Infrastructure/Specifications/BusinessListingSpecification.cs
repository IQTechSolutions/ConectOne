using BusinessModule.Domain.Entities;
using BusinessModule.Domain.RequestFeatures;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace BusinessModule.Infrastructure.Specifications
{
    /// <summary>
    /// Represents a specification for querying and filtering <see cref="BusinessListing"/> entities, including support
    /// for related data loading.
    /// </summary>
    /// <remarks>This specification is designed to filter <see cref="BusinessListing"/> entities based on the
    /// provided parameters, such as approval status and associated user ID. It also configures eager loading for
    /// related entities, including images and categories, to ensure that necessary data is included in the query
    /// results.</remarks>
    public sealed class BusinessListingSpecification : Specification<BusinessListing>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessListingSpecification"/> class with the specified
        /// parameters for filtering and including related data.
        /// </summary>
        /// <remarks>This constructor sets up the filtering criteria based on the provided parameters,
        /// such as  whether the business listing is approved and the associated user ID. It also configures  the
        /// necessary includes for related entities, such as images and categories, to ensure  they are eagerly
        /// loaded.</remarks>
        /// <param name="p">The parameters used to define the filtering criteria and includes for the business listings.</param>
        public BusinessListingSpecification(BusinessListingPageParameters p)
        {
            Criteria = PredicateBuilder.New<BusinessListing>(true);

            if(p.Status is not null)
                Criteria = Criteria.And(c => c.Status == p.Status);

            if (p.UserId is not null)
                Criteria = Criteria.And(c => c.UserId == p.UserId);

            AddInclude(s => s.Include(v => v.Images).ThenInclude(i => i.Image));
            AddInclude(s => s.Include(v => v.ListingTier));
            AddInclude(i => i.Include(c => c.Categories).ThenInclude(c => c.Category));
        }
    }
}
