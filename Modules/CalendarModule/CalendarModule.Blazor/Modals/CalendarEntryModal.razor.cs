using CalendarModule.Application.ViewModels;
using CalendarModule.Domain.Constants;
using CalendarModule.Domain.Enums;
using CalendarModule.Domain.Interfaces;
using ConectOne.Blazor.Extensions;
using ConectOne.Blazor.Modals;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Utilities;

namespace CalendarModule.Blazor.Modals
{
    /// <summary>
    /// Represents a modal dialog for creating, editing, or deleting a calendar entry.
    /// </summary>
    public partial class CalendarEntryModal
    {
        private bool _canUpdate;
        private bool _canDelete;
        private List<UserInfoDto> _availableUsers = new();
        private List<RoleDto> _availableRoles = new();
        private readonly Func<UserInfoDto?, string?> _userInfoConverter = p => p.DisplayName ?? p.EmailAddress ?? p.UserName ?? p.UserId;
        private readonly Func<RoleDto?, string?> _roleConverter = p => p.Name;
        private readonly IEnumerable<MudColor> _customPalette = new List<IEnumerable<MudColor>>
        {
            MudColor.GenerateTintShadePalette("#DC143C", shadeStep: 0),
            MudColor.GenerateTintShadePalette("#1E90FF", tintStep: 0),
            MudColor.GenerateTintShadePalette("#8E24AA").Reverse(),
            MudColor.GenerateAnalogousPalette("#40E0D0"),
            MudColor.GenerateMultiGradientPalette(["#FF4500", "#32CD32", "#8A2BE2"]),
            MudColor.GenerateGradientPalette("#e2cb4bff", "#f7470dff"),
        }.SelectMany(palette => palette);

        #region Parameters

        /// <summary>
        /// Provides the current user's authentication state, used to evaluate access permissions.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the instance of the MudBlazor dialog.
        /// </summary>
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        /// <summary>
        /// Gets or sets the calendar entry view model.
        /// </summary>
        [Parameter] public CalendarEntryViewModel CalendarEntry { get; set; } = new CalendarEntryViewModel();

        /// <summary>
        /// Gets or sets a value indicating whether the delete button should be shown.
        /// </summary>
        [Parameter] public bool ShowDeleteButton { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for an appointment.
        /// </summary>
        [Parameter] public string AppointmentId { get; set; }

        /// <summary>
        /// Injects the authorization service for checking user permissions.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// SnackBar for showing user notifications, including errors and confirmations.
        /// </summary>
        [Inject] public ISnackbar Snackbar { get; set; }

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; }

        /// <summary>
        /// Gets or sets the NavigationManager used to manage URI navigation and location state within the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs and for responding to location changes. This property is typically injected by the Blazor
        /// framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets or sets the service used to manage appointments.
        /// </summary>
        [Inject] public IAppointmentService AppointmentService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the user service used to manage user-related operations.
        /// </summary>
        [Inject] public IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to manage and query user roles.
        /// </summary>
        [Inject] public IRoleService RoleService { get; set; } = null!;

        /// <summary>
        /// Handles changes to the selected start date for an event.
        /// </summary>
        /// <remarks>If the selected date is in the past, an error message is displayed. Additionally, if
        /// the  selected start date is later than the current end date, the end date is updated to match  the start
        /// date.</remarks>
        /// <param name="date">The newly selected start date. Can be <see langword="null"/>.</param>
        private void OnStartDateSelectionChanged(DateTime? date)
        {
            if (date.Value < DateTime.Now)
            {
                Snackbar.AddError("You cannot appointment an event in the past");
            }

            CalendarEntry.StartDate = date;
            if (date > CalendarEntry.EndDate)
            {
                CalendarEntry.EndDate = date;
            }
        }

        /// <summary>
        /// Handles changes to the selected end date of the calendar entry.
        /// </summary>
        /// <remarks>If the provided date is earlier than the start date of the calendar entry, an error
        /// message is displayed. Otherwise, the end date of the calendar entry is updated.</remarks>
        /// <param name="date">The newly selected end date. Can be <see langword="null"/>.</param>
        private void OnEndDateSelectionChanged(DateTime? date)
        {
            if (date.Value < CalendarEntry.StartDate)
            {
                Snackbar.AddError("You cannot appointment an event where the appointment ends before it starts");
            }

            CalendarEntry.EndDate = date;
        }

