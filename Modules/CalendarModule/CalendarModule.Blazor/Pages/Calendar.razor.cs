using CalendarModule.Application.ViewModels;
using CalendarModule.Blazor.Modals;
using CalendarModule.Domain.DataTransferObjects;
using CalendarModule.Domain.Enums;
using CalendarModule.Domain.Interfaces;
using CalendarModule.Domain.RequestFeatures;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using GroupingModule.Domain.DataTransferObjects;
using GroupingModule.Domain.Interfaces;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using Radzen;
using Radzen.Blazor;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces.SchoolEvents;
using SchoolsModule.Domain.RequestFeatures;
using DialogOptions = MudBlazor.DialogOptions;

namespace CalendarModule.Blazor.Pages
{
    /// <summary>
    /// The Calendar component is responsible for displaying a calendar view of school events.
    /// It allows users to view events, navigate to event details, and interact with calendar items.
    /// </summary>
    public partial class Calendar
    {
        private IList<CalendarEntryDto> appointments = new List<CalendarEntryDto>();
        private List<CalendarEntryDto> _activeDayAppointments = [];
        private SelectedListItem _upcommingAppointments;
        private RadzenScheduler<CalendarEntryDto> _scheduler = null!;
        private CategoryDto? _category;
        private readonly SchoolEventPageParameters _args = new();
        private bool _canCreate;
        private bool _loaded;

        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

       /// <summary>
        /// The dialog service for displaying modal dialogs.
        /// </summary>
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        /// <summary>
        /// Provides access to configuration settings.
        /// </summary>
        [Inject] public IConfiguration Configuration { get; set; } = null!;

        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        [Inject] public ICategoryService<ActivityGroup> ActivityGroupCategoryService { get; set; } = null!;

        [Inject] public IAppointmentService AppointmentService { get; set; } = null!;

        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// The ID of the category to filter events by, if any.
        /// </summary>
        [Parameter] public string? CategoryId { get; set; }

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

                if (!string.IsNullOrEmpty(CategoryId))
                {
                    var categoryResult = await ActivityGroupCategoryService.CategoryAsync(CategoryId);
                    if (categoryResult.Succeeded)
                    {
                        _category = categoryResult.Data;
                        _args.CategoryId = CategoryId;
                    }
                }

                if (user.IsInRole(RoleConstants.Parent))
                    _args.ParentId = user.GetUserId();

                if (user.IsInRole(RoleConstants.Learner))
                    _args.LearnerId = user.GetUserId();

