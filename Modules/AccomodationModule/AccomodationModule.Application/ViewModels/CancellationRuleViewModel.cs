using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a cancellation rule, providing details about the rule's conditions and associated
    /// lodging.
    /// </summary>
    /// <remarks>This class is typically used to transfer cancellation rule data between the application
    /// layers. It includes properties for the rule's unique identifier, the number of days required for cancellation, 
    /// the associated lodging identifier, the monetary amount involved, and a textual description of the
    /// rule.</remarks>
    public class CancellationRuleViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationRuleViewModel"/> class.
        /// </summary>
        public CancellationRuleViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationRuleViewModel"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see
        /// cref="CancellationRuleDto"/> to the corresponding properties of the <see cref="CancellationRuleViewModel"/>
        /// instance.</remarks>
        /// <param name="modal">The <see cref="CancellationRuleDto"/> containing the cancellation rule data to initialize the view model.</param>
        public CancellationRuleViewModel(CancellationRuleDto modal)
        {
            CancellationRuleId = modal.CancellationRuleId;
            CancellationDays = modal.CancellationDays;
            Ammount = modal.Ammount;
            Rule = modal.Rule;
            LodgingId = modal.LodgingId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationRuleViewModel"/> class using the specified modal
        /// view model.
        /// </summary>
        /// <remarks>The properties of the <see cref="CancellationRuleViewModel"/> are populated based on
        /// the values provided in the <paramref name="modal"/>. Ensure that the <paramref name="modal"/> contains valid
        /// data before passing it to this constructor.</remarks>
        /// <param name="modal">The modal view model containing the cancellation rule data. This parameter must not be <see
        /// langword="null"/>.</param>
        public CancellationRuleViewModel(CancellationRuleModalViewModel modal)
        {
            CancellationDays = modal.CancellationDays;
            Ammount = modal.Ammount;
            Rule = modal.Rule;
            LodgingId = modal.LodgingId;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the identifier of the cancellation rule associated with the current entity.
        /// </summary>
        public int? CancellationRuleId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a lodging entity.
        /// </summary>
        public string LodgingId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of days allowed for cancellation.
        /// </summary>
        public int CancellationDays { get; set; }

        /// <summary>
        /// Gets or sets the monetary amount associated with the transaction.
        /// </summary>
        public double Ammount { get; set; }

        /// <summary>
        /// Gets or sets the rule associated with the current operation or configuration.
        /// </summary>
        public string Rule { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of the cancellation rule to a <see cref="CancellationRuleDto"/>.
        /// </summary>
        /// <returns>A <see cref="CancellationRuleDto"/> representing the cancellation rule, with properties mapped from the
        /// current instance.</returns>
        public CancellationRuleDto ToDto()
        {
            return new CancellationRuleDto
            {
                CancellationRuleId = this.CancellationRuleId ?? 0,
                CancellationDays = this.CancellationDays,
                Ammount = this.Ammount,
                Rule = this.Rule,
                LodgingId = this.LodgingId
            };
        }

        #endregion
    }
}
