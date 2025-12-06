using System.ComponentModel.DataAnnotations.Schema;
using AccomodationModule.Domain.Enums;
using ConectOne.Domain.Entities;
using ConectOne.Domain.Extensions;

namespace AccomodationModule.Domain.Entities
{
    /// <summary>
    /// Represents the display type information for vacation inclusions.
    /// This class is used to define how vacation inclusions are displayed, including the type, display order, and column selection.
    /// </summary>
    public class VacationInclusionDisplayTypeInformation : EntityBase<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationInclusionDisplayTypeInformation"/> class.
        /// </summary>
        public VacationInclusionDisplayTypeInformation() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VacationInclusionDisplayTypeInformation"/> class by copying the
        /// values from an existing instance.
        /// </summary>
        /// <param name="info">The instance of <see cref="VacationInclusionDisplayTypeInformation"/> from which to copy values. Cannot be
        /// <see langword="null"/>.</param>
        public VacationInclusionDisplayTypeInformation(VacationInclusionDisplayTypeInformation info)
        {
            VacationInclusionDisplayType = info.VacationInclusionDisplayType;
            DisplayOrder = info.DisplayOrder;
            ColumnSelection = info.ColumnSelection;
            VacationId = info.VacationId;
        }

        #endregion

        #region Properties

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

        #endregion

        #region ReadOnly Properties

        /// <summary>
        /// Gets the display name for the vacation inclusion display type, based on its description.
        /// </summary>
        public string DisplayName => VacationInclusionDisplayType.GetDescription();

        #endregion

        #region One to Many Relationships

        /// <summary>
        /// Gets or sets the unique identifier for the associated vacation.
        /// </summary>
        [ForeignKey(nameof(Vacation))] public string? VacationId { get; set; }

        /// <summary>
        /// Gets or sets the vacation details for the current user or entity.
        /// </summary>
        public Vacation? Vacation { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a new instance of <see cref="VacationInclusionDisplayTypeInformation"/> that is a copy of the
        /// current instance.
        /// </summary>
        /// <returns>A new <see cref="VacationInclusionDisplayTypeInformation"/> object that is a copy of this instance, with a
        /// new unique identifier.</returns>
        public VacationInclusionDisplayTypeInformation Clone()
        {
            return new VacationInclusionDisplayTypeInformation(this) { Id = Guid.NewGuid().ToString() };
        }

        #endregion
    }
}
