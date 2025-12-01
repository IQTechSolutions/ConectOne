using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Components.Modals
{
    /// <summary>
    /// Represents a modal component for displaying daily calendar entries.
    /// </summary>
    public partial class CalendarDailyEntriesModal
    {
        /// <summary>
        /// Gets or sets the current instance of the dialog being managed by this component.
        /// </summary>
        /// <remarks>This property is typically used to interact with the dialog instance, such as closing
        /// the dialog or performing actions related to its lifecycle. It is provided via cascading parameters and is
        /// not intended to be set directly by consumers.</remarks>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used to perform HTTP requests.
        /// </summary>
        /// <remarks>The provider must be set before making any HTTP requests. Dependency injection is
        /// used to supply the implementation.</remarks>
        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        /// <summary>
        /// Provides localized strings for the calendar entries modal.
        /// </summary>
        [Inject] public IStringLocalizer<CalendarDailyEntriesModal> Localizer { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date and time of the entry.
        /// </summary>
        [Parameter] public DateTime EntryDate { get; set; }

        /// <summary>
        /// Represents a collection of school events.
        /// </summary>
        /// <remarks>This field is initialized as an empty list of <see cref="SchoolEventDto"/> objects.
        /// It is intended to store event data for use within the class.</remarks>
        private IEnumerable<SchoolEventDto> _events = new List<SchoolEventDto>();

        /// <summary>
        /// Cancels the dialog.
        /// </summary>
        void Cancel() => MudDialog.Cancel();

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var dayEntriesResult = await SchoolEventQueryService.PagedSchoolEventsForAppAsync(new SchoolEventPageParameters() { StartDate = EntryDate.ToString("O") });
                if (dayEntriesResult.Succeeded)
                    _events = dayEntriesResult.Data;
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}