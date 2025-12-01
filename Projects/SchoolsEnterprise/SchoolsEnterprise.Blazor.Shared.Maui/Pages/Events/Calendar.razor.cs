using CalendarModule.Application.ViewModels;
using CalendarModule.Blazor.Modals;
using CalendarModule.Domain.DataTransferObjects;
using CalendarModule.Domain.Enums;
using CalendarModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Interfaces;
using GroupingModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Radzen;
using Radzen.Blazor;
using SchoolsEnterprise.Blazor.Shared.Maui.Components.Modals;
using SchoolsModule.Domain.RequestFeatures;
using DialogOptions = MudBlazor.DialogOptions;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Pages.Events
{
    /// <summary>
    /// The Calendar component is responsible for displaying a calendar view of school events.
    /// It allows users to view events, navigate to event details, and interact with calendar items.
    /// </summary>
    public partial class Calendar
    {
        /// <summary>
        /// Gets the collection of calendar appointments associated with the current context.
        /// </summary>
        IList<CalendarEntryDto> appointments = new List<CalendarEntryDto>();

        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public IBaseHttpProvider Provider { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        [Inject] public ISnackbar SnackBar { get; set; } = null!;
        
        /// <summary>
        /// The ID of the category to filter events by, if any.
        /// </summary>
        [Parameter] public string? CategoryId { get; set; }

        /// <summary>
        /// The category details, if a category ID is provided.
        /// </summary>
        private CategoryDto? _category;

        /// <summary>
        /// Parameters for paging and filtering school events.
        /// </summary>
        private readonly SchoolEventPageParameters _args = new();

        /// <summary>
        /// Indicates whether the component has completed loading and can render content.
        /// </summary>
        private bool _loaded;

        /// <summary>
        /// List of calendar items representing school events.
        /// </summary>
        private List<CalendarEntryDto> _eventList = [];

        /// <summary>
        /// The logger service for logging messages.
        /// </summary>
        private RadzenScheduler<CalendarEntryDto> _scheduler = null!;

        /// <summary>
        /// The title of the page.
        /// </summary>
        private string PageTitle => "Calendar";

        /// <summary>
        /// Navigates to the event details page for the specified event ID.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        public void NavigateToEventDetails(string eventId)
        {
            NavigationManager.NavigateTo($"/events/{eventId}");
        }

        /// <summary>
        /// Navigates to the specified URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        private void NavigateToPage(string url)
        {
            NavigationManager.NavigateTo(url, true);
        }

        /// <summary>
        /// The list of appointments to display on the calendar.
        /// </summary>
        private async Task ShowCalendarModal()
        {
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            var dialog = await DialogService.ShowAsync<CalendarEntryModal>("Confirm", options);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var resultItem = (CalendarEntryViewModel)result.Data;
                var calendarItem = new CalendarEntryDto(resultItem.Id, resultItem.Name, resultItem.StartDate, resultItem.StartTime,
                    resultItem.EndDate, resultItem.EndTime, CalendarEntryType.Event, "", resultItem.FullDayEvent);

                var creationResult = await Provider.PutAsync("calendar", calendarItem);
                if (creationResult.Succeeded)
                {
                    _eventList.Add(calendarItem);

                    await _scheduler.Reload();
                    SnackBar.Add("Item was added successfully", Severity.Success);
                }

                StateHasChanged();
            }
        }

        /// <summary>
        /// Handles the event when a calendar slot is selected.
        /// </summary>
        /// <param name="args">The argument parameters</param>
        private async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            
        }

        /// <summary>
        /// Handles the event when a calendar item is selected.
        /// </summary>
        /// <param name="obj"></param>
        private async Task OnMoreSelectAsyncTask(SchedulerMoreSelectEventArgs obj)
        {
            var parameters = new DialogParameters<ActiveDaySelections>
            {
                { x => x.CalendarEntries, obj.Appointments.Select(c => (CalendarEntryDto)c.Data).ToList() },
                { x => x.ActiveDate, obj.Start.Date}
            };

            await DialogService.ShowAsync<ActiveDaySelections>("Confirm", parameters);
        }

        /// <summary>
        /// Handles the event when a calendar item is selected.
        /// </summary>
        /// <param name="args"></param>
        private async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<CalendarEntryDto> args)
        {
            if (args.Data.CalendarEntryType == CalendarEntryType.Event)
            {
                NavigationManager.NavigateTo(args.Data.Url);
            }
            else
            {
                var options = new MudBlazor.DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
                var parameters = new DialogParameters<CalendarEntryModal>
                {
                    { x => x.CalendarEntry, new CalendarEntryViewModel(args.Data) },
                    { x => x.ShowDeleteButton, true }
                };

                var dialog = await DialogService.ShowAsync<CalendarEntryModal>("Confirm", parameters, options);
                var result = await dialog.Result;

                if (!result!.Canceled)
                {
                    var resultItem = (CalendarEntryViewModel)result.Data;
                    var item = new CalendarEntryDto(resultItem.Id, resultItem.Name, resultItem.StartDate, resultItem.StartTime, resultItem.EndDate, resultItem.EndTime, CalendarEntryType.Event, "", resultItem.FullDayEvent);

                    var creationResult = await Provider.PostAsync("calendar", item);
                    if (creationResult.Succeeded)
                    {
                        var appointment = _eventList.FirstOrDefault(c => c.Id == resultItem.Id);
                        var index = _eventList.IndexOf(appointment);

                        _eventList[index] = item;

                        await _scheduler.Reload();
                        SnackBar.Add("Item was updated successfully", Severity.Success);
                    }
                }
            }
        }

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var authState = await AuthenticationStateTask;
                var user = authState.User;

                if (user.IsInRole(RoleConstants.Parent))
                    _args.ParentId = user.GetUserId();

                if (user.IsInRole(RoleConstants.Learner))
                    _args.LearnerId = user.GetUserId();

                if (!string.IsNullOrEmpty(CategoryId))
                {
                    var categoryResult = await Provider.GetAsync<CategoryDto>($"activities/categories/{CategoryId}");
                    if (categoryResult.Succeeded)
                    {
                        _category = categoryResult.Data;
                        _args.CategoryId = CategoryId;
                    }
                }

                var eventListResult = await Provider.GetPagedAsync<CalendarEntryDto, SchoolEventPageParameters>("schoolevents/all/app/calendar", _args);

                if (eventListResult.Succeeded)
                {
                    foreach (var eventItem in eventListResult.Data)
                    {
                        _eventList.Add(eventItem with { Url =   $"/activities/activitygroups/events/update/{eventItem.Id}" });
                    }
                }

                var calendarEntryListResult = await Provider.GetAsync<List<CalendarEntryDto>>($"calendar?{(new CalendarPageParameters() { StartDate = DateTime.MinValue, EndDate = DateTime.MaxValue }).GetQueryString()}");

                if (calendarEntryListResult.Succeeded)
                {
                    foreach (var eventItem in calendarEntryListResult.Data)
                    {
                        _eventList.Add(eventItem with { Url =   $"/activities/activitygroups/events/update/{eventItem.Id}" });
                    }
                }

                _loaded = true;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Handles the event that occurs when a day is selected in the scheduler.
        /// </summary>
        /// <param name="args">An object that contains the event data for the day selection, including the selected day and associated
        /// appointments.</param>
        void OnDaySelect(SchedulerDaySelectEventArgs args)
        {
            //  console.Log($"DaySelect: Day={args.Day} AppointmentCount={args.Appointments.Count()}");
        }

        /// <summary>
        /// Handles the rendering of a scheduler slot, allowing customization of its appearance based on the slot's
        /// context.
        /// </summary>
        /// <remarks>Use this method to apply custom styles or attributes to scheduler slots during
        /// rendering. Modifying the attributes in the provided event arguments will affect how the slot is displayed in
        /// the scheduler UI.</remarks>
        /// <param name="args">An object containing information about the slot being rendered, including its view, start time, and a
        /// collection of attributes that can be modified to affect rendering.</param>
        void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            // Highlight today in month view
            if (args.View.Text == "Month" && args.Start.Date == DateTime.Today)
            {
                args.Attributes["style"] = "background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
            }

            // Highlight working hours (9-18)
            if ((args.View.Text == "Week" || args.View.Text == "Day") && args.Start.Hour > 8 && args.Start.Hour < 19)
            {
                args.Attributes["style"] = "background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
            }
        }

        /// <summary>
        /// Handles the rendering event for an appointment in the scheduler, allowing customization of its appearance or
        /// attributes.
        /// </summary>
        /// <remarks>This method is typically used to modify the visual presentation or metadata of
        /// individual appointments as they are rendered in the scheduler. Avoid calling StateHasChanged within this
        /// method, as it may cause rendering issues or infinite loops.</remarks>
        /// <param name="args">The event data for the appointment render operation, containing information about the appointment and
        /// attributes that can be modified.</param>
        void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<CalendarEntryDto> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop

            // if (args.Data.Name == "Birthday")
            // {
            //     args.Attributes["style"] = "background: red";
            // }
        }

        /// <summary>
        /// Handles the movement of an appointment within the scheduler and updates its start and end times accordingly.
        /// </summary>
        /// <remarks>If the appointment is successfully updated, the scheduler is reloaded to reflect the
        /// changes. If the update fails, error messages are displayed to the user.</remarks>
        /// <param name="args">The event data containing information about the appointment being moved and the target slot date.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        async Task OnAppointmentMove(SchedulerAppointmentMoveEventArgs args)
        {
            var draggedAppointment = appointments.FirstOrDefault(x => x == args.Appointment.Data);
            var index = appointments.IndexOf(draggedAppointment);

            if (draggedAppointment != null)
            {
                var duration = draggedAppointment.EndDate - draggedAppointment.StartDate;

                if (args.SlotDate.TimeOfDay == TimeSpan.Zero)
                {
                    draggedAppointment = draggedAppointment with { StartDate = args.SlotDate.Date.Add(draggedAppointment.StartDate.Value.TimeOfDay), EndDate = args.SlotDate.Add(duration.Value) };
                }
                else
                {
                    draggedAppointment = draggedAppointment with { StartDate = args.SlotDate, EndDate = args.SlotDate.Add(duration.Value) };
                }

                var updateResult = await Provider.PostAsync("calendar", draggedAppointment);
                if (!updateResult.Succeeded) SnackBar.AddErrors(updateResult.Messages);
                else
                {
                    appointments[index] = draggedAppointment;
                    await _scheduler.Reload();
                }
            }
        }

        /// <summary>
        /// Represents a collection of grouped calendar entries and an associated expanded state, typically used to
        /// manage selection and display in calendar-based user interfaces.
        /// </summary>
        public class SelectedListItem
        {
            /// <summary>
            /// Gets or sets the collection of calendar entries grouped by date.
            /// </summary>
            public List<IGrouping<DateTime, CalendarEntryDto>> Items { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the item is expanded.
            /// </summary>
            public bool Expanded { get; set; }
        }
    }
}
