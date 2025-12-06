using AccomodationModule.Domain.DataTransferObjects;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents a view model for a vacation inclusion, which defines additional services
    /// or features included in a vacation package.
    /// </summary>
    public record VacationInclusionViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor that initializes a new instance of <see cref="VacationInclusionViewModel"/>.
        /// </summary>
        public VacationInclusionViewModel() { }

        /// <summary>
        /// Initializes a new instance of <see cref="VacationInclusionViewModel"/> using a <see cref="VacationInclusionDto"/>.
        /// </summary>
        /// <param name="vacationInclusion">The DTO containing vacation inclusion details.</param>
        public VacationInclusionViewModel(VacationInclusionDto vacationInclusion)
        {
            VacationInclusionId = string.IsNullOrEmpty(vacationInclusion.VacationInclusionId) ? Guid.NewGuid().ToString() : vacationInclusion.VacationInclusionId;
            VacationId = vacationInclusion.VacationId;
            VacationExtensionId = vacationInclusion.VacationExtensionId;
            Name = vacationInclusion.Name;
            Description = vacationInclusion.Description;
            Selector = vacationInclusion.Selector;
            Order = vacationInclusion.Order;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the vacation inclusion.
        /// </summary>
        public string VacationInclusionId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the associated vacation package ID.
        /// </summary>
        public string VacationId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the associated vacation extension ID (if applicable).
        /// </summary>
        public string VacationExtensionId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the vacation inclusion.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional description for the vacation inclusion.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the selector for the vacation inclusion.
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        /// Gets or sets the order of the vacation inclusion.
        /// </summary>
        public int Order { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance of <see cref="VacationInclusion"/> to a <see cref="VacationInclusionDto"/>.
        /// </summary>
        /// <returns>A <see cref="VacationInclusionDto"/> that represents the current instance, including its identifier,
        /// associated vacation and extension IDs, name, description, selector, and order.</returns>
        public VacationInclusionDto ToDto()
        {
            return new VacationInclusionDto
            {
                VacationInclusionId = this.VacationInclusionId,
                VacationId = this.VacationId,
                VacationExtensionId = this.VacationExtensionId,
                Name = this.Name,
                Description = this.Description,
                Selector = this.Selector,
                Order = this.Order
            };
        }

        #endregion
    }
}