        private void OnAudienceTypeChanged(AppointmentAudienceType audienceType)
        {
            CalendarEntry.AudienceType = audienceType;

            if (audienceType != AppointmentAudienceType.SpecificUsers)
            {
                CalendarEntry.InvitedUsers = new List<UserInfoDto>();
            }

            if (audienceType != AppointmentAudienceType.SpecificRoles)
            {
                CalendarEntry.InvitedRoles = new List<RoleDto>();
            }
            StateHasChanged();
        }

        /// <summary>
        /// Deletes the current calendar entry after user confirmation.
        /// </summary>
        private async Task DeleteCalendarEntry()
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this calendar entry?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var removalResult = await AppointmentService.DeleteAsync(CalendarEntry.Id);
                if (removalResult.Succeeded)
                {
                    MudDialog.Close(CalendarEntry);
                    NavigationManager.NavigateTo("/calendar", true);
                }
            }
        }

        /// <summary>
        /// Creates a new calendar entry and closes the dialog.
        /// </summary>
        public void CreateAsync()
        {
            if (CalendarEntry.AudienceType == AppointmentAudienceType.SpecificUsers && CalendarEntry.InvitedUsers.Count() == 0)
            {
                Snackbar.Add("Please select at least one user to invite.", Severity.Error);
                return;
            }

            if (CalendarEntry.AudienceType == AppointmentAudienceType.SpecificRoles && CalendarEntry.InvitedRoles.Count() == 0)
            {
                Snackbar.Add("Please select at least one role to invite.", Severity.Error);
                return;
            }

            if (new DateTime(CalendarEntry.StartDate.Value.Year, CalendarEntry.StartDate.Value.Month,
                    CalendarEntry.StartDate.Value.Day, CalendarEntry.StartTime.Value.Hours,
                    CalendarEntry.StartTime.Value.Minutes, CalendarEntry.StartTime.Value.Seconds) <= DateTime.Now)
            {
                Snackbar.Add("You cannot add an event that starts in the past", Severity.Error);
                return;
            }

            if (CalendarEntry.AudienceType != AppointmentAudienceType.SpecificUsers)
            {
                CalendarEntry.InvitedUsers = new List<UserInfoDto>();
            }

            if (CalendarEntry.AudienceType != AppointmentAudienceType.SpecificRoles)
            {
                CalendarEntry.InvitedRoles = new List<RoleDto>();
            }
            // Closes the MudDialog and passes CalendarEntry as the data payload.
            MudDialog.Close(CalendarEntry);
        }

        /// <summary>
        /// Cancels the selection process and closes the dialog.
        /// </summary>
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        #endregion

        /// <summary>
        /// Asynchronously initializes the component, setting up authorization permissions and loading calendar entry
        /// data if an appointment ID is provided.
        /// </summary>
        /// <remarks>This method determines whether the user has the necessary permissions to update or
        /// delete calendar entries. If an <see cref="AppointmentId"/> is specified, it retrieves the corresponding
        /// calendar entry data and processes the response for display.</remarks>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;

            _canUpdate = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.CalendarPermissions.Edit)).Succeeded;
            _canDelete = (await AuthorizationService.AuthorizeAsync(authState.User, Permissions.CalendarPermissions.Delete)).Succeeded;

            var usersTask = UserService.PagedUsers(new UserPageParameters { PageSize = 200 });
            var rolesTask = RoleService.AllRoles();

            if (!string.IsNullOrEmpty(AppointmentId))
            {
                var result = await AppointmentService.GetByIdAsync(AppointmentId);
                result.ProcessResponseForDisplay(Snackbar, () =>
                {
                    CalendarEntry = new CalendarEntryViewModel(result.Data);
                });
            }

            var usersResult = await usersTask;
            if (usersResult.Succeeded)
            {
                _availableUsers = (usersResult.Data ?? new List<UserInfoDto>())
                    .Where(u => !string.IsNullOrWhiteSpace(u.UserId))
                    .ToList();
            }
            else if (usersResult.Messages.Any())
            {
                foreach (var message in usersResult.Messages)
                {
                    Snackbar.Add(message, Severity.Error);
                }
            }

            var rolesResult = await rolesTask;
            rolesResult.ProcessResponseForDisplay(Snackbar, () =>
            {
                _availableRoles = rolesResult.Data?.Where(r => !string.IsNullOrWhiteSpace(r.Id)).ToList() ?? new List<RoleDto>();
            });

            await base.OnInitializedAsync();
        }
    }
}
