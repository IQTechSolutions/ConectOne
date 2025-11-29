using System.Security.Claims;
using IdentityModule.Application.ViewModels;
using IdentityModule.Blazor.Modals;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Enums;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;

namespace SchoolsEnterprise.Blazor.Shared.Pages.Authentication
{
    /// <summary>
    /// The RejectedRegistrations component displays and manages user registrations 
    /// that have been marked as Rejected. It uses the <see cref="AccountsProvider"/> 
    /// to fetch data and a <see cref="MudTable{UserInfoViewModel}"/> for displaying paged results.
    /// Features include:
    /// 1. Searching and sorting rejected registrations via <see cref="UserPageParameters"/>.
    /// 2. Exporting user data to Excel.
    /// 3. Opening a modal to register a new user (e.g., switching status or re-registrations).
    /// 4. Using authorization checks to enable or disable features (create/export/approve).
    /// </summary>
    public partial class RejectedRegistrations
    {
        private UserInfoViewModel _registration = new();
        private MudTable<UserInfoViewModel> _pendingRegistrationTable = default!;
        private ClaimsPrincipal _currentUser = default!;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;
        private bool _loaded;
        private bool _canCreateUsers;
        private bool _canExportUsers;
        private bool _canApproveRegistrations;

        /// <summary>
        /// Gets or sets the user service used to manage user-related operations within the component.
        /// </summary>
        /// <remarks>This property is typically injected by the framework and should not be set manually
        /// in most scenarios.</remarks>
        [Inject] public IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the export service used to perform data export operations.
        /// </summary>
        [Inject] public IExportService ExportService { get; set; }

        /// <summary>
        /// Gets or sets the service used to handle authorization operations.
        /// </summary>
        /// <remarks>This property is typically injected by the dependency injection framework and should
        /// not be set manually in most cases. Ensure that a valid implementation of <see cref="IAuthorizationService"/>
        /// is configured in the dependency injection container.</remarks>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs in the application.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime instance used for invoking JavaScript functions from .NET.
        /// </summary>
        [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

        /// <summary>
        /// Gets or sets the <see cref="ISnackbar"/> service used to display notifications or messages to the user.
        /// </summary>
        /// <remarks>The <see cref="ISnackbar"/> service is typically used to display transient messages
        /// or alerts in the UI. Ensure that the service is properly configured and injected before use.</remarks>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        // Defines query parameters for fetching users (e.g., page number, status).
        // Here, Active = false and RegistrationStatus = Rejected ensures we only fetch "rejected" registrations.
        private UserPageParameters _pageParameters = new UserPageParameters
        {
            Active = false,
            RegistrationStatus = RegistrationStatus.Rejected
        };
               
        /// <summary>
        /// Provided authentication state from Blazor; used to retrieve the current user's claims if needed.
        /// </summary>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        #region Methods

        /// <summary>
        /// Instructs the MudTable to reload its data (triggering GetPendingRegistrationsAsync).
        /// Useful after certain operations like adding or removing a user.
        /// </summary>
        private void ReloadTable()
        {
            _pendingRegistrationTable.ReloadServerData();
        }

        /// <summary>
        /// Fetches a paged list of rejected registrations. Called automatically by the MudTable 
        /// when the table is first rendered or when the user interacts with pagination or sorting controls.
        /// </summary>
        /// <param name="state">The current state of the table (page, page size, sort parameters).</param>
        /// <param name="token">Cancellation token to cancel a long fetch operation.</param>
        /// <returns>A <see cref="TableData{UserInfoViewModel}"/> containing total item count and current items.</returns>
        private async Task<TableData<UserInfoViewModel>> GetPendingRegistrationsAsync(TableState state, CancellationToken token)
        {
            // Adjust the paging parameters based on user interactions in the MudTable
            _pageParameters.PageNr = state.Page + 1;        // MudTable is zero-based, server is likely one-based
            _pageParameters.PageSize = state.PageSize;

            // Adjust sorting direction in _pageParameters based on user interactions
            if (state.SortDirection == SortDirection.Ascending)
                _pageParameters.OrderBy = $"{state.SortLabel} desc";
            else if (state.SortDirection == SortDirection.Descending)
                _pageParameters.OrderBy = $"{state.SortLabel} asc";
            else
                _pageParameters.OrderBy = null;

            // Fetch data via AccountsProvider
            var response = await UserService.PagedUsers(_pageParameters);

            // Return data in the format MudTable requires (Items + TotalItems)
            return new TableData<UserInfoViewModel>
            {
                TotalItems = response.TotalCount,
                Items = response.Data.Select(c => new UserInfoViewModel(c))
            };
        }

        /// <summary>
        /// Opens a modal dialog to register a new user. If the dialog completes successfully, 
        /// refreshes the table to reflect any new data.
        /// </summary>
        private async Task InvokeModal()
        {
            // Prepare optional parameters to pass into the dialog (if needed)
            var parameters = new DialogParameters();
            // Define some display options for the dialog (size, close button, etc.)
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            // Display the modal (RegisterUserModal is presumably another component or page)
            var dialog = DialogService.Show<RegisterUserModal>("Register New User", parameters, options);
            var result = await dialog.Result;

            // If user didn't cancel, reload the table to see new data
            if (!result.Canceled)
            {
                _pendingRegistrationTable.ReloadServerData();
            }
        }

        /// <summary>
        /// Updates the local search parameter and triggers a table reload.
        /// This is typically bound to a search input or filter UI.
        /// </summary>
        /// <param name="value">The user's search text.</param>
        public void SetSearchString(string value)
        {
            _pageParameters.SearchText = value;
            _pendingRegistrationTable.ReloadServerData();
        }

        /// <summary>
        /// Exports the filtered user data to an Excel file. Uses the <see cref="IJSRuntime"/> 
        /// to initiate file download on the client side, and a <see cref="SnackBar"/> to show results.
        /// </summary>
        private async Task ExportToExcel()
        {
            var base64 = await ExportService.ExportToExcelAsync(new UserPageParameters());

            await JsRuntime.InvokeVoidAsync("Download", new
            {
                ByteArray = base64,
                FileName = $"{nameof(Users).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            });

            // Provide user feedback in the UI
            SnackBar.Add(
                string.IsNullOrWhiteSpace(_searchString) ? "Users exported" : "Filtered Users exported",
                Severity.Success
            );
        }

        /// <summary>
        /// A placeholder for changing the registration status of a user to a given status.
        /// For instance, un-rejecting or re-approving a registration. Not yet implemented.
        /// </summary>
        /// <param name="userId">The identifier of the user whose status is being changed.</param>
        /// <param name="status">The new registration status.</param>
        public async Task ChangeRegistrationStatusRegistration(string userId, RegistrationStatus status)
        {
            // Implementation to be filled based on application logic
        }

        #endregion        

        #region Overrides

        /// <summary>
        /// Called when the component initializes. Retrieves the current user's claims 
        /// to check permissions (create/export/approve registrations), then marks the 
        /// component as loaded, allowing the table to display data.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Retrieve the current authentication state
            var authState = await AuthenticationStateTask;
            _currentUser = authState.User;

            // Check if the user is allowed to create users
            _canCreateUsers =
                (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Create))
                .Succeeded;
            // Check if the user can export user data
            _canExportUsers =
                (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Export))
                .Succeeded;
            // Check if the user can approve registrations (or view roles in this context)
            _canApproveRegistrations =
                (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.View))
                .Succeeded;

            // Mark the component as loaded so the UI can display
            _loaded = true;
        }

        #endregion
    }
}
