using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a rule defining child policy constraints and behaviors for a specific room.
    /// </summary>
    /// <remarks>This class encapsulates the age range, allowance status, special rate usage, and
    /// formula-based calculations for child policies. It is typically used to define and enforce rules regarding
    /// children in accommodations or similar contexts.</remarks>
    public class ChildPolicyRule : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the minimum age required for eligibility.
        /// </summary>
        public int MinAge { get; set; }

        /// <summary>
        /// Gets or sets the maximum age, in years, allowed for a specific operation or entity.
        /// </summary>
        public int MaxAge { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation is allowed.
        /// </summary>
        public bool Allowed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a special rate should be applied.
        /// </summary>
        public bool UseSpecialRate { get; set; }

        /// <summary>
        /// Gets or sets the type of formula used for child policy calculations.
        /// </summary>
        public string? ChildPolicyFormualaType { get; set; } = "N";

        /// <summary>
        /// Gets or sets the value used in the child policy formula calculation.
        /// </summary>
        public double ChildPolicyFormualaValue { get; set; } = 1;

        /// <summary>
        /// Gets the description of the child policy formula based on its type, value, and age range.
        /// </summary>
        public string Description { get { return ChildPolicyFormualaType.GetChildPolicyAbbreviation(ChildPolicyFormualaValue, MinAge, MaxAge); } }

        /// <summary>
        /// Gets or sets a custom description associated with the object.
        /// </summary>
        public string? CustomDescription { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated room.
        /// </summary>
        [ForeignKey(nameof(Room))] public int? RoomId { get; set; }

        /// <summary>
        /// Gets or sets the room associated with the current context.
        /// </summary>
        public Room? Room { get; set; }

        /// <summary>
        /// Returns a string representation of the current child policy rule.
        /// </summary>
        /// <returns>A string that represents the child policy rule.</returns>
        public override string ToString()
        {
            return $"Child Policy Rule";
        }
    }

    /// <summary>
    /// Generates a child policy abbreviation based on the specified cancellation formula type, value, and age range.
    /// </summary>
    public static class ChildPolicyRuleExtensions
    {
        /// <summary>
        /// Generates a descriptive abbreviation for a child policy based on the specified cancellation formula type,
        /// value, and age range.
        /// </summary>
        /// <param name="cancellationFormualaType">The type of cancellation formula. Valid values are: <list type="bullet">
        /// <item><term>"N"</term><description>Indicates that the age range is not allowed.</description></item>
        /// <item><term>"P"</term><description>Indicates a percentage-based charge.</description></item>
        /// <item><term>"R"</term><description>Indicates a fixed monetary amount.</description></item> </list></param>
        /// <param name="cancellationFormulaValue">The value associated with the cancellation formula. Represents a percentage for type "P" or a fixed monetary
        /// amount for type "R".</param>
        /// <param name="minAge">The minimum age in the age range for the policy.</param>
        /// <param name="maxAge">The maximum age in the age range for the policy.</param>
        /// <returns>A string representing the child policy abbreviation. The format depends on the 
        /// name="cancellationFormualaType">: <list type="bullet"> <item><term>"N"</term><description>Returns a string
        /// in the format "<paramref name="minAge"/> - <paramref name="maxAge"/> is not allowed".</description></item>
        /// <item><term>"P"</term><description>Returns a string in the format "<paramref
        /// name="cancellationFormulaValue"/>% of total charge".</description></item>
        /// <item><term>"R"</term><description>Returns a string in the format "<paramref
        /// name="cancellationFormulaValue"/> (fixed amount)".</description></item> <item><term>Other
        /// values</term><description>Returns an empty string.</description></item> </list></returns>
        public static string GetChildPolicyAbbreviation(this string? cancellationFormualaType, double cancellationFormulaValue, int minAge, int maxAge)
        {
			return cancellationFormualaType switch
			{
				"N" => $"{minAge} - {maxAge} is not allowed",
				"P" => $"{cancellationFormulaValue}% of total charge",
				"R" => cancellationFormulaValue.ToString("C") + "(fixed ammount)",
				_ => string.Empty,
			};
		}
    }
    
}
