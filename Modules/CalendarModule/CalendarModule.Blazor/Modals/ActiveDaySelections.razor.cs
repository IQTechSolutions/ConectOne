using CalendarModule.Domain.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CalendarModule.Blazor.Modals
{
    /// <summary>
    /// Represents a component that manages the selection of active days and displays related calendar entries.
    /// </summary>
    /// <remarks>This component is designed to be used within a dialog context. It provides functionality to
    /// display  calendar entries for a specific date and allows the user to cancel the selection process.</remarks>
    public partial class ActiveDaySelections
    {
        /// <summary>
        /// Gets the current instance of the dialog being managed by this component.
        /// </summary>
        /// <remarks>This property is used to interact with the dialog instance, such as closing the
        /// dialog or passing data back to the caller.</remarks>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the active date associated with the current context.
        /// </summary>
        [Parameter] public DateTime ActiveDate { get; set; }

        /// <summary>
        /// Gets or sets the collection of calendar entries to be displayed.
        /// </summary>
        [Parameter] public IEnumerable<CalendarEntryDto> CalendarEntries { get; set; } = new List<CalendarEntryDto>();

        /// <summary>
        /// Gets or sets the navigation manager used to programmatically navigate and obtain information about the
        /// current URI.
        /// </summary>
        /// <remarks>This property is typically provided by dependency injection in Blazor applications.
        /// Use it to navigate to different pages or to access the current navigation state.</remarks>
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Cancels the selection process and closes the dialog.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
