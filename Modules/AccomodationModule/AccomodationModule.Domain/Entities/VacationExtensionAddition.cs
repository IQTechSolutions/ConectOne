using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents an extension to a vacation, allowing for additional services, pricing, and lodging details.
    /// Inherits image file collection functionality from the base class.
    /// </summary>
    public class VacationExtensionAddition : EntityBase<string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the foreign key reference to the associated vacation.
        /// Can be null if not linked to a specific vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? ParentVacationId { get; set; }

        /// <summary>
        /// Navigation property to the associated vacation.
        /// </summary>
        public Vacation? ParentVacation { get; set; }

        /// <summary>
        /// Gets or sets the foreign key reference to the associated vacation extension.
        /// Can be null if not linked to a specific vacation extension.
        /// </summary>
        [ForeignKey(nameof(Extension))] public string? ExtensioId { get; set; }

        /// <summary>
        /// Navigation property to the associated vacation extension.
        /// </summary>
        public Vacation? Extension { get; set; }

        #endregion
    }
}
