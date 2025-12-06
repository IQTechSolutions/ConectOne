using System.ComponentModel.DataAnnotations.Schema;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents a payment rule entity, including details such as rule date, type, value, and associated booking form.
    /// Inherits from <see cref="EntityBase{TId}"/> to include common entity properties.
    /// </summary>
    public class PaymentRule : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRule"/> class.
        /// </summary>
        public PaymentRule() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRule"/> class with the specified <see cref="PaymentRule"/>.
        /// </summary>
        /// <param name="rule">The payment rule entity to initialize the new instance with.</param>
        public PaymentRule(PaymentRule rule)
        {
            MonthsBeforeVacationStart = rule.MonthsBeforeVacationStart;
            RuleType = rule.RuleType;
            PaymentType = rule.PaymentType;
            Value = rule.Value;
            VacationId = rule.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the number of months before a specified event or date.
        /// </summary>
        public int MonthsBeforeVacationStart { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment rule type.
        /// </summary>
        public PaymentRuleType RuleType { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment rule.
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// Gets or sets the value of the payment rule.
        /// </summary>
        public double Value { get; set; } = 1;

        /// <summary>
        /// Gets the description of the payment rule based on its type and value.
        /// </summary>
        public string Description => RuleType.GetPaymentRuleAbbreviation(Value);

        #endregion

        #region One To Many Relationship

        /// <summary>
        /// Gets or sets the ID of the Vacation associated with the payment rule.
        /// </summary>
        [ForeignKey(nameof(Vacation))]
        public string? VacationId { get; init; }

        /// <summary>
        /// Gets or sets the Vacation associated with the payment rule.
        /// </summary>
        public Vacation? Vacation { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a new instance of the <see cref="PaymentRule"/> class that is a copy of the specified rule.
        /// </summary>
        /// <param name="rule">The <see cref="PaymentRule"/> instance to clone.</param>
        /// <returns>A new <see cref="PaymentRule"/> object that is a copy of the specified rule, with a unique identifier.</returns>
        public PaymentRule Clone()
        {
            return new PaymentRule(this) { Id = Guid.NewGuid().ToString() };
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Returns a string representation of the payment rule.
        /// </summary>
        /// <returns>A string representing the payment rule.</returns>
        public override string ToString()
        {
            return $"Booking Payment Rule";
        }

        #endregion
    }

    /// <summary>
    /// Extension methods for the <see cref="PaymentRuleType"/> enum.
    /// </summary>
    public static class PaymentRuleExtensions
    {
        /// <summary>
        /// Gets the abbreviation for the payment rule type based on its value.
        /// </summary>
        /// <param name="paymentRuleType">The type of the payment rule.</param>
        /// <param name="paymentRuleValue">The value of the payment rule.</param>
        /// <returns>A string representing the abbreviation of the payment rule.</returns>
        public static string GetPaymentRuleAbbreviation(this PaymentRuleType paymentRuleType, double paymentRuleValue)
        {
            return paymentRuleType switch
            {
                PaymentRuleType.PercentageOfTotalCharge => $"{paymentRuleValue}% of total charge",
                PaymentRuleType.PercentageOfDeposit => $"{paymentRuleValue}% of your deposit",
                PaymentRuleType.FixedAmount => paymentRuleValue.ToString("C") + " (fixed amount)",
                _ => string.Empty,
            };
        }
    }
}
