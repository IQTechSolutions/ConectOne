using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for vacation cancellation rules.
    /// Represents the rules and policies for canceling a vacation.
    /// </summary>
    public record VacationCancellationRuleDto
    {
        #region Constructors

        /// <summary>
        /// Default constructor for creating a new instance of <see cref="VacationCancellationRuleDto"/>.
        /// </summary>
        public VacationCancellationRuleDto() { }

        /// <summary>
        /// Initializes a new instance of <see cref="VacationCancellationRuleDto"/> using a <see cref="VacationCancellationRule"/> entity.
        /// </summary>
        /// <param name="rule">The entity containing vacation cancellation rule details.</param>
        public VacationCancellationRuleDto(VacationCancellationRule rule)
        {
            Id = rule.Id;
            CancellationText = rule.RuleText;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or initializes the ID of the vacation cancellation rule.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Gets or initializes the cancellation text of the vacation cancellation rule.
        /// </summary>
        public string CancellationText { get; init; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts this DTO into a <see cref="VacationCancellationRule"/> entity for persistence in the database.
        /// </summary>
        /// <returns>A <see cref="VacationCancellationRule"/> entity with the DTO's data.</returns>
        public VacationCancellationRule ToVacationCancellationRule()
        {
            return new VacationCancellationRule()
            {
                RuleText = CancellationText
            };
        }

        #endregion
    }
}
