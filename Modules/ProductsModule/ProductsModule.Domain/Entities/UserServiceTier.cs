using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;
using IdentityModule.Domain.Entities;

namespace ProductsModule.Domain.Entities
{
    /// <summary>
    /// Represents the association between a user and a service tier.
    /// </summary>
    /// <remarks>This class defines the relationship between an <see cref="ApplicationUser"/> and a <see
    /// cref="ServiceTier"/>. Each instance associates a specific user with a specific service tier.</remarks>
    public class UserServiceTier : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the associated application user.
        /// </summary>
        [ForeignKey(nameof(ApplicationUser))]public string ApplicationUserId { get; set; }

        /// <summary>
        /// Gets or sets the application user associated with the current context.
        /// </summary>
        public ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the service tier associated with the application user.
        /// </summary>
        [ForeignKey(nameof(ApplicationUser))]public string ServiceTierId { get; set; }

        /// <summary>
        /// Gets or sets the service tier for the application.
        /// </summary>
        public ServiceTier ServiceTier { get; set; }
    }
}
