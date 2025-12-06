using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// ViewModel for representing vacation inclusion display type information.
    /// This class is used to bind data between the UI and the application logic.
    /// It includes details such as the display type, display order, and column selection.
    /// </summary>
    public class VacationInclusionDisplayTypeInformationViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationInclusionDisplayTypeInformationViewModel"/> class with default values.
        /// </summary>
        public VacationInclusionDisplayTypeInformationViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationInclusionDisplayTypeInformationViewModel"/> class using a <see cref="VacationInclusionDisplayTypeInformationDto"/>.
        /// </summary>
        /// <param name="dto">The <see cref="VacationInclusionDisplayTypeInformationDto"/> to map to this ViewModel.</param>
        public VacationInclusionDisplayTypeInformationViewModel(VacationInclusionDisplayTypeInformationDto dto)
        {
            VacationInclusionDisplayTypeInformationId = dto.VacationInclusionDisplayTypeInformationId;
            VacationInclusionDisplayType = dto.VacationInclusionDisplayType;
            DisplayOrder = dto.DisplayOrder;
            ColumnSelection = dto.ColumnSelection;
            VacationId = dto.VacationId;
            VacationExtensionId = dto.VacationExtensionId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the vacation inclusion display type information.
        /// </summary>
        public string? VacationInclusionDisplayTypeInformationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the type of vacation inclusion display (e.g., Accommodation, Meals & Activities).
        /// </summary>
        public VacationInclusionDisplayTypes VacationInclusionDisplayType { get; set; }

        /// <summary>
        /// Gets or sets the display order for the vacation inclusion display type.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the column selection for the vacation inclusion display type.
        /// </summary>
        public string ColumnSelection { get; set; }

        /// <summary>
        /// The identity of the vacation this inclusion display type section is associated with
        /// </summary>
        public string? VacationId { get; set; }

        /// <summary>
        /// The identity of the vacation extension this inclusion display type section is associated with
        /// </summary>
        public string? VacationExtensionId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current instance to a <see cref="VacationInclusionDisplayTypeInformationDto"/> object.
        /// </summary>
        /// <returns>A <see cref="VacationInclusionDisplayTypeInformationDto"/> object that contains the data from the current
        /// instance.</returns>
        public VacationInclusionDisplayTypeInformationDto ToDto()
        {
            return new VacationInclusionDisplayTypeInformationDto
            {
                VacationInclusionDisplayTypeInformationId = this.VacationInclusionDisplayTypeInformationId,
                VacationInclusionDisplayType = this.VacationInclusionDisplayType,
                DisplayOrder = this.DisplayOrder,
                ColumnSelection = this.ColumnSelection,
                VacationId = this.VacationId,
                VacationExtensionId = this.VacationExtensionId
            };
        }

        #endregion
    }
}
