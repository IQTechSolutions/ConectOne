using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using SchoolsModule.Blazor.Modals;
using SchoolsModule.Domain.RequestFeatures;
using System.Security.Claims;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using ConectOne.Domain.ResultWrappers;
using FilingModule.Blazor.Managers;
using GroupingModule.Application.ViewModels;
using MessagingModule.Domain.Enums;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SchoolsModule.Application.ViewModels;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.SchoolEvents;

namespace SchoolsModule.Blazor.Components.ActivityGroupEvents
{
    /// <summary>
    /// The ActivityGroupEventTable component is responsible for displaying a table of activity group events.
    /// It provides functionality to search, sort, delete, and export events, as well as resend event requests.
    /// </summary>
    public partial class ActivityGroupEventTable
    {
        private IEnumerable<SchoolEventViewModel> _events = [];
        private MudTable<SchoolEventViewModel> _table = null!;
        private int _totalItems;
        private int _currentPage;
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;
        private bool _loaded;
        private bool _canCreate;
        private bool _canEdit;
        private bool _canDelete;
        private bool _canExport;
        private bool _canSendNotification;
        private bool _canCreateMessage;

        /// <summary>
        /// The current authentication state, used to determine the logged-in user's identity.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Injected HTTP provider for making API requests.
        /// </summary>
        [Inject] public ISchoolEventQueryService SchoolEventQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to execute commands related to school events.
        /// </summary>
        [Inject] public ISchoolEventCommandService SchoolEventCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to export school event data.
        /// </summary>
        [Inject] public ISchoolEventExportService SchoolEventExportService { get; set; } = null!;

        /// <summary>
        /// Injected file manager for handling file downloads.
        /// </summary>
        [Inject] public IBlazorDownloadFileManager BlazorDownloadFileManager { get; set; } = null!;

        /// <summary>
        /// Injected dialog service for displaying dialogs.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Injected snack bar service for displaying notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Injected navigation manager for navigating between pages.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the event status chip should be displayed.
        /// </summary>
        [Parameter]
        public bool ShowEventStatusChip { get; set; } = false;

        /// <summary>
        /// The ID of the learner, if any.
        /// </summary>
        [Parameter] public string? LearnerId { get; set; } = null;

        /// <summary>
        /// Indicates whether to show archived events.
        /// </summary>
        [Parameter] public bool Archived { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the component is currently active.
        /// </summary>
        [Parameter] public bool Active { get; set; } = false;

        /// <summary>
        /// Event callback for resending event attendance requests.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendEventAttendanceRequest { get; set; }

        /// <summary>
        /// Event callback for resending event transport requests.
        /// </summary>
        [Parameter] public EventCallback<ResendPermissionsNotificationArgs> ResendEventTransportRequest { get; set; }
        
        /// <summary>
        /// Parameters for paging and filtering school events.
        /// </summary>
        private readonly SchoolEventPageParameters _args = new() { SearchText = "CreatedOn desc" };

        /// <summary>
        /// The current user's claims principal.
        /// </summary>
        private ClaimsPrincipal _principal = null!;

        /// <summary>
        /// Handles the search functionality by updating the search text and reloading the table data.
        /// </summary>
        /// <param name="text">The search text.</param>
        private async Task OnSearch(string text)
        {
            _args.SearchText = text;
            await _table.ReloadServerData();
        }

        /// <summary>
        /// Deletes an event by its ID after user confirmation.
        /// </summary>
        /// <param name="parentId">The ID of the event to delete.</param>
        private async Task DeleteEvent(string parentId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this parent from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var removalResult = await SchoolEventCommandService.DeleteAsync(parentId, default);

                async void SuccessAction()
                {
                    await _table.ReloadServerData();
                }

                removalResult.ProcessResponseForDisplay(SnackBar, SuccessAction);
            }
        }

        /// <summary>
        /// Creates a new event for the activity category.
        /// </summary>
        private async Task CreateNewEventForActivityEvent()
        {
            var parameters = new DialogParameters<AddEventToActivityCategoryModal>();

            var dialog = await DialogService.ShowAsync<AddEventToActivityCategoryModal>("Confirm", parameters);
            var result = await dialog.Result;
            var categoryViewModel = (CategoryViewModel)result!.Data!;

            if (!result.Canceled)
            {
                NavigationManager.NavigateTo($"/activities/activitygroups/events/create/{categoryViewModel.CategoryId}");
            }
        }

        /// <summary>
        /// Resends the activity group attendance request for the specified event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        public async Task OnResendActivityGroupAttendanceRequest(string eventId)
        {
            var args = new ResendPermissionsNotificationArgs() { ConsentType = ConsentTypes.Attendance, EventId = eventId };
            await ResendEventAttendanceRequest.InvokeAsync(args);
        }

        /// <summary>
        /// Resends the activity group transport request for the specified event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        public async Task OnResendActivityGroupTransportRequest(string eventId)
        {
            var args = new ResendPermissionsNotificationArgs() { ConsentType = ConsentTypes.Transport, EventId = eventId };
            await ResendEventTransportRequest.InvokeAsync(args);
        }

        /// <summary>
        /// Exports the events to an Excel file.
        /// </summary>
        private async Task ExportToExcel()
        {
            var exportArgs = new SchoolEventPageParameters();
            var response = await SchoolEventExportService.ExportEvents(exportArgs);
            if (!response.Succeeded) SnackBar.AddErrors(response.Messages);

            await BlazorDownloadFileManager.DownloadFile($"School Events_{DateTime.Now:ddMMyyyyHHmmss}.xlsx", Convert.FromBase64String(response.Data),
                "application/octet-stream");
            SnackBar.Add(@"School Events Exported", Severity.Success);
        }

