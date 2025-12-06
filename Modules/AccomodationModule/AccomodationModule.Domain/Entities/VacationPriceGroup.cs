using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a grouping of vacation-related pricing information.
    /// </summary>
    /// <remarks>This class associates a name with a specific vacation and serves as a container for
    /// pricing-related data. It includes a reference to the associated vacation entity.</remarks>
    public class VacationPriceGroup : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the name associated with the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? VacationId { get; set; }

        /// <summary>
        /// Gets or sets the vacation details for the current user or entity.
        /// </summary>
        public Vacation? Vacation { get; set; }
    }
}
