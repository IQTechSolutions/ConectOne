using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;
using System.ComponentModel;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for representing payment rules in the application.
    /// This ViewModel is used to transfer payment rule data between the UI and the backend.
    /// </summary>
    public class PaymentRuleViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRuleViewModel"/> class.
        /// </summary>
        public PaymentRuleViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRuleViewModel"/> class with the specified <see cref="PaymentRuleDto"/>.
        /// </summary>
        /// <param name="rule">The payment rule DTO to initialize the ViewModel with.</param>
        public PaymentRuleViewModel(PaymentRuleDto rule)
        {
            PaymentRuleId = rule.PaymentRuleId;
            MonthsBeforeVacationStart = rule.MonthsBeforeVacationStart;
            RuleType = rule.RuleType;
            PaymentType = rule.PaymentType;
            Value = rule.Value;
            VacationId = rule.VacationId;
            VacationExtensionId = rule.VacationExtensionId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the payment rule.
        /// </summary>
        public string PaymentRuleId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the number of months before a specified event or date.
        /// </summary>
        [DisplayName("Months Before Vacation Starts")] public int MonthsBeforeVacationStart { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment rule.
        /// </summary>
        public PaymentRuleType RuleType { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment rule.
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// Gets or sets the value of the payment rule.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the ID of the booking form associated with the payment rule.
        /// </summary>
        public string VacationId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the vacation extension associated with the payment rule.
        /// </summary>
        public string VacationExtensionId { get; set; }

        /// <summary>
        /// Gets the description of the payment rule based on its type and value.
        /// </summary>
        public string Description => RuleType.GetPaymentRuleAbbreviation(Value);

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current <see cref="PaymentRule"/> instance to a <see cref="PaymentRuleDto"/>.
        /// </summary>
        /// <returns>A <see cref="PaymentRuleDto"/> object that represents the current payment rule,  including its identifier,
        /// timing, type, value, and associated vacation details.</returns>
        public PaymentRuleDto ToDto()
        {
            return new PaymentRuleDto()
            {
                PaymentRuleId = PaymentRuleId,
                MonthsBeforeVacationStart = MonthsBeforeVacationStart,
                RuleType = RuleType,
                PaymentType = PaymentType,
                Value = Value,
                VacationId = VacationId,
                VacationExtensionId = VacationExtensionId
            };
        }

        #endregion
    }
}
