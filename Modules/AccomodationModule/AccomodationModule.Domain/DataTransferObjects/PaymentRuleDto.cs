using System.ComponentModel;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing payment rules.
    /// This DTO is used to transfer payment rule data between different layers of the application.
    /// </summary>
    public record PaymentRuleDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRuleDto"/> class.
        /// </summary>
        public PaymentRuleDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRuleDto"/> class with the specified <see cref="PaymentRule"/>.
        /// </summary>
        /// <param name="rule">The entity retrieved from the database</param>
        public PaymentRuleDto(PaymentRule rule)
        {
            PaymentRuleId = rule.Id;
            MonthsBeforeVacationStart = rule.MonthsBeforeVacationStart;
            RuleType = rule.RuleType;
            PaymentType = rule.PaymentType;
            Value = rule.Value;
            VacationId = rule.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID of the payment rule.
        /// </summary>
        public string PaymentRuleId { get; init; } = null!;

        /// <summary>
        /// Gets or sets the number of months before a specified event or date.
        /// </summary>
        [DisplayName("Months Before Vacation Starts")] public int MonthsBeforeVacationStart { get; init; }

        /// <summary>
        /// Gets the type of the payment rule.
        /// </summary>
        public PaymentRuleType RuleType { get; init; }

        /// <summary>
        /// Gets the type of the payment type.
        /// </summary>
        public PaymentType PaymentType { get; init; }

        /// <summary>
        /// Gets the value of the payment rule.
        /// </summary>
        public double Value { get; init; } = 1;

        /// <summary>
        /// Gets the ID of the vacation associated with the payment rule.
        /// </summary>
        public string? VacationId { get; init; }

        /// <summary>
        /// Gets the ID of the vacation extension associated with the payment rule.
        /// </summary>
        public string? VacationExtensionId { get; init; }

        /// <summary>
        /// Gets the description of the payment rule based on its type and value.
        /// </summary>
        public string Description => RuleType.GetPaymentRuleAbbreviation(Value);

        #endregion

        /// <summary>
        /// Creates a new payment rule
        /// </summary>
        /// <returns>The new payment rule</returns>
        public PaymentRule ToPaymentRule()
        {
            return new PaymentRule()
            {
                Id = PaymentRuleId,
                MonthsBeforeVacationStart = MonthsBeforeVacationStart,
                RuleType = RuleType,
                PaymentType = PaymentType,
                Value = Value,
                VacationId = VacationId
            };
        }

        /// <summary>
        /// Updates an existing payment rule
        /// </summary>
        /// <param name="paymentRule">The rule to be updated</param>
        public void UpdatePaymentRuleValues(in PaymentRule paymentRule)
        {
            paymentRule.MonthsBeforeVacationStart = MonthsBeforeVacationStart;
            paymentRule.RuleType = RuleType;
            paymentRule.PaymentType = PaymentType;
            paymentRule.Value = Value;
        }
    }
}