        /// <summary>
        /// Exports the attendance list for the specified event to an Excel file.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        private async Task ExportAttendanceToExcel(string eventId)
        {
            try
            {
                var response = await SchoolEventExportService.ExportEventConsents(eventId, ConsentTypes.Attendance);
                if (!response.Succeeded) SnackBar.AddErrors(response.Messages);

                if (response.Data is not null)
                {
                    var result = await BlazorDownloadFileManager.DownloadFile(
                        $"School Event Attendance_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                        Convert.FromBase64String(response.Data),
                        "application/octet-stream");
                    if (result.Succeeded)
                        SnackBar.Add(@"Attendance Export done successfully", Severity.Success);
                    else
                        SnackBar.AddError(result.ErrorMessage);
                }
                else
                {
                    SnackBar.AddError("No Data found to export!");
                }

                    
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                SnackBar.AddError(e.Message);
            }

        }

        /// <summary>
        /// Exports the transport list for the specified event to an Excel file.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        private async Task ExportTransportToExcel(string eventId)
        {
            try
            {
                var response = await SchoolEventExportService.ExportEventConsents(eventId, ConsentTypes.Transport);
                if (!response.Succeeded) SnackBar.AddErrors(response.Messages);

                if (response.Data is not null)
                {
                    var result = await BlazorDownloadFileManager.DownloadFile($"School Event Transport List_{DateTime.Now:ddMMyyyyHHmmss}.xlsx", Convert.FromBase64String(response.Data),
                "application/octet-stream");
                    if (result.Succeeded)
                        SnackBar.Add(@"Transport information exported succesfully", Severity.Success);
                    else
                        SnackBar.AddError(result.ErrorMessage);
                }
                else
                {
                    SnackBar.AddError("No Data found to export!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                SnackBar.AddError(e.Message);
            }
        }

        #region Table Setup

        /// <summary>
        /// Sets the page parameters for the table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        private void SetPageParameters(TableState state)
        {
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                var sortDirection = state.SortDirection switch
                {
                    SortDirection.Ascending => "asc",
                    SortDirection.Descending => "desc",
                    _ => ""
                };

                _args.OrderBy = state.SortDirection != SortDirection.None ? $"{state.SortLabel} {sortDirection}" : string.Empty;
            }

            _args.PageSize = state.PageSize;
            _args.PageNr = state.Page + 1;
        }

        /// <summary>
        /// Loads the data from the server based on the current page number, page size, and table state.
        /// </summary>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <param name="state">The current state of the table.</param>
        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            PaginatedResult<SchoolEventDto> request;

            _args.Active = Active;

            //if (_principal.IsInRole(RoleConstants.Parent))
            //{
            //    _args.ParentId = _principal.GetUserId();
            //    request = await Provider.GetPagedAsync<SchoolEventDto, SchoolEventPageParameters>("schoolevents/pagedevents/byparent", _args);
            //}
            //else if (_principal.IsInRole(RoleConstants.Teacher))
            //{
            //    _args.TeacherId = _principal.GetUserId();
            //    request = await Provider.GetPagedAsync<SchoolEventDto, SchoolEventPageParameters>("schoolevents/pagedevents/byteacher", _args);
            //}
            //else if (_principal.IsInRole(RoleConstants.Learner))
            //{
            //    _args.LearnerId = _principal.GetUserId();
            //    request = await Provider.GetPagedAsync<SchoolEventDto, SchoolEventPageParameters>("schoolevents/pagedevents/bylearner", _args);
            //}
            //else
            //{
            //    request = await Provider.GetPagedAsync<SchoolEventDto, SchoolEventPageParameters>("schoolevents/pagedevents/minimalinfo", _args);
            //}

            request = await SchoolEventQueryService.PagedSchoolEventsMinimalAsync(_args);

            if (request.Succeeded)
            {
                _totalItems = request.TotalCount;
                _currentPage = request.CurrentPage;
                _events = request.Data.Select(c => new SchoolEventViewModel(c));
            }
            SnackBar.AddErrors(request.Messages);
        }

        /// <summary>
        /// Reloads the table data from the server based on the current table state.
        /// </summary>
        /// <param name="state">The current state of the table.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the table data.</returns>
        private async Task<TableData<SchoolEventViewModel>> ServerReload(TableState state, CancellationToken token)
        {
            SetPageParameters(state);

            await LoadData(state.Page, state.PageSize, state);

            _table.Items = _events.ToList();

            return new TableData<SchoolEventViewModel> { TotalItems = _totalItems, Items = _table.Items };
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Lifecycle method invoked when the component is first initialized.
        /// Sets the user principal and initializes the component state.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _principal = authState.User;

            _canCreate = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.EventPermissions.Create)).Succeeded;
            _canEdit = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.EventPermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.EventPermissions.Delete)).Succeeded;
            _canExport = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.EventPermissions.Export)).Succeeded;
            _canSendNotification = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.EventPermissions.SendNotification)).Succeeded;
            _canCreateMessage = (await AuthorizationService.AuthorizeAsync(authState.User, SchoolsModule.Domain.Constants.Permissions.EventPermissions.CreateMessage)).Succeeded;

            if (Archived)
            {
                _args.Archived = true;
            }

            _loaded = true;

            await base.OnInitializedAsync();
        }

        #endregion
    }
}