                _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, CalendarModule.Domain.Constants.Permissions.CalendarPermissions.Create)).Succeeded;

                _loaded = true;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Asynchronously loads Google Calendar events within the specified date range and updates the scheduler with
        /// the retrieved events.
        /// </summary>
        /// <remarks>This method retrieves events from Google Calendar using the specified date range,
        /// converts them into  <see cref="CalendarEntryDto"/> objects, and adds them to the internal collection of
        /// appointments.  If any events occur on the current date, they are also added to the active day appointments
        /// collection. After processing, the scheduler is reloaded if it is initialized, and the component state is
        /// updated.</remarks>
        /// <param name="args">The event arguments containing the start and end dates for the calendar events to load.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task LoadGoogleCalendarEventsAsync(SchedulerLoadDataEventArgs args)
        {
            //var googleResult = await GoogleCalendarService.GetEvents(args.Start, args.End);
            //if (googleResult.Succeeded)
            //{
            //    foreach (var gEvent in googleResult.Data)
            //    {
            //        var start = gEvent.Start.DateTime ?? DateTime.Parse(gEvent.Start.Date);
            //        var end = gEvent.End.DateTime ?? DateTime.Parse(gEvent.End.Date);
            //        var entry = new CalendarEntryDto(gEvent.Id,
            //            gEvent.Summary ?? string.Empty,
            //            start,
            //            start.TimeOfDay,
            //            end,
            //            end.TimeOfDay,
            //            CalendarEntryType.Event,
            //            gEvent.HtmlLink ?? string.Empty, false);
            //        appointments.Add(entry);
            //        if (entry.StartDate.Value.Date == DateTime.Now.Date)
            //        {
            //            _activeDayAppointments.Add(entry);
            //        }
            //    }

            //    if (_scheduler != null)
            //    {
            //        await _scheduler.Reload();
            //    }

            //    StateHasChanged();
            //}
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
                    resultItem.EndDate, resultItem.EndTime, CalendarEntryType.Event, "", resultItem.FullDayEvent)
                {
                    AudienceType = resultItem.AudienceType,
                    InvitedUsers = resultItem.InvitedUsers.ToList(),
                    InvitedRoles = resultItem.InvitedRoles.ToList(),
                    Color = resultItem.Color
                };

                var creationResult = await AppointmentService.AddAsync(calendarItem);
                if (creationResult.Succeeded)
                {
                    appointments.Add(calendarItem);
                    if(resultItem.StartDate.Value.Date == _scheduler.CurrentDate)
                        _activeDayAppointments.Add(calendarItem);

                    await _scheduler.Reload();
                    Snackbar.Add("Item was added successfully", Severity.Success);
                }

                StateHasChanged();
            }
        }

        /// <summary>
        /// Displays a modal dialog for creating or editing a calendar entry and processes the result.
        /// </summary>
        /// <remarks>This method opens a modal dialog using the <see cref="DialogService"/> to allow the
        /// user to create or edit a calendar entry. If the dialog is confirmed, the resulting data is processed and
        /// sent to the server for creation or update. Upon successful creation, the new calendar entry is added to the
        /// local collection and the scheduler is reloaded.</remarks>
        /// <param name="appointmentId">The unique identifier of the appointment to be edited. If null or empty, a new calendar entry will be
        /// created.</param>
        /// <returns></returns>
        private async Task ShowCalendarModal(string appointmentId)
        {
            var parameters = new DialogParameters<CalendarEntryModal>
            {
                { x => x.AppointmentId, appointmentId }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            var dialog = await DialogService.ShowAsync<CalendarEntryModal>("Confirm", parameters, options);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var resultItem = (CalendarEntryViewModel)result.Data;



                var calendarItem = new CalendarEntryDto(resultItem.Id, resultItem.Name, resultItem.StartDate, resultItem.StartTime,
                    resultItem.EndDate, resultItem.EndTime, CalendarEntryType.Event, "", resultItem.FullDayEvent)
                {
                    AudienceType = resultItem.AudienceType,
                    InvitedUsers = resultItem.InvitedUsers.ToList(),
                    InvitedRoles = resultItem.InvitedRoles.ToList()
                };

                var creationResult = await AppointmentService.EditAsync(calendarItem);
                if (creationResult.Succeeded)
                {
                    appointments.Add(calendarItem);
                    if (resultItem.StartDate.Value.Date == _scheduler.CurrentDate)
                        _activeDayAppointments.Add(calendarItem);

                    await _scheduler.Reload();
                    Snackbar.Add("Item was added successfully", Severity.Success);
                }

                StateHasChanged();
            }
        }

        /// <summary>
        /// Deletes a calendar entry group after user confirmation.
        /// </summary>
        /// <remarks>This method displays a confirmation dialog to the user before proceeding with the
        /// deletion. If the user confirms, the specified calendar entry group is removed, and the scheduler is reloaded
        /// to reflect the changes.</remarks>
        /// <param name="appointmentId">The unique identifier of the calendar entry group to be deleted. Cannot be null or empty.</param>
        /// <returns></returns>
        private async Task DeleteCalendarEntryGroup(string appointmentId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, $"Are you sure you want to remove this appointment from the calendar?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var removalResponse = await AppointmentService.DeleteAsync(appointmentId);
                removalResponse.ProcessResponseForDisplay(Snackbar, async () =>
                {
                    _scheduler.Reload();
                });
            }
        }

        /// <summary>
        /// Handles the event when a calendar slot is selected.
        /// </summary>
        /// <param name="args">The argument parameters</param>
        private async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            _activeDayAppointments = args.Appointments.Select(c => (CalendarEntryDto)c.Data).ToList();
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
                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
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
                    var item = new CalendarEntryDto(resultItem.Id, resultItem.Name, resultItem.StartDate, resultItem.StartTime, resultItem.EndDate, resultItem.EndTime, CalendarEntryType.Event, "", resultItem.FullDayEvent)
                    {
                        AudienceType = resultItem.AudienceType,
                        InvitedUsers = resultItem.InvitedUsers.ToList(),
                        InvitedRoles = resultItem.InvitedRoles.ToList()
                    };

                    var creationResult = await AppointmentService.EditAsync(item);
                    if (creationResult.Succeeded)
                    {
                        var appointment = appointments.FirstOrDefault(c => c.Id == resultItem.Id);
                        var index = appointments.IndexOf(appointment);

                        appointments[index] = item;

                        await _scheduler.Reload();
                        Snackbar.Add("Item was updated successfully", Severity.Success);
                    }
                }
            }
        }
        
        /// <summary>
        /// Handles the loading of calendar and scheduler data for the specified date range.
        /// </summary>
        /// <remarks>This method retrieves calendar entries and school events from the data provider and
        /// populates the internal appointments collection. If the configuration enables Google Calendar integration, it
        /// also triggers the loading of Google Calendar events asynchronously.  The method ensures that appointments
        /// for the current day are identified and stored separately for quick access.</remarks>
        /// <param name="args">The event arguments containing the start and end dates for the data to be loaded.</param>
        /// <returns></returns>
        private async Task OnLoadData(SchedulerLoadDataEventArgs args)
        {
            appointments.Clear();

            var eventListResult = await SchoolEventQueryService.PagedSchoolEventsForAppCalendarAsync(_args);

            if (eventListResult.Succeeded)
            {
                foreach (var eventItem in eventListResult.Data)
                {
                    var appointment = eventItem with { Url = $"/activities/activitygroups/events/update/{eventItem.Id}" };

                    if (eventItem.FullDayEvent)
                    {
                        appointment = eventItem with { StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(23,59,59)};
                    }

                    appointments.Add(appointment);
                }
            }

            var calendarEntryListResult = await AppointmentService.GetAllAsync(new CalendarPageParameters() { StartDate = DateTime.Now, EndDate = DateTime.MaxValue });

            if (calendarEntryListResult.Succeeded)
            {
                foreach (var eventItem in calendarEntryListResult.Data)
                {
                    appointments.Add(eventItem with { Url = $"javascript:null" });
                }
            }

            if (Configuration.GetValue<bool>("UseGoogleCalendar"))
            {
                _ = LoadGoogleCalendarEventsAsync(args);
            }

            appointments = appointments.ToList();

            if (appointments.Any(c => c.StartDate?.Date == DateTime.Now.Date))
            {
                _activeDayAppointments = appointments.Where(c => c.StartDate?.Date == DateTime.Now.Date).ToList();
            }
            if (appointments.Any(c => c.StartDate?.Date > DateTime.Now.Date))
            {
                var bb = appointments.Where(c => c.StartDate?.Date > DateTime.Now.Date).ToList();
                _upcommingAppointments = new SelectedListItem { Items = bb.Select(g => new CalendarEntryViewModel(g)).GroupBy(c => c.StartDate.Value.Date).Take(10).ToList(), Expanded = false };
            }

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Handles the event triggered when a day is selected in the scheduler.
        /// </summary>
        /// <param name="args">The event arguments containing details about the selected day, including the day and its associated
        /// appointments.</param>
        private void OnDaySelect(SchedulerDaySelectEventArgs args)
        {
            //  console.Log($"DaySelect: Day={args.Day} AppointmentCount={args.Appointments.Count()}");
        }

        /// <summary>
        /// Handles the rendering of scheduler slots, allowing customization of their appearance based on specific
        /// conditions.
        /// </summary>
        /// <remarks>This method customizes the appearance of scheduler slots based on the following
        /// conditions: <list type="bullet"> <item> <description>If the view is "Month" and the slot represents today's
        /// date, the slot is highlighted.</description> </item> <item> <description>If the view is "Week" or "Day" and
        /// the slot falls within working hours (9 AM to 6 PM), the slot is highlighted.</description> </item> </list>
        /// The highlighting is applied by modifying the slot's style attribute.</remarks>
        /// <param name="args">The event arguments containing information about the slot being rendered, such as its start time, view type,
        /// and attributes.</param>
        private void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            // Highlight today in month view
            if (args.View.Text == "Month" && args.Start.Date == DateTime.Today)
            {
                args.Attributes["style"] = "background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
            }

            // Highlight working hours (9-18)
            if ((args.View.Text == "Week" || args.View.Text == "Day") && args.Start.Hour > 8 && args.Start.Hour < 19)
            {
                args.Attributes["style"] = $"background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
            }
        }

        /// <summary>
        /// Handles the rendering of appointments in the scheduler and allows customization of their appearance.
        /// </summary>
        /// <remarks>This method customizes the appearance of appointments based on their type. For
        /// example, appointments of type  <see cref="CalendarEntryType.Google"/> are styled with a red background.
        /// Avoid calling <see cref="ComponentBase.StateHasChanged"/>  within this method to prevent potential infinite
        /// rendering loops.</remarks>
        /// <param name="args">The event arguments containing information about the appointment being rendered, including its data and
        /// attributes.</param>
        private void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<CalendarEntryDto> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop

            if (args.Data.CalendarEntryType == CalendarEntryType.Google)
            {
                args.Attributes["style"] = $"background: {(string.IsNullOrEmpty(args.Data.Color) ? "red" : args.Data.Color)}";
            }
            else
            {
                args.Attributes["style"] = $"background: {(string.IsNullOrEmpty(args.Data.Color) ? "blue" : args.Data.Color)}";
            }
        }

        /// <summary>
        /// Handles the event triggered when an appointment is moved to a new time slot in the scheduler.
        /// </summary>
        /// <remarks>This method updates the appointment's start and end times based on the new time slot,
        /// sends the updated appointment to the server, and reloads the scheduler if the update is successful.  If the
        /// update fails, error messages are displayed in the user interface.</remarks>
        /// <param name="args">The event arguments containing details about the moved appointment and the target time slot.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task OnAppointmentMove(SchedulerAppointmentMoveEventArgs args)
        {
            var draggedAppointment = appointments.FirstOrDefault(x => x == args.Appointment.Data);

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

                var updateResult = await AppointmentService.EditAsync(draggedAppointment);
                if (!updateResult.Succeeded) Snackbar.AddErrors(updateResult.Messages);
                else
                {
                    await _scheduler.Reload();
                }
            }
            await InvokeAsync(StateHasChanged);
        }

    }

    /// <summary>
    /// Represents a selected list item containing grouped calendar entries and an expansion state.
    /// </summary>
    public class SelectedListItem
    {
        /// <summary>
        /// Gets or sets the list of grouped calendar entries by date.
        /// </summary>
        public List<IGrouping<DateTime, CalendarEntryViewModel>> Items { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the list item is expanded.
        /// </summary>
        public bool Expanded { get; set; }
    }
}
