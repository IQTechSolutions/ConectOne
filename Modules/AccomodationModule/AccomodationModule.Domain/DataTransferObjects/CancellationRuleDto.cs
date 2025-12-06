using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents a data transfer object (DTO) for cancellation rules associated with a lodging.
    /// </summary>
    /// <remarks>This class is used to encapsulate cancellation rule data for transfer between different
    /// layers of the application. It provides properties to define the cancellation policy, including the number of
    /// days before booking when cancellation is allowed, the formula type and value for calculating cancellation fees,
    /// and the associated lodging identifier.</remarks>
    public class CancellationRuleDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationRuleDto"/> class.
        /// </summary>
        public CancellationRuleDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationRuleDto"/> class using the specified cancellation
        /// rule.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="CancellationRule"/>
        /// object to the corresponding properties of the <see cref="CancellationRuleDto"/> instance.</remarks>
        /// <param name="modal">The <see cref="CancellationRule"/> object containing the cancellation rule details to initialize the DTO.</param>
        public CancellationRuleDto(CancellationRule modal)
        {
            CancellationRuleId = modal.Id;
            CancellationDays = modal.DaysBeforeBookingThatCancellationIsAvailable;
            Ammount = modal.CancellationFormualaValue;
            Rule = modal.CancellationFormualaType;
            LodgingId = modal.LodgingId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the cancellation rule.
        /// </summary>
        public int CancellationRuleId { get; init; } 

        /// <summary>
        /// Gets the unique identifier for the lodging.
        /// </summary>
        public string LodgingId { get; init; } = null!;

        /// <summary>
        /// Gets the number of days allowed for cancellation after a booking is made.
        /// </summary>
        public int CancellationDays { get; init; }

        /// <summary>
        /// Gets the amount associated with the transaction or operation.
        /// </summary>
        public double Ammount { get; init; }

        /// <summary>
        /// Gets the rule associated with the current configuration or operation.
        /// </summary>
        public string Rule { get; init; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance to a <see cref="CancellationRule"/> object.
        /// </summary>
        /// <remarks>This method maps the properties of the current instance to a new <see
        /// cref="CancellationRule"/> object. The resulting object contains details about cancellation policies,
        /// including the number of days before booking when cancellation is allowed, the formula type and value for
        /// calculating cancellation fees, and the associated lodging identifier.</remarks>
        /// <returns>A <see cref="CancellationRule"/> object representing the cancellation policy defined by the current
        /// instance.</returns>
        public CancellationRule ToCancellationRule()
        {
            return new CancellationRule()
			{
				DaysBeforeBookingThatCancellationIsAvailable = this.CancellationDays,
				CancellationFormualaType = this.Rule,
				CancellationFormualaValue = this.Ammount,
				LodgingId = this.LodgingId
			};
		}

        #endregion
    }
}