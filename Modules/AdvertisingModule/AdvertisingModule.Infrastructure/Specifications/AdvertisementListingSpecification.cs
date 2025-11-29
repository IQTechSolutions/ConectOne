using AdvertisingModule.Domain.Entities;
using AdvertisingModule.Domain.RequestFeatures;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingModule.Infrastructure.Specifications
{
    /// <summary>
    /// Represents a specification for filtering and including related data when querying advertisements.
    /// </summary>
    /// <remarks>This specification is designed to construct query criteria and include related entities based
    /// on the  parameters provided in an <see cref="AdvertisementListingPageParameters"/> instance. It supports
    /// filtering  advertisements by status and user ID, and includes advertisement images and their associated image
    /// data.</remarks>
    public sealed class AdvertisementListingSpecification : Specification<Advertisement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvertisementListingSpecification"/> class with the specified
        /// page parameters to define filtering and inclusion criteria for advertisement listings.
        /// </summary>
        /// <remarks>The constructor builds a specification for querying advertisements based on the
        /// provided parameters. If <paramref name="p.Status"/> is specified, the query will filter advertisements by
        /// their status. If <paramref name="p.UserId"/> is specified, the query will filter advertisements by the
        /// associated user ID. Additionally, the query includes related advertisement images and their associated image
        /// data.</remarks>
        /// <param name="p">The parameters used to filter and include data in the advertisement listing query. This includes optional
        /// criteria such as advertisement status and user ID.</param>
        public AdvertisementListingSpecification(AdvertisementListingPageParameters p)
        {
            Criteria = PredicateBuilder.New<Advertisement>(true);

            if(p.Status is not null)
                Criteria = Criteria.And(c => c.Status == p.Status);

            if (p.UserId is not null)
                Criteria = Criteria.And(c => c.UserId == p.UserId);

            AddInclude(s => s.Include(v => v.AdvertisementTier));
            AddInclude(s => s.Include(v => v.Images).ThenInclude(i => i.Image));
        }
    }
}
