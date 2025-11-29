using System.Security.Claims;
using ConectOne.Blazor.Modals;
using IdentityModule.Application.ViewModels;
using IdentityModule.Blazor.Modals;
using IdentityModule.Domain.Constants;
using IdentityModule.Domain.Interfaces;
using IdentityModule.Domain.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;

namespace IdentityModule.Blazor.Components
{
    /// <summary>
    /// A Blazor component for displaying and managing a paginated list of users.
    /// Includes functionality for searching, exporting data, and managing roles or profiles.
    /// </summary>
    public partial class UsersList
    {
        private UserPageParameters _pageParameters = new UserPageParameters { Active = true };
        private UserInfoViewModel _user = new();
        private MudTable<UserInfoViewModel> _userTable = null!;
        private ClaimsPrincipal _currentUser = null!;

        private string _searchString = "";
        private bool _dense;
        private bool _striped = true;
        private bool _bordered;
        private bool _loaded;
        private bool _canCreateUsers;
        private bool _canSearchUsers;
        private bool _canExportUsers;
        private bool _canViewRoles;

        #region Parameters and Injections

        /// <summary>
        /// Gets or sets the task that provides the authentication state of the current user.
        /// </summary>
        /// <remarks>This property is typically used in Blazor applications to access the authentication
        /// state  within a component. The <see cref="AuthenticationState"/> provides information about the  user's
        /// identity and authentication status.</remarks>
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the HTTP provider used for making HTTP requests.
        /// </summary>
        /// <remarks>The provider must be set before making any HTTP requests. Dependency injection is
        /// used to supply the implementation.</remarks>
        [Inject] public IUserService UserService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the export service used to perform data export operations.
        /// </summary>
        [Inject] public IExportService ExportService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display snack bar notifications to the user.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to authorize user actions within the application.
        /// </summary>
        [Inject] public IAuthorizationService AuthorizationService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the NavigationManager used to manage and interact with URI navigation in the application.
        /// </summary>
        /// <remarks>The NavigationManager provides methods and properties for programmatically navigating
        /// to different URIs and for responding to navigation events. It is typically injected by the Blazor
        /// framework.</remarks>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Gets or sets the JavaScript runtime abstraction used to invoke JavaScript functions from .NET code.
        /// </summary>
        /// <remarks>This property is typically set by the Blazor framework through dependency injection.
        /// Use this property to perform JavaScript interop operations within a Blazor component or service.</remarks>
        [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to display dialogs to the user.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the role associated with the current context.
        /// </summary>
        [Parameter] public string? Role { get; set; } = null!;

        #endregion

        #region Methods

        /// <summary>
        /// Reloads the user table to reflect the latest data.
        /// </summary>
        private async Task ReloadTable()
        {
            _loaded = false;
            await _userTable.ReloadServerData();
            _loaded = true;
        }

        /// <summary>
        /// Exports the currently filtered user data to an Excel file.
        /// </summary>
        private async Task ExportToExcel()
        {
            var base64 = await ExportService.ExportToExcelAsync(_pageParameters);
            await JSRuntime.InvokeVoidAsync("Download", new
            {
                ByteArray = base64,
                FileName = string.IsNullOrEmpty(_pageParameters.Role) ? $"users_{DateTime.Now:ddMMyyyyHHmmss}.xlsx" : $"{_pageParameters.Role}_users_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            });
            SnackBar.Add(string.IsNullOrWhiteSpace(_searchString) ? "Users exported" : "Filtered Users exported", Severity.Success);
        }

        /// <summary>
        /// Opens a modal dialog for registering a new user.
        /// </summary>
        private async Task InvokeModal()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = await DialogService.ShowAsync<RegisterUserModal>("Register New User", parameters, options);
            var result = await dialog.Result;
            if (!result!.Canceled)
            {
                await ReloadTable();
            }
        }

        /// <summary>
        /// Navigates to the profile page of the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        private void ViewProfile(string userId)
        {
            NavigationManager.NavigateTo($"/registrations/users/{userId}");
        }

        /// <summary>
        /// Navigates to the role management page for a user, with restrictions on super users.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="email">The email of the user.</param>
        private void ManageRoles(string userId, string email)
        {
            NavigationManager.NavigateTo($"/security/roles/{userId}");
        }

        /// <summary>
        /// Deletes a specific user from the databases
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        private async Task DeleteBlogEntry(string userId)
        {
            var parameters = new DialogParameters<ConformtaionModal>
            {
                { x => x.ContentText, "Are you sure you want to remove this user from this application?" },
                { x => x.ButtonText, "Yes" },
                { x => x.Color, MudBlazor.Color.Success }
            };

            var dialog = await DialogService.ShowAsync<ConformtaionModal>("Confirm", parameters);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                var removalResult = await UserService.RemoveUserAsync(userId);
                if (removalResult.Succeeded)
                    await ReloadTable();
            }
        }

        #endregion

        #region Table Setup

        /// <summary>
        /// Fetches user data for the table with pagination and sorting support.
        /// </summary>
        /// <param name="state">The current state of the table (e.g., page, sort).</param>
        /// <param name="token">Cancellation token for async operations.</param>
        /// <returns>A <see cref="TableData{T}"/> object containing user data.</returns>
        private async Task<TableData<UserInfoViewModel>> GetUsersAsync(TableState state, CancellationToken token)
        {
            _pageParameters.PageNr = state.Page + 1;
            _pageParameters.PageSize = state.PageSize;

            if (state.SortDirection == SortDirection.Ascending)
                _pageParameters.OrderBy = $"{state.SortLabel} desc";
            else if (state.SortDirection == SortDirection.Descending)
                _pageParameters.OrderBy = $"{state.SortLabel} asc";
            else _pageParameters.OrderBy = null;

            var response = await UserService.PagedUsers(_pageParameters);

            return new TableData<UserInfoViewModel>
            {
                TotalItems = response.TotalCount,
                Items = response.Data.Select(c => new UserInfoViewModel(c))
            };
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Performs initialization logic for the component, including fetching permissions for the current user.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            _pageParameters.Role = Role;

            var authState = await AuthenticationStateTask;
            _currentUser = authState.User;

            _canCreateUsers = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Create)).Succeeded;
            _canSearchUsers = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Search)).Succeeded;
            _canExportUsers = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Export)).Succeeded;
            _canViewRoles = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.View)).Succeeded;

            _loaded = true;
        }

        #endregion       
    }
}
