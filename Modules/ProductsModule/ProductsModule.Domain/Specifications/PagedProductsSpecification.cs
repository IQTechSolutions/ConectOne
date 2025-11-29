using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using ProductsModule.Domain.Entities;
using ProductsModule.Domain.RequestFeatures;

namespace ProductsModule.Domain.Specifications
{
    /// <summary>
    /// Specification for paginated product queries with basic filters.
    /// </summary>
    public sealed class PagedProductsSpecification : Specification<Product>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedProductsSpecification"/> class with the specified product
        /// parameters to filter and include related data.
        /// </summary>
        /// <remarks>This specification applies a filter based on the <see
        /// cref="ProductsParameters.Active"/> property and includes related data such as pricing and images in the
        /// query.</remarks>
        /// <param name="parameters">The parameters used to configure the specification, including filters for active products and other
        /// criteria.</param>
        public PagedProductsSpecification(ProductsParameters parameters)
        {
            Criteria = p => true;

            if (parameters.Active)
                Criteria = Criteria.And(p => p.Active);
            else
                Criteria = Criteria.And(p => !p.Active);

            AddInclude(q => q.Include(p => p.Pricing));
            AddInclude(q => q.Include(p => p.Images));
        }
    }
}
