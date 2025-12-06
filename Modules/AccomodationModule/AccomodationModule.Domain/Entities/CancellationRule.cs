using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a cancellation rule entity, including details such as days before booking, formula type, value, and associated lodging.
    /// Inherits from <see cref="EntityBase{TId}"/> to include common entity properties.
    /// </summary>
    public class CancellationRule : EntityBase<int>
    {

        #region Properties

        /// <summary>
        /// Gets or sets the number of days before booking that cancellation is available.
        /// </summary>
        public int DaysBeforeBookingThatCancellationIsAvailable { get; set; } = 1;

        /// <summary>
        /// Gets or sets the type of the cancellation formula.
        /// </summary>
        public string CancellationFormualaType { get; set; }

        /// <summary>
        /// Gets or sets the value of the cancellation formula.
        /// </summary>
        public double CancellationFormualaValue { get; set; } = 1;

        /// <summary>
        /// Gets the description of the cancellation rule based on its type and value.
        /// </summary>
        public string Description => CancellationFormualaType.GetCancellationRuleAbbreviation(CancellationFormualaValue);

        /// <summary>
        /// Gets or sets the ID of the lodging associated with the cancellation rule.
        /// </summary>
        [ForeignKey(nameof(Lodging))]
        public string LodgingId { get; set; }

        /// <summary>
        /// Gets or sets the lodging associated with the cancellation rule.
        /// </summary>
        public Lodging Lodging { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the cancellation rule.
        /// </summary>
        /// <returns>A string representing the cancellation rule.</returns>
        public override string ToString()
        {
            return $"Cancellation Rule";
        }

        #endregion
    }

    /// <summary>
    /// Extension methods for the <see cref="CancellationRule"/> class.
    /// </summary>
    public static class CancellationRuleExtensions
    {
        /// <summary>
        /// Gets the abbreviation for the cancellation rule type based on its value.
        /// </summary>
        /// <param name="cancellationFormualaType">The type of the cancellation formula.</param>
        /// <param name="cancellationFormulaValue">The value of the cancellation formula.</param>
        /// <returns>A string representing the abbreviation of the cancellation rule.</returns>
        public static string GetCancellationRuleAbbreviation(this string cancellationFormualaType, double cancellationFormulaValue)
        {
            return cancellationFormualaType switch
            {
                "P" => $"{cancellationFormulaValue}% of total charge",
                "D" => $"{cancellationFormulaValue}% of your deposit",
                "N" => $"amount equal to {cancellationFormulaValue} nights accommodation",
                "A" => cancellationFormulaValue.ToString("C") + " (fixed amount)",
                _ => string.Empty,
            };
        }
    }

}
