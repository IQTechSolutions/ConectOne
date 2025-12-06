using AccomodationModule.Domain.Entities;
using AccomodationModule.Domain.Enums;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing vacation inclusion display type information.
    /// This class is used to transfer data between layers, such as from the database to the UI or API.
    /// It includes details such as the display type, display order, and column selection.
    /// </summary>
    public record VacationInclusionDisplayTypeInformationDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationInclusionDisplayTypeInformationDto"/> class with default values.
        /// </summary>
        public VacationInclusionDisplayTypeInformationDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationInclusionDisplayTypeInformationDto"/> class using a <see cref="VacationInclusionDisplayTypeInformation"/> entity.
        /// </summary>
        /// <param name="model">The <see cref="VacationInclusionDisplayTypeInformation"/> entity to map to this DTO.</param>
        public VacationInclusionDisplayTypeInformationDto(VacationInclusionDisplayTypeInformation model)
        {
            VacationInclusionDisplayTypeInformationId = model.Id;
            VacationInclusionDisplayType = model.VacationInclusionDisplayType;
            DisplayOrder = model.DisplayOrder;
            ColumnSelection = model.ColumnSelection;
            VacationId = model.VacationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier for the vacation inclusion display type information.
        /// </summary>
        public string? VacationInclusionDisplayTypeInformationId { get; init; }

        /// <summary>
        /// Gets the type of vacation inclusion display (e.g., Accommodation, Meals & Activities).
        /// </summary>
        public VacationInclusionDisplayTypes VacationInclusionDisplayType { get; init; }

        /// <summary>
        /// Gets the display order for the vacation inclusion display type.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets the column selection for the vacation inclusion display type.
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
        /// Converts the DTO into a <see cref="VacationInclusionDisplayTypeInformation"/> entity.
        /// </summary>
        /// <returns>An <see cref="VacationInclusionDisplayTypeInformation"/> entity representing the vacation inclusion display type information.</returns>
        public VacationInclusionDisplayTypeInformation ToVacationInclusionDisplayTypeInformation()
        {
            return new VacationInclusionDisplayTypeInformation()
            {
                Id = VacationInclusionDisplayTypeInformationId!,
                VacationInclusionDisplayType = VacationInclusionDisplayType,
                DisplayOrder = DisplayOrder,
                ColumnSelection = ColumnSelection,
                VacationId = VacationId
            };
        }

        /// <summary>
        /// Updates the values of an existing <see cref="VacationInclusionDisplayTypeInformation"/> entity with the properties of this DTO.
        /// </summary>
        /// <param name="vacationInclusionDisplayTypeInformation">The vacation inclusion display type information entity to update.</param>
        public void UpdateVacationInclusionDisplayTypeInformationValues(in VacationInclusionDisplayTypeInformation vacationInclusionDisplayTypeInformation)
        {
            vacationInclusionDisplayTypeInformation.VacationInclusionDisplayType = VacationInclusionDisplayType;
            vacationInclusionDisplayTypeInformation.DisplayOrder = DisplayOrder;
            vacationInclusionDisplayTypeInformation.ColumnSelection = ColumnSelection;
        }

        #endregion
    }
}
