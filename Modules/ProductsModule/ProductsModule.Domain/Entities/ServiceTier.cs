using System.ComponentModel.DataAnnotations.Schema;
using FilingModule.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace ProductsModule.Domain.Entities
{
    /// <summary>
    /// Represents a service tier, which defines a specific level of service with associated properties such as name,
    /// price, and role.
    /// </summary>
    /// <remarks>A service tier is used to categorize and manage different levels of service offerings. It
    /// includes properties for  identifying the tier (e.g., <see cref="Name"/> and <see cref="Description"/>), pricing
    /// information (<see cref="Price"/>),  and its relationship to roles (<see cref="Role"/> and <see cref="RoleId"/>).
    /// The tier can also be marked as active  (<see cref="Active"/>) or default (<see cref="Default"/>), and its order
    /// of processing or display can be specified  using the <see cref="Order"/> property.</remarks>
    public class ServiceTier : FileCollection<ServiceTier, string>
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the object.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the item.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the order in which this item is processed or displayed.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the default configuration is enabled.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated role.
        /// </summary>
        [ForeignKey(nameof(Role))] public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the role associated with the application.
        /// </summary>
        public ApplicationRole Role { get; set; }

        /// <summary>
        /// Gets or sets the collection of tier services associated with the current entity.
        /// </summary>
        public virtual ICollection<TierService> TierServices { get; set; } = [];
    }
}
