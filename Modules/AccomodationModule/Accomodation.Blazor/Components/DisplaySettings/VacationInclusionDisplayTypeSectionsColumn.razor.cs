using AccomodationModule.Application.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Accomodation.Blazor.Components.DisplaySettings
{
    /// <summary>
    /// Represents a column in the vacation inclusion display type sections, providing details about the vacation to be
    /// displayed.
    /// </summary>
    public partial class VacationInclusionDisplayTypeSectionsColumn
    {
        /// <summary>
        /// Gets or sets the name of the column to display in the vacation inclusion display type sections.
        /// </summary>
        [Parameter, EditorRequired] public string ColumnName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the vacation details to be displayed in the inclusion display type sections.
        /// </summary>
        [Parameter] public VacationViewModel? Vacation { get; set; }
    }
}
