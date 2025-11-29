using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace ProductsModule.Domain.Entities
{
    /// <summary>
    /// Represents the association between a service tier and an offered service.
    /// </summary>
    /// <remarks>This class defines the relationship between a specific <see cref="ServiceTier"/> and an <see
    /// cref="OfferedService"/>. It includes foreign key properties to link to the respective entities.</remarks>
    public class TierService : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the identifier of the associated service tier.
        /// </summary>
        [ForeignKey(nameof(ServiceTier))] public string ServiceTierId { get; set; }

        /// <summary>
        /// Gets or sets the service tier for the application.
        /// </summary>
        public ServiceTier ServiceTier { get; set; }
        
        /// <summary>
        /// Gets or sets the identifier of the offered service.
        /// </summary>
        [ForeignKey(nameof(OfferedService))] public string OfferedServiceId { get; set; }

        /// <summary>
        /// Gets or sets the service being offered.
        /// </summary>
        public OfferedService OfferedService { get; set; }
    }
}
