using CalendarModule.Domain.DataTransferObjects;
using Heron.MudCalendar;
using Microsoft.AspNetCore.Components;

namespace CalendarModule.Blazor.Components
{
    /// <summary>
    /// Represents a template for displaying a calendar week day event.
    /// </summary>
    public partial class CalendarWeekDayTemplate
    {
        #region Parameters

        /// <summary>
        /// Gets or sets the function to retrieve information about a calendar entry based on the event ID.
        /// </summary>
        [Parameter] public Func<string, Task<CalendarEntryDto>> ItemInfo { get; set; }

        /// <summary>
        /// A list of the calendar items for the day
        /// </summary>
        [Parameter] public List<CalendarItem> DayItems { get; set; } = new();

        #endregion

        #region Private Fields

        /// <summary>
        /// Holds the information about the school event.
        /// </summary>
        private List<CalendarEntryDto> _schoolEvents = new List<CalendarEntryDto>();

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Method invoked when the component is initialized.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            foreach (var item in DayItems)
            {
                _schoolEvents.Add(await ItemInfo(item.Text));
            }
            await base.OnInitializedAsync();
        }

        #endregion
    }
}