using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a vacation cancellation rule, including the rule text and associated vacation.
    /// </summary>
    public class VacationCancellationRule : EntityBase<string>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the text of the cancellation rule.
        /// </summary>
        public string RuleText { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? VacationId { get; set; }

        /// <summary>
        /// Navigation property to the associated vacation.
        /// </summary>
        public Vacation? Vacation { get; set; }

        #endregion
    }
}